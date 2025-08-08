# =========================
# 1. Base build image (SDK)
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BookStoreFullStackNg.Data/*.csproj BookStoreFullStackNg.Data/
COPY BookStoreFullStackNg.Api/*.csproj BookStoreFullStackNg.Api/
RUN dotnet restore BookStoreFullStackNg.Api

COPY . .

RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src/BookStoreFullStackNg.Api

# Run BOTH migrations at build time
RUN dotnet ef database update --context BookStoreContext \
    --project ../BookStoreFullStackNg.Data \
    --startup-project . \
 && dotnet ef database update --context AuthContext \
    --project ../BookStoreFullStackNg.Data \
    --startup-project .

# Seed only BookStore.db (Auth.db usually doesn't need seed data)
RUN apt-get update && apt-get install -y sqlite3 \
 && sqlite3 /src/BookStoreFullStackNg.Api/BookStore.db < /src/BookStoreFullStackNg.Api/seed.sql

RUN dotnet publish -c Release -o /app/publish

# =========================
# 2. Runtime image
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY --from=build /src/BookStoreFullStackNg.Api/BookStore.db .
COPY --from=build /src/BookStoreFullStackNg.Api/Auth.db .
COPY --from=build /src/BookStoreFullStackNg.Api/seed.sql .

RUN mkdir -p /app/Uploads

EXPOSE 5001
ENTRYPOINT ["dotnet", "BookStoreFullStackNg.Api.dll", "--urls", "http://0.0.0.0:5001"]
