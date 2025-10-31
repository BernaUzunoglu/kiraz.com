# syntax=docker/dockerfile:1

# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece csproj'u kopyala ve restore et (cache dostu)
COPY ./kiraz.com/kiraz.com.csproj ./
RUN dotnet restore "kiraz.com.csproj"

# Proje kaynaklarýný kopyala
COPY ./kiraz.com/. ./

# Publish
RUN dotnet publish "kiraz.com.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Render 'PORT' veriyor; sabit port kullanmayýn
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE 10000

ENTRYPOINT ["dotnet", "kiraz.com.dll"]
