#!/usr/bin/env bash

set -e
docker-compose down
docker-compose up -d --build

docker-compose run --rm end-to-end-test bash -c "dotnet test"