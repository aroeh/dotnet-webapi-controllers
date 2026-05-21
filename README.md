# Overview
This project is intended to be used for educational and reference purposes and will be updated (as time allows) as new technologies and libraries are released.  
This is a .Net Web API using Controllers demonstrating several concepts for useful and more common features and attributes of APIs.


## Demonstrated Features
- Clean Architecture concept
- Options pattern
- Global Exception Handling
- Health Checks on dependent services
- API Versioning by Query String
- Dependency Injection
- HSTS Security Headers
- Docker container and Docker Compose
- TODO: Open API Documentation on endpoints


# Dependencies
- Azure
- MongoDB
- Docker
- Docker Compose V2


# Getting Started
This project has a dependency on MongoDB via a containerized instance of MongoDB.  The MongoDb project uses the options pattern to read values from app configuration and make them available for dependency injection.  This enables us to keep the setup within the Infrastructure project and easily find references to the options type to find where it is used.  See the following sections to setup the local credentials for the MongoDB container and Azure App Configuration.


## Docker Environment File
Currently volume binding to read user secrets for the .Net app is not working.  This is a workaround solution until I get that or something better working for reading secrets in the docker container.  Currently the only secret needed will be the Azure App Configuration Connection String.

Both a compose.yaml and compose.override.yaml are setup within the solution to set service configurations and extend them for the development environment.  The compose.override.yaml requires variables to be set and it is recommended to create a .env file so the secrets will not be commited to the code repository.

1. In the same directory containing the compose.yaml, create a new file and name it `dev.env`
```
dev.env
```

2. Add the following to the .env file
```
# Restuarant API Variables
USER_SECRETS_PATH=${APPDATA}/Microsoft/UserSecrets:/root/.microsoft/usersecrets:ro
APP_CONFIG=<your-app-config-connection-string>

# DB Variables
MONGO_USERNAME=<mongo-username>
MONGO_PASSWORD=<mongo-password>
```


## MongoDB Container
1. For the container choose a username and password and make note of it, as it will be used in the Azure App Configuration

> For local development you can use any values you want for the container, but some tutorial values commonly used are as follows: 
```
UserName: AzureDiamond
Password: hunter2
```

> DO NOT STORE THESE IN THE CODE REPOSITORY OR USE IN A PRODUCTION SCENARIO

2. Update the `dev.env` file with the MongoDB username and password that you want to use
```
MONGO_USERNAME=<mongo-username>
MONGO_PASSWORD=<mongo-password>
```


## Azure App Configuration
1. Signup for an Azure Subscription if you do not already have an account [Azure Portal](https://portal.azure.com)
2. Create a new Subscription for resources - Use the lowest cost tier, Free should be available for all items in these steps
3. Create a new Resource Group
4. Create a new App Configuration Resource
    * Use the subscription and resource group 
    * Provide a unique name for the app configuration
    * Select your pricing tier: We're using Free
    * Enable access keys to use a connection string for authentication and connecting
        > The Connection string is the easiest way to start for local development, and should not be used in a Production scenario
    * Use the Automatic Network Access options
5. Setup the API Project with User Secrets for connecting to the App Configuration
```
dotnet user-secrets init
dotnet user-secrets set AppConfiguration:Endpoint "<your-App-Configuration-endpoint>"
dotnet user-secrets set AppConfiguration:ConnectionString "<your-App-Configuration-connection-string>"
```

6. Create the following keys replacing both the `<mongo-username>` and `<mongo-password>` with your chosen username and password

| Key | Value | Label |
| --- | ----- | ----- |
| RestuarantApi:Settings:HealthEndpoint | /healthz | No Label |
| RestuarantApi:Settings:MongoDb:ConnectionString | `mongodb://<mongo-username>:<mongo-password>@localhost:27017` | Development |
| RestuarantApi:Settings:MongoDb:ConnectionString | `mongodb://<mongo-username>:<mongo-password>@mongo:27017` | Docker |
| RestuarantApi:Settings:MongoDb:DatabaseName | restuarants | Development |
| RestuarantApi:Settings:MongoDb:DatabaseName | restuarants | Docker |

7. Update the `dev.env` and set the `APP_CONFIG` variable with the Azure App Config connection string.  Replace `<your-app-config-connection-string>` with the connection string for your Azure App Configuration Service
> DO NOT STORE THE CONNECTION STRING IN THE CODE REPOSITORY OR USE IN A PRODUCTION SCENARIO


# Run the Solution
The easiest way to run this solution is to use docker compose as that will build the api project and provide containers for the data.  But there are other options as well.


## API Dockerfiles
There are 2 Dockerfiles present in the Demo.Restuarants.API project:
- Dockerfile
- Dockerfile_NoBuild

> Dockerfile is the initial setup default within the repository.

Each one demonstrates a different approach and potential use case.

### Dockerfile
Dockerfile is typically the default kind of file that Visual Studio auto generates with adding container support to a project.  
This example handles building the solution in a base image and then publishing the code to a runtime image.
This could be useful when wanting to debug and letting the docker runtime handle all of the work, or if you don't want to manually build and publish code before spinning up a new image and container.

### Dockerfile_NoBuild
This is a much smaller and much more simple docker file.  It requires code to have already been published to copy into the runtime image.
The Docker build and container spin up is much faster since it doesn't have to build the solution in the image itself.  
This scenario is useful in pipeline scenarios where the code may have already been built and published by prior tasks in a job.

To use this docker file in the solution, do the following:

1. In a command line, navigate to the directory containing the Demo.Restuarants.API.csproj
```
cd <path>\Demo.Restuarants.API
```

> Alter the path variable to match your local environment

2. Build the project either at the project or solution level
```
dotnet build
```

3. Publish the code into a directory using the Release configuration
```
dotnet publish Demo.Restuarants.API.csproj -c Release -o publish /p:UseAppHost=false
```

> This creates a new directory at `<path>\Demo.Restuarants.API\publish`

4. Update the docker-compose.yaml section for webapi.
    - Change the value of build.docker from Demo.Restuarants.API/Dockerfile to `Demo.Restuarants.API/Dockerfile_NoBuild`


## Docker Compose
1. Optional - build all containers in the compose yaml
```
docker compose build
```
> To build a specific container use `docker compose build <service-name>`

2. Compose up the containers
```
docker compose --env-file dev.env up
```
> If you do not want to debug, the add the -d parameter.  `docker compose up -d`
> docker compose up will also build all images if they do not exist, so step 1 is optional

> By default docker will read from both the compose.yaml and compose.override.yaml.  To different files use the `-f` parameter.  ex: 
```
docker compose -f compose.yaml -f compose.override.yaml --env-file dev.env up
```

3. Use an http client like Postman or the http files in Visual Studio to send requests to the API

4. Stop the containers when done with testing (or leave them running)
```
docker compose stop
```

> Use the start command to start the containers again
```
docker compose start
```

### Clean Up
Once containers are no longer needed you can remove them all using the compose down command
```
docker compose down
```

> Images can also be deleted using the compose down command
```
docker compose --env-file dev.env down --rmi 'all'
```

## Docker
1. Pull the mongo db image
```
docker pull mongo
```

2. Build and Run the mongo db container
```
docker run -d -p 27017:27017 -e "MONGO_INITDB_ROOT_USERNAME=mongoUser" -e "MONGO_INITDB_ROOT_PASSWORD=mongoPassword" --name mongoLocal mongo
```
> Replace values for MONGO_INITDB_ROOT_USERNAME, MONGO_INITDB_ROOT_PASSWORD, and --name to those of your choosing if you prefer.  But note them as they are needed for the connection string in the next step

3. Build the API Project Image
```
docker build -t my_api_image -f Dockerfile .
```
> Docker Context will vary depending on where you run the command.  This command assumes that it is being run from the same path containing the Dockerfile.  ie: "<PATH>\Demo.Restuarants.API\"

4. Run the Api image and container
```
docker run -d -p 5017:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "MONGODB_CONN=mongodb://mongoUser:mongoPassword@mongoLocal:27017" -e "ASPNETCORE_URLS=http://+:80" --name my_api_container my_api_image
```

5. Build the MVC Project Image
```
docker build -t my_client_image -f Dockerfile .
```
> Docker Context will vary depending on where you run the command.  This command assumes that it is being run from the same path containing the Dockerfile.  ie: "<PATH>\Demo.Restuarants.API\"

6. Run the Api image and container
```
docker run -d -p 5120:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" --name my_client_container my_client_image
```

> This repo is not demonstrating security for the mongo db admin user or connection strings.  The best practice would be to store the credentials in a secure vault and retrieve them from the vault.

7. Create a docker network
```
docker network create my_network
```

8. Add the mongo db and api containers to the network
```
docker network connect mongoLocal
```

```
docker network connect my_api_container
```

```
docker network connect my_client_container
```

9. Test the api using an http client like Postman or Visual Studio and .http files


## Containers and IDE
1. Create and use the Mongo DB container
> You can use either the docker or docker compose commands
> If using docker compose, you may need to pause the api container

2. Using an IDE of your choice, select a profile to use from the launchSettings.json

3. Run and/or debug the solution
> If you set a different user name and password for the Mongo DB container, then you will also need to update the environment variable in your launchSettings.json profile



# References
- [MongoDB](https://www.mongodb.com/)
- [Web API Versioning](https://github.com/dotnet/aspnet-api-versioning)
- [Output Caching](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-8.0)
- [HealthChecks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0)
- [Metrics](https://learn.microsoft.com/en-us/aspnet/core/log-mon/metrics/metrics?view=aspnetcore-8.0)
- [HTTP File Secrets](https://devblogs.microsoft.com/visualstudio/safely-use-secrets-in-http-requests-in-visual-studio-2022/)
- [Docker Compose CLI](https://docs.docker.com/compose/reference/)
- [Docker Compose Repo](https://github.com/docker/awesome-compose/tree/master)
- [Dotnet CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/)
- [Http Client Factory](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory)
- [Refit GitHub](https://github.com/reactiveui/refit)
- [Azure App Configuration](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-aspnet-core-app)
