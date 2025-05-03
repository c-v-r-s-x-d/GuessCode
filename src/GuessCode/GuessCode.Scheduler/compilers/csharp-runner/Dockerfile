FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY src/GuessCode/GuessCode.Scheduler/compilers/csharp-runner/run.sh /app/run.sh
RUN chmod +x /app/run.sh

ENTRYPOINT ["/app/run.sh"]
