FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5290

ENV ASPNETCORE_URLS=http://+:5290

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["JoyKioskApi/JoyKioskApi.csproj", "JoyKioskApi/"]
RUN dotnet restore "JoyKioskApi/JoyKioskApi.csproj"
COPY . .
WORKDIR "/src/JoyKioskApi"
RUN dotnet build "JoyKioskApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "JoyKioskApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JoyKioskApi.dll"]