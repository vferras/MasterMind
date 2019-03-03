FROM microsoft/dotnet:2.2-sdk AS dotnet-builder 
ARG main

COPY ${main}*.csproj ${main}

RUN dotnet restore ${main}

COPY ${main} ${main}

WORKDIR ${main}
RUN dotnet build
