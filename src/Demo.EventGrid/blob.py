import os
import time
from azure.storage import CloudStorageAccount
from azure.storage.blob import BlockBlobService

try:
    account_name = os.environ['AZURE_STORAGE_ACCOUNT']
    account_key = os.environ['AZURE_STORAGE_ACCESS_KEY']
    account = CloudStorageAccount(account_name, account_key)
    assert account_name is not None
    assert account_key is not None
    account is not None
except:
    print "Error getting Azure details"

def list_blobs(account):
        
        # Create a Block Blob Service object
        blockblob_service = account.create_block_blob_service()
        container_name = 'voi'
        generator = blockblob_service.list_blobs(container_name)
        for blob in generator:
            print('\tBlob Name: ' + blob.name)
            blockblob_service.delete_blob(container_name, blob.name)

def create_blobs(account):
        

        # Create a Block Blob Service object
        blockblob_service = account.create_block_blob_service()
        container_name = 'voi'
        for x in range(0, 100):
            name = str(x) + '-file.txt'
            with open(name, "w") as f:
                f.write("")
            full_path_to_file = os.path.join(os.path.dirname(__file__), name)
            blockblob_service.create_blob_from_path(container_name, name, full_path_to_file)
            os.remove(name)
            time.sleep(10)


create_blobs(account)
list_blobs(account)