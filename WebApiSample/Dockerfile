FROM microsoft/dotnet:2.1.0-preview2-runtime-alpine AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:8080
EXPOSE 8080

FROM microsoft/dotnet:2.1.300-preview2-sdk AS build
WORKDIR /src
COPY WebApiSample.sln ./
COPY docker-compose.dcproj ./
COPY WebApiSample/WebApiSample.csproj WebApiSample/
RUN dotnet restore -nowarn:msb3202,nu1503 *.sln
COPY . .
WORKDIR /src/WebApiSample
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#USER nobody
ENTRYPOINT ["dotnet", "WebApiSample.dll"]
