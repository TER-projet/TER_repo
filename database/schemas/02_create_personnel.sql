CREATE TABLE personnel (
    id_personnel INT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    num_tel VARCHAR(15) UNIQUE NOT NULL,
    fonction VARCHAR(50) NOT NULL
);