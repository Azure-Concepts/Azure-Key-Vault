   Param( 
        [Parameter(Mandatory=$True)]
        [String]
        $certpath,
        [Parameter(Mandatory=$True)]
        [String]
        $AppPrincipalId 
     )

connect-msolservice
$cer = New-Object System.Security.Cryptography.X509Certificates.X509Certificate
$cer.Import($certpath)  # Replace path of your certificate based on enviroment
$binCert = $cer.GetRawCertData()
$credValue = [System.Convert]::ToBase64String($binCert); # Replace your App Principal ID 
New-MsolServicePrincipalCredential -AppPrincipalId $AppPrincipalId -Type asymmetric -Value $credValue -Usage verify