FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY . . 
RUN dotnet restore 
RUN dotnet publish FreeIntegration.Api.csproj -c Release -o out
WORKDIR out
ENV ASPNETCORE_URLS="http://*:40555" 
ENTRYPOINT ["dotnet","FreeIntegration.Api.dll"]

