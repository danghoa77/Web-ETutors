# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

# Use the SDK image for development
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /src
COPY ["eTutoring.csproj", "./"]
RUN dotnet restore "./eTutoring.csproj"
COPY . .
WORKDIR "/src/."

# Start the app in development mode
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://+:5001"]