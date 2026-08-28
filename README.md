# Projet7-Findexium (PostTrades)

API REST developpee pour Findexium, groupe Tresorerie et marches financiers.
L'application PostTrades expose et securise les donnees post-transaction utilisees
entre le front office et le back office.

Le projet part d'un code existant incomplet, corrige et complete pour obtenir une
API RESTful conforme aux bonnes pratiques, avec authentification et autorisation.

## Technologies

- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core 10 (Code First, migrations)
- SQL Server LocalDB
- ASP.NET Core Identity (gestion des utilisateurs et des roles)
- Authentification par jeton JWT (Bearer)
- Swagger / OpenAPI (documentation et test des endpoints)

## Fonctionnalites

- CRUD REST complet pour les cinq entites financieres : Bid, CurvePoint, Rating,
  RuleName, Trade.
- Gestion des utilisateurs via Identity : inscription, connexion, changement de mot
  de passe, attribution et retrait de roles.
- Authentification JWT : chaque appel aux endpoints proteges exige un jeton valide.
- Autorisation par role : les operations d'administration sont reservees au role Admin.
- Mots de passe haches (jamais stockes en clair).

## Structure

- `Domain/` : les entites (classes POCO) et l'utilisateur Identity.
- `Data/` : le contexte EF Core (IdentityDbContext) et l'amorcage initial.
- `Repositories/` : l'acces aux donnees des entites financieres.
- `Controllers/` : les endpoints REST.
- `Models/` : les objets de transfert (DTO) d'entree et de sortie.
- `Migrations/` : les migrations EF Core.

## Prerequis

- SDK .NET 10
- SQL Server LocalDB (instance `(localdb)\MSSQLLocalDB`)

## Installation et execution

1. Restaurer les dependances :
   ```
   dotnet restore
   ```
2. Verifier la chaine de connexion dans `P7CreateRestApi/appsettings.json`
   (par defaut : base `PostTrades` sur LocalDB).
3. Creer la base et appliquer les migrations :
   ```
   dotnet ef database update --project P7CreateRestApi
   ```
   (ou via la Console du Gestionnaire de package : `Update-Database`).
4. Lancer l'application :
   ```
   dotnet run --project P7CreateRestApi
   ```
5. Ouvrir Swagger (en environnement Development) a l'adresse indiquee au demarrage,
   par exemple `https://localhost:7210/swagger`.

## Securite : se connecter et appeler un endpoint protege

Au demarrage, un compte administrateur et les roles Admin/User sont crees
automatiquement s'ils n'existent pas.

- Compte administrateur par defaut : identifiant `admin`, mot de passe `Admin123!`.

Etapes dans Swagger :

1. `POST /Login/login` avec l'identifiant et le mot de passe pour obtenir un jeton.
2. Cliquer sur `Authorize` et coller le jeton.
3. Les endpoints proteges repondent alors normalement ; sans jeton valide ils
   renvoient 401, et sans le role requis 403.

L'inscription publique (`POST /User/register`) cree toujours un utilisateur avec le
role User uniquement. La promotion en Admin est reservee a un administrateur.

## Auteur

Dominique
