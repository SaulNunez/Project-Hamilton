FROM ubuntu:20.04

EXPOSE 7778

ARG DEBIAN_FRONTEND=noninteractive
RUN apt-get update && \
apt-get install -y libglu1 xvfb libxcursor1

COPY ./build/StandaloneLinux64 /game
RUN chmod +x /game
RUN mkdir /tmp/logs
ENTRYPOINT /game/StandaloneLinux64 -batchmode -nographics