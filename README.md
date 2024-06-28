# DB
docker compose up 

# Backend
dotnet run

# Frontend
npm run dev

# Docker

## Frontend

### .env file
docker run --env-file .env -p 3000:3000 sda-frontend

https://docs.docker.com/compose/environment-variables/set-environment-variables/#use-the-env_file-attribute

### Env variables
Works with dynamic vars

docker run --env PUBLIC_BACKEND_API=http://localhost:5221 -p 3000:3000 sda-frontend