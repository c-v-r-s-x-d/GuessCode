#!/bin/bash
javac /app/userdata/Main.java 2> /app/userdata/errors.txt
if [ $? -eq 0 ]; then
    timeout 5 java -cp /app/userdata Main < /app/userdata/input.txt
else
    cat /app/userdata/errors.txt
fi
