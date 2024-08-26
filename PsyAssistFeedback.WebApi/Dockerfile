#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7452
EXPOSE 7453

FROM ubuntu:16.04
ENV DEBIAN_FRONTEND noninteractive.
RUN apt-get update
RUN apt-get upgrade -y
RUN apt-get install -y language-pack-ru
ENV LANGUAGE ru_RU.UTF-8
ENV LANG ru_RU.UTF-8
ENV LC_ALL ru_RU.UTF-8
RUN locale-gen ru_RU.UTF-8 && dpkg-reconfigure locales
ENTRYPOINT /bin/bash

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PsyAssistFeedback.WebApi/PsyAssistFeedback.WebApi.csproj", "PsyAssistFeedback.WebApi/"]
COPY ["PsyAssistFeedback.Application/PsyAssistFeedback.Application.csproj", "PsyAssistFeedback.Application/"]
COPY ["PsyAssistFeedback.Domain/PsyAssistFeedback.Domain.csproj", "PsyAssistFeedback.Domain/"]
COPY ["PsyAssistFeedback.Persistence/PsyAssistFeedback.Persistence.csproj", "PsyAssistFeedback.Persistence/"]
RUN dotnet restore "./PsyAssistFeedback.WebApi/PsyAssistFeedback.WebApi.csproj"
COPY . .
WORKDIR "/src/PsyAssistFeedback.WebApi"
RUN dotnet build "./PsyAssistFeedback.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PsyAssistFeedback.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PsyAssistFeedback.WebApi.dll"]
