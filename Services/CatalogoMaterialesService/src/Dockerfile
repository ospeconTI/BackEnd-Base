FROM  mcr.microsoft.com/dotnet/sdk:5.0.103 AS build-env
WORKDIR /app
RUN mkdir Application
RUN mkdir Domain
RUN mkdir Infrastructure

# Copy csproj and restore as distinct layers
COPY Application/*.csproj ./Application
COPY Domain/*.csproj ./Domain
COPY Infrastructure/*.csproj ./Infrastructure
WORKDIR /app/Application
RUN dotnet restore

# Copy everything else and build
WORKDIR /app
COPY . ./
WORKDIR /app/Application
RUN dotnet publish -c Release -o out

# Build runtime image
FROM  mcr.microsoft.com/dotnet/sdk:5.0.103
WORKDIR /app
COPY --from=build-env /app/Application/out .
ENTRYPOINT ["dotnet", "Application.dll"]