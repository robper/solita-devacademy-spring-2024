This repo contains a 'fullstack' web application built using C#, ASP.NET, Svelte, Typescript & Postgres.
The data consists of bike stations and bike journeys, which start and end at a station.
The data and 'assignment' is described provided here: [/db/Readme](https://github.com/robper/solita-devacademy-spring-2024/blob/master/db/README.md), or [here](https://github.com/solita/dev-academy-spring-2024-exercise).

I have no affiliation with Solita, nor did i participate or send in this to their academy, I just wanted something to do and try Sveltekit.

# Screenshots
![image](https://github.com/user-attachments/assets/cbe753b5-1a0f-44b8-ad21-c6eed543501e)
![image](https://github.com/user-attachments/assets/c96a6409-b750-4b40-bf41-90377afdbb38)
![image](https://github.com/user-attachments/assets/c6d093c0-bb20-48be-beb4-5f38fc2cc97b)
![image](https://github.com/user-attachments/assets/5ed02ff3-f709-4353-affd-c7e35356b127)

# Components

- Database

    Contains stations and bike journeys inbetween these stations.
    Data is loaded at build using a tarball.
- API

    An HTTP API to help fetch and filter data from the database, implemented in C# & ASP.Net.
- Web application

    Gets the data from the database through the API layer and presents it as tables on the /station url, and as a map on /map.  
    It's implemented with Svelte/Sveltekit using Typescript and CSS.  
    "Two" npm packages are used: 'dotenv' to handle environment variables, 'leaflet' to provide the interactive map.  
    Map tiles are provided from OpenStreetMap.

# Run with Docker

The entire project can be run using a docker compose file is provided.

Services accessible on host:  

>Backend/API: <http://localhost:5221>  
>Frontend/Web: <http://localhost:3000>  
>Adminer: <http://localhost:8088>

## Requirements

- [Docker](https://www.docker.com/community-edition#/download)
- [Docker Compose](https://docs.docker.com/compose/install/)  
(or Podman with docker-compose, Podman compose does not work)

Clone the repo

    git clone https://github.com/robper/solita-devacademy-spring-2024.git

Run using docker compose

    docker compose up --build

# Build from source

## Requirements

- Docker OR Postgres
- .Net 8
- Node.js & npm

## DB

This compose starts adminer aswell as Postgres

    docker compose up

## Backend

    dotnet run

## Frontend

Requires the file '/sda-frontend/.env' with the following content:

>PUBLIC_BACKEND_API=<http://localhost:5221>

Run:

    npm install
    npm run dev

Or with docker

    docker run --env PUBLIC_BACKEND_API=http://localhost:5221 -p 3000:3000 sda-frontend
# Problems

The database does sometimes not load on first build.

# TODO / Can-do / Will-not-do

Overall:

- Error handling front- and backend
- Tests

Backend:

- List & pagination for journeys

Single Station:

- List all journeys, there's space to the right

Map:

- When hovering or selecting a station in the sidebar, expand the marker

Leaflet alternatives which probably fit better:  
<https://github.com/ShipBit/sveltekit-leaflet>  
<https://github.com/ngyewch/svelte-leafletjs>  
<https://github.com/imIfOu/svelte-map-leaflet>  
But probably I'd use Mapbox, Maptiler or similar.


# References

[Dockerfile](https://gist.github.com/aradalvand/04b2cad14b00e5ffe8ec96a3afbb34fb)
