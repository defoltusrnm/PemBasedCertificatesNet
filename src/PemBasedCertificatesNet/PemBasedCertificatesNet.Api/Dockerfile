﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PemBasedCertificatesNet.Api/PemBasedCertificatesNet.Api.csproj", "PemBasedCertificatesNet.Api/"]
RUN dotnet restore "PemBasedCertificatesNet.Api/PemBasedCertificatesNet.Api.csproj"
COPY . .
WORKDIR "/src/PemBasedCertificatesNet.Api"
RUN dotnet build "PemBasedCertificatesNet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PemBasedCertificatesNet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PemBasedCertificatesNet.Api.dll"]
