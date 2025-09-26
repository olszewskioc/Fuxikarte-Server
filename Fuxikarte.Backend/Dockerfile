# Fuxikarte.Backend/Dockerfile
### Stage 1: build (usa o SDK para compilar/publish)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas os .csproj primeiro (melhora cache de layers)
COPY Fuxikarte.Backend/*.csproj ./Fuxikarte.Backend/
WORKDIR /src/Fuxikarte.Backend
RUN dotnet restore

# Copia todo o código e publica (Release)
WORKDIR /src
COPY . .
RUN dotnet publish Fuxikarte.Backend/Fuxikarte.Backend.csproj -c Release -o /app/publish

### Stage 2: runtime (imagem menor apenas com ASP.NET)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expor porta interna (o container servirá em 80)
ENV ASPNETCORE_URLS=http://+:80

# Executa a dll
ENTRYPOINT ["dotnet", "Fuxikarte.Backend.dll"]
