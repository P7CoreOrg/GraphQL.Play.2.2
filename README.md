# GraphQL.Play.2.2

# Requirments
[Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)  
I think there is c# 8 preview code in some of the projects where it may not work with 2017.
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <LangVersion>preview</LangVersion>

  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="12.0.1" />
  </ItemGroup>

</Project>
```

[.net core 2.2.105 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2)  
[Altair GraphQL Client](https://altair.sirmuel.design/)  
[Postman](https://www.getpostman.com/) 


## Ports 
In the projects **./GraphQL.Play.2.2/src/.vs/config/** folder there is a file called **applicationhost.config**  
Make sure that it is set to launch the project using the 44371 port.  In our [appsettings.json](./src/IdentityServer4-Extension-Grants-App/appsettings.json)  there is a configuration that expect it to be 44371.  You can change the appsetting.json, but its best to understand how **applicationhost.config** works because we can use a free dns lookup like [xip.io](http://xip.io/) when we need a non localhost domain.  

```
<site name="IdentityServer4-Extension-Grants-App" id="5">
  <application path="/" applicationPool="IdentityServer4-Extension-Grants-App AppPool">
    <virtualDirectory path="/" physicalPath="H:\github\P7CoreOrg\GraphQL.Play.2.2\src\IdentityServer4-Extension-Grants-App" />
  </application>
  <bindings>
    <binding protocol="http" bindingInformation="*:29552:localhost" />
    <binding protocol="https" bindingInformation="*:44371:localhost" />
  </bindings>
</site>
```

Also, [launchSettings.json](./src/IdentityServer4-Extension-Grants-App/Properties/launchSettings.json) lets you use xip.io.  
This is probably the best thing to do as working with IISExpress can turn problematic at times.  

```
"IdentityServer4_Extension_Grants_App": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://graphqlplay.127.0.0.1.xip.io:5001;http://localhost:5000"
    }
  }
  
```
# [Extensibility Points](./docs/extensibility.md)  


# The Host App
This [GraphQL demo app](src/IdentityServer4-Extension-Grants-App) implements the following use cases;  
[AuthUseCases](https://github.com/AuthUseCases/Flows)  

After you run the **IdentityServer4-Extension-Grants-App** you should be able to make a **client_credentials** call to verify that the OAuth2 stack is working.
```
curl -X POST \
  https://localhost:44371/connect/token \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -H 'Postman-Token: f9540e6f-7ce5-43e9-8efa-ce781a48695f' \
  -H 'cache-control: no-cache' \
  -d 'grant_type=client_credentials&client_id=b2b-client&client_secret=secret&undefined='
