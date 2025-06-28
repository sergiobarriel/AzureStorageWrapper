#!/bin/bash

# Instructions to run this script:
# 1. Make sure you have Azurite installed. You can install it using npm:
#    npm install -g azurite
# 2. Make sure you have the .NET SDK installed and the `dotnet` command available in your PATH.
# 3. Give execution permissions to this file with the following command:
#    chmod +x run-test.sh
# 4. Run the script with the following command:
#    ./run-test.sh

echo "Starting Azurite..."
azurite -s -l /path/to/azurite -d /path/to/azurite/debug.log &

echo "Running tests..."
dotnet test
