Minecraft
```
az container create --resource-group Containers --name mjsdemo-aci-minecraft --image openhack/minecraft-server --dns-name-label mjsdemo-aci-minecraft --ports 25565 --environment-variables EULA=TRUE
```
```
az container delete --resource-group Containers --name mjsdemo-aci-minecraft
```