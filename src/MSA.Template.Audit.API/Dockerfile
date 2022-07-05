FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MSA.Template.Audit.API/MSA.Template.Audit.API.csproj", "MSA.Template.Audit.API/"]
RUN dotnet restore "src/MSA.Template.Audit.API/MSA.Template.Audit.API.csproj"
COPY . .
WORKDIR "/src/MSA.Template.Audit.API"
RUN dotnet build "MSA.Template.Audit.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSA.Template.Audit.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSA.Template.Audit.API.dll"]
