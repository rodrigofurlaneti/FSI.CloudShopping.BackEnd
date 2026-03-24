FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/FSI.CloudShopping.WebAPI/FSI.CloudShopping.WebAPI.csproj", "src/FSI.CloudShopping.WebAPI/"]
COPY ["src/FSI.CloudShopping.Application/FSI.CloudShopping.Application.csproj", "src/FSI.CloudShopping.Application/"]
COPY ["src/FSI.CloudShopping.Domain/FSI.CloudShopping.Domain.csproj", "src/FSI.CloudShopping.Domain/"]
COPY ["src/FSI.CloudShopping.Infrastructure/FSI.CloudShopping.Infrastructure.csproj", "src/FSI.CloudShopping.Infrastructure/"]
RUN dotnet restore "src/FSI.CloudShopping.WebAPI/FSI.CloudShopping.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/FSI.CloudShopping.WebAPI"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FSI.CloudShopping.WebAPI.dll"]
