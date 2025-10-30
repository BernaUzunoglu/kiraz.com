# Build a�amas�
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Proje dosyas�n� kopyala ve restore et
COPY kiraz.com/*.csproj ./kiraz.com/
RUN dotnet restore ./kiraz.com/kiraz.com.csproj

# Geri kalan dosyalar� kopyala ve build et
COPY . .
WORKDIR /app/kiraz.com
RUN dotnet publish -c Release -o /app/out

# Runtime a�amas�
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Render i�in port ayar�
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "kiraz.com.dll"]
