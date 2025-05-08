#!/bin/bash
mkdir -p /app/userdata/project

cd /app/userdata/project
dotnet new console --output . --force > /dev/null 2>&1

cp /app/userdata/input/code.cs /app/userdata/project/Program.cs

dotnet build --nologo > /app/userdata/build.log 2>&1

if [ $? -eq 0 ]; then
    timeout 5 dotnet run --no-build < /app/userdata/input/input.txt
else
    cat /app/userdata/build.log
fi