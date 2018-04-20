variable "azure_location" {
  description = "Azure Location, e.g. North Europe"
}

variable "env" {
  description = "Environment Name"
}

variable "prefix" {
  description = "Prefix"
}

variable "resource_group_name" {
  description = "Azure Resource Group Name"
}

variable "azure_service_principal_client_id" {
  description = "Azure Client ID"
}

variable "azure_service_principal_client_secret" {
  description = "Azure Client Secret"
}

variable "master_count" {
  description = "Number of master VMs to create"
}

variable "dns_name_prefix" {
  description = "DNS prefix for the cluster"
}

variable "linux_agent_count" {
  description = "Number of worker VMs to initially create"
}

variable "linux_agent_vm_size" {
  description = "Azure VM type"
}

variable "linux_admin_username" {
  description = "Administrative username for the VMs"
}

variable "linux_admin_ssh_publickey" {
  description = "SSH public key in PEM format to apply to VMs"
}

variable "cluster_name" {
  description = "Name of the K8s cluster"
}

