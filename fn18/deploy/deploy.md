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


####
Blazor
```
az storage blob upload-batch --account-name mjsdemoblazor -s . -d '$web'
```
```
az storage blob update --account-name mjsdemoblazor -c '$web' -n _framework/wasm/mono.wasm --content-type application/wasm
```

https://mjsdemoblazorfunc.azurewebsites.net/api/Moderator?code=3zcanx10pHFWnp/kMzMeTGIHxTfif9arKQSewEJfn1jVojyZMn5kNw==