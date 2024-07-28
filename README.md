This repo:

- Database

    It contains stations and bike journeys inbetween these stations.
    Data is loaded at build using a tarball.
- API layer

    An HTTP API to help fetch and filter data from the database, implemented in C# & ASP.Net.
- Web application

    Gets the data from the database through the API layer and presents it as tables on the /station url, and as a map on /map.  
    It's implemented with Svelte/Sveltekit using Typescript and plain CSS.  
    Two npm packages are used: 'dotenv' to handle environment variables, 'leaflet' to provide the interactive map.

The original "assignment" is described in [/db/Readme](https://github.com/robper/solita-devacademy-spring-2024/blob/master/db/README.md).  
I made this to refamiliarize myself with basic concepts, and to test doing somehting with a map on the frontend.

# Run with Docker

The entire project including db, backend & frontend, can be run using a docker compose file is provided.

Services exposed to host:  

>Backend/API: <http://localhost:5221>  
>Frontend/Web: <http://localhost:3000>

## Requirements

- [Docker](https://www.docker.com/community-edition#/download)
- [Docker Compose](https://docs.docker.com/compose/install/)  
(or Podman with docker-compose, Podman compoes does not work)

Clone the repo

    git clone https://github.com/robper/solita-devacademy-spring-2024.git

Run using docker compose

    docker compose up --build

This require ~850MB of HDD space, for whatever reason.

# Build from source

## Requirements

- Docker OR Postgres
- .Net 8
- Node.js & npm

## DB

This compose starts adminer aswell as Postgres

    docker compose up

or however you want to run Postgres.

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

# TODO / Can-do

Overall:

- Error handling front- and backend

Backend:

- List & pagination for journeys
- Tests to check that the database was imported correctly

Single Station:

- List all journeys, there's space to the right

Map:

- When hovering or selecting a station in the sidebar, expand the marker

# Conclusions

CSS is still annoying.  
80% of JS time is spent on small things and edge-cases.  
Not sure about Typescript, it's neat but sometimes requires you to do alot of things to achieve very little change.

If I were to use Leaflet again, I would probably use something like:  
<https://github.com/ShipBit/sveltekit-leaflet>  
<https://github.com/ngyewch/svelte-leafletjs>  
<https://github.com/imIfOu/svelte-map-leaflet>  
But probably I'd use Mapbox, Maptiler or similar.


# References

[Dockerfile](https://gist.github.com/aradalvand/04b2cad14b00e5ffe8ec96a3afbb34fb)
