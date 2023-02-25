# Catalog Service
Service to list and manage items

## Building app
dotnet build

## Running app
dotnet run

## Installing MongoDB with localhost volume
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo