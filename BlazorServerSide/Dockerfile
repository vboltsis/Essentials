FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:.80 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlazorServerSide/BlazorServerSide.csproj", "BlazorServerSide/"]
RUN dotnet restore "./BlazorServerSide/BlazorServerSide.csproj"
COPY . .
WORKDIR "/src/BlazorServerSide"
RUN dotnet build "./BlazorServerSide.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BlazorServerSide.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorServerSide.dll"]