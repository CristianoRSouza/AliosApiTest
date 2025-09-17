Write-Host "Iniciando APIs do Ailos..." -ForegroundColor Green

# Iniciar API ContaCorrente
Write-Host "Iniciando API ContaCorrente na porta 5001..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'src\APIs\Ailos.ContaCorrente.API'; dotnet run --urls='https://localhost:5001;http://localhost:5000'"

Start-Sleep -Seconds 5

# Iniciar API Transferência
Write-Host "Iniciando API Transferencia na porta 5002..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'src\APIs\Ailos.Transferencia.API'; dotnet run --urls='https://localhost:5002;http://localhost:5003'"

Write-Host "Aguardando APIs iniciarem..." -ForegroundColor Cyan
Start-Sleep -Seconds 10

# Abrir navegador com ambas as abas
Write-Host "Abrindo Swagger das APIs no navegador..." -ForegroundColor Green
Start-Process "https://localhost:5001/swagger"
Start-Sleep -Seconds 2
Start-Process "https://localhost:5002/swagger"

Write-Host ""
Write-Host "APIs iniciadas e abertas no navegador!" -ForegroundColor Green
Write-Host "ContaCorrente: https://localhost:5001/swagger" -ForegroundColor White
Write-Host "Transferencia: https://localhost:5002/swagger" -ForegroundColor White
Write-Host ""
Read-Host "Pressione Enter para continuar"
