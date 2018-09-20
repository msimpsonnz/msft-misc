Create Resource Group and deploy ARM

```
New-AzureRmResourceGroup -Name FN18 -Location "South Central US"
New-AzureRmResourceGroupDeployment -Name FN18-Deployment -ResourceGroupName FN18 `
-TemplateFile .\azuredeploy.json -searchName mjsfn18
```