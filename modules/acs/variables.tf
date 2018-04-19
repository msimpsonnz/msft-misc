variable "azure_client_id" {
  description = "Azure Client ID"
}

variable "azure_client_secret" {
  description = "Azure Client Secret"
}

variable "azure_location" {
  description = "Azure Location, e.g. North Europe"
  default = "Australia Southeast"
}

variable "env" {
  description = "Environment Name"
  default = "test"
}

variable "prefix" {
  description = "Prefix"
  default = "terra"
}

variable "net_resource_group_name" {
  description = "Azure Resource Group Name"
}

variable "vnet_name" {
  description = "VNET Name"
}

variable "subnet_name" {
  description = "Subnet Name"
}

variable "subnet_id" {
  description = "Subnet ID"
}

variable "k8s_resource_group_name" {
  description = "Azure Resource Group Name"
}

variable "acs_engine_config_file" {
  description = "File name and location of the ACS Engine config file"
  default = "k8s.json"
}

variable "acs_engine_config_file_rendered" {
  description = "File name and location of the ACS Engine config file"
  default = "k8s_rendered.json"
}

variable "master_vm_count" {
  description = "Number of master VMs to create"
  default = 1
}

variable "dns_prefix" {
  description = "DNS prefix for the cluster"
}

variable "vm_size" {
  description = "Azure VM type"
  default = "Standard_A2"
}

variable "first_master_ip" {
  description = "First consecutive IP address to be assigned to master nodes"
}

variable "worker_vm_count" {
  description = "Number of worker VMs to initially create"
  default = 1
}

variable "admin_user" {
  description = "Administrative username for the VMs"
  default = "azureuser"
}

variable "ssh_key" {
  description = "SSH public key in PEM format to apply to VMs"
}

variable "cluster_name" {
  description = "Name of the K8s cluster"
  default = "k8sexample-cluster"
}

