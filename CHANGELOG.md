# Changelog

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