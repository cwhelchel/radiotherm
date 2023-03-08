#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8180
EXPOSE 40443

FROM base AS final
WORKDIR /app
COPY /bin/Release/net6.0/publish .
ENTRYPOINT ["dotnet", "RadioThermWebApp.dll"]
