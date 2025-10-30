# Build a�amas�
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyas�n� kopyala ve restore et
COPY ../kiraz.com/*.csproj ./ 
RUN dotnet restore

# Geri kalan dosyalar� kopyala ve build et
COPY ../kiraz.com/. ./
RUN dotnet publish -c Release -o /app/publish

# Runtime a�amas�
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render 10000 portunu kullan�r
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "kiraz.com.dll"]
