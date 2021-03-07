# Nojira (ノジラ)

[![Code Analysis](https://github.com/fsegaud/nojira/actions/workflows/code.yml/badge.svg)](https://github.com/fsegaud/nojira/actions/workflows/code.yml)
[![Build](https://github.com/fsegaud/nojira/actions/workflows/build.yml/badge.svg)](https://github.com/fsegaud/nojira/actions/workflows/build.yml)
[![Release](https://img.shields.io/github/v/release/fsegaud/nojira?include_prereleases)](https://github.com/fsegaud/nojira/releases/tag/v0.5-dev)

Nojira is a software suite that allows remote logging.

It relies on .NET 4.6.1, and includes packages _NancyFX_, _SQLite-net_, _Newtonsoft.Json_ and their respective dependences.

## Nojira.Server

This is the process that will serve API request and web interface requests.

Log data are stored in a local SQLite database.

### Configuration

Configuration can be done by editing the _config.json_ file.

```json
{
  "Title": "Nojira Server",
  "BaseUri": "http://localhost:80",
  "MaxConnections": 16,
  "DatabasePath": "logs.db",
  "DatabasePrevPath": "logs-prev.db"
}
```

### Database

Tables are automatically created if the database is empty.

```sql
CREATE TABLE "Log" (
"Id" integer primary key autoincrement not null ,
"Timestamp" bigint ,
"MachineName" varchar(32) ,
"Type" varchar(32) ,
"Project" varchar(32) ,
"Tag" varchar(32) ,
"Message" varchar(1024) )
```

### API

```
<BaseUri>/log/{machine}/{type}/{project}/{tag}/{message*}
```

### Web interface

The web interface is accessible at `BaseUri` (by default: http://localhost/, default user account is `nojira:nojira`).

![web interface screenshot](README.md.files/web.png)

#### Query

The web interface offer the possibility to query logs based on specific conditions. 

```
key=value0[,value1,...][;key=value0[,value1,...];...]
```

Here is an example :

```
project=nojira; tag=client,test; type=info,warning,error
```

## Nojira.Admin

This utility allows administration task to be done, including managing the users that are allowed to access the front-end.

| Short arg | Long arg      | Parameters            | Description                               |
|-----------|---------------|-----------------------|-------------------------------------------|
| -h        | --help        |                       | Prompt the help.                          |
| -l        | --list-users  |                       | List the current users.                   |
| -a        | --add-user    | `username` `password` | Add a new user.                           |
| -d        | --delete-user | `username`            | Delete the user identified by `username`. |
| -c        | --clear-logs  |                       | Clears all logs from database.            |


As the example, the first command you should run:

```
Nojira.Admin -d nojira -a yourname yourpassword -l
```

## Nojira.Client

This is the C# client that send the HTTP requests.

```csharp
// Setup.
NojiraClient.Uri = "http://localhost:80";
NojiraClient.Project = "nojira";

// Logs.
NojiraClient.LogInfo("test", "test of an info message.");
NojiraClient.LogWarning("test", "test of an warning message.");
NojiraClient.LogError("test", "test of an error message.");
```

## Nojira.Utils

A Library that contains all classes that are common to the Nojira backend, such as `Config` and `Database`.

## Nojira.Test

A simple test program that makes use of the `Nojira.Client` to send request to the `Nojira.Server`.
