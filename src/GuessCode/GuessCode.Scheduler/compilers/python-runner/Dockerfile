# docker/python-runner/Dockerfile
FROM python:3.11-slim

WORKDIR /app

COPY src/GuessCode/GuessCode.Scheduler/compilers/python-runner/run.sh /app/run.sh
RUN chmod +x /app/run.sh

ENTRYPOINT ["/app/run.sh"]
