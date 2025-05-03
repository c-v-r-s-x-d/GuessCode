#!/bin/bash
cp /app/userdata/input/* /app/userdata/output/
cd /app/userdata/output/
g++ code.cpp -o code.out 2> errors.txt
if [ $? -eq 0 ]; then
    timeout 5 ./code.out < input.txt
else
    cat errors.txt
fi
