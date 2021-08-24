#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebMVC1/WebMVC1.csproj", "WebMVC1/"]
RUN dotnet restore "WebMVC1/WebMVC1.csproj"
COPY . .
WORKDIR "/src/WebMVC1"
RUN dotnet build "WebMVC1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebMVC1.csproj" -c Release -o /app/publish

FROM base AS final
ENV TZ Asia/Bangkok
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebMVC1.dll"]
