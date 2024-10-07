# StitchTrack

StitchTrack is a full-stack inventory management system built using ASP.NET Core and React. This application enables users to effectively manage crochet-related data such as materials (yarn, safety eyes, stuffing) and finished products. With comprehensive features for tracking, product creation, and material usage management, StitchTrack helps streamline inventory and project organization. The application includes robust API testing using xUnit to ensure high-quality functionality.

## Key Features

- **Inventory Management**: Manage and track inventory of yarn, safety eyes, stuffing, and other crochet materials.
- **Finished Product Tracking**: Create and manage finished crochet products, including the materials used and their quantities.
- **Detailed Material Information**: Track specific details for materials like fiber type, weight, color, and size.
- **API Integration**: Well-defined APIs to interact with frontend and external systems.
- **Unit Testing**: Comprehensive unit tests implemented using xUnit.

## Prerequisites

- **.NET SDK 8.0**
- **Node.js v20.16**
- **Microsoft SQL Server 2022**
  - To create the server with Docker:
    ```sh
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<yourPassword>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
    ```

## Getting Started

### Server Setup

1. Navigate to the `StitchTrackAPI` directory:
   ```sh
   cd src/StitchTrackAPI
   ```
2. Set up the `ConnectionString` user secret:
   ```sh
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionString" "Server=localhost; Database=<yourDatabase>; User Id=sa; Password=<yourPassword>; TrustServerCertificate=True"
   ```
3. Run the migrations to initialize the database:
   ```sh
   dotnet ef database update
   ```
4. Start the API server:
   ```sh
   dotnet run
   ```
5. View the Swagger UI at `https://localhost:<port>/swagger` to explore the API endpoints.

### Client Setup

1. Navigate to the `frontend` directory:
   ```sh
   cd frontend
   ```
2. Install frontend dependencies:
   ```sh
   npm install
   ```
3. Start the frontend server:
   ```sh
   npm run start
   ```

### Running Tests

To run the tests for the API, use the following command:

```sh
dotnet test
```

## Project Structure

```
StitchTrack/
├── .github/                   # GitHub configuration files
├── sql-code/                  # SQL scripts for initializing the database
├── src/                       # Source code for the project
│   ├── FrontEnd/              # React frontend for user interface
│   ├── StitchTrackAPI/        # Backend API built with ASP.NET Core
│   └── StitchTrackAPI.Tests/  # Unit tests implemented using xUnit
├── .gitignore                 # Git ignore file
├── README.md                  # Project documentation
└── StitchTrack.sln            # Visual Studio solution file
```
