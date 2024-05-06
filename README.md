# Chest #

Lykke Web API to store and retrieve key value data.
It stores any string value against a unique key. The key is composed of three parts and is case in-sensitive

1. Category
2. Collection
3. Key

## Prerequisites

This project requires a running instance of [MS Sql Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017) and the connection string to be configured (see configuration section below).  

To download and install MS Sql Server you can follow the [instructions here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
It is further possible to run MS Sql Server as per [instructions here](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-2017)

NOTE: If you are running Chest inside a docker container pointing to MS Sql Server running on your Windows machine then make sure to set the host in the connection string to ```docker.for.win.localhost```.


## How to configure

All variables (Secrets/Settings) can be specified via ```appSettings.json``` file OR by environment variables / secrets.

### Secrets variables

This project requires specification of the [following user secrets](src/Chest/Program.cs?fileviewer=file-view-default#Program.cs-21) in order to function:

  | Parameter | Description
  | --- | --- |
  | ConnectionStrings:Chest / CHEST_CONNECTIONSTRING | Connection string to sql database |
  | ChestClientSettings:ApiKey / CHEST_API_KEY | Api key for Chest service |

As mentioned before, these secrets can also be set via ```appSettings.json``` file OR by environment variables, there is no strict requirement to provide them via secrets file

The secrets configuration mechanism differs when running the project directly or running inside a container. For detailed config specific to each platform, check section below.

### Settings variables

You can configure the ```appSettings.json``` replacing default values with desired ones for each variable. 

You can also add a file called ```appSettings.Custom.json``` with custom which will override the variables from ```appSettings.json``` or compose with it. 

Additionally you can add a file called ```appSettings.{environment}.json``` with environment specific configuration which will override the variables from ```appSettings.json``` and ```appSettings.Custom.json``` or compose with them.

These available variables are detailed below:

  | Parameter | Description |
  | --- | --- |
  | urls | Url that service will be exposed |
  | serilog:* | Serilog settings including output template, rolling file interval and file size limit |

### Settings schema:
<!-- MARKDOWN-AUTO-DOCS:START (CODE:src=./template.json) -->
<!-- MARKDOWN-AUTO-DOCS:END -->

### Log specific configuration

- Logging mechanism in place uses Serilog with some enrichers to exposed better and more detailed logs (i.e [FromLogContext](https://github.com/serilog/serilog/wiki/Enrichment), [WithMachineName](https://github.com/serilog/serilog-enrichers-environment), [WithThreadId](https://github.com/serilog/serilog-enrichers-thread), [WithDemystifiedStackTraces](https://github.com/nblumhardt/serilog-enrichers-demystify)).

- There are two custom [middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1) injected in the application's request pipeline to enhance logs:
  
  1) [ExceptionHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/ExceptionHandlerMiddleware.cs) providing a global `Try/Catch` for unhandled [Exceptions](https://docs.microsoft.com/en-us/dotnet/api/system.exception?view=netcore-2.1)
  2) [LogHandlerMiddleware](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/src/Lykke.Middlewares/LogHandlerMiddleware.cs) providing a global `Logging` for [HttpRequests](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequest?view=aspnetcore-2.1)

  For more info please check [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) repository.

- Default configuration outputs log for [Console](https://github.com/serilog/serilog-sinks-console) and [File](https://github.com/serilog/serilog-sinks-file) with message format being the same for both:

  ```json
  {
    "Args": {
      "outputTemplate": "[{Timestamp:u}] [{Application}:{Version}:{Environment}] [{Level:u3}] [{RequestId}] [{CorrelationId}] [{ExceptionId}] {Message:lj} {NewLine}{Exception}"
    }
  }
  ```

  With following details:

  | Parameter | Description |
  | --- | --- |
  | Timestamp:u | Outputs current timestamp in UTC |
  | Application | Outputs the application name |
  | Version | Outputs the application version |
  | Version | Outputs the environment name from where application is running on, as defined by ASPNETCORE_ENVIRONMENT |
  | Level:u3 | Outputs the log message level as three-character uppercase (i.e ERR, INF, WRN, etc) |
  | RequestId | Request identifier generated by the framework or automatically the custom [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) |
  | CorrelationId | Correlation identifier generated by the action (request header, hosted service action handler) or automatically by the custom [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) |
  | ExceptionId | Exception identifier generated by the exception data or automatically by the custom [Lykke.Middlewares](https://bitbucket.org/lykke-snow/lykke.middlewares/src/dev/) |
  | Message:lj | Log message with embedded data in JSON format |
  | NewLine | Line break |
  | Exception | Exception message with stack trace |

  Some configuration options can be checked [here](https://github.com/serilog/serilog/wiki/Formatting-Output) and [here](https://github.com/serilog/serilog/wiki/Configuration-Basics)

### Platform specific configurations

All the configuration above can be set via ```appSettings.json```, but if you don't want to use it, below are some handful examples on how to do such based on where you are running it from

- If running the project from Visual Studio:  

  If the [user secrets](https://blogs.msdn.microsoft.com/mihansen/2017/09/10/managing-secrets-in-net-core-2-0-apps/) for the project are not provided via ```appSettings.json``` it can be configured from [secrets.json](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) like example below: 
  
  *NOTE*: File content should match the [expected required configuration](src/Chest/Program.cs?fileviewer=file-view-default#Program.cs-21).

  *NOTE* These secret values in example below are invalid
    ```json
    {
      "ConnectionStrings:Chest": "<chest-db-connection-string>",
      "ChestClientSettings:ApiKey": "<api-key-for-chest-service>"
    }
    ```

- If you are running the project from the command line:  

  If the [user secrets](https://blogs.msdn.microsoft.com/mihansen/2017/09/10/managing-secrets-in-net-core-2-0-apps/) for the project are not provided via ```appSettings.json``` it can be configured from the command line in either Windows or Linux. You can set the secrets using the following command from within the ```src/Chest``` folder:
  
  *NOTE*: You may need to run a ```dotnet restore``` before you try these commands.

  *NOTE*: Secrets provided should match the [expected required configuration](src/Chest/Program.cs?fileviewer=file-view-default#Program.cs-21).

  *NOTE* These secret values in example below are invalid

    ```cmd
      dotnet user-secrets set "ConnectionStrings:Chest" "<Chest DB connection string>"
    ```

- If running the project inside a container:  
  
  If the [user secrets](https://blogs.msdn.microsoft.com/mihansen/2017/09/10/managing-secrets-in-net-core-2-0-apps/) for the project are not provided via ```appSettings.json``` it can be configured from the [environment variables](https://docs.docker.com/compose/environment-variables/#the-env_file-configuration-option) used to run the docker container.
  To do this you need to create an `.env` file in the `src/Docker` folder and enter key/value pairs in the format `KEY=VALUE` for each secret.

  *NOTE*: File content should match the [expected required configuration](src/Chest/Program.cs?fileviewer=file-view-default#Program.cs-21).

  *NOTE* These secret values in example below are invalid

    ```cmd
      CHEST_CONNECTIONSTRING: <Chest DB connection string>
      CHEST_API_KEY: <Api key for Chest service>
    ```

### Add https enforcement for Chest

Set environment variables

  ```
    Kestrel__Certificates__Default__Path:</root/.aspnet/https/certFile.pfx>
    Kestrel__Certificates__Default__Password:<certificate password>
  ```

In order to map path of certificate we need to add additional volume to docker-compose.yml file

  ```
    volumes:
      - ./https/:/root/.aspnet/https/
  ``` 

Update appsettings.Deployment.json file and mention the https port
 
 ``` 
  "urls": "https://*:443;"
 ```

Configuration of secrets.json file in order to use https

  ```json
    "Kestrel": {
      "EndPoints": {
        "HttpsInlineCertFile": {
          "Url": "https://*:443",
          "Certificate": {
            "Path": "<path to .pfx file>",
            "Password": "<certificate password>"
          }
        }
      }
    }
  ```

Example of Dockerfile

  ```
    FROM microsoft/dotnet:2.1.5-aspnetcore-runtime AS base
    WORKDIR /app
    EXPOSE 443

    FROM microsoft/dotnet:2.1.403-sdk AS build
    COPY . ./
    RUN cp NuGet.*onfig /usr/local/share/NuGet.Config 2>/dev/null || :
    WORKDIR /src/Chest
    RUN dotnet build -c Release -r linux-x64 -o /app

    FROM build AS publish
    RUN dotnet publish -c Release -r linux-x64 -o /app

    FROM base AS final
    WORKDIR /app
    COPY --from=publish /app .
    ENTRYPOINT ["dotnet", "Chest.dll"]
  ```

## How to Use

A basic health check and version check can be performed by hitting this endpoint: `http://{chest-base-url}/api/isAlive`.
All endpoints are documented via Swagger which can be found under this URL: `http://{chest-base-url}/swagger/ui/`.

## How to Debug

### Using Visual Studio

Set the start-up project to ```Chest``` and launch it.
This will run the project directly using dotnet.exe. The application will listen on port 5011.

### Using Visual Studio Tools for Docker

Set the start-up project to ```docker-compose``` and launch it.
This will run the project inside a docker container running behind nginx. Nginx will listen on port 5011 and forward calls to the application.

### From the Command Line

Navigate to ```src/Chest``` folder and type ```dotnet run```.
You can also launch it with docker-compose command: Navigate to ```src/Docker``` and type ```docker-compose up```.
This will run the project directly using dotnet.exe without attaching the debugger. You will need to use your debugger of choice to attach to the dotnet.exe process.

# How to build docker image

This project contains a set of required files for a complete Docker image build, ready for usage. Only required input is a valid NuGet.config file with source for dependent libraries.

  Example of valid NuGet.config
  ```xml
  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    <packageSources>
      <add key="private-source" value="http://private-source.url/nuget/" />
    </packageSources>
  </configuration>
  ```

  With valid NuGet.config on your hands, you can simply copy it to workspace folder and run `./src/build`.

  Example of automation script
  ```cmd
  cd workspace/folder/
  cp original/path/for/NuGet.config .
  cd src/
  ./build
  ```

## How to Migrate from Postgres to MS Sql

Before doing the migration, the new version needs to be deployed. EF automatic migration based on Code First will take place, creating the objects on MS Sql database.

### Using Migration script

The `scripts/migratePostgresToMsSql.py` script can run in any python environment. It connects to source `Postgres` database and copy all the data to a target `MS Sql` database.

Steps to successfully run it:

  1. Open the `scripts/migratePostgresToMsSql.py` script in your preferred editor

  2. Change the connection variables setting ServerURL, Port, DatabaseName, UserName and Password:

    * postgresEngine = getSqlEngine('postgresql+psycopg2', 'postgres.server.url', '5432', 'dbName', 'username', 'password')
    * msSqlEngine = getSqlEngine('mssql+pyodbc', 'mssql.server.url', '1433', 'dbName', 'username', 'password', 'SQL+Server')

  3. Run the script and wait, it will take some minutes.

  4. Check the table `chest.tb_keyValueData` inside the new MS Sql database, it should have the same data as your old Postgres database

### Using other tools

There are a lot of other migration tools available out there, including the possibility to simply extract the data to a .csv file and import it on your new MS Sql database using the Import Wizard.

Feel free to choose the one that best suits your needs.

## How to run integration tests

* Stop `Chest` service
* Remove `Chest` connection string from user secrets
* Create an empty new database in sql server (probaly in your local dev machine's SQL Server)
* Correct `Chest` connection string in the `testsettings.json`
* Run the integration test