```
## Result
```
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQzMDczNjksImV4cCI6MTU1NDMxMDk2OSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiM1BBcGkiXSwiY2xpZW50X2lkIjoiYjJiLWNsaWVudCIsImNsaWVudF9uYW1lc3BhY2UiOiJiMmItb3JnIiwic2NvcGUiOlsiM1BBcGkiXX0.nH_Cfr60qweUPFIcVGGqFhnUIDtuVRED9fJTcnNFT3EfHwcGK3Ix4lZMESTntLgygUj-hI5DHFl0zQA1oE7oETW41T6bZ1CRfoXF4046_tAuxUqqpQVAhnjzTEcS-nJoupw28dbBUUHTh0aeRz7vr5H8sbVq_m_-J6Sw-asS_eebabTxSOd46sp735gtnFfssuJX-xGeTO3ilKw56T2X2s8LTF8QyzrpOndpPBLzBoMOm0vf8WZRios5vWkGP6sEsrycEfIkcuZXvkQsFbfKfvPRxXOH_U00EuJWIIJxHr76Cso6wiTnv9ot2cukF2CaloP2aHHSgpvFgT_2gbyaWw",
    "expires_in": 3600,
    "token_type": "Bearer"
}
```

The **client_credentials** call is what B2B clients would use to get their bear access_tokens to use when making downstream authorized calls into the gateway. 


# How to get a google id_token
[oidcreference](https://oidcreference.azurewebsites.net/)
**IMPORTANT** Make sure you agree to the cookies are being used prompt prior to attempting a google login.


# Bind Query
```
query q($input: bind!) {
  bind(input: $input){
    authorization{
      authority
      access_token
      httpHeaders{
        name
        value
      }
    }
  }
}
```
```
{
  "input": {
    "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImE0MzEzZTdmZDFlOWUyYTRkZWQzYjI5MmQyYTdmNGU1MTk1NzQzMDgiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIxMDk2MzAxNjE2NTQ2LWVkYmw2MTI4ODF0N3JrcGxqcDNxYTNqdW1pbnNrdWxvLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiMTA5NjMwMTYxNjU0Ni1lZGJsNjEyODgxdDdya3BsanAzcWEzanVtaW5za3Vsby5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsIm5vbmNlIjoiNjM2ODk5MDQ2NzI4OTI3NTY0Lk56QXpaR1k1T1dJdE1qVTNNeTAwWkRNMUxXSTBaakl0WldWa1ptTTFObUUxTURKbE5XUmpNbVV3WVRJdFlUVXlNeTAwTmpJd0xXSm1PR0V0WlRNNVptRXdPREUzWkdNeSIsIm5hbWUiOiJIZXJiIFN0YWhsIiwicGljdHVyZSI6Imh0dHBzOi8vbGg0Lmdvb2dsZXVzZXJjb250ZW50LmNvbS8tdXZPc3RBRzhUcWsvQUFBQUFBQUFBQUkvQUFBQUFBQUFGVTAvaU9OSWpKbjNkZHMvczk2LWMvcGhvdG8uanBnIiwiZ2l2ZW5fbmFtZSI6IkhlcmIiLCJmYW1pbHlfbmFtZSI6IlN0YWhsIiwibG9jYWxlIjoiZW4iLCJpYXQiOjE1NTQzMDc4NzMsImV4cCI6MTU1NDMxMTQ3MywianRpIjoiZmJmN2UyZWVhZjYyNGIyYTNkMDBjOTU2ZWQ2ZDBmZDYzMmNkMmNmZCJ9.ZxFf_U4aycyh5CIAddUdFKphXjRME03EmuebtAWe4nUTpkRLpCRUEYE619D8o42j5HN3wHJWuc8ar8ESMcYw_w6OiW4zqsfdiAjmZqO1XpPVjePHIEb8P-gVPBrmNQYQTGyjISEaIXhH8oMo6jQkxwh8RDq7sB__3TyPU4h_PhqeOQfo18yy0cZQTQFRRPzxMfVJANMmaai1_z1yZfVwPs5XVb1I1Lw8PTHuKzQAjtB0g3T-ANx2nO4qDVdVTDG_4JzKc5PtLpXUEMsiSnogUaicfBnvjoYUUdQ7noGNUrl5y3U_X8axrV9Mwk5UtSi4EGrk84NV4ih3t6kAgkykYw",
    "tokenScheme": "google",
    "exchange": "google-my-custom",
     "extras":["a","b","c"]
  }
}
```
Produces... [jwt.io](https://jwt.io/#debugger-io?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQzMDc5MDAsImV4cCI6MTU1NDMxMTUwMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDMwNzkwMCwiaWRwIjoibG9jYWwiLCJjbGllbnRfbmFtZXNwYWNlIjpbIkRhZmZ5IER1Y2siLCJEYWZmeSBEdWNrIl0sInJvbGUiOlsiYSIsImIiLCJjIiwidXNlciJdLCJzY29wZSI6WyJncmFwaFFMUGxheSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJhcmJpdHJhcnlfcmVzb3VyY2Vfb3duZXIiXX0.GqKpliB4i5MZZ7zN0neI9a470yIw39ueCiSgF5Gkv33CUvYd3HPsFdOPb00tV8PeRlCWtAcQcJlSR-Z66Smhu8Ue2gexv-8RUPatiCbuJy41rJ_xel08socLeuCZqwsbdATrfwCDLKoSOMRUfvMqpAgXLb1FbGiZhCXOZ7QIUStikBqt0Eyu-KWWIU-VxkZxrJIlo7wt043sQOBSh_qUjCqzuZZnDwB7bAhRFbhWSVT23ZH0c_mHVXMgr4GvkVgpEAGCZRg6StV9xsZnndo1QCiQ0iJ-X9n6MTr-_TaZT8kr1HCYsttDrnCGcaoiORxP60E9UOZRhSPMGElugPHUMg&publicKey=-----BEGIN%20PUBLIC%20KEY-----%0AMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArfiIm%2Bsg%2FHE%2Bk71K5GAj%0A%2FwRm36X87pR9UivWeepzAGVU0aKM9pFs3i%2BJftHhozYq8yrumx%2BwwCgXqi%2FgZN64%0AbDOkt1zM7fopRO6jfgJzMTJFL7jUs1T1yHzXefSQLG6ZPf0KsvAz5L2WdezdVrYi%0A6YZCG873QkAgSfx5ZSR96x5TPqUxh%2BIzdWp62xtLhySzMNpTXHTkRzhdtDKdPahU%0AW3M6LIYcnDvPrNE5adk9eJzpsgUOcUG7JrEY2UqJ9VkCeES2Q%2FDzGpcTGLQoLG4i%0AEJ%2FuGIHMZID66A00MwXbJb8kyri6GoGtWjtxLSFHF%2Feet4mho%2BUd92fpFWxyD4SG%0AxQIDAQAB%0A-----END%20PUBLIC%20KEY-----%0A)  

```
{
  "data": {
    "bind": {
      "authorization": {
        "authority": "https://localhost:44371",
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQzMDc5MDAsImV4cCI6MTU1NDMxMTUwMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDMwNzkwMCwiaWRwIjoibG9jYWwiLCJjbGllbnRfbmFtZXNwYWNlIjpbIkRhZmZ5IER1Y2siLCJEYWZmeSBEdWNrIl0sInJvbGUiOlsiYSIsImIiLCJjIiwidXNlciJdLCJzY29wZSI6WyJncmFwaFFMUGxheSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJhcmJpdHJhcnlfcmVzb3VyY2Vfb3duZXIiXX0.GqKpliB4i5MZZ7zN0neI9a470yIw39ueCiSgF5Gkv33CUvYd3HPsFdOPb00tV8PeRlCWtAcQcJlSR-Z66Smhu8Ue2gexv-8RUPatiCbuJy41rJ_xel08socLeuCZqwsbdATrfwCDLKoSOMRUfvMqpAgXLb1FbGiZhCXOZ7QIUStikBqt0Eyu-KWWIU-VxkZxrJIlo7wt043sQOBSh_qUjCqzuZZnDwB7bAhRFbhWSVT23ZH0c_mHVXMgr4GvkVgpEAGCZRg6StV9xsZnndo1QCiQ0iJ-X9n6MTr-_TaZT8kr1HCYsttDrnCGcaoiORxP60E9UOZRhSPMGElugPHUMg",
        "httpHeaders": [
          {
            "name": "x-authScheme",
            "value": "self"
          }
        ]
      }
    }
  }
}
```

# Identity Query
```
query{
  authRequired{
    claims{
      name
      value
    }
  }
}
```
Headers:
```
Authorization : Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQzMDc5MDAsImV4cCI6MTU1NDMxMTUwMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDMwNzkwMCwiaWRwIjoibG9jYWwiLCJjbGllbnRfbmFtZXNwYWNlIjpbIkRhZmZ5IER1Y2siLCJEYWZmeSBEdWNrIl0sInJvbGUiOlsiYSIsImIiLCJjIiwidXNlciJdLCJzY29wZSI6WyJncmFwaFFMUGxheSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJhcmJpdHJhcnlfcmVzb3VyY2Vfb3duZXIiXX0.GqKpliB4i5MZZ7zN0neI9a470yIw39ueCiSgF5Gkv33CUvYd3HPsFdOPb00tV8PeRlCWtAcQcJlSR-Z66Smhu8Ue2gexv-8RUPatiCbuJy41rJ_xel08socLeuCZqwsbdATrfwCDLKoSOMRUfvMqpAgXLb1FbGiZhCXOZ7QIUStikBqt0Eyu-KWWIU-VxkZxrJIlo7wt043sQOBSh_qUjCqzuZZnDwB7bAhRFbhWSVT23ZH0c_mHVXMgr4GvkVgpEAGCZRg6StV9xsZnndo1QCiQ0iJ-X9n6MTr-_TaZT8kr1HCYsttDrnCGcaoiORxP60E9UOZRhSPMGElugPHUMg

x-authScheme : One

```
Produces...
```
{
  "data": {
    "authRequired": {
      "claims": [
        {
          "name": "nbf",
          "value": "1552967414"
        },
        {
          "name": "exp",
          "value": "1552971014"
        },
        {
          "name": "iss",
          "value": "https://localhost:44371"
        },
        {
          "name": "aud",
          "value": "https://localhost:44371/resources"
        },
        {
          "name": "aud",
          "value": "graphQLPlay"
        },
        {
          "name": "client_id",
          "value": "arbitrary-resource-owner-client"
        },
        {
          "name": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
          "value": "104758924428036663951"
        },
        {
          "name": "auth_time",
          "value": "1552967414"
        },
        {
          "name": "http://schemas.microsoft.com/identity/claims/identityprovider",
          "value": "local"
        },
        {
          "name": "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
          "value": "application"
        },
        {
          "name": "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
          "value": "limited"
        },
        {
          "name": "client_namespace",
          "value": "Daffy Duck"
        },
        {
          "name": "scope",
          "value": "graphQLPlay"
        },
        {
          "name": "scope",
          "value": "graphQLPlay"
        },
        {
          "name": "scope",
          "value": "graphQLPlay"
        },
        {
          "name": "scope",
          "value": "graphQLPlay"
        },
        {
          "name": "scope",
          "value": "offline_access"
        },
        {
          "name": "http://schemas.microsoft.com/claims/authnmethodsreferences",
          "value": "arbitrary_resource_owner"
        },
        {
          "name": "access_token",
          "value": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTI5Njc0MTQsImV4cCI6MTU1Mjk3MTAxNCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1Mjk2NzQxNCwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImFwcGxpY2F0aW9uIiwibGltaXRlZCJdLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInNjb3BlIjpbImdyYXBoUUxQbGF5IiwiZ3JhcGhRTFBsYXkiLCJncmFwaFFMUGxheSIsImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.YHgKcQHF1ZrOqDG-Is1YsO73ROkC9rDbNKPn7DfRT07sq4SsKhp5bZFi7LKpKbsXcK8WI_3ZkTQeQLt29xdqSITM6bglCiepyb3ZXrsx1PLMBvfrZAW7-9m0Act8UH_g2FUgh8OzogLwshQNCLy0FBYjN68f61hyJUJOzraYFI_v8GFJS_DE54fiTjNchGyeFR9XRzrgTTvBLs3_g20enRZhbZbNxMwEb4bUQ1woUugXyTqdI652WfNMOeaGylepFTNQJT-c_z1DMChapGlQDSryrhDcYJY3_tIendF25BIwXoRYKyGdcY1JM7r8Q92zyQG35WB_F-dpA_VhUJMAYw"
        }
      ]
    }
  }
}
```


