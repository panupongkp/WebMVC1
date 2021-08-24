
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update && apt-get install -y libgdiplus

WORKDIR /app
EXPOSE 80
EXPOSE 443
 
 FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

 
WORKDIR /src

COPY ["/WebMVC1.sln", "/src/"]
COPY ["/WebMVC1/WebMVC1.csproj", "/src/WebMVC1/"]

RUN dotnet restore "/src/" 
COPY . .
WORKDIR "/src/"

RUN dotnet build "/src/WebMVC1/WebMVC1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "/src/WebMVC1/WebMVC1.csproj" -c Release -o /app

FROM base AS final
# echo "Asia/Bangkok" > /etc/timezone
ENV TZ Asia/Bangkok
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet" , "WebMVC1.dll"] 