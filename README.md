The original "assignment" is descripbed in the Readme in /db.
I made this to refamiliarize myself with basic concepts, and to test doing somehting with a map on the frontend.


# Deploy
The entire project including db, backend & frontend, can be run using a docker compose file is provided.

Default ports:
Backend API: 5221
Frontend/web: 3000

Both can be accessed on http://localhost:{port}

## Requirements
- [Docker](https://www.docker.com/community-edition#/download)
- [Docker Compose](https://docs.docker.com/compose/install/)

Clone the repo
```
git clone https://github.com/robper/solita-devacademy-spring-2024.git
```
Run using docker compose
```
docker compose up
```

# Build from source
## Requirements
- Docker OR Postgres
- .Net 8
- Node.js & npm

## DB

This compose starts adminer aswell as Postgres
```
docker compose up 
```
or however you want to run your Postgres.

## Backend

```
dotnet run
```
Can be run standalone with docker using

## Frontend
Requires the file '/sda-frontend/.env' with the following content:
```
PUBLIC_BACKEND_API=http://localhost:5221
```
Run:
```
npm install
npm run dev
```

Or with docker
```
docker run --env PUBLIC_BACKEND_API=http://localhost:5221 -p 3000:3000 sda-frontend
```

# TODO / Can-do
- Error handling
- Map
- Compose networking https://docs.docker.com/compose/networking/ 2 nws
- Pagination
- Single station view, list all journeys + pagination
- Docker secrets

https://gist.github.com/aradalvand/04b2cad14b00e5ffe8ec96a3afbb34fb