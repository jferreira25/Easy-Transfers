FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY Api/Easy.Transfers.Admin.csproj Api/
COPY CrossCutting.Configuration/Easy.Transfers.CrossCutting.Configuration.csproj CrossCutting.Configuration/
COPY Domain/Easy.Transfers.Domain.csproj Domain/
COPY Infrastructure.Data/Easy.Transfers.Infrastructure.Data.csproj Infrastructure.Data/
COPY Infrastructure.Publisher/Easy.Transfers.Infrastructure.Publisher.csproj Infrastructure.Publisher/
COPY Infrastructure.Service/Easy.Transfers.Infrastructure.Service.csproj Infrastructure.Service/
COPY Infrastructure.Subscriber/Easy.Transfers.Infrastructure.Subscriber.csproj Infrastructure.Subscriber/
COPY Tests.Integration/Easy.Transfers.Tests.Integration.csproj Tests.Integration/
COPY Tests.Shared/Easy.Transfers.Tests.Shared.csproj Tests.Shared/
COPY Tests.Unity/Easy.Transfers.Tests.Unity.csproj Tests.Unity/
RUN dotnet restore 

# Copy everything else and build
COPY . .
WORKDIR "/src/Api"

RUN mv appsettings.Docker.json appsettings.json
RUN dotnet publish "Easy.Transfers.Admin.csproj" -c Release -o /app


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Easy.Transfers.Admin.dll"]