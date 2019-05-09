resourceGroup=<Resource Group Name>
frontDoor=<Front Door Name>
frontDoorEndpoint=<Front Door URL>
probeName=defaultProbe
loadBalanceName=defaultLoadBalance
blobEndpoint=<Storage Account>
blobName=Uploads
functionsEndpoint=<Function Endpoint>
functionsName=Functions
webEndpoint=<Blob Website>

az network front-door create \
    --backend-address $webEndpoint \
    --name $frontDoor \
    --resource-group $resourceGroup

az network front-door probe create \
    --front-door-name $frontDoor \
    --interval "30" \
    --name $probeName \
    --path "/" \
    --resource-group $resourceGroup

az network front-door load-balancing create \
    --front-door-name $frontDoor \
    --name $loadBalanceName \
    --resource-group $resourceGroup \
    --sample-size "4" \
    --successful-samples-required "2"

az network front-door backend-pool create \
    --address $blobEndpoint \
    --front-door-name $frontDoor \
    --load-balancing $loadBalanceName \
    --name $blobName \
    --probe $probeName \
    --resource-group $resourceGroup

az network front-door backend-pool create \
    --address $functionsEndpoint \
    --front-door-name $frontDoor \
    --load-balancing $loadBalanceName \
    --name $functionsName \
    --probe $probeName \
    --resource-group $resourceGroup

az network front-door routing-rule create \
    --backend-pool $blobName \
    --front-door-name $frontDoor \
    --frontend-endpoints $frontDoorEndpoint \
    --name $blobName \
    --resource-group $resourceGroup \
    --patterns '/uploads/*'

az network front-door routing-rule create \
    --backend-pool $functionsName \
    --front-door-name $frontDoor \
    --frontend-endpoints $frontDoorEndpoint \
    --name $functionsEndpoint \
    --resource-group $resourceGroup \
    --patterns "/api/*"