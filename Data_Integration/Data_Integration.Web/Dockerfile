FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402 AS build
WORKDIR /src
COPY ["Data_Integration.Core/*.csproj","Data_Integration.Core/"]
COPY ["Data_Integration.Web/*.csproj","Data_Integration.Web/"]
COPY ["Data_Integration.Data/*.csproj","Data_Integration.Data/"]
RUN dotnet restore "Data_Integration.Web/Data_Integration.Web.csproj"
COPY . .
WORKDIR "/src/Data_Integration.Web"
RUN dotnet build "Data_Integration.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Data_Integration.Web.csproj" -c Release -o /app

FROM publish AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet","Data_Integration.Web.dll"]