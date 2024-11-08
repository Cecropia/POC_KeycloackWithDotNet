# POC_KeycloackWithDotNet

---

### Development Environment Limitation

This application can be run on macOS; however, its development is limited to computers running Windows operating systems due to issues with NuGet central package version management in Visual Studio for Mac. While Visual Studio for Mac used central package version management, package versions must be defined using the `PackageVersion` item rather than within `PackageReference` items. Microsoft withdrew support for Visual Studio for Mac at the end of August 2023, which further limits the development on macOS platforms. 

For full development capabilities, it is recommended to use Visual Studio on Windows.

---
# Prerequisites
1. [Visual Studio](https://visualstudio.microsoft.com/)
1. Node
   1. Download NVM installer for node version managment from [repository](https://github.com/coreybutler/nvm-windows/releases)
   1. Open a new Powershell terminal and run `nvm install lts`
   1. At the end of the command run an instruction like `nvm use version` will be prompted execute it
   1. close current Powershell terminal and open a new one and verify node version with `node -v` it should output a node version 


# Environment Setup
1. Install [Docker](https://www.docker.com/products/docker-desktop/)
1. From [Docker compose folder](/Docker/) run : `docker-compose --env-file env up --build -d`
   1. user name and password for Keycloak can be edited form the [env file](/Docker/env)
   1. Keycloak version can be updated from the [env file](/Docker/env)
   1. Seed Realm, Clients and Users can be edited from the [realm json file](/Docker/Multiplied.json) or a different json file can be reference from the [env file](/Docker/env)
1. Log in into [Keycloak](http://localhost:8080/admin) Use the user **admin** password **admin** 
1. Navigate to [app](http://localhost:8081/), In order to login in the app use the user **johndoe** password **jhondoe123**

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
1. Click on a specific workflow run to view the detailed output of the build and test process.

These CI workflows are essential for maintaining project health, ensuring that code changes are reliable, and providing a strong foundation for future development.


### Creating a Client in a Keycloak Realm

To set up a client in Keycloak, follow these steps to create either a Machine-to-Machine (M2M) client or a standard user-facing client with regular login functionality.

#### Steps to Create a Client

1. **Access the Keycloak Admin Console**: Navigate to your Keycloak admin console and select the desired realm.

1. **Create a New Client**:
   - Go to **Clients** in the sidebar.
   - Click **Create** and provide a unique **Client ID**. Choose a descriptive name to indicate the client’s purpose (e.g., "MyAppAPI" for M2M, "MyAppWeb" for user logins).

#### Machine-to-Machine (M2M) Clients

Machine-to-machine clients are used for server-to-server communication, where the client authenticates using its credentials rather than relying on a user login flow. The setup includes enabling client authentication and specifying the audience scope.

1. **Client Authentication**: Under the **Settings** tab, make sure that **Service Accounts Enabled** is set to **ON**. This will allow the client to authenticate itself using client credentials (e.g., client ID and secret).
1. **Scope & Audience:** Define the required audience and specify the scopes that this client can access within your application. Follow the steps  [## How to create an audience]

#### Regular Login Clients

For user-facing clients, such as web applications, the primary configuration involves enabling browser-based authentication without client credentials or specialized audience scopes.

1. **Disable Client Authentication**:
   - Since users log in directly, there is no need for the client to authenticate itself. **Service Accounts Enabled** should be set to **OFF**.


## How to create an audience 

An audience in Keycloak is a specific identifier that can be included in access tokens. This allows you to control which services or applications can accept a particular token, enhancing security and fine-grained access control.

To create an audience for a client in Keycloak, which we’ll call `untrusted-audience`, follow these steps:

### Step 1: Log in to Keycloak Admin Console
1. Open the Keycloak Admin Console by navigating to `http://<keycloak-host>:8080` in your browser.
1. Log in with an admin account.

### Step 2: Create or Select a Realm
1. In the left sidebar, select the **Realm** where you want to create the audience.
1. If you need a new realm, you can create one by selecting **Add Realm** and entering a name.

### Step 3: Create a Client Scope
1. Click **Client Scopes** tab.
1. Click **Create Client Scopes** to add a new client scope.
1. Give it a name, e.g., `untrusted-audience`.
1. Set Type to `Optional`.

### Step 4: Create an Audience Mapper:
   - Go to the **Mappers** tab of your client scope.
   - Click **Configure a new mapper**.
   - Select **Audience**.
   - Complete your audience **Name**.
   - In the **Included Custom Audience** field, enter the desired audience value, such as `your-api-endpoint`.
   - Enable the **Add to Access Token** checkbox.

### Step 5:  **Assign the Client Scope to Your Client:
   - Go to the **Clients** list.
   - Select **Your Client ID**.
   - Go to the **Client Scopes** tab of your client.
   - Add the newly created client scope  ()`untrusted-audience`) to your client.

**Understanding the Process:**

- **Client Scope:** This defines a logical grouping of permissions. In this case, we're creating a scope solely for the purpose of adding an audience to tokens.
- **Audience Mapper:** This maps a specific value (the audience) to the access token. By enabling the "Add to Access Token" option, the specified audience will be included in the `aud` claim of the token.

## How to Use the Audience:

**On the Resource Server:**
   - include the scope variable in you login request.
   - When your resource server (e.g., an API) receives an access token, it can validate the `aud` claim.
   - If the `aud` claim matches the expected audience (in our case, `your-api-endpoint`), the token is considered valid for that specific resource. 

## Additional Considerations:

- **Multiple Audiences:** You can add multiple audiences to a single client by creating additional audience mappers.
- **Dynamic Audiences:** For more advanced scenarios, you might consider using dynamic client scopes or other Keycloak features to generate audiences based on specific conditions.
- **Security Best Practices:** Always validate tokens on your resource servers to ensure they are valid and intended for your specific service.

## Summary
These steps create a client called `untrusted-audience` and configure it so that when tokens are issued, they contain the `aud` claim with `untrusted-audience`. This enables services to validate that tokens are specifically intended for the `untrusted-audience` client.