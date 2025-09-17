CREATE TABLE IF NOT EXISTS CONTACORRENTE (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Cpf TEXT NOT NULL,
    Senha TEXT NOT NULL,
    Nome TEXT NOT NULL,
    Ativo INTEGER NOT NULL DEFAULT 1,
    DataCriacao TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE TABLE IF NOT EXISTS MOVIMENTO (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdRequisicao TEXT NOT NULL,
    ContaCorrenteId INTEGER NOT NULL,
    Valor REAL NOT NULL,
    Tipo TEXT NOT NULL CHECK (Tipo IN ('C', 'D')),
    DataMovimento TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (ContaCorrenteId) REFERENCES CONTACORRENTE(Id)
);

CREATE TABLE IF NOT EXISTS TRANSFERENCIA (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdRequisicao TEXT NOT NULL,
    ContaOrigemId INTEGER NOT NULL,
    ContaDestinoId INTEGER NOT NULL,
    Valor REAL NOT NULL,
    DataTransferencia TEXT NOT NULL DEFAULT (datetime('now'))
);

-- Senhas hasheadas com SHA256 + salt "ailos_salt"
-- Senha original: "123456" -> Hash: "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI="
-- Senha original: "654321" -> Hash: "Xohimnoiu3luduF2RUWlBo/TlHsqsLuS1jacNGOyUGg="
-- Senha original: "senha123" -> Hash: "9jqo3MFNF4BVDbqQdSlC9WyFhMY5upfF8eMHghpFBSY="

INSERT OR IGNORE INTO CONTACORRENTE (Id, Cpf, Senha, Nome, Ativo) VALUES 
(1, '12345678901', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 'João Silva', 1),
(2, '98765432100', 'Xohimnoiu3luduF2RUWlBo/TlHsqsLuS1jacNGOyUGg=', 'Maria Santos', 1),
(3, '11122233344', '9jqo3MFNF4BVDbqQdSlC9WyFhMY5upfF8eMHghpFBSY=', 'Pedro Oliveira', 1);
