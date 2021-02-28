# Nojira (ノジラ)

[![Build](https://github.com/fsegaud/nojira/actions/workflows/build.yml/badge.svg)](https://github.com/fsegaud/nojira/actions/workflows/build.yml)

Nojira is a software suite that allows remote logging.

It relies on .NET 4.6.1, and includes packages _NancyFX_, _SQLite-net_, _Newtonsoft.Json_ and their respective dependences.

## nojira-daemon

This is the process that will serve API request and web interface requests.

Log data are stored in a local SQLite database.

### Configuration

Configuration can be done by editing the _config.json_ file.
```json
{
  "Title": "nojira-v0.2-dev",
  "BaseUri": "http://localhost:1410",
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
"Project" varchar(32) ,
"Type" varchar(16) ,
"Project" varchar(32) ,
"Tag" varchar(64) ,
"Message" varchar(256) )
```

### API

```
BaseUri/log/{machine}/{type}/{project}/{tag}/{message*}
```

### Web interface

The web interface is accessible at `BaseUri` (by default: http://localhost:1410/).
![web interface screenshot](README.md.files/web.png)

## nojira-client

This is the C# client that send the HTTP requests.
```csharp
// Setup.
NojiraClient.Uri = "http://localhost:1410";
NojiraClient.Project = "nojira";

// Logs.
NojiraClient.LogInfo("test", "test of an info message.");
NojiraClient.LogWarning("test", "test of an warning message.");
NojiraClient.LogError("test", "test of an error message.");
```

## nojira-test

A simple test program that makes use of the `nojira-client` to send request to the `nojira-daemon`.
