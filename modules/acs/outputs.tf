# output "master_fqdn" {
#   value = "${azurerm_container_service.container_service.master_profile.fqdn}"
# }

# output "ssh_command_master0" {
#   value = "ssh ${var.linux_admin_username}@${azurerm_container_service.container_service.master_profile.fqdn} -A -p 22"
# }