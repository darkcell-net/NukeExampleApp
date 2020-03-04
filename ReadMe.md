# Nuke Example Application

A simple web API example application which does nothing, it exists to show how Nuke can be used for the beginnings of a CI/CD framework.

## Building the Application

### Steps

Execute `dotnet run` in the `build` folder to build, test and package the application.

If you wish to use specific targets execute `dotnet run <Target> -skip` in the build folder.
Valid targets are `Clean`, `Compile`, `Test` and `Package` (the default).