# ---------- Étape 1 : build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# On copie d'abord le .csproj seul pour profiter du cache Docker :
# le restore ne se relance que si les dépendances changent.
COPY src/Ter.Api/Ter.Api.csproj src/Ter.Api/
RUN dotnet restore src/Ter.Api/Ter.Api.csproj

COPY src/ src/
RUN dotnet publish src/Ter.Api/Ter.Api.csproj -c Release -o /app/publish --no-restore

# ---------- Étape 2 : runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Utilisateur non-root fourni par l'image
USER $APP_UID
ENTRYPOINT ["dotnet", "Ter.Api.dll"]