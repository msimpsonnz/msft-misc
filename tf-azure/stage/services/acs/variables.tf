variable "azure_location" {
  description = "Azure Location, e.g. North Europe"
  default = "Australia Southeast"
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

variable "azure_tenant_id" {
  description = "Azure Tenant ID"
}

variable "azure_subscription_id" {
  description = "Azure Subscription ID"
}

variable "azure_service_principal_client_id" {
  description = "Azure Client ID"
}

variable "azure_service_principal_client_secret" {
  description = "Azure Client Secret"
}

variable "cluster_name" {
  type        = "string"
  description = "Name of the cluster"
}

variable "dns_name_prefix" {
  type        = "string"
  description = "Sets the domain name prefix for the cluster. The suffix 'master' will be added to address the master agents and the suffix 'agent' will be added to address the linux agents."
}

variable "linux_agent_count" {
  type        = "string"
  default     = "1"
  description = "The number of Kubernetes linux agents in the cluster. Allowed values are 1-100 (inclusive). The default value is 1."
}

#complete, up-to-date list of VM sizes can be found at https://docs.microsoft.com/en-us/azure/virtual-machines/linux/sizes
variable "linux_agent_vm_size" {
  type        = "string"
  default     = "Standard_D2_v2"
  description = "The size of the virtual machine used for the Kubernetes linux agents in the cluster."
}

variable "linux_admin_username" {
  type        = "string"
  description = "User name for authentication to the Kubernetes linux agent virtual machines in the cluster."
}

variable "linux_admin_ssh_publickey" {
  type        = "string"
  description = "Configure all the linux virtual machines in the cluster with the SSH RSA public key string. The key should include three parts, for example 'ssh-rsa AAAAB...snip...UcyupgH azureuser@linuxvm'"
}

variable "master_count" {
  type        = "string"
  default     = "1"
  description = "The number of Kubernetes masters for the cluster. Allowed values are 1, 3, and 5. The default value is 1."
}