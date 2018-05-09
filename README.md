# How to write testable web applications - sample project
Sample project for web applications testing lessons.

### **Old version**: *If you are looking for old version built on ASP.NET WebApi2 (.NET Framework 4.6.2) and AngularJS 1.6 check branch named [old](https://github.com/Katulus/WebApplictionTestingExample/tree/old)*

## Prerequisites
You need:
- [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/windows)
- [Visual Studio 2017](https://www.visualstudio.com) or [Visual Studio Code](https://code.visualstudio.com/download) to edit the code, debug etc.
- [Node.js](https://nodejs.org/en/)
- [Angular CLI](https://cli.angular.io)
- [Chrome browser](https://www.google.com/chrome/)

## Quick start
1. Clone this repository
1. Go to `src\Server\` and run `dotnet run` from command line. This compiles and starts server part of the application. (`dotnet` is part of .NET Core SDK)
1. Go to `src\Client\ClientApp\` and run `npm install` to install NPM packages. Then run `ng serve`. This compiles and starts client part of the application. (`ng` is part of Angular CLI)
1. Navigate to http://localhost:4200 and you should see running application

## Running tests
### Server
Server tests can be run from Visual Studio Test Explorer or using `dotnet test` command. Visual Studio way is easy, tests just apper in Test Eplorer after build. (**Note:** `EndToEndApiTests` need running server to work)

To run tests from command line you need to:
- **Unit tests:** Go to `src\tests\ServerTests\` and run `dotnet test`. It runs unit tests for server part.
- **E2E tests:** These tests **need running server** so run server using `dotnet run` in `src\Server\` first. Then go to `src\tests\EndToEndApiTests\` and run `dotnet test`. It runs E2E tests for server part. 

**Debugging tests**

You can debug server tests from Visual Studio. Just put breakpoints to code and, right-click on test in *Test Explorer* and select *Debug selected test*

### Client
Client tests can be run using `Angular CLI`.
- **Unit tests:** Go to `src\Client\ClientApp\` and run `ng test`. This command starts Chrome browser and executes client unit tests in it. You'll also see tests results in command line where you executed the command.
- **E2E tests:** These tests **need running server** so run server using `dotnet run` in `src\Server\` first. Then go to `src\Client\ClientApp\` and run `ng e2e`. This command starts Chrome browser and executes E2E tests by interacting with application UI.

**Debugging tests**

You can debug client tests from Visual Studio Code. See [Chrome Debugging with Angular CLI](https://github.com/Microsoft/vscode-recipes/tree/master/Angular-CLI) for details how to set it up.