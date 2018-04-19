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
}

variable "cidr_subnet" {
  description = "CIDR range of the only subnet in the VPC"
}