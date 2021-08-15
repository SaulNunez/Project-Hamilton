FROM ubuntu:20.04

EXPOSE 7778

COPY ./build/StandaloneLinux64 /game
RUN chmod +x /game
RUN mkdir /tmp/logs
ENTRYPOINT /game/StandaloneLinux64 -batchmode -logfile /tmp/logs/server.log