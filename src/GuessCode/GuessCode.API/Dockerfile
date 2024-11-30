FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GuessCode.API/GuessCode.API.csproj", "GuessCode.API/"]
COPY ["GuessCode.API.Models/GuessCode.API.Models.csproj", "GuessCode.API.Models/"]
COPY ["GuessCode.DAL/GuessCode.DAL.csproj", "GuessCode.DAL/"]
COPY ["GuessCode.Domain/GuessCode.Domain.csproj", "GuessCode.Domain/"]
RUN dotnet restore "GuessCode.API/GuessCode.API.csproj"
COPY . .
WORKDIR "/src/GuessCode.API"
RUN dotnet build "GuessCode.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GuessCode.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GuessCode.API.dll"]
