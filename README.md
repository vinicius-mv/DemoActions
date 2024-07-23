# Simple WebAPI with .NET 6 and Docker

This repository contains a simple WebAPI project using the classic WeatherForecast template in .NET 6. The application is containerized using Docker and includes a Docker Compose setup that defines an NGINX container for load balancing and rate limiting.

## Features

- **.NET 6 WebAPI**: A basic WeatherForecast API template.
- **Dockerized**: The application runs in Docker containers.
- **Docker Compose**: Defines the setup for running the API and NGINX.
- **NGINX Load Balancing**: NGINX is configured to load balance requests between two instances of the WebAPI.
- **Rate Limiting**: Basic rate limiting is configured in NGINX to handle incoming requests.

## Project Structure

- **/src**: Contains the .NET 6 WebAPI project.
- **/docker-compose.yml**: Defines the Docker services for the WebAPI and NGINX.
- **/nginx.conf**: Configuration file for NGINX.

## Docker Compose Configuration

The Docker Compose setup includes:

- **DemoActionsAPI**: The WebAPI service, built from the `API/Dockerfile`, with two replicas for load balancing.
- **NGINX**: The NGINX service, built from the `Dockerfile.nginx`, configured to load balance and rate limit requests.

## Getting Started

- **Clone the repository**

- **Build and run the containers**
   
$ docker-compose up --build

- **Access the API: Open your browser**

navigate to http://<my_domain>/WeatherForecast

