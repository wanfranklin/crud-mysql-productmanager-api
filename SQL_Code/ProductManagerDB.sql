CREATE DATABASE ProductManagerDB;

USE ProductManagerDB;

CREATE TABLE Products (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL
);

INSERT INTO Products (Nome, Preco) VALUES
('Produto 1', 5.00),
('Produto 2', 6.00);

SELECT * FROM Products;

UPDATE Products SET Nome = 'Sprite' WHERE (Id = 3);
UPDATE Products SET Nome = 'Guarana' WHERE (Id = 4);


GRANT ALL PRIVILEGES ON *.* TO 'root'@'localhost' WITH GRANT OPTION;
FLUSH PRIVILEGES;

ALTER USER 'root'@'localhost' IDENTIFIED BY 'admin';