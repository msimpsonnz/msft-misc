variable "azure_tenant_id" {
  description = "Azure Tenant ID"
}

variable "azure_subscription_id" {
  description = "Azure Subscription ID"
}

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

variable "virtualnetworkname" {
  description = "Name of the virtual network"
}

variable "cidr" {
  description = "CIDR range of the VPC"
  default = "10.99.0.0/16"
}

variable "cidr_subnet" {
  description = "CIDR range of the only subnet in the VPC"
  default = "10.99.0.0/24"
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

variable "dns_prefix" {
  description = "DNS prefix for the cluster"
}

variable "first_master_ip" {
  description = "First consecutive IP address to be assigned to master nodes"
}

variable "ssh_key" {
  description = "SSH public key in PEM format to apply to VMs"
}