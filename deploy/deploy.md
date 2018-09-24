Create Resource Group and deploy ARM

```
New-AzureRmResourceGroup -Name FN18 -Location "South Central US"
New-AzureRmResourceGroupDeployment -Name FN18-Deployment -ResourceGroupName FN18 `
-TemplateFile .\azuredeploy.json -searchName mjsfn18
```

Or
```
az group deployment create -g FN18 --template-file azuredeploy.json \
--parameters searchName=mjsfn18

```

```
SELECT * FROM c WHERE c.Type = "comment" and c._ts >= @HighWaterMark ORDER BY c._ts
```