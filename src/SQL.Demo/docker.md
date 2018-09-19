#### Running SQL Locally

### Setup a container locally
This command
* latest 2017 image from Docker Hub
* Accept EULA and use default password
* Exposes SQL on port 41433 to host machine
* Uses a volume mount on the host machine to persist data
* Give the container a friendly name 

```
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=<YourStrong!Passw0rd>' `
  -p 41433:1433 `
  -v c:/Docker/mssql:/var/opt/mssql `
  --name sql1 `
  -d microsoft/mssql-server-linux:2017-latest
```

### Restore the Database within the container
```
RESTORE DATABASE AdventureWorks2012
  FROM DISK = '/var/opt/mssql/bak/AdventureWorks2012.bak'
  WITH MOVE 'AdventureWorks2012' TO '/var/opt/mssql/data/AdventureWorks2012.mdf',  
  MOVE 'AdventureWorks2012_log' TO '/var/opt/mssql/data/AdventureWorks2012.ldf';  
```

### 
```
CREATE DATABASE AdventureWorks2012   
    ON (FILENAME = '/var/opt/mssql/data/AdventureWorks2012.mdf'),   
    (FILENAME = '/var/opt/mssql/data/AdventureWorks2012.ldf')   
    FOR ATTACH;
```