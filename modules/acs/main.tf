resource "azurerm_resource_group" "container_service" {
  name     = "${var.prefix}-${var.env}-${var.resource_group_name}"
  location = "${var.azure_location}"

    tags {
    env = "${var.env}"
  }
}

resource "azurerm_container_service" "container_service" {
  name                   = "${var.cluster_name}"
  resource_group_name    = "${azurerm_resource_group.container_service.name}"
  location               = "${var.azure_location}"
  orchestration_platform = "Kubernetes"

  master_profile {
    count      = "${var.master_count}"
    dns_prefix = "${var.dns_name_prefix}-master"
  }

  agent_pool_profile {
    name       = "agentpools"
    count      = "${var.linux_agent_count}"
    dns_prefix = "${var.dns_name_prefix}-agent"
    vm_size    = "${var.linux_agent_vm_size}"
  }

  linux_profile {
    admin_username = "${var.linux_admin_username}"

    ssh_key {
      key_data = "${var.linux_admin_ssh_publickey}"
    }
  }

  service_principal {
    client_id = "${var.azure_service_principal_client_id}"
    client_secret = "${var.azure_service_principal_client_secret}"
  }

  diagnostics_profile {
    enabled = false
  }

  tags {
    Source = "Azure Quickstarts for Terraform"
  }
}