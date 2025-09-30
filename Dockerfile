# ------------------------------------------------------------------
# STAGE 1: BASE (Runtime)
# ------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 10000

# ------------------------------------------------------------------
# STAGE 2: BUILD
# ------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. COPIA O ARQUIVO DO PROJETO PRIMEIRO
#    Caminho de origem: CampaignBudgetingAPI/CampaignBudgetingAPI.csproj
#    Caminho de destino: CampaignBudgetingAPI/
COPY ["CampaignBudgetingAPI/CampaignBudgetingAPI.csproj", "CampaignBudgetingAPI/"]

# 2. RESTAURA AS DEPENDÊNCIAS
RUN dotnet restore "CampaignBudgetingAPI/CampaignBudgetingAPI.csproj"

# 3. COPIA O RESTANTE DO CÓDIGO
COPY . .

# 4. DEFINE O WORKDIR DENTRO DA PASTA DO PROJETO e CONSTRÓI
WORKDIR "/src/CampaignBudgetingAPI" 
RUN dotnet build "./CampaignBudgetingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ------------------------------------------------------------------
# STAGE 3: PUBLISH
# ------------------------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# O WORKDIR permanece o mesmo do estágio build (/src/CampaignBudgetingAPI)

# Publica o projeto usando o caminho local (pois o WORKDIR já está na pasta do projeto)
# Removemos o prefixo de pasta e o prefixo "./" para simplicidade
RUN dotnet publish "CampaignBudgetingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ------------------------------------------------------------------
# STAGE 4: FINAL (Production)
# ------------------------------------------------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# O ENTRYPOINT continua o mesmo
ENTRYPOINT ["dotnet", "CampaignBudgetingAPI.dll"]
