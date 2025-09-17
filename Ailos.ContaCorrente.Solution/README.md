# Ailos - Sistema de Conta Corrente e Transferências

## Arquitetura de Containers

O sistema é composto por 4 containers independentes:

### 🐳 Containers
1. **ailos-sqlite** - Banco de dados SQLite
2. **ailos-kafka** - Message broker Kafka (+ Zookeeper)
3. **ailos-contacorrente-api** - API de Conta Corrente
4. **ailos-transferencia-api** - API de Transferências

### 🔐 Segurança Implementada

#### Criptografia de Senhas
- **Hash SHA256** com salt personalizado
- Senhas **nunca armazenadas em texto plano**
- Verificação segura no login

#### Autenticação JWT
- Tokens com expiração de 1 hora
- Chave secreta de 32 caracteres
- Validação em todos os endpoints protegidos

### 🚀 Como Executar

```bash
# Navegar para o diretório docker
cd docker

# Executar todos os containers
docker-compose up --build

# Executar em background
docker-compose up -d --build

# Parar todos os containers
docker-compose down

# Ver logs
docker-compose logs -f
```

### 📡 Endpoints Disponíveis

#### API Conta Corrente (http://localhost:5001)
- **POST** `/api/contacorrente/cadastrar` - Cadastrar conta
- **POST** `/api/contacorrente/login` - Login
- **POST** `/api/contacorrente/inativar` - Inativar conta (requer auth)
- **POST** `/api/contacorrente/movimentacao` - Movimentação (requer auth)
- **GET** `/api/contacorrente/saldo` - Consultar saldo (requer auth)

#### API Transferência (http://localhost:5002)
- **POST** `/api/transferencia` - Efetuar transferência (requer auth)

#### Swagger UI
- ContaCorrente: http://localhost:5001/swagger
- Transferência: http://localhost:5002/swagger

### 🗄️ Banco de Dados

**SQLite** compartilhado entre as APIs via volume Docker:
- Localização: `/data/ailos.db`
- **Dados de teste** (senhas originais para teste):

| CPF | Senha Original | Nome |
|-----|---------------|------|
| 12345678901 | 123456 | João Silva |
| 98765432100 | 654321 | Maria Santos |
| 11122233344 | senha123 | Pedro Oliveira |

### 📨 Kafka

**Broker Kafka** disponível em:
- Externo: `localhost:9092`
- Interno: `kafka:29092`

### 🧪 Teste Rápido

```bash
# 1. Fazer login (senha original: 123456)
curl -X POST http://localhost:5001/api/contacorrente/login \
  -H "Content-Type: application/json" \
  -d '{"contaOuCpf":"12345678901","senha":"123456"}'

# 2. Usar o token retornado para consultar saldo
curl -X GET http://localhost:5001/api/contacorrente/saldo \
  -H "Authorization: Bearer {TOKEN}"

# 3. Fazer uma movimentação (crédito)
curl -X POST http://localhost:5001/api/contacorrente/movimentacao \
  -H "Authorization: Bearer {TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{"idRequisicao":"MOV001","valor":100.00,"tipo":"C"}'

# 4. Fazer uma transferência
curl -X POST http://localhost:5002/api/transferencia \
  -H "Authorization: Bearer {TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{"idRequisicao":"TRANS001","contaDestinoId":2,"valor":50.00}'

# 5. Inativar conta
curl -X POST http://localhost:5001/api/contacorrente/inativar \
  -H "Authorization: Bearer {TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{"senha":"123456"}'
```

### 📊 Monitoramento

```bash
# Ver status dos containers
docker-compose ps

# Ver logs específicos
docker-compose logs ailos-contacorrente-api
docker-compose logs ailos-transferencia-api
docker-compose logs ailos-kafka

# Acessar container SQLite
docker exec -it ailos-sqlite sh
sqlite3 /data/ailos.db
```

### 🔧 Funcionalidades Implementadas

#### ✅ API Conta Corrente:
- **Cadastrar**: CPF + senha → número da conta
- **Login**: CPF/conta + senha → token JWT
- **Inativar**: senha + token → inativa conta
- **Movimentar**: débito/crédito com validações
- **Saldo**: consulta com cálculo em tempo real

#### ✅ API Transferência:
- **Transferir**: débito origem + crédito destino
- **Estorno**: automático em caso de falha
- **Validações**: contas ativas, valores positivos

#### ✅ Validações de Negócio:
- **INVALID_DOCUMENT**: CPF inválido
- **USER_UNAUTHORIZED**: Credenciais incorretas
- **INVALID_ACCOUNT**: Conta não encontrada
- **INACTIVE_ACCOUNT**: Conta inativa
- **INVALID_VALUE**: Valor não positivo
- **INVALID_TYPE**: Tipo de movimento inválido

### 🏗️ Estrutura Técnica

- **.NET 8** com Entity Framework Core
- **SQLite** como banco de dados
- **SHA256 + Salt** para hash de senhas
- **JWT** para autenticação
- **Kafka** para messaging
- **Docker Compose** para orquestração
- **Clean Architecture** com CQRS
- **Estorno automático** em transferências

### ⚠️ Importante para Produção

- Alterar chaves JWT e salt de senha
- Usar HTTPS em produção
- Implementar rate limiting
- Configurar logs estruturados
- Usar banco de dados dedicado (PostgreSQL/SQL Server)

O sistema está **100% completo** conforme especificações!
