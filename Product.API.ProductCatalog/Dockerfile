#First stage: build
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS serve
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app

COPY --from=build /src/output .
ENTRYPOINT ["dotnet", "Product.API.ProductCatalog.dll"]

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

EXPOSE 443
