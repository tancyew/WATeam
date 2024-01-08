# Use the official .NET 8 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy the project files to the container
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET 8 Runtime image as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory inside the container
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build-env /app/out .

# Expose both ports 80 and 443
EXPOSE 80
EXPOSE 443

# Command to run the application
ENTRYPOINT ["dotnet", "WATeam.Api.dll"]