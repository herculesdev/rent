echo off
echo %1
IF [%1] == [] GOTO ExitBatch

:RunMigration
echo "Running..."
dotnet ef migrations add %1 --context RenterContext --output-dir ./Data/Migrations --startup-project ../Rent.Renter.Api/
Exit

:ExitBatch
echo "Missing migration name"
Exit