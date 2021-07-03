FROM ubuntu:20.04

EXPOSE 7778

RUN sudo apt update && sudo apt install -y apache2
COPY build/StandaloneLinux64 /game
RUN sudo chmod +x /game
RUN /game/StandaloneLinux64 -logfile ~/server.log