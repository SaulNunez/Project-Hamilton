FROM httpd:latest

COPY build/WebGL/WebGL/ /usr/local/apache2/htdocs/
#COPY ./apache/my-httpd.conf /usr/local/apache2/conf/httpd.conf
EXPOSE 80