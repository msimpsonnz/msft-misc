# Create a resource group
resource "azurerm_resource_group" "default" {
  name     = "${var.prefix}-${var.env}-${var.net_resource_group_name}"
  location = "${var.azure_location}"
}

# Azure Virtual Network
resource "azurerm_virtual_network" "default" {
  name                = "${var.env}-${var.virtualnetworkname}"
  address_space       = ["${var.cidr}"]
  location            = "${var.azure_location}"
  resource_group_name = "${var.prefix}-${var.env}-${var.net_resource_group_name}"
  depends_on = ["azurerm_resource_group.default"]
}

# Azure Virtual Network -> Subnet
resource "azurerm_subnet" "default" {
  name                 = "${var.virtualnetworkname}_subnet"
  resource_group_name  = "${var.prefix}-${var.env}-${var.net_resource_group_name}"
  virtual_network_name = "${azurerm_virtual_network.default.name}"
  address_prefix       = "${var.cidr_subnet}"
  depends_on = ["azurerm_virtual_network.default"]
}