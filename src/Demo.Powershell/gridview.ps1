


Function GetAzureRmSubscription ([bool] $Gui = $false, [string] $SubscriptionId)
{
  if ($gui) {
    $GetSubscriptionId = Get-AzureRmSubscription | Select-Object -Property SubscriptionId, Name | Out-GridView -PassThru
    $SubscriptionId = $GetSubscriptionId.SubscriptionId
  }

  Set-AzureRmContext -SubscriptionId $SubscriptionId
  Write-Output $SubscriptionId
 }