# Secure Web API with SSL using Docker and Nginx

This guide describes how to set up SSL for your web API using Docker, Nginx, and self-signed certificates.

## Prerequisites

- Self-signed SSL certificates (`certificate.crt` and `private.key`)

## Step-by-Step Guide

### 1. Generate Self-Signed Certificates

If you don't already have self-signed certificates, you can generate them using OpenSSL

```sh
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout ./private.key -out ./certificate.crt
```

Move certificates to folder <repository_root>/nginx/certs/

### 2. Update Dockerfile.nginx
- Expose port 443

Exposing the port 443 in the docker file is mainly informative, you still need to expose the ports in the docker compose file.

```
FROM nginx:alpine

# Expose ports
EXPOSE 80
EXPOSE 443   # <===========
```

### 3. Adjust <repository_root>/nginx/nginx.conf to expose SSL port 443 and set up the SSL certificates path

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
        server_name  thisisvinicius.dev www.thisisvinicius.dev;

        # redirect to https
        location / {
            return 301 https://$host$request_uri;
        }
    }
            
    server {
        listen 443 ssl;        # <======================

        # server_name  _; # case you only have IP
        # server_name  <my_server_domain> <www.my_server_domain>;
        server_name  quantdev.ovh www.quantdev.ovh; 
        
        ssl_certificate /etc/nginx/certs/certificate.crt;    # <======================
        ssl_certificate_key /etc/nginx/certs/private.key;    # <======================

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

```yml
services:
  DemoActionsAPI:
    image: demonactionsapi
    build:
      context: .
      dockerfile: API/Dockerfile
    ports:
      - "5000"
    deploy:
      replicas: 2
      restart_policy:
        condition: on-failure

  nginx:
    image: nginx
    build:
      context: .
      dockerfile: Dockerfile.nginx
    ports:
      - "80:80"
      - "443:443"            # <========
    volumes:
      - ./nginx/nginx.conf:/etc/nginx/nginx.conf:ro
      - ./nginx/html:/usr/share/nginx/html:ro
      - ./nginx/certs:/etc/nginx/certs:ro

volumes:
  nginx_conf:
  html:
  certs:
```

## 5. Build and Run the Containers 

Rebuild your Docker images and start the containers

```
docker-compose down
docker-compose up --build
```

## 6. Accessing the Web API

You can now access your web API securely over HTTPS. Note that since you are using a self-signed certificate, you may need to manually trust the certificate in your browser or client.
