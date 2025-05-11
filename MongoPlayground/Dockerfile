FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./src/MongoPlayground.csproj ./src/
RUN dotnet restore ./src/MongoPlayground.csproj

COPY ./src/ ./src/
RUN dotnet publish ./src/MongoPlayground.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "MongoPlayground.dll"]
