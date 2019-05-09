module "acs" {
  source = "../../../modules/acs"
  
  azure_location = "${var.azure_location}"
  prefix = "${var.prefix}"
  env = "${var.env}"
  resource_group_name = "${var.resource_group_name}"

  cluster_name = "${var.cluster_name}"
  dns_name_prefix = "${var.dns_name_prefix}"
  linux_agent_count = "${var.linux_agent_count}"
  linux_agent_vm_size = "${var.linux_agent_vm_size}"
  linux_admin_username = "${var.linux_admin_username}"
  master_count = "${var.master_count}"
  linux_admin_ssh_publickey = "${var.linux_admin_ssh_publickey}"

  azure_service_principal_client_id = "${var.azure_service_principal_client_id}"
  azure_service_principal_client_secret = "${var.azure_service_principal_client_secret}"

}