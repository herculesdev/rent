echo off
echo %1
IF [%1] == [] GOTO ExitBatch

:RunMigration
echo "Running..."
dotnet ef migrations add %1 --context BackofficeContext --output-dir ./Data/Migrations --startup-project ../Rent.Backoffice.Api/
Exit

:ExitBatch
echo "Missing migration name"
Exit