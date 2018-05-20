# README #

Adding migrations
To create the migrations, open a command prompt in the IdentityServer project directory. In the command prompt run these two commands:

dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb


## Introduction

> Para Instanar 
* dotnet restore
* dotnet build
* dotnet run
Or
* dotnet watch run

## Installation

> Migrations
* dotnet ef migrations add initial
* dotnet ef database drop
Or
* dotnet ef database update



## node generete api do swagger
java -jar D:\projetos\dotnetcore\swagger-codegen-cli-2.3.1.jar generate -i http://localhost:5000/swagger/v1/swagger.json -l typescript-angular
