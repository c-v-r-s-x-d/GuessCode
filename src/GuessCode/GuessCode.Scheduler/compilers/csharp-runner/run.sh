#!/bin/bash
mkdir -p /app/project
cp /app/userdata/code.cs /app/userdata/project/Program.cs

cd /app/userdata/project
dotnet new console --output . --force > /dev/null 2>&1
dotnet build --nologo > /app/userdata/build.log 2>&1

if [ $? -eq 0 ]; then
    timeout 5 dotnet run --no-build < /app/userdata/input.txt
else
    cat /app/userdata/build.log
fi
