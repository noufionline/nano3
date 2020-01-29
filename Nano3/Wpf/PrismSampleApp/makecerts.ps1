Write-Host "Creating Certificates for Self-Signed Testing"

$thumb = '<TODO>'

Write-Host "Getting Root Certificate"
$cert = Get-ChildItem "cert://localmachine/my/$($thumb)"

# Client Auth
Write-Host "Creating Client Auth Certificate"
$clientCert = New-SelfSignedCertificate -Type Custom -KeySpec Signature `
-Subject "CN=MeterClientCert" -KeyExportPolicy Exportable `
-FriendlyName "MeterClientCert" `
-HashAlgorithm sha256 -KeyLength 2048 `
-NotAfter (Get-Date).AddMonths(24) `
-CertStoreLocation "cert:\LocalMachine\My" `
-Signer $cert -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.2")

$PFXPass = ConvertTo-SecureString -String "P@ssw0rd!" -Force -AsPlainText

Write-Host "Exporting Certificates to File"

Export-PfxCertificate -Cert $clientCert `
-Password $PFXPass `
-FilePath meterselfcert.pfx

