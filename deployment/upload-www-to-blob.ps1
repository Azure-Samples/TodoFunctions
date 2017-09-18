$ProgressPreference="SilentlyContinue"

$destinationUri = "https://$($Env:STORAGE_HOSTNAME)/www"
$destinationKey = $Env:STORAGE_KEY
.\deployment\AzCopy\AzCopy.exe /Source:.\ToDoFunctions\www /Dest:$destinationUri /DestKey:$destinationKey /SetContentType /S /Y

$storageContext = New-AzureStorageContext -ConnectionString $Env:STORAGE_CONNECTION
Set-AzureStorageContainerAcl -Container "www" -Permission Container -Context $storageContext

