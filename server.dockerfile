FROM ubuntu:20.04

EXPOSE 7778

COPY ./build/StandaloneLinux64 /game
RUN chmod +x /game
RUN /game/StandaloneLinux64 -logfile ~/server.log