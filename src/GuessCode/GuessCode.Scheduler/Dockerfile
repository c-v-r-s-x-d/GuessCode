FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GuessCode.Scheduler/GuessCode.Scheduler.csproj", "GuessCode.Scheduler/"]
COPY ["GuessCode.Domain/GuessCode.Domain.csproj", "GuessCode.Domain/"]
COPY ["GuessCode.DAL/GuessCode.DAL.csproj", "GuessCode.DAL/"]
RUN dotnet restore "GuessCode.Scheduler/GuessCode.Scheduler.csproj"
COPY . .
WORKDIR "/src/GuessCode.Scheduler"
RUN dotnet build "GuessCode.Scheduler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GuessCode.Scheduler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GuessCode.Scheduler.dll"]
