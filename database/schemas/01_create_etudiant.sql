CREATE TABLE etudiant (
    id_etudiant INT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    num_tel VARCHAR(15) UNIQUE NOT NULL,
    est_boursier BOOLEAN NOT NULL,
    classe VARCHAR(50) NOT NULL
);