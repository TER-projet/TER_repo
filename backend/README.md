# Backend : API ASP.NET Core (.NET 10)

> **À lire en entier avant d'écrire la moindre ligne de code.** Ce document explique comment lancer le backend, comment le code est organisé, et comment travailler avec Git/GitHub sans se marcher dessus. Questions : demandez au responsable backend.

## Sommaire

1. [Prérequis](#1-prérequis)
2. [Démarrage rapide](#2-démarrage-rapide)
3. [Travailler au quotidien en local](#3-travailler-au-quotidien-en-local)
4. [Architecture des dossiers](#4-architecture-des-dossiers)
5. [Le parcours d'une requête](#5-le-parcours-dune-requête)
6. [Ajouter une fonctionnalité pas à pas](#6-ajouter-une-fonctionnalité-pas-à-pas)
7. [Conventions de code](#7-conventions-de-code)
8. [Base de données et migrations](#8-base-de-données-et-migrations)
9. [Tests](#9-tests)
10. [Workflow GitHub (un ticket = une branche = une PR)](#10-workflow-github)
11. [Relecture et fusion (pour le responsable backend)](#11-relecture-et-fusion)
12. [Dépannage](#12-dépannage)

---

## 1. Prérequis

| Outil | Version | Vérification |
|---|---|---|
| Git | récente | `git --version` |
| SDK .NET | **10** | `dotnet --version` |
| Docker Desktop | récente | `docker --version` |
| VS Code + extension **C# Dev Kit** | récente | |
| Postman | récente | pour tester l'API |
| Outil EF Core | | `dotnet tool install --global dotnet-ef` |

## 2. Démarrage rapide

Depuis un terminal (PowerShell) :

```powershell
git clone <url-du-repo> TER_repo
cd TER_repo

# 1. Créer votre fichier de configuration locale (jamais committé)
copy .env.example .env
# puis ouvrez .env et renseignez DB_PASSWORD

# 2. Lancer la base de données + l'API
docker compose up --build
```

Ouvrez `http://localhost:8080/health`. Vous devez voir :

```json
{"status":"healthy","database":"up"}
```

Si c'est le cas, tout fonctionne. Sinon, voir la section [Dépannage](#12-dépannage).

### Variables du fichier `.env`

| Variable | Rôle | Défaut |
|---|---|---|
| `DB_PASSWORD` | mot de passe PostgreSQL (**obligatoire**) | aucun |
| `DB_PORT` | port de la base **sur votre machine** | `5432` |
| `API_PORT` | port de l'API **sur votre machine** | `8080` |

> Le fichier `.env` est personnel : il est dans `.gitignore`. Ne le committez jamais, et ne mettez jamais de mot de passe dans le code ou dans `appsettings.json`.

## 3. Travailler au quotidien en local

Reconstruire l'image Docker à chaque modification est lent. En développement, on lance **seulement la base** dans Docker et l'API directement avec .NET (rechargement automatique) :

```powershell
# Terminal 1, à la racine du repo : la base de données
docker compose up db

# Terminal 2, dans backend/ : l'API avec rechargement à chaud
dotnet watch --project src/Ter.Api
```

**Configuration à faire une seule fois** : donner à l'API locale la chaîne de connexion (elle est stockée hors du repo, dans les « user-secrets ») :

```powershell
cd backend/src/Ter.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=ter_db;Username=ter_user;Password=VOTRE_DB_PASSWORD"
```

Remplacez `5432` par votre `DB_PORT` si vous l'avez modifié, et le mot de passe par celui de votre `.env`. Le port de l'API s'affiche dans la console au démarrage.

**Déboguer dans VS Code** : ouvrez le dossier `backend/`, posez un point d'arrêt (clic dans la marge), puis `F5`.

| Commande (depuis `backend/`) | Effet |
|---|---|
| `dotnet build` | compile |
| `dotnet watch --project src/Ter.Api` | lance l'API avec rechargement |
| `dotnet test` | exécute les tests |
| `docker compose up --build` (racine) | base + API dans Docker |
| `docker compose down` (racine) | arrête tout |
| `docker compose down -v` (racine) | arrête tout **et supprime les données** |

## 4. Architecture des dossiers

Le backend est **un seul projet** organisé en couches par dossiers. Chaque dossier a un rôle précis : respectez-le.

```
backend/
├── README.md                    ← ce fichier
├── Dockerfile
├── Ter.slnx                     ← solution (Api + Tests)
├── src/
│   └── Ter.Api/
│       ├── Controllers/         ← points d'entrée HTTP (routes, codes de réponse)
│       ├── Models/
│       │   ├── Entities/        ← classes qui correspondent aux TABLES de la base
│       │   └── Dtos/            ← objets échangés avec le client (requêtes / réponses)
│       ├── Services/            ← LOGIQUE MÉTIER et accès à la base
│       ├── Validators/          ← règles de validation des requêtes (FluentValidation)
│       ├── Data/
│       │   ├── AppDbContext.cs  ← connexion à la base (EF Core)
│       │   ├── Configurations/  ← description des tables (colonnes, index, clés)
│       │   └── Migrations/      ← historique du schéma (généré par dotnet ef)
│       ├── Filters/             ← code transversal (validation automatique)
│       ├── Extensions/          ← enregistrement automatique des services
│       └── Program.cs           ← configuration et démarrage de l'application
└── tests/
    └── Ter.Tests/               ← tests unitaires (mêmes dossiers que Ter.Api)
```

### Qui fait quoi ?

| Dossier | Contient | Ne doit PAS contenir |
|---|---|---|
| `Controllers/` | routes, appel du service, choix du code HTTP (200, 201, 404…) | logique métier, accès à la base |
| `Models/Entities/` | classes des tables | logique, validation HTTP |
| `Models/Dtos/` | `record` de requêtes et de réponses | entités de la base |
| `Services/` | règles métier, requêtes EF Core, transformation entité → DTO | notions HTTP (`Ok()`, `NotFound()`…) |
| `Validators/` | règles de format (obligatoire, longueur, > 0…) | accès à la base |
| `Data/Configurations/` | une classe par entité (noms de table, tailles, index) | code métier |

### Règles d'or

1. **Un contrôleur est « mince »** : il reçoit la requête, appelle un service, renvoie un code HTTP. Rien d'autre.
2. **On ne renvoie jamais une entité** depuis l'API : toujours un DTO. Ainsi, on contrôle ce qui sort (pas de mot de passe haché, pas de champs internes).
3. **Toute la logique métier et tout l'accès à la base sont dans les services.** Un contrôleur n'utilise jamais `AppDbContext` (sauf `HealthController`).
4. **Toute requête qui modifie des données a un validateur** dans `Validators/`.
5. **Tout est asynchrone** (`async`/`await`) et reçoit un `CancellationToken`.
6. **Rangez vos fichiers par dossier technique, avec des sous-dossiers par fonctionnalité si besoin** (`Dtos/Articles/`, `Services/Articles/`).

## 5. Le parcours d'une requête

```
Client (Postman, frontend)
   │  POST /api/articles  { "nom": "Tarte", "prix": 2.2 }
   ▼
Controller        reçoit le JSON, le transforme en CreateArticleRequest
   ▼
ValidationFilter  cherche un validateur pour ce type → CreateArticleRequestValidator
   │              règles non respectées ? → réponse 400 immédiate (le contrôleur n'est pas appelé)
   ▼
Controller        appelle IArticleService
   ▼
Service           applique la logique métier, utilise AppDbContext (EF Core)
   ▼
PostgreSQL        lit / écrit les données
   ▼
Controller        transforme le résultat en code HTTP (201, 404, 409…) et répond
```

## 6. Ajouter une fonctionnalité pas à pas

Exemple **illustratif** : créer un article (`POST /api/articles`). Suivez le même enchaînement pour votre ticket (remplacez `Article` par votre entité).

### Étape 1 : l'entité (`Models/Entities/Article.cs`)

```csharp
namespace Ter.Api.Models.Entities;

public class Article
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nom { get; set; } = string.Empty;
    public decimal Prix { get; set; }
}
```

### Étape 2 : les DTO (`Models/Dtos/Articles/ArticleDtos.cs`)

```csharp
namespace Ter.Api.Models.Dtos.Articles;

public record CreateArticleRequest(string Nom, decimal Prix);

public record ArticleResponse(Guid Id, string Nom, decimal Prix);
```

### Étape 3 : le validateur (`Validators/CreateArticleRequestValidator.cs`)

```csharp
using FluentValidation;
using Ter.Api.Models.Dtos.Articles;

namespace Ter.Api.Validators;

public class CreateArticleRequestValidator : AbstractValidator<CreateArticleRequest>
{
    public CreateArticleRequestValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Prix).GreaterThan(0);
    }
}
```

Rien à enregistrer : le validateur est trouvé et appliqué automatiquement.

### Étape 4 : la configuration de la table (`Data/Configurations/ArticleConfiguration.cs`)

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Nom).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Prix).HasPrecision(10, 2);
        builder.HasIndex(a => a.Nom).IsUnique();
    }
}
```

Puis ajoutez **une ligne** dans `Data/AppDbContext.cs` :

```csharp
public DbSet<Article> Articles => Set<Article>();
```

### Étape 5 : le service (`Services/IArticleService.cs` et `Services/ArticleService.cs`)

```csharp
using Ter.Api.Models.Dtos.Articles;

namespace Ter.Api.Services;

public interface IArticleService
{
    Task<ArticleResponse?> ObtenirAsync(Guid id, CancellationToken ct);
    Task<bool> NomExisteAsync(string nom, CancellationToken ct);
    Task<ArticleResponse> CreerAsync(CreateArticleRequest request, CancellationToken ct);
}
```

```csharp
using Microsoft.EntityFrameworkCore;
using Ter.Api.Data;
using Ter.Api.Models.Dtos.Articles;
using Ter.Api.Models.Entities;

namespace Ter.Api.Services;

public class ArticleService(AppDbContext db) : IArticleService
{
    public async Task<ArticleResponse?> ObtenirAsync(Guid id, CancellationToken ct) =>
        await db.Articles
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new ArticleResponse(a.Id, a.Nom, a.Prix))
            .FirstOrDefaultAsync(ct);

    public async Task<bool> NomExisteAsync(string nom, CancellationToken ct) =>
        await db.Articles.AnyAsync(a => a.Nom == nom, ct);

    public async Task<ArticleResponse> CreerAsync(CreateArticleRequest request, CancellationToken ct)
    {
        var article = new Article { Nom = request.Nom, Prix = request.Prix };
        db.Articles.Add(article);
        await db.SaveChangesAsync(ct);
        return new ArticleResponse(article.Id, article.Nom, article.Prix);
    }
}
```

**Convention de nommage importante** : la classe s'appelle `XxxService` et son interface `IXxxService`. Elles sont alors enregistrées **automatiquement** (pas de modification de `Program.cs`).

### Étape 6 : le contrôleur (`Controllers/ArticlesController.cs`)

```csharp
using Microsoft.AspNetCore.Mvc;
using Ter.Api.Models.Dtos.Articles;
using Ter.Api.Services;

namespace Ter.Api.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController(IArticleService articleService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ArticleResponse>> Obtenir(Guid id, CancellationToken ct)
    {
        var article = await articleService.ObtenirAsync(id, ct);
        return article is null ? NotFound() : Ok(article);
    }

    [HttpPost]
    public async Task<ActionResult<ArticleResponse>> Creer(CreateArticleRequest request, CancellationToken ct)
    {
        if (await articleService.NomExisteAsync(request.Nom, ct))
            return Conflict(new { message = $"L'article '{request.Nom}' existe déjà." });

        var cree = await articleService.CreerAsync(request, ct);
        return CreatedAtAction(nameof(Obtenir), new { id = cree.Id }, cree);
    }
}
```

### Étape 7 : la migration, le test, la PR

1. Créez la migration (voir [section 8](#8-base-de-données-et-migrations)) et appliquez-la.
2. Testez avec Postman : le cas de succès **et** les cas d'erreur (champ vide, doublon, id inconnu).
3. Suivez le [workflow GitHub](#10-workflow-github).

## 7. Conventions de code

### Nommage

| Élément | Convention | Exemple |
|---|---|---|
| Classes, méthodes, propriétés | `PascalCase` | `PointDeVente`, `CreerAsync` |
| Variables locales, paramètres | `camelCase` | `nomArticle` |
| Interfaces | préfixe `I` | `IArticleService` |
| Méthodes asynchrones | suffixe `Async` | `ObtenirAsync` |
| Vocabulaire métier | **en français** | `Article`, `Reservation`, `Commande` |
| Suffixes techniques | en anglais | `...Controller`, `...Service`, `...Request`, `...Response` |
| Tables SQL | minuscules, pluriel, `snake_case` | `points_de_vente` |

**Verbes standards des services** (pour que tout le monde parle le même langage) : `ListerAsync`, `ObtenirAsync`, `CreerAsync`, `ModifierAsync`, `SupprimerAsync`.

### API REST

- Routes : `api/<ressource-au-pluriel>` en minuscules avec tirets : `api/points-de-vente`.
- JSON en `camelCase` (automatique).

| Opération | Méthode | Réponse de succès |
|---|---|---|
| Lister | `GET /api/articles` | 200 |
| Lire un élément | `GET /api/articles/{id}` | 200 |
| Créer | `POST /api/articles` | 201 + en-tête `Location` |
| Modifier | `PUT /api/articles/{id}` | 200 |
| Supprimer | `DELETE /api/articles/{id}` | 204 |

| Erreur | Code |
|---|---|
| Données invalides | 400 (automatique via le validateur) |
| Non connecté | 401 |
| Rôle insuffisant | 403 |
| Ressource introuvable | 404 |
| Conflit (doublon, état incompatible) | 409 |

### Qualité

- Pas de code commenté laissé « au cas où » ; pas de `Console.WriteLine` de débogage.
- Les avertissements du compilateur sur la nullabilité (`string?`) se corrigent, ils ne se masquent pas (pas de `!` pour les faire taire).
- `decimal` pour l'argent, `Guid` pour les identifiants.
- Un commentaire explique le **pourquoi**, pas le **quoi**.

## 8. Base de données et migrations

Une **migration** est un fichier C# qui décrit un changement du schéma de la base. L'historique des migrations est versionné dans Git (`Data/Migrations/`).

**Prérequis** : la base tourne (`docker compose up db`) et les user-secrets sont configurés (section 3).

Depuis `backend/` :

```powershell
# Créer une migration après avoir modifié une entité ou une configuration
dotnet ef migrations add NomDeLaMigration --project src/Ter.Api --output-dir Data/Migrations

# L'appliquer à votre base locale
dotnet ef database update --project src/Ter.Api

# Annuler la dernière migration (si elle n'a PAS encore été poussée)
dotnet ef migrations remove --project src/Ter.Api
```

Donnez un nom explicite : `AjoutTableArticles`, `AjoutPrixSurArticle`.

**Vérifier le résultat dans la base** :

```powershell
docker compose exec db psql -U ter_user -d ter_db
# puis : \dt  (liste les tables), \q (quitter)
```

### Règles d'équipe pour les migrations

1. **Avant de créer une migration, faites `git pull origin main`** pour partir de la dernière version du schéma.
2. **Ne modifiez jamais une migration déjà fusionnée dans `main`** : créez-en une nouvelle.
3. **Committez toujours** les fichiers de migration (et le fichier `AppDbContextModelSnapshot.cs`) avec votre code.
4. Si deux personnes créent une migration en parallèle, la deuxième PR aura un conflit sur le snapshot. Solution :
   ```powershell
   dotnet ef migrations remove --project src/Ter.Api   # supprime VOTRE migration
   git merge origin/main                               # récupère celle de l'autre
   dotnet ef migrations add NomDeLaMigration --project src/Ter.Api --output-dir Data/Migrations
   ```
5. Si votre base locale devient incohérente : `docker compose down -v` puis `docker compose up db` et `dotnet ef database update` (**supprime vos données locales**).

## 9. Tests

```powershell
dotnet test
```

Les tests sont dans `tests/Ter.Tests/` avec les mêmes sous-dossiers que `Ter.Api` (`Validators/`, `Services/`…). Exemple de test de validateur :

```csharp
using Ter.Api.Models.Dtos.Articles;
using Ter.Api.Validators;
using Xunit;

namespace Ter.Tests.Validators;

public class CreateArticleRequestValidatorTests
{
    private readonly CreateArticleRequestValidator _validator = new();

    [Fact]
    public void Nom_vide_est_refuse()
    {
        var resultat = _validator.Validate(new CreateArticleRequest("", 4.5m));
        Assert.False(resultat.IsValid);
    }

    [Fact]
    public void Requete_correcte_est_acceptee()
    {
        var resultat = _validator.Validate(new CreateArticleRequest("Tarte", 2.2m));
        Assert.True(resultat.IsValid);
    }
}
```

**Minimum attendu par ticket** : un test par règle de validation et par règle métier importante, plus un test manuel Postman de chaque endpoint (succès et erreurs).

## 10. Workflow GitHub

> **Règle absolue : personne ne pousse directement sur `main`.** Tout passe par une Pull Request relue et validée par le responsable backend.

**Un ticket ClickUp = une branche = une Pull Request.**

### Vue d'ensemble

```
main ──●─────────────────────────────●── (fusion par le responsable)
        \                           /
         feature/PV-01-creer-pv ──●──●──●  (votre travail, puis Pull Request)
```

### Pas à pas

**1. Prenez un ticket** dans ClickUp, passez-le en « En cours » et assignez-le-vous.

**2. Partez d'un `main` à jour** :

```powershell
git checkout main
git pull origin main
```

**3. Créez votre branche** (une par ticket) :

```powershell
git checkout -b feature/PV-01-creer-point-de-vente
```

Nom de branche : `<type>/<ID-ticket>-<description-courte-en-minuscules>`

| Type | Quand l'utiliser |
|---|---|
| `feature/` | nouvelle fonctionnalité |
| `fix/` | correction de bug |
| `chore/` | configuration, outillage, refactoring |
| `docs/` | documentation seule |

**4. Développez en faisant de petits commits réguliers** (plusieurs par jour, pas un énorme à la fin) :

```powershell
git add .
git commit -m "feat(PV-01): ajouter l'entité PointDeVente"
```

Format du message : `type(ID-ticket): description à l'infinitif ou au présent`. Types : `feat`, `fix`, `chore`, `docs`, `test`, `refactor`.

Avant chaque `git add .`, vérifiez avec `git status` que vous ne committez ni `.env`, ni `bin/`, ni `obj/`.

**5. Testez avant de pousser.** Tout doit être vert :

- [ ] `dotnet build` : aucune erreur
- [ ] `dotnet test` : tous les tests passent
- [ ] `docker compose up --build` : l'API démarre et `/health` répond `healthy`
- [ ] Postman : vos endpoints fonctionnent (succès **et** cas d'erreur)

**6. Mettez votre branche à jour avec `main`** (d'autres PR ont peut-être été fusionnées) :

```powershell
git fetch origin
git merge origin/main
```

En cas de **conflit**, VS Code les affiche : choisissez « Accept Both Changes » quand les deux versions sont utiles, retirez les marqueurs `<<<<<<<`, `=======`, `>>>>>>>`, puis :

```powershell
git add .
git commit
```

Relancez ensuite les tests (étape 5). Si vous hésitez devant un conflit, demandez de l'aide plutôt que de deviner.

**7. Poussez votre branche** :

```powershell
git push -u origin feature/PV-01-creer-point-de-vente
```

**8. Ouvrez la Pull Request** sur GitHub :

- Cliquez sur **Compare & pull request** (bandeau jaune sur la page du repo).
- Base : `main`, comparaison : votre branche.
- Titre : `feat(PV-01): créer un point de vente`.
- Remplissez le modèle : ticket, ce qui change, comment tester, checklist cochée.
- **Reviewers** : sélectionnez le responsable backend.
- Passez le ticket ClickUp en « En revue » et collez le lien de la PR.

**9. Attendez la relecture. Ne fusionnez pas vous-même.**

- Des commentaires ? Corrigez **sur la même branche** (`git add`, `git commit`, `git push`) : la PR se met à jour toute seule. Répondez aux commentaires, puis redemandez une relecture.
- Une fois approuvée, le responsable fusionne la PR.

**10. Nettoyez après la fusion** :

```powershell
git checkout main
git pull origin main
git branch -d feature/PV-01-creer-point-de-vente
```

Passez le ticket ClickUp en « Terminé ».

### Bonnes pratiques

- **Petites PR** : visez moins de 400 lignes modifiées. Une PR énorme est relue superficiellement.
- **Ne touchez qu'à ce que votre ticket demande.** Pas de « petit nettoyage » dans les fichiers des autres.
- **Une migration = une PR** : ne mélangez pas plusieurs changements de schéma.
- **Synchronisez-vous souvent** avec `main` (étape 6), au moins chaque matin.
- **Bloqué plus de 30 minutes ?** Demandez de l'aide dans le canal de l'équipe, avec le message d'erreur complet.
- **Un secret a été poussé par erreur ?** Prévenez immédiatement le responsable : il faut changer le mot de passe, car supprimer le fichier ne l'efface pas de l'historique Git.

### Définition de « terminé » pour un ticket

- [ ] Le comportement demandé par le ticket fonctionne
- [ ] Les cas d'erreur sont gérés avec les bons codes HTTP
- [ ] Validateur + tests écrits
- [ ] Les règles d'architecture (section 4) sont respectées
- [ ] La PR est approuvée et fusionnée dans `main`
- [ ] Le ticket ClickUp est à jour

## 11. Relecture et fusion

*Section destinée au responsable backend.*

### Checklist de relecture

- Les fichiers sont dans les bons dossiers ; le contrôleur est mince ; aucun `AppDbContext` dans un contrôleur.
- L'API renvoie des DTO, jamais des entités.
- Un validateur existe pour chaque requête qui modifie des données.
- Les codes HTTP respectent la section 7.
- `async`/`await` et `CancellationToken` partout.
- Pas de secret, de `.env`, de `bin/` ou `obj/` dans la PR.
- Migration présente si le schéma change, avec un nom explicite.
- Les tests passent (et couvrent la règle principale).
- La PR est de taille raisonnable et ne contient que ce que le ticket demande.

### Fusionner

Utilisez **Squash and merge** : tous les commits de la branche deviennent un seul commit propre sur `main`. Cochez ensuite **Delete branch**.

### Protéger `main` (réglage à faire une fois)

Sur GitHub : **Settings → Branches → Add branch ruleset** (ou *Add classic branch protection rule*) pour `main` :

- **Require a pull request before merging**
- **Require approvals : 1**
- **Dismiss stale pull request approvals when new commits are pushed**
- **Require review from Code Owners** (avec le fichier `.github/CODEOWNERS` ci-dessous)
- **Block force pushes**
- Quand la CI existera : **Require status checks to pass** (build + tests)

Fichier `.github/CODEOWNERS` (remplacez par votre pseudo GitHub) :

```
# Le responsable backend est automatiquement demandé en relecture sur tout ce qui touche au backend
/backend/  @votre-pseudo-github
```

> Vos propres PR doivent aussi être approuvées : demandez à un autre membre, ou autorisez les administrateurs à contourner la règle.

Le fichier `.github/pull_request_template.md` pré-remplit le formulaire de chaque nouvelle PR.

## 12. Dépannage

| Symptôme | Cause probable | Solution |
|---|---|---|
| `DB_PASSWORD is required` au lancement de Docker | `.env` absent ou incomplet | `copy .env.example .env` puis renseignez `DB_PASSWORD` |
| `ports are not available ... 5432` (Windows) | port réservé par Windows ou déjà utilisé par un PostgreSQL local | mettez `DB_PORT=5433` dans votre `.env` |
| `/health` répond 503 ou `database: down` | base arrêtée ou mauvais mot de passe | `docker compose ps` (db doit être `healthy`) ; si vous avez changé `DB_PASSWORD` après coup : `docker compose down -v` |
| `password authentication failed` | le mot de passe n'est lu qu'à la **première** création du volume | `docker compose down -v` puis `docker compose up` |
| `Value cannot be null (connectionString)` avec `dotnet watch` ou `dotnet ef` | user-secrets non configurés | refaites la configuration de la section 3 |
| `Unable to resolve service for type 'IXxxService'` | le service ne respecte pas la convention de nommage ou n'est pas dans `Services/` | classe `XxxService` + interface `IXxxService`, dans le dossier `Services/` |
| Le validateur ne se déclenche pas | il n'hérite pas de `AbstractValidator<LeTypeDeLaRequête>` | vérifiez le type générique et que la classe est publique |
| `dotnet ef` : « No project was found » | mauvais dossier courant | lancez la commande depuis `backend/` |
| Erreur de build dans Docker mais pas en local | fichier au mauvais nom/casse (Windows ignore la casse, Linux non) | respectez exactement la casse des noms de fichiers |
| `git push` refusé | la branche protégée `main` | poussez votre **branche** (`feature/...`), jamais `main` |

---

*Ce README fait partie du code : si vous trouvez une erreur ou un oubli, corrigez-le dans une PR `docs/...`.*