FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 8080

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY WebApiSample.sln ./
COPY WebApiSample/WebApiSample.csproj WebApiSample/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/WebApiSample
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app
RUN chmod -R 0005 /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod -R 0005 ./
#RUN chown -R nobody /root
USER nobody
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "WebApiSample.dll"]
