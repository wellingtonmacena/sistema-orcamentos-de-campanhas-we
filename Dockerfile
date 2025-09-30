# See https://aka.ms/customizecontainer to learn how to customize your container for debugging and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used during VS Fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY [".CampaignBudgetingAPI/CampaignBudgetingAPI.csproj", "/"]
RUN dotnet restore "CampaignBudgetingAPI/CampaignBudgetingAPI.csproj"
COPY . .
WORKDIR "/src/CampaignBudgetingAPI"
RUN dotnet build "./CampaignBudgetingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CampaignBudgetingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running in VS normal mode (default when not using Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CampaignBudgetingAPI.dll"]