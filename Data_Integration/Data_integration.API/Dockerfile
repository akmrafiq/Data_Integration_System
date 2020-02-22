FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Data_Integration.Core/*.csproj","Data_Integration.Core/"]
COPY ["Data_Integration.Web/*.csproj","Data_Integration.Web/"]
COPY ["Data_Integration.Data/*.csproj","Data_Integration.Data/"]
COPY ["Data_integration.API/*.csproj","Data_integration.API/"]
RUN dotnet restore "Data_integration.API/Data_integration.API.csproj"
COPY . .
WORKDIR "/src/Data_integration.API"
RUN dotnet build "Data_integration.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Data_integration.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Data_integration.API.dll"]