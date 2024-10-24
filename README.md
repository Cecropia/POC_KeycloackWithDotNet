# POC_KeycloackWithDotNet

---

### Development Environment Limitation

This application can be run on macOS; however, its development is limited to computers running Windows operating systems due to issues with NuGet central package version management in Visual Studio for Mac. While Visual Studio for Mac used central package version management, package versions must be defined using the `PackageVersion` item rather than within `PackageReference` items. Microsoft withdrew support for Visual Studio for Mac at the end of August 2023, which further limits the development on macOS platforms. 

For full development capabilities, it is recommended to use Visual Studio on Windows.

---

# KeyCloak Setup
1. Install [Docker](https://www.docker.com/products/docker-desktop/)
1. Create the operational network with the following command `docker network create keycloak_net`
1. From [Keycloak DockerFile directory](/KeyCloak/) run : `docker-compose --env-file env up --build -d`
   1. user name and password can be edited form the [env file](/KeyCloak/env)
   1. Keycloak version can be updated from the [env file](/KeyCloak/env)
   1. Seed Realm, Clients and Users can be edited from the [realm json file](/KeyCloak/Multiplied.json) or a different json file can be reference from the [env file](/KeyCloak/env)
1. Log in into [Keycloak](http://localhost:8080/admin)

# DotNet Setup
1. From [.NET DockerFile directory](/KeyCloakIntegration/) run: `docker build -t dotnet_keycloak-image -f DockerFile .`
1. Start app with the following command `docker run  --net keycloak_net --name dotnet_app  -d -p 8081:8080 dotnet_keycloak-image`
1. Navigate to [app](http://localhost:8081/)

# GitHub Actions

This repository uses [GitHub Actions](https://github.com/features/actions) for Continuous Integration (CI) to ensure code quality and project health.

## Active GitHub Actions

Currently, the following workflows are active:

1. **Build and Test Workflow**:
   - **Trigger**: This workflow runs on every push and pull request to the `main` branch.
   - **Purpose**: It automatically builds the solution and runs all unit tests using the `.NET Core` framework.
   - **Command**: The workflow uses `dotnet test` with the filter `FullyQualifiedName~UnitTests` to ensure only unit tests are executed.

   **Why it matters**:  
   Continuous testing is critical to the development process because it provides immediate feedback on the health of the codebase. Every change is verified automatically, helping detect bugs early and reducing the risk of breaking production code. By keeping the tests up-to-date, you ensure the project remains maintainable and scalable.

## Benefits of CI with GitHub Actions

- **Early Bug Detection**: By running tests on every push and pull request, developers are alerted to issues early, preventing regressions and maintaining code quality.
- **Consistent Builds**: Ensures that the project builds consistently across environments and catches build issues before they make it to production.
- **Automated Testing**: Automates the process of running tests, which saves time and encourages developers to write and maintain a comprehensive test suite.
- **Improved Code Quality**: Automated workflows provide continuous assurance that code adheres to defined quality standards, including passing tests and compiling correctly.

## How to View CI Results

1. Navigate to the "Actions" tab of the repository to view recent workflow runs.
2. Click on a specific workflow run to view the detailed output of the build and test process.

These CI workflows are essential for maintaining project health, ensuring that code changes are reliable, and providing a strong foundation for future development.
