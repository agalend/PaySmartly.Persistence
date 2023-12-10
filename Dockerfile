# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY PaySmartly.Persistance/*.csproj ./PaySmartly.Persistance/
RUN dotnet restore

# copy everything else and build app
COPY PaySmartly.Persistance/. ./PaySmartly.Persistance/
WORKDIR /source/PaySmartly.Persistance
RUN dotnet publish -c release -o /PaySmartly.Persistance --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

LABEL author="Stefan Bozov"

WORKDIR /PaySmartly.Persistance
COPY --from=build /PaySmartly.Persistance ./
ENTRYPOINT ["dotnet", "PaySmartly.Persistance.dll"]