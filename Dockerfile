FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY . .

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

COPY --from=build /app/publish .
COPY wwwroot ./wwwroot

EXPOSE 80

ENTRYPOINT ["dotnet", "CSMS.dll"]
