FROM microsoft/azure-cli

RUN mkdir -p /var/lib/terraform && \
    curl https://releases.hashicorp.com/terraform/0.11.7/terraform_0.11.7_linux_amd64.zip --output terraform.zip && \
    unzip terraform.zip -d /var/lib/terraform && \
    rm terraform.zip && \
    /var/lib/terraform/terraform --version && \
echo "------------------ Terraform Successfully Installed ------------------"