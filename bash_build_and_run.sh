#!/bin/bash
# dotnet build

echo "dotnet build!"

bash ./bash_build.sh



cd "./src/GraphQLPlayTokenExchangeOnlyApp"
dotnet run --no-build --launch-profile GraphQLPlayTokenExchangeOnlyApp_ubuntu

