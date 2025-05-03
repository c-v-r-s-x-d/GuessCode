FROM gcc:13.1.0

WORKDIR /app

COPY src/GuessCode/GuessCode.Scheduler/compilers/cpp-runner/run.sh /app/run.sh
RUN chmod +x /app/run.sh

ENTRYPOINT ["/app/run.sh"]
