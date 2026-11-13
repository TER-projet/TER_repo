CREATE TABLE stocker (
    quantite_stock INT NOT NULL,
    id_point_vente INT NOT NULL, -- clé étrangère
    id_produit INT NOT NULL, -- clé étrangère

    PRIMARY KEY (id_point_vente, id_produit),

    FOREIGN KEY (id_point_vente) REFERENCES point_de_vente(id_point_vente),
    FOREIGN KEY (id_produit) REFERENCES produit(id_produit)
);