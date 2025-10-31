# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# csproj ve restore
COPY ./kiraz.com.csproj ./
RUN dotnet restore "kiraz.com.csproj"

# kaynaklarý kopyala (bin/obj .dockerignore ile dýþlanacak)
COPY . ./

# publish
RUN dotnet publish "kiraz.com.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
ENTRYPOINT ["dotnet", "kiraz.com.dll"]
