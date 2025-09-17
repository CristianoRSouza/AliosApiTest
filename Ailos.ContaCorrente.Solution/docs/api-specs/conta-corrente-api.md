# API Conta Corrente - Especificação

## Endpoints

### POST /api/contacorrente/cadastrar
Cadastra uma nova conta corrente.

**Request:**
```json
{
  "cpf": "12345678901",
  "senha": "123456",
  "nome": "João Silva"
}
```

**Response 200:**
```json
{
  "numeroConta": 1
}
```

**Response 400:**
```json
{
  "mensagem": "CPF inválido",
  "tipo": "INVALID_DOCUMENT"
}
```

### POST /api/contacorrente/login
Efetua login e retorna token JWT.

**Request:**
```json
{
  "contaOuCpf": "12345678901",
  "senha": "123456"
}
```

**Response 200:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### POST /api/contacorrente/movimentacao
Realiza movimentação na conta (requer autenticação).

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "idRequisicao": "REQ001",
  "contaCorrenteId": 1,
  "valor": 100.00,
  "tipo": "C"
}
```

**Response:** 204 No Content

### GET /api/contacorrente/saldo
Consulta saldo da conta (requer autenticação).

**Headers:**
```
Authorization: Bearer {token}
```

**Response 200:**
```json
{
  "numeroConta": 1,
  "nomeTitular": "João Silva",
  "dataHoraConsulta": "2024-01-15T10:30:00Z",
  "saldo": 150.00
}
```

## Códigos de Erro

- **INVALID_DOCUMENT**: CPF inválido
- **USER_UNAUTHORIZED**: Credenciais incorretas
- **INVALID_ACCOUNT**: Conta não encontrada
- **INACTIVE_ACCOUNT**: Conta inativa
- **INVALID_VALUE**: Valor inválido
- **INVALID_TYPE**: Tipo de movimento inválido
