-- ============================================
-- Script de criação e popularização do banco
-- ProductManagerDB
-- ============================================

CREATE DATABASE IF NOT EXISTS ProductManagerDB;

USE ProductManagerDB;

-- Criar tabela
CREATE TABLE IF NOT EXISTS Products (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL
);

-- Limpar dados existentes
TRUNCATE TABLE Products;

-- Inserir produtos de exemplo (diversos tipos e faixas de preço)
INSERT INTO Products (Nome, Preco) VALUES
-- Eletrônicos
('Notebook Dell Inspiron 15', 4599.90),
('Mouse Logitech MX Master 3S', 349.90),
('Teclado Mecânico Keychron K2', 499.90),
('Monitor LG UltraWide 29"', 1899.90),
('Webcam Logitech C920', 299.90),

-- Acessórios
('Fone de Ouvido JBL Tune 510BT', 199.90),
('Capa para iPhone 15 Pro', 89.90),
('Carregador USB-C 65W Anker', 159.90),

-- Papelaria
('Caderno Tilibra 10 Matérias', 29.90),
('Caneta Bic Cristal (Caixa c/ 20)', 18.90),
('Lápis de Cor Faber-Castell (12 cores)', 34.90),

-- Alimentos
('Café Pilão Torrado e Moído 500g', 16.90),
('Azeite Extra Virgem Gallo 500ml', 39.90),
('Chocolate Garoto Ao Leite 90g', 8.90),
('Refrigerante Coca-Cola 2L', 9.90),

-- Casa
('Lâmpada LED Bulb Philco 9W', 12.90),
('Extensão 6 Tomadas Prolam 3m', 29.90),
('Filtro de Água Brita Classic', 89.90),

-- Esportes
('Bola de Futebol Penalty Society', 79.90),
('Garrafa Térmica Lily 500ml', 69.90);

-- Verificar dados inseridos
SELECT Id, Nome, FORMAT(Preco, 2) AS Preco FROM Products ORDER BY Id;
SELECT COUNT(*) AS TotalProdutos FROM Products;
