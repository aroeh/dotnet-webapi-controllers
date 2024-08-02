# Overview
This project is intended to be used for educational and reference purposes and will be updated (as time allows) as new technologies and libraries are released.  This is a .Net Web API using Controllers demonstrating several concepts for useful and more common features and attributes of APIs.

## Demonstrated Features
- Global Exception Handling
- Health Checks on dependent services
- Controller Output Caching
- API Versioning by URL Path
- Dependency Injection
- HSTS Security Headers
- using http files and external variables

# Dependencies
- MongoDB

# Getting Started
This project has a dependency on MongoDB.  You can either use a deployed MongoDB Atlas instance and a connection to connect, or you can use a containerized instance of MongoDB.

View the MongoService.cs file and review the constructor.  Identify how you will be retrieving your connection string and either comment/uncomment the lines present or alter the code.

## MongoDB Atlas
1. Signup for MongoDB if you do not already have an account - Use the free tier of the service
2. Create a new project and database
3. Get the connection string to the database and store somewhere secure, ex: local secrets config file
4. Create a new collection in the database
5. Build and Run the API
6. Use .http files or Postman to send requests to the API

## MongoDB Container
1. Build all images and containers from the docker-compose.yaml file
2. Run the containers using docker-compose.yaml
3. Use .http files or Postman to send requests to the API


# References
- [MongoDB](https://www.mongodb.com/)
- [Web API Versioning](https://github.com/dotnet/aspnet-api-versioning)
- [Output Caching](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-8.0)
- [HealthChecks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0)
- [Metrics](https://learn.microsoft.com/en-us/aspnet/core/log-mon/metrics/metrics?view=aspnetcore-8.0)
- [HTTP File Secrets](https://devblogs.microsoft.com/visualstudio/safely-use-secrets-in-http-requests-in-visual-studio-2022/)