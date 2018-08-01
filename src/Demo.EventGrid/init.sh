# Setup variables
storageAccountName=<storageAccountName>
resourceGroup=<resourceGroup>
endpoint=<endpoint>
eventSubscriptionName=<eventSubscriptionName>
containerName=<containerName>

# Get storage account details
storageid=$(az storage account show --name $storageAccountName --resource-group $resourceGroup --query id --output tsv)

# Create Event Grid subscription
## endpoint: Azure Function
## prefix filter: Listens to all events on the enitre container
## deadletter: location for failed messages
## ttl: how long will this event live
## attempts: how many retries
az eventgrid event-subscription create \
  --resource-id $storageid \
  --name $eventSubscriptionName \
  --endpoint $endpoint \
  --subject-begins-with /blobServices/default/containers/voi/ \
  --deadletter-endpoint $storageid/blobServices/default/containers/$containerName \
  --event-ttl 1 \
  --max-delivery-attempts 1

# Get storage key for upload
export AZURE_STORAGE_ACCOUNT=$storageAccountName
export AZURE_STORAGE_ACCESS_KEY="$(az storage account keys list --account-name $storageAccountName --resource-group $resourceGroup --query "[0].value" --output tsv)"

# Upload a test file
touch testfile.txt
az storage blob upload --file testfile.txt --container-name voi --name testfile.txt