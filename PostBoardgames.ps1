$boardgames = Get-Content ".\Boardgames.json" -Raw | ConvertFrom-Json

foreach ($boardgame in $boardgames) {
    $boardgameJson = $boardgame | ConvertTo-Json
    $response = Invoke-RestMethod -Uri "https://localhost:7003/products" -Method Post -Body $boardgameJson -ContentType "application/json"
    Write-Host "Added boardgame $($boardgame.Name) with id $($response.Id)"
}

