# Simple WebAPI in .NET 6 with Docker and NGINX Web Server

This repository contains a simple WebAPI project using the classic WeatherForecast template in .NET 6. The application is containerized using Docker and includes a Docker Compose setup that defines an NGINX container for load balancing and rate limiting.

## Features

- **.NET 6 WebAPI**: A basic WeatherForecast API template.
- **Dockerized**: The application runs in Docker containers.
- **Docker Compose**: Defines the setup for running the API and NGINX.
- **NGINX Load Balancing**: NGINX is configured to load balance requests between two instances of the WebAPI.
- **Rate Limiting**: Basic rate limiting is configured in NGINX to handle incoming requests.

## Project Structure

- **./API/**: Contains the .NET 6 WebAPI project files.
- **docker-compose.yml**: Defines the Docker services for the WebAPI and NGINX.
- **./nginx/nginx.conf**: Configuration file for NGINX.
- **./dev-certs/**: guide to set up SSL access and development certificates

## Docker Compose Configuration

The Docker Compose setup includes:

- **DemoActionsAPI**: The WebAPI service, built from the `API/Dockerfile`, with two replicas for load balancing.
- **NGINX**: The NGINX service, built from the `Dockerfile.nginx`, configured to load balance and rate limit requests.

## Getting Started

- **Clone the repository**

- **Build and run the containers**

```ssh
docker-compose up --build
```

- **Access the API: Open your browser**

navigate to http://<my_domain>/WeatherForecast

## Testing the rate limit

- **Install Siege**

```ssh
sudo apt install siege
```

- **Run the test**

```ssh
siege -c 30 -r 1 http://localhost/
```

where:

c: Number of concurrent users.

r: Number of repetitions of the test.

- **Check the NGINX logs for entries related to rate limiting**

When the rate limit is working, you should see some requests being denied with a 503 status code, indicating that the rate limit has been reached.

Logs: Check the NGINX logs for entries related to rate limiting. You can find these logs in the default log location or the one specified in your NGINX configuration.

```ssh
tail -f /var/log/nginx/access.log
```

```ssh
tail -f /var/log/nginx/error.log
```



