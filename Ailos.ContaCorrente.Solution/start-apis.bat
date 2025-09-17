@echo off
echo Iniciando APIs do Ailos...

echo Iniciando API ContaCorrente na porta 5001...
start "ContaCorrente API" cmd /k "cd /d src\APIs\Ailos.ContaCorrente.API && dotnet run --urls=https://localhost:5001;http://localhost:5000"

timeout /t 5 /nobreak > nul

echo Iniciando API Transferencia na porta 5002...
start "Transferencia API" cmd /k "cd /d src\APIs\Ailos.Transferencia.API && dotnet run --urls=https://localhost:5002;http://localhost:5003"

echo Aguardando APIs iniciarem...
timeout /t 10 /nobreak > nul

echo Abrindo Swagger das APIs no navegador...
start https://localhost:5001/swagger
timeout /t 2 /nobreak > nul
start https://localhost:5002/swagger

echo.
echo APIs iniciadas e abertas no navegador!
echo ContaCorrente: https://localhost:5001/swagger
echo Transferencia: https://localhost:5002/swagger
echo.
pause
