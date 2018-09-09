# Setup variables
storageAccountName=<storageAccountName>
resourceGroup=<resourceGroup>
endpoint=<endpoint>
eventSubscriptionName=<eventSubscriptionName>
containerName=<containerName>
AZURE_STORAGE_ACCOUNT=<>
AZURE_STORAGE_ACCESS_KEY=<>

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

#cd /mnt/c/r/GitHub/misc/src/Demo.EventGrid/

python /mnt/c/r/GitHub/misc/src/Demo.EventGrid/blob.py voi 20 1