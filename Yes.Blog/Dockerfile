#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base


WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Yes.Blog/Yes.Blog.csproj", "Yes.Blog/"]
#RUN dotnet nuget remove source nuget.org
#RUN dotnet nuget add source https://nuget.cdn.azure.cn/v3/index.json -n azure.nuget
#RUN dotnet nuget remove source nuget.org
#RUN dotnet nuget add source https://mirrors.cloud.tencent.com/nuget/ -n tencent_nuget
RUN dotnet restore "./Yes.Blog/./Yes.Blog.csproj"
COPY . .
WORKDIR "/src/Yes.Blog"
RUN dotnet build "./Yes.Blog.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Yes.Blog.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yes.Blog.dll"]