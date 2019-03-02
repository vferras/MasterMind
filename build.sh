#!/usr/bin/env bash
docker stop $(docker ps -qa)

set -e
docker-compose down
docker-compose up -d --build