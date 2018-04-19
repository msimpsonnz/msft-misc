output "azurerm_virtual_network_default_name" {
  value = "${azurerm_virtual_network.default.name}"
}

output "virtualnetwork_subnet_default_name" {
  value = "${azurerm_subnet.default.name}"
}

output "virtualnetwork_subnet_default_id" {
  value = "${azurerm_subnet.default.id}"
}