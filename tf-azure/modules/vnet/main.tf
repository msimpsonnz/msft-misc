# Create a resource group
resource "azurerm_resource_group" "vnet" {
  name     = "${var.prefix}-${var.env}-${var.resource_group_name}"
  location = "${var.azure_location}"
}

# Azure Virtual Network
resource "azurerm_virtual_network" "this" {
  name                = "${var.env}-${var.virtualnetworkname}"
  address_space       = ["${var.cidr}"]
  location            = "${var.azure_location}"
  resource_group_name = "${var.prefix}-${var.env}-${var.resource_group_name}"
  depends_on = ["azurerm_resource_group.vnet"]
}

# Azure Virtual Network -> Subnet
resource "azurerm_subnet" "k8s" {
  name                 = "${var.virtualnetworkname}_subnet"
  resource_group_name  = "${var.prefix}-${var.env}-${var.resource_group_name}"
  virtual_network_name = "${azurerm_virtual_network.this.name}"
  address_prefix       = "${var.cidr_subnet}"
  depends_on = ["azurerm_virtual_network.this"]
}