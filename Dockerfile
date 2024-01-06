# Build StageDockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src

COPY ./src/ /src

RUN mkdir -p /root/.nuget/NuGet
COPY ./config/NuGetPackageSource.Config /root/.nuget/NuGet/NuGet.Config 

RUN dotnet restore ./CommandService/

#   publish
RUN dotnet publish ./CommandService/ -o /publish --configuration Release


# Publish Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ARG git_branch
WORKDIR /app
COPY --from=build-env /publish .
COPY ./src/CommandService/RS.MF.ModeTracking.CommandWebService.xml /app
ENV port=80
RUN ls /app
ENV ASPNETCORE_ENVIRONMENT=$git_branch
ENV ASPNETCORE_URLS=http://+:$port

ENTRYPOINT ["dotnet", "RS.MF.ModeTracking.CommandWebService.dll"]