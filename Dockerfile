# FROM mcr.microsoft.com/dotnet/aspnet:6.0
# COPY bin/Release/net6.0/publish/ app/
# WORKDIR /app
# ENTRYPOINT ["dotnet", "FinanceAdvisorApi.dll"]

# Use official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything and build/publish app
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use runtime image for smaller image size
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port and entrypoint
EXPOSE 80
ENTRYPOINT ["dotnet", "FinanceAdvisorApi.dll"]
