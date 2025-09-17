#!/bin/bash

echo "Criando usuários de teste..."

# Usuário 1
curl -X POST http://localhost:5001/api/contacorrente/cadastrar \
  -H "Content-Type: application/json" \
  -d '{"cpf":"12345678901","senha":"123456","nome":"João Silva"}'

# Usuário 2  
curl -X POST http://localhost:5001/api/contacorrente/cadastrar \
  -H "Content-Type: application/json" \
  -d '{"cpf":"98765432100","senha":"654321","nome":"Maria Santos"}'

# Usuário 3
curl -X POST http://localhost:5001/api/contacorrente/cadastrar \
  -H "Content-Type: application/json" \
  -d '{"cpf":"11122233344","senha":"senha123","nome":"Pedro Costa"}'

echo "Usuários criados!"
