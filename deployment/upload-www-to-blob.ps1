$ProgressPreference="SilentlyContinue"

$destinationUri = "https://$($Env:STORAGE_HOSTNAME)/www"
$destinationKey = $Env:STORAGE_KEY
.\deployment\AzCopy\AzCopy.exe /Source:.\ToDoFunctions\www /Dest:$destinationUri /DestKey:$destinationKey /SetContentType /S /Y
if ($LastExitCode -ne 0) { throw "azcopy returned code $($LastExitCode)" }

$storageContext = New-AzureStorageContext -ConnectionString $Env:STORAGE_CONNECTION -ErrorAction Stop
Set-AzureStorageContainerAcl -Container "www" -Permission Container -Context $storageContext -ErrorAction Stop
