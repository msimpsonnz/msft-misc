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

variable "virtualnetworkname" {
  description = "Name of the virtual network"
}

variable "cidr" {
  description = "CIDR range of the VPC"
}

variable "cidr_subnet" {
  description = "CIDR range of the only subnet in the VPC"
}