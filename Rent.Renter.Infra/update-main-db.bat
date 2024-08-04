echo off
:RunUpdate
echo "Running..."
dotnet ef database update --context RenterContext --startup-project ../Rent.Renter.Api/
Exit