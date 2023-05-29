FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /src
COPY ./*.csproj .
RUN dotnet restore -r linux-amd
COPY . .
RUN dotnet publish -r linux-amd -c Release -o /publish

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Earthquake.Emergency.dll"]