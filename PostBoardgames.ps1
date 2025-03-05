$boardgames = Get-Content ".\Boardgames2.json" -Raw | ConvertFrom-Json -Depth 10

foreach ($boardgame in $boardgames) {
    $boardgameJson = $boardgame | ConvertTo-Json -Depth 10
    $response = Invoke-RestMethod -Uri "https://localhost:7003/boardgames" -Method Post -Body $boardgameJson -ContentType "application/json"
    Write-Host "Added boardgame $($boardgame.Name) with id $($response.Id)"
}

