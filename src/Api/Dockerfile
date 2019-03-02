# DOTNET STAGE
FROM microsoft/dotnet:2.2-sdk AS dotnet-builder 
ARG main
ARG test

COPY ${main}*.csproj ${main}
COPY ${test}*.csproj ${test}

RUN dotnet restore ${main}
RUN dotnet restore ${test}

COPY ${main} ${main}
COPY ${test} ${test}

RUN dotnet test ${test}
RUN dotnet publish ${main} -c Release -o /build

# RUNTIME IMAGE STAGE
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=dotnet-builder /build .
ENTRYPOINT [ "dotnet", "Api.dll"]
