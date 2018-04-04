FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:8080
EXPOSE 8080

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY WebApiSample.sln ./
COPY WebApiSample/WebApiSample.csproj WebApiSample/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/WebApiSample
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod -R 0005 ./
USER www-data
ENTRYPOINT ["dotnet", "WebApiSample.dll"]
