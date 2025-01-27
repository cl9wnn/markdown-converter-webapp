FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 41301
EXPOSE 44352

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MarkdownProcessor/MarkdownProcessor/Markdown.csproj", "MarkdownProcessor/MarkdownProcessor/"]
COPY ["WebApp/API/API.csproj", "WebApp/API/"]
COPY ["WebApp/Application/Application.csproj", "WebApp/Application/"]
COPY ["WebApp/Core/Core.csproj", "WebApp/Core/"]
COPY ["WebApp/Persistence/Persistence.csproj", "WebApp/Persistence/"]

RUN dotnet restore "MarkdownProcessor/MarkdownProcessor/Markdown.csproj"
RUN dotnet restore "WebApp/API/API.csproj"

COPY . .

WORKDIR "/src/MarkdownProcessor/MarkdownProcessor"
RUN dotnet build "Markdown.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/WebApp/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]