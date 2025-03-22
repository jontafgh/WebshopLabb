param(
    [string]$environment = "Development",
    [string]$launchProfile = "https",
    [string]$connectionStringKey = "WebshopDbPc",
    [bool]$dropDatabase = $false,
    [bool]$createDatabase = $false
)

$projectName = "WebshopBackend"
$baseUrl = "https://localhost:7003/"



#Install module SqlServer if not already installed
if (-not (Get-Module -ErrorAction Ignore -ListAvailable SqlServer)) {
    Write-Verbose "Installing SqlServer module for the current user..."
    Install-Module -Scope CurrentUser SqlServer -ErrorAction Stop
}
Import-Module SqlServer

# Set the environment variable
$env:ASPNETCORE_ENVIRONMENT = $environment

# Read the connection string from appsettings.Development.json
$appSettings = Get-Content ".\$projectName\appsettings.json" | ConvertFrom-Json
$connectionString = $appSettings.ConnectionStrings.$connectionStringKey
Write-Host "Database Connection String: $connectionString" -ForegroundColor Blue

# Get the database name from the connection string
if ($connectionString -match "Database=(?<dbName>[^;]+)")
{
    $databaseName = $matches['dbName']
    Write-Host "Database Name: $databaseName" -ForegroundColor Blue
}else{
    Write-Host "Database Name not found in connection string" -ForegroundColor Red
    exit
}

# Check if the database exists
$conStringDbExcluded = $connectionString -replace "Database=[^;]+;", ""
$queryDbExists = Invoke-Sqlcmd -ConnectionString $conStringDbExcluded -Query "Select name FROM sys.databases WHERE name='$databaseName'"
if($queryDbExists){
    if($dropDatabase -or (Read-Host "Do you want to drop the database? (y/n)").ToLower() -eq "y") {

        # Drop the database
        Invoke-Sqlcmd -ConnectionString $connectionString -Query  "USE master;ALTER DATABASE $databaseName SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE $databaseName;"
        Write-Host "Database $databaseName dropped." -ForegroundColor Green
    }
}

# Create the database from the model
if(Select-String -LiteralPath ".\$projectName\Program.cs" -Pattern "EnsureCreated()"){
    Write-Host "The project uses EnsureCreated() to create the database from the model." -ForegroundColor Yellow
} else {
    if($createDatabase -or (Read-Host "Should dotnet ef migrate and update the database? (y/n)").ToLower() -eq "y") {

        dotnet ef migrations add "UpdateModelFromScript_$(Get-Date -Format "yyyyMMdd_HHmmss")" --project ".\$projectName\$projectName.csproj"
        dotnet ef database update --project ".\$projectName\$projectName.csproj"
    }
}

# Run the application
if((Read-Host "Start the server from Visual studio? (y/n)").ToLower() -ne "y") {
    Start-Process -FilePath "dotnet" -ArgumentList "run --launch-profile $launchProfile --project .\$projectName\$projectName.csproj" -WindowStyle Normal
    Write-Host "Wait for the server to start..." -ForegroundColor Yellow
}

# Continue with the rest of the script
Read-Host "Press Enter to continue when the server is started..."

# Post boardgames to the server
$boardgames = Get-Content ".\Boardgames.json" -Raw | ConvertFrom-Json -Depth 10

foreach ($boardgame in $boardgames) {
    $boardgameJson = $boardgame | ConvertTo-Json -Depth 10
    $response = Invoke-RestMethod -Uri $baseUrl"boardgames" -Method Post -Body $boardgameJson -ContentType "application/json"
    Write-Host "Added boardgame $($boardgame.Name) with id $($response.Id)"
}

# Script completed
Write-Host "Script completed." -ForegroundColor Green

