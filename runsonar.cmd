dotnet tool install -g coveralls.net
dotnet tool install --global dotnet-sonarscanner
dotnet tool install --global dotnet-testx
set /p token=<sonar.txt
del ".\coverage\results.xml"
md ".\coverage"

dotnet sonarscanner begin /k:"P7CoreOrg_GraphQL.Play.2.2"  /v:"1.0.0" /o:"p7coreorg" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="%token%" /d:sonar.language="cs" /d:sonar.exclusions=" **/*.js,**/js/**/*,**/bin/**/*,**/obj/**/*,**/*jquery*"   /d:sonar.cs.opencover.reportsPaths="%cd%\coverage\results.xml"
dotnet restore .\src\GraphQL-AspNetCore-2.2.sln
dotnet build .\src\GraphQL-AspNetCore-2.2.sln

dotnet testx --discover-projects "XUnitServer*.csproj" 
rem csmacnz.Coveralls --opencover --input .\coverage\results.xml --repoToken wkx6b1oX3u3tEY1XZxi1wfv3XXoDj1sMa --serviceName travis-pro

rem dotnet test ./src/XUnitServer_App_Identity/XUnitServer_App_Identity.csproj   --logger trx --collect "Code coverage"   /p:CollectCoverage=true /p:CoverletOutputFormat=\"json\" /p:CoverletOutput=..\..\test-results\ /p:MergeWith=..\..\test-results\coverage.json
rem dotnet test ./src/XUnitServer_OAuth2/XUnitServer_OAuth2.csproj               --logger trx --collect "Code coverage"   /p:CollectCoverage=true /p:CoverletOutputFormat=\"json\" /p:CoverletOutput=..\..\test-results\ /p:MergeWith=..\..\test-results\coverage.json
rem dotnet test ./src/XUnitServer_TokenExchange/XUnitServer_TokenExchange.csproj --logger trx --collect "Code coverage"   /p:CollectCoverage=true /p:CoverletOutputFormat=\"json\" /p:CoverletOutput=..\..\test-results\ /p:MergeWith=..\..\test-results\coverage.json

rem dotnet test ./src/XUnitServer_App_Identity/XUnitServer_App_Identity.csproj --logger trx --collect "Code coverage"   /p:CollectCoverage=true /p:CoverletOutputFormat=\"json\" /p:CoverletOutput=.\coverage\coverage.json
rem dotnet test ./src/XUnitServer_OAuth2/XUnitServer_OAuth2.csproj --logger trx --collect "Code coverage"               /p:CollectCoverage=true /p:CoverletOutputFormat=\"json\" /p:CoverletOutput=.\coverage\coverage.json /p:MergeWith='..\XUnitServer_App_Identity\coverage\coverage.json'
rem dotnet test ./src/XUnitServer_TokenExchange/XUnitServer_TokenExchange.csproj --logger trx --collect "Code coverage" /p:CollectCoverage=true /p:CoverletOutputFormat=\"json\" /p:CoverletOutput=.\coverage\coverage.json /p:MergeWith='..\XUnitServer_OAuth2\coverage\coverage.json'
dotnet sonarscanner end /d:sonar.login="%token%"


rem del ".\test-results\coverage.json"
rem dotnet test ./src/XUnitServer_App_Identity/XUnitServer_App_Identity.csproj   --logger trx --collect "Code coverage"   /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=..\..\test-results\ /p:MergeWith=..\..\test-results\coverage.json
rem dotnet test ./src/XUnitServer_OAuth2/XUnitServer_OAuth2.csproj               --logger trx --collect "Code coverage"   /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=..\..\test-results\ /p:MergeWith=..\..\test-results\coverage.json
rem dotnet test ./src/XUnitServer_TokenExchange/XUnitServer_TokenExchange.csproj --logger trx --collect "Code coverage"   /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=..\..\test-results\ /p:MergeWith=..\..\test-results\coverage.json
