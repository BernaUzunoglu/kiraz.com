# syntax=docker/dockerfile:1

# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece csproj'u kopyala ve restore et (cache dostu)
COPY kiraz.com/kiraz.com.csproj kiraz.com/
RUN dotnet restore kiraz.com/kiraz.com.csproj

# Tüm kaynaklarý kopyala ve publish et
COPY . .
RUN dotnet publish kiraz.com/kiraz.com.csproj -c Release -o /publish /p:UseAppHost=false

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publish .

# Render 'PORT' ortam deðiþkeni saðlar; onu dinleyelim
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# EXPOSE zorunlu deðil ama bilgi amaçlý býrakýlabilir
EXPOSE 10000

ENTRYPOINT ["dotnet", "kiraz.com.dll"]
