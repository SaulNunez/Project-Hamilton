FROM httpd:2.4

COPY ./build/WebGL/. /usr/local/apache2/htdocs
#COPY ./apache/my-httpd.conf /usr/local/apache2/conf/httpd.conf
EXPOSE 80