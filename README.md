# Projet7-Findexium (PostTrades)

API REST développée pour Findexium, groupe Trésorerie et marchés financiers.
L'application PostTrades expose et sécurise les données post-transaction utilisées
entre le front office et le back office.

Le projet part d'un code existant incomplet, corrigé et complété pour obtenir une
API RESTful conforme aux bonnes pratiques, avec authentification et autorisation.

## Technologies

- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core 10 (Code First, migrations)
- SQL Server LocalDB
- ASP.NET Core Identity (gestion des utilisateurs et des rôles)
- Authentification par jeton JWT (Bearer)
- Swagger / OpenAPI (documentation et test des endpoints)

## Fonctionnalités

- CRUD REST complet pour les cinq entités financières : Bid, CurvePoint, Rating,
  RuleName, Trade.
- Gestion des utilisateurs via Identity : inscription, connexion, changement de mot
  de passe, attribution et retrait de rôles.
- Authentification JWT : chaque appel aux endpoints protégés exige un jeton valide.
- Autorisation par rôle : les opérations d'administration sont réservées au rôle Admin.
- Mots de passe hachés (jamais stockés en clair).

## Structure

- `Domain/` : les entités (classes POCO) et l'utilisateur Identity.
- `Data/` : le contexte EF Core (IdentityDbContext) et l'amorçage initial.
- `Repositories/` : l'accès aux données des entités financières.
- `Controllers/` : les endpoints REST.
- `Models/` : les objets de transfert (DTO) d'entrée et de sortie.
- `Migrations/` : les migrations EF Core.

## Prérequis

- SDK .NET 10
- SQL Server LocalDB (instance `(localdb)\MSSQLLocalDB`)

## Installation et exécution

1. Restaurer les dépendances :
   ```
   dotnet restore
   ```
2. Vérifier la chaîne de connexion dans `P7CreateRestApi/appsettings.json`
   (par défaut : base `PostTrades` sur LocalDB).
3. Créer la base et appliquer les migrations :
   ```
   dotnet ef database update --project P7CreateRestApi
   ```
   (ou via la Console du Gestionnaire de package : `Update-Database`).
4. Lancer l'application :
   ```
   dotnet run --project P7CreateRestApi
   ```
5. Ouvrir Swagger (en environnement Development) à l'adresse indiquée au démarrage,
   par exemple `https://localhost:7210/swagger`.

## Sécurité : se connecter et appeler un endpoint protégé

Au démarrage, un compte administrateur et les rôles Admin/User sont créés
automatiquement s'ils n'existent pas.

- Compte administrateur par défaut : identifiant `admin`, mot de passe `Admin123!`.

Étapes dans Swagger :

1. `POST /Login/login` avec l'identifiant et le mot de passe pour obtenir un jeton.
2. Cliquer sur `Authorize` et coller le jeton.
3. Les endpoints protégés répondent alors normalement ; sans jeton valide ils
   renvoient 401, et sans le rôle requis 403.

L'inscription publique (`POST /User/register`) crée toujours un utilisateur avec le
rôle User uniquement. La promotion en Admin est réservée à un administrateur.

## Auteur

Dominique
