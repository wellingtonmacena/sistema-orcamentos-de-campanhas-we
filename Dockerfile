# ------------------------------------------------------------------
# STAGE 1: BASE (Runtime)
# ------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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

# 2. RESTAURA AS DEPEND�NCIAS
RUN dotnet restore "CampaignBudgetingAPI/CampaignBudgetingAPI.csproj"

# 3. COPIA O RESTANTE DO C�DIGO
COPY . .

# 4. DEFINE O WORKDIR DENTRO DA PASTA DO PROJETO e CONSTR�I
WORKDIR "/src/CampaignBudgetingAPI" 
RUN dotnet build "./CampaignBudgetingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ------------------------------------------------------------------
# STAGE 3: PUBLISH
# ------------------------------------------------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# O WORKDIR permanece o mesmo do est�gio build (/src/CampaignBudgetingAPI)

# Publica o projeto usando o caminho local (pois o WORKDIR j� est� na pasta do projeto)
# Removemos o prefixo de pasta e o prefixo "./" para simplicidade
RUN dotnet publish "CampaignBudgetingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ------------------------------------------------------------------
# STAGE 4: FINAL (Production)
# ------------------------------------------------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define o Kestrel para ouvir na porta 8080/8081, que � o padr�o da imagem
# � importante que o Kestrel escute em 0.0.0.0. A imagem base j� faz isso.
# A Render injeta a vari�vel PORT. O ASP.NET Core lida com o mapeamento.
# No entanto, se quiser ser *muito* expl�cito:
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

# O ENTRYPOINT continua o mesmo
ENTRYPOINT ["dotnet", "CampaignBudgetingAPI.dll"]
