FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 3100
ENV ASPNETCORE_URLS=http://*:3100

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Incrementer/Incrementer.csproj", "Incrementer/"]
RUN dotnet restore "Incrementer/Incrementer.csproj"
COPY . .
WORKDIR "/src/Incrementer"
RUN dotnet build "Incrementer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Incrementer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Incrementer.dll"]