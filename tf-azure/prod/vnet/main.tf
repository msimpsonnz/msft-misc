module "vnet" {
  source = "../../modules/vnet"
  
  azure_location = "${var.azure_location}"
  prefix = "${var.prefix}"
  env = "${var.env}"

  resource_group_name = "${var.resource_group_name}"
  virtualnetworkname = "${var.virtualnetworkname}"
  cidr = "${var.cidr}"
  cidr_subnet = "${var.cidr_subnet}"

}