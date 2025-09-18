$body = @{
    numbers = @(2, 5, 3, 8, 4, 1)
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7049/numbers" -Method Post -Body $body -ContentType "application/json"


# $Body = @{ numbers = (1..1000000) } | ConvertTo-Json
# Invoke-RestMethod -Uri "https://localhost:7049/numbers/process/parallel" -Method Post -Body $Body -ContentType "application/json"