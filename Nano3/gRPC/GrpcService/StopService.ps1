if (Test-Connection -ComputerName GRPC -Quiet) 
{
	Get-Service –ComputerName 192.168.30.208 -Name grpc -ErrorAction Stop | Stop-Service -WarningAction SilentlyContinue

}