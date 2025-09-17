#!/bin/bash
echo "Testando API Transferência..."

cd "/mnt/c/Users/crist/Desktop/AliosApiTest/Ailos.ContaCorrente.Solution/src/APIs/Ailos.Transferencia.API"

# Verificar se dotnet está disponível
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET não está instalado ou não está no PATH"
    exit 1
fi

# Tentar compilar
echo "Compilando projeto..."
dotnet build --no-restore > build.log 2>&1

if [ $? -eq 0 ]; then
    echo "✅ Compilação bem-sucedida"
    echo "Iniciando API na porta 5002..."
    dotnet run --urls="https://localhost:5002;http://localhost:5003" &
    API_PID=$!
    
    # Aguardar alguns segundos
    sleep 10
    
    # Testar se está respondendo
    if curl -k -s https://localhost:5002/swagger > /dev/null 2>&1; then
        echo "✅ API Transferência está respondendo na porta 5002"
        kill $API_PID
    else
        echo "❌ API Transferência não está respondendo"
        kill $API_PID 2>/dev/null
    fi
else
    echo "❌ Erro na compilação:"
    cat build.log
fi
