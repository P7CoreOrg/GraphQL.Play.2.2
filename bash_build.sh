#!/bin/bash
# dotnet build

echo "dotnet build!"

dotnet clean ./src/GraphQL-AspNetCore-2.2.sln

dotnet restore ./src/GraphQL-AspNetCore-2.2.sln
dotnet build ./src/GraphQL-AspNetCore-2.2.sln
dotnet test ./src/GraphQL-AspNetCore-2.2.sln --no-build  
