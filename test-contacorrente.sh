#!/bin/bash
echo "Testando API ContaCorrente..."

cd "/mnt/c/Users/crist/Desktop/AliosApiTest/Ailos.ContaCorrente.Solution/src/APIs/Ailos.ContaCorrente.API"

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
    echo "Iniciando API na porta 60849..."
    dotnet run --urls="https://localhost:60849;http://localhost:60850" &
    API_PID=$!
    
    # Aguardar alguns segundos
    sleep 10
    
    # Testar se está respondendo
    if curl -k -s https://localhost:60849/swagger > /dev/null 2>&1; then
        echo "✅ API ContaCorrente está respondendo na porta 60849"
        kill $API_PID
    else
        echo "❌ API ContaCorrente não está respondendo"
        kill $API_PID 2>/dev/null
    fi
else
    echo "❌ Erro na compilação:"
    cat build.log
fi
