# Changelog

## Unreleased - ????-??-??
### Added
- [daemon] Added an icon to the .exe file.
- [daemon] Added a query system to filter logs.
### Changed
- [daemon] Changed message maximum length to 1024 instead of 256 characters.
- [global] Updated assemblies info.
- [global] Renamed Visual Studio project files, diretories and binary names to match standard .Net nomenclature.

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