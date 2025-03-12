# AIDA - AI Developer Assistant

## Overview

AIDA (AI Developer Assistant) is an ASP.NET Web API application built using .NET 9. This application leverages AI to convert requirements into Pull Requests, streamlining the development process and enhancing productivity. It includes a subscription management system integrated with Stripe.

## Features

- Convert requirements into Pull Requests using AI
- Built with ASP.NET Web API and .NET 9
- Integrated subscription management with Stripe
- Comprehensive testing infrastructure (unit, integration)
- React-based frontend with TypeScript, Vite, and Tailwind CSS

## Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio or any other compatible IDE
- Node.js and npm (for the frontend)
- Docker and Docker Compose (optional, for containerized setup)
- Stripe account (for subscription features)

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/patrob/aida-example-repo.git
   ```
2. Navigate to the project directory:
   ```sh
   cd aida-example-repo
   ```
3. Restore the dependencies:
   ```sh
   dotnet restore
   ```
4. Set up the frontend:
   ```sh
   cd src/Aida.Api/ClientApp
   npm install
   ```

### Configuration

#### Stripe Configuration
To enable the subscription features, you need to set up your Stripe credentials:

1. Create a `.env` file in the project root directory or configure user secrets in Visual Studio
2. Add the following settings (replace with your actual Stripe keys):
   ```
   Stripe__ApiKey=your_stripe_secret_key
   Stripe__WebhookSecret=your_stripe_webhook_secret
   ```

You can also add these settings to `appsettings.Development.json`:
```json
{
  "Stripe": {
    "ApiKey": "your_stripe_secret_key",
    "WebhookSecret": "your_stripe_webhook_secret"
  }
}
```

### Running the Application

#### Option 1: Using .NET CLI

1. Build the application:
   ```sh
   dotnet build
   ```
2. Run the application:
   ```sh
   dotnet run --project src/Aida.Api/Aida.Api.csproj
   ```

#### Option 2: Using Visual Studio

1. Open the solution file (`Aida.sln`) in Visual Studio
2. Configure the Stripe settings using User Secrets
3. Set the Aida.Api project as the startup project
4. Press F5 or click the "Start" button

#### Option 3: Using Docker

1. Build and run the Docker containers:
   ```sh
   docker-compose up --build
   ```

The application will be available at:
- HTTP: http://localhost:7006
- HTTPS: https://localhost:7007

### Testing

The project includes several types of tests:

#### Unit Tests
```sh
dotnet test tests/Aida.Api.UnitTests/Aida.Api.UnitTests.csproj
```

#### Integration Tests
```sh
dotnet test tests/Aida.Api.IntegrationTests/Aida.Api.IntegrationTests.csproj
```

For integration tests, the application uses mock services including a Stripe API mock to avoid making real API calls.

### Frontend Development

For frontend development:

1. Navigate to the ClientApp directory:
   ```sh
   cd src/Aida.Api/ClientApp
   ```

2. Start the development server:
   ```sh
   npm run dev
   ```

## Project Structure

- `src/Aida.Api`: Main API project with controllers, middleware, and services
- `src/Aida.Core`: Core business logic and domain models
- `tests/`: Unit, integration, and testing utilities
- `terraform/`: Infrastructure as code for deployment

## Troubleshooting

### Common Issues

1. **Stripe API errors**: Ensure you've correctly configured the Stripe API keys in your application settings.

2. **Database connection issues**: The application uses an in-memory database by default for development. No additional setup is required.

3. **Frontend build errors**: Make sure you've installed all the necessary npm dependencies by running `npm install` in the ClientApp directory.

## Contributing

Please see the [CODEOWNERS](./CODEOWNERS) file for information on project ownership and contribution guidelines.
