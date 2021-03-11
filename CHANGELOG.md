# Changelog

## Unreleased - ????-??-??
## Added
- [server] Added an adminitration page in the web UI, only accessible to admin users.
- [utils] Added `Guid` and `Admin` fields in the `User` table of the database (for forms authentication and admin page access).
- [utils] Added a `RequireAuth` field in the configuration file to allow access to the log page without logging.
## Changed
- [server] Revamped the Web UI.
- [server] Basic authentication has been replaced with form authentication.
- [server] Added an option to disable authentication in the configuration.
- [server] Added a subtitle field in the configuration.
- [server] Default account password is now randomized in release builds.

## v0.5-dev - 2021-03-07
### Added
- [server] The Nojira server now user basic authentication to let users access the web interface.
- [admin] A new console application `Nojira.Admin` provides functions to manage users.
- [utils] `Database` and `Config` now reside in their specific assembly: `Nojira.Utils`.
- [utils] Added functions to manager users and hash passwords.
### Changed
- [server] The Nancy view/model system is now used to generate the html code instead of pasta string builders. See the _Nojira.Server/Views_ directory.
- [server] Hitting que q key will qui the application.
- [global] All release files are now copied to the _.release/unreleased_ directory as a post-build step.

## v0.4-dev - 2021-03-04
### Added
- [server] Added an icon to the .exe file.
- [server] Added a query system to filter logs.
### Changed
- [server] Renamed `Nojira.Daemon` to `Nojira.Server` (namespace, binaries, directories, vs files).
- [server] Changed message maximum length to 1024 instead of 256 characters.
- [server] Default listen port is no `80` instead of `1410`.
- [server] Changed filter query key-value separator from `:` to `=`.
- [global] Renamed Visual Studio project files, diretories and binary names to match standard .Net nomenclature.
- [global] Updated assemblies info.

## v0.3-dev - 2021-03-01
### Added
- [daemon] A log filter based on machine names.
### Changed
- [daemon] Changed the web interface font family and title size.

## v0.2-dev - 2021-02-28
### Added
- [daemon] Added a new database field `Log.MachineName` and the corresponding `machine` API field.
- [client] `Environment.MachineName` is now sent as log info.
### Changed
- [daemon] Database field `Log.Project` is now 32 characters long instead of 16.
- [global] Excluded _AssemblyInfo.cs_ from StyleCop.
### Deprecated
- [client] `Nojira.Client.RemoteLog` class has been renamed to `Nojira.Client.NojiraClient`.


## v0.1-dev - 2021-02-28
### Added
- [daemon] `nojira-daemon` to answer to API requests and server HTTP requests.
- [client] `nojira-client` to send log requests.
- [test] `nojira-test` to test `nojira-daemon` and `nojira-client`.