# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["eTutoring.csproj", "./"]
RUN dotnet restore "./eTutoring.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "eTutoring.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eTutoring.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "eTutoring.dll"]