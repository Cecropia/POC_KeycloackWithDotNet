FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /KeyCloakIntegration
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - 
RUN apt-get install -y nodejs
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish ./src/Web/ -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build-env /KeyCloakIntegration/out .
ENTRYPOINT ["dotnet", "KeyCloakIntegration.Web.dll"]