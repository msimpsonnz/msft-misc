provider "azurerm" {
  tenant_id = "${var.azure_tenant_id}"
  subscription_id = "${var.azure_subscription_id}"
  client_id = "${var.azure_service_principal_client_id}"
  client_secret = "${var.azure_service_principal_client_secret}"
}