FROM openjdk:21-slim

WORKDIR /app

COPY src/GuessCode/GuessCode.Scheduler/compilers/java-runner/run.sh /app/run.sh
RUN chmod +x /app/run.sh

ENTRYPOINT ["/app/run.sh"]
