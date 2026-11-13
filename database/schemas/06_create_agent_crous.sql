CREATE TABLE agent_crous (
    id_agent INT PRIMARY KEY,
    nom VARCHAR(50) NOT NULL,
    prenom VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    num_tel VARCHAR(15) UNIQUE NOT NULL,
    user_role VARCHAR(50) NOT NULL

    id_point_vente INT NOT NULL, -- clé étrangère
    FOREIGN KEY (id_point_vente) REFERENCES point_de_vente(id_point_vente)
);