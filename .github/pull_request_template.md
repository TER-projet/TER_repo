## Ticket
<!-- ID ClickUp, ex. : PV-01 -->

## Ce qui change
<!-- 2 à 3 phrases : qu'avez-vous fait et pourquoi ? -->

## Comment tester
<!-- Étapes précises, ex. : requête POST /api/points-de-vente avec ce JSON -> 201 attendu -->

## Checklist (à cocher avant de demander la relecture)
- [ ] La branche est à jour avec `main`
- [ ] `dotnet build` passe sans erreur
- [ ] `dotnet test` passe
- [ ] J'ai testé les endpoints avec Postman (cas de succès ET cas d'erreur)
- [ ] `docker compose up --build` fonctionne
- [ ] Aucun secret, `.env`, `bin/` ou `obj/` dans le commit
- [ ] Si j'ai créé une migration : elle est incluse et `dotnet ef database update` fonctionne
- [ ] Les règles d'architecture du README sont respectées (contrôleur fin, DTO, validateur, service)