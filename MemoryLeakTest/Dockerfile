﻿FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MemoryLeakTest/MemoryLeakTest.csproj", "MemoryLeakTest/"]
RUN dotnet restore "MemoryLeakTest/MemoryLeakTest.csproj"
COPY . .
WORKDIR "/src/MemoryLeakTest"
RUN dotnet build "MemoryLeakTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MemoryLeakTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MemoryLeakTest.dll"]
