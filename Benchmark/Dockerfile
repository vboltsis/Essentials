FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Benchmark.csproj", "Benchmark/"]
RUN dotnet restore "Benchmark/Benchmark.csproj"

WORKDIR "/src/Benchmark"
COPY . .

RUN dotnet build "Benchmark.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Benchmark.csproj" -c Release -o /app/publish -f net7.0 -r linux-x64
#RUN dotnet publish "Benchmark.csproj" -c Release -o /app/publish -p:UseAppHost=false -f net7.0

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/Benchmark/Benchmark.csproj .
COPY --from=build /app/build/Benchmark.dll .
COPY --from=build /app/build/Benchmark.dll /app/obj/Release/net7.0/ref/Benchmark.dll
COPY --from=build /app/build/ /app/bin/Release/net7.0/

ENTRYPOINT ["dotnet", "Benchmark.dll"]
