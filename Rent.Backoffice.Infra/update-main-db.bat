echo off
:RunUpdate
echo "Running..."
dotnet ef database update --context BackofficeContext --startup-project ../Rent.Backoffice.Api/
Exit