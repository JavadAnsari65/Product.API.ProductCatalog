#First stage: build
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o output

# Second stage:serve the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS serve
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app

# Copy binary from build to runtime-env
COPY --from=build /src/output .
ENTRYPOINT ["dotnet", "Product.API.ProductCatalog.dll"]

# If you're using a port other than the default 80 or 443,
# you'll need to use the ENV instruction to set the ASPNETCORE_URLS environment variable.
ENV ASPNETCORE_URLS=http://+:80

# Open up port 80 for incoming web traffic
EXPOSE 80

# In development, you might want to expose other ports like 443 for HTTPS.
EXPOSE 443






#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
## Copy the csproj file and restore dependencies
#COPY ["Product.API.ProductCatalog.csproj", "./"]
#RUN dotnet restore "Product.API.ProductCatalog.csproj"
#
## Copy everything else and build the app
#COPY . .
#WORKDIR "/src"
#RUN dotnet build "Product.API.ProductCatalog.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Product.API.ProductCatalog.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Product.API.ProductCatalog.dll"]