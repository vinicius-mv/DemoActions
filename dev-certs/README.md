# Secure Web API with SSL using Docker and Nginx

This guide describes how to set up SSL for your web API using Docker, Nginx, and self-signed certificates.

## Prerequisites

- Self-signed SSL certificates (`selfsigned.crt` and `selfsigned.key`)

## Step-by-Step Guide

### 1. Generate Self-Signed Certificates

If you don't already have self-signed certificates, you can generate them using OpenSSL

```sh
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout dev-certs/selfsigned.key -out dev-certs/selfsigned.crt
```

### 2. Update Dockerfile.nginx
- Copy the certicate files to the nginx folder: ```/etc/nginx/selfsigned.crt```
- Expose port 443

Exposing the port 443 in the docker file is mainly informative, you still need to expose the ports in the docker compose file.

```
FROM nginx:alpine

# Copy Nginx configuration file
COPY nginx.conf /etc/nginx/nginx.conf

# Copy SSL certificates
COPY ./dev-certs/selfsigned.crt /etc/nginx/selfsigned.crt  # <===========
COPY ./dev-certs/selfsigned.key /etc/nginx/selfsigned.key  # <===========

# Expose ports
EXPOSE 80
EXPOSE 443   # <===========
```

### 3. Adjust nginx.conf to expose SSL port 443.

```
events {}

http {
    limit_req_zone $binary_remote_addr zone=mylimit:10m rate=10r/s;

    upstream DemoActionsAPI {
        server DemoActionsAPI:80;
        server DemoActionsAPI:80;
    }

    server {
        listen 80;

        # server_name  _; # case you only have IP
        # server_name  <my_server_domain> <www.my_server_domain>;
         server_name _;

        # redirect to https
        location / {
            return 301 https://$host$request_uri;
        }
    }

    server {
        listen 443 ssl;  # <=========

        # server_name  _; # case you only have IP
        # server_name  <my_server_domain> <www.my_server_domain>;
        server_name _;

        ssl_certificate /etc/nginx/selfsigned.crt;      # <========= DEVELOPMENT ONLY 
        ssl_certificate_key /etc/nginx/selfsigned.key;  # <========= DEVELOPMENT ONLY

        location / {
            limit_req zone=mylimit burst=20 nodelay;
            proxy_pass http://DemoActionsAPI;
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }
    }
}

```

### 4. Update docker-compose.yml

Ensure your docker-compose.yml file expose the port 443 for nginx

```
version: '3.8'

services:
  DemoActionsAPI:
    image: demonactionsapi
    build:
      context: .
      dockerfile: API/Dockerfile.webapi
    ports:
      - "5000"
    deploy:
      replicas: 2
      restart_policy:
        condition: on-failure

  nginx:
    image: nginx
    build:
      context: ./nginx
      dockerfile: Dockerfile.nginx
    ports:
      - "80:80"
      - "443:443"  # <========= 

```

## 5. Build and Run the Containers 

Rebuild your Docker images and start the containers

```
docker-compose down
docker-compose up --build
```

## 6. Accessing the Web API

You can now access your web API securely over HTTPS. Note that since you are using a self-signed certificate, you may need to manually trust the certificate in your browser or client.
