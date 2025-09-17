# Clean Architecture - Ailos

## Visão Geral

O projeto Ailos segue os princípios da Clean Architecture, organizando o código em camadas bem definidas:

## Camadas

### 1. Domain (Domínio)
- **Entities**: ContaCorrente, Movimento, Transferencia
- **Enums**: TipoMovimento, TipoFalha
- **Interfaces**: Contratos dos repositórios
- **Exceptions**: Exceções de negócio
- **Services**: Lógica de domínio

### 2. Application (Aplicação)
- **Commands**: CadastrarContaCorrente, MovimentarContaCorrente, EfetuarTransferencia
- **Queries**: ObterSaldoContaCorrente, ValidarLogin
- **Handlers**: Implementação dos comandos e queries
- **DTOs**: Objetos de transferência de dados
- **Validators**: Validações de entrada

### 3. Infrastructure (Infraestrutura)
- **Data**: Repositórios e DbContext
- **Security**: Serviços de JWT
- **Cache**: Implementações de cache
- **Messaging**: Integração com Kafka
- **External**: Serviços externos

### 4. API (Apresentação)
- **Controllers**: Endpoints REST
- **Middleware**: Interceptadores
- **Configuration**: Configurações da aplicação

## Benefícios

- **Testabilidade**: Cada camada pode ser testada independentemente
- **Manutenibilidade**: Separação clara de responsabilidades
- **Flexibilidade**: Fácil troca de implementações
- **Escalabilidade**: Arquitetura preparada para crescimento
