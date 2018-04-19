module "net" {
  source = "./modules/net"

  net_resource_group_name = "${var.net_resource_group_name}"
  virtualnetworkname = "vnet1"
  cidr = "${var.cidr}"
  cidr_subnet = "${var.cidr_subnet}"

}

module "acs" {
  source = "./modules/acs"

  azure_client_id = "${var.azure_client_id}"
  azure_client_secret = "${var.azure_client_secret}"

  net_resource_group_name = "${var.net_resource_group_name}"
  vnet_name = "${var.env}-${var.virtualnetworkname}"
  subnet_name = "${module.net.virtualnetwork_subnet_default_name}"
  subnet_id = "${module.net.virtualnetwork_subnet_default_id}"

  k8s_resource_group_name = "${var.k8s_resource_group_name}"
  dns_prefix = "${var.dns_prefix}"
  first_master_ip = "${var.first_master_ip}"
  ssh_key = "${var.ssh_key}"
}