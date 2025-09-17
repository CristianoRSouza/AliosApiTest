# API Transferência - Especificação

## Endpoints

### POST /api/transferencia
Efetua transferência entre contas (requer autenticação).

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "idRequisicao": "TRANS001",
  "contaDestinoId": 2,
  "valor": 50.00
}
```

**Response:** 204 No Content

**Response 400:**
```json
{
  "mensagem": "Conta destino não encontrada",
  "tipo": "INVALID_ACCOUNT"
}
```

## Fluxo de Transferência

1. **Validação**: Verifica se a conta origem está ativa e tem saldo
2. **Débito**: Realiza débito na conta origem via API ContaCorrente
3. **Crédito**: Realiza crédito na conta destino via API ContaCorrente
4. **Estorno**: Em caso de falha no crédito, realiza estorno automático
5. **Persistência**: Salva o registro da transferência

## Integração

A API de Transferência integra com a API ContaCorrente através de:
- **HTTP Client**: Chamadas REST internas
- **JWT Token**: Repasse do token de autenticação
- **Compensação**: Estorno automático em caso de falha

## Códigos de Erro

Herda todos os códigos da API ContaCorrente:
- **INVALID_ACCOUNT**: Conta não encontrada
- **INACTIVE_ACCOUNT**: Conta inativa
- **INVALID_VALUE**: Valor inválido
