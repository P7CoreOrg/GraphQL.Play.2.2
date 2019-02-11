# GraphQL.Play.2.2

This GraphQL demo app implements the following use cases;  
[AuthUseCases](https://github.com/AuthUseCases/Flows)  



3 IDP's are currently supported  

[demoidentityserverio](https://demo.identityserver.io/Account/Login)  
To get an id_token, run the following client app.
[oidc-client-js](https://github.com/IdentityModel/oidc-client-js)  
![oidc-js Home Page](./docs/home.png)  
![UserManager Sample with IdentityServer with code flow](./docs/oidc-signin.png)  

[p7identityserver4](https://p7identityserver4.azurewebsites.net/)  
To get an id_token, follow the following [instructions](https://p7identityserver4.azurewebsites.net/docs/arbitrary_identity.md)  

[google](https://accounts.google.com/.well-known/openid-configuration)  




# Bind Query
```
query q($input: bind!) {
  bind(input: $input){
    authorization{
      access_token
    	authority
    	expires_in
   	  refresh_token
   	  token_type
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
    "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IkZENkFGOTIyQTAyNTM4NzE5RjhBQjVBRTM0NjdCMjA1MEU2QUExMkUiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJfV3I1SXFBbE9IR2Zpcld1TkdleUJRNXFvUzQifQ.eyJuYmYiOjE1NDk5MjQxNDAsImV4cCI6MTU0OTk2MDE0MCwiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJhcmJpdHJhcnktcmVzb3VyY2Utb3duZXItY2xpZW50IiwiY2F0IiwiZG9nIl0sImlhdCI6MTU0OTkyNDE0MCwiYXRfaGFzaCI6ImdTeWdYTXVLRkFRalM3a1R0cDhrc1EiLCJzdWIiOiJQb3JreVBpZyIsImF1dGhfdGltZSI6MTU0OTkyNDEzOSwiaWRwIjoibG9jYWwiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJ0ZWRAdGVkLmNvbSIsIm5hbWUiOiJ0ZWRAdGVkLmNvbSIsImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwiYW1yIjpbImFyYml0cmFyeV9pZGVudGl0eSIsImFnZW50OnVzZXJuYW1lOmFnZW50MEBzdXBwb3J0dGVjaC5jb20iLCJhZ2VudDpjaGFsbGVuZ2U6ZnVsbFNTTiIsImFnZW50OmNoYWxsZW5nZTpob21lWmlwIl0sImN1c3RvbV9wYXlsb2FkIjp7ImEiOnsiMCI6eyJTdHJlZXQxIjoiMCBNb250YW5hIEF2ZSIsIlN0cmVldDIiOm51bGwsIlN0cmVldDMiOm51bGwsIlppcCI6IjkwNDAzIiwiQ2l0eSI6IlNhbnRhIE1vbmljYSIsIlN0YXRlIjoiQ2FsaWZvcm5pYSIsIkNvdW50cnkiOiJVU0EifSwiMSI6eyJTdHJlZXQxIjoiMSBNb250YW5hIEF2ZSIsIlN0cmVldDIiOm51bGwsIlN0cmVldDMiOm51bGwsIlppcCI6IjkwNDAzIiwiQ2l0eSI6IlNhbnRhIE1vbmljYSIsIlN0YXRlIjoiQ2FsaWZvcm5pYSIsIkNvdW50cnkiOiJVU0EifX19fQ.JCmibEtkyHCXk4Gflyi3eoJaXqU8OT5F2PbVMz3t2sBNQZv5mJ28HGnz1qx2S6gLEdq3USk-KRB0BF0sFCCZ6BhD3mpcIDkUBCtcO5z-_CT_zU-4mlp-75oONNNY4k-unLpcFEBaNR_9baPCCpkRGFXn4T149IMvERJPRk05YBkRJcyXCXBE8UARdpG7SdpTkQPTINRLWWhMz-QKT5iuOIN3nTuCwyzzkeoLWXS5BI04uyhG4LNKdaVZIcGojwOnjzf0Ho80z5M2w9EQQmEno_y6UkUofUp8fAVdiQSTH2_WhEDtYpHW_kGIpTBe7M1FQbfm7SQQvVlBswjQLfRUlg",
    "tokenScheme": "p7identityserver4"
  }
}
```
Produces... [jwt.io](https://jwt.io/#debugger-io?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IkZENkFGOTIyQTAyNTM4NzE5RjhBQjVBRTM0NjdCMjA1MEU2QUExMkUiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJfV3I1SXFBbE9IR2Zpcld1TkdleUJRNXFvUzQifQ.eyJuYmYiOjE1NDk5MjQ1MzUsImV4cCI6MTU0OTkyODEzNSwiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3A3aWRlbnRpdHlzZXJ2ZXI0LmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiJQb3JreVBpZyIsImF1dGhfdGltZSI6MTU0OTkyNDUzNSwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImFwcGxpY2F0aW9uIiwibGltaXRlZCJdLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInNjb3BlIjpbImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.jLta7yUrz7kpo5uLKUmW7XAVRPT1vwb50mutoYxuBtzjjs4Clnbx-O9YyQqV27al4QdwLTvsu4LwrJ4Adad4UDTRhrMyC1ayIm7b-acv6YcIx4LG08BQvlBCNgYtAKwCaD-JO8kYKkgtNgJ9ySmMUbtDFnQ6SDhyhYdxExGtDm3gub7mJXkYcrll8-IuQYKxo-aIhcMUkIuYwE5nM7Sze3VQnZFCCzQ2_hQ_X-GblZU9f9XiO9wb4MlAz2OQZj9-tKoPhDpyfbHea2C6XJsOYBMBXduDYotxLyOXs67zRLl9MQjBWZ6IqMPIbKXmhiVd9q7UP3jQ1urG4yKhv6ga2w)  

```
{
  "data": {
    "bind": {
      "authorization": {
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IkZENkFGOTIyQTAyNTM4NzE5RjhBQjVBRTM0NjdCMjA1MEU2QUExMkUiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJfV3I1SXFBbE9IR2Zpcld1TkdleUJRNXFvUzQifQ.eyJuYmYiOjE1NDk5MjQ1MzUsImV4cCI6MTU0OTkyODEzNSwiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3A3aWRlbnRpdHlzZXJ2ZXI0LmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiJQb3JreVBpZyIsImF1dGhfdGltZSI6MTU0OTkyNDUzNSwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImFwcGxpY2F0aW9uIiwibGltaXRlZCJdLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInNjb3BlIjpbImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.jLta7yUrz7kpo5uLKUmW7XAVRPT1vwb50mutoYxuBtzjjs4Clnbx-O9YyQqV27al4QdwLTvsu4LwrJ4Adad4UDTRhrMyC1ayIm7b-acv6YcIx4LG08BQvlBCNgYtAKwCaD-JO8kYKkgtNgJ9ySmMUbtDFnQ6SDhyhYdxExGtDm3gub7mJXkYcrll8-IuQYKxo-aIhcMUkIuYwE5nM7Sze3VQnZFCCzQ2_hQ_X-GblZU9f9XiO9wb4MlAz2OQZj9-tKoPhDpyfbHea2C6XJsOYBMBXduDYotxLyOXs67zRLl9MQjBWZ6IqMPIbKXmhiVd9q7UP3jQ1urG4yKhv6ga2w",
        "authority": "https://p7identityserver4.azurewebsites.net",
        "expires_in": 3600,
        "refresh_token": "CfDJ8MVlmnGGAk1Ah5xhnpEOsbEkVGjaHsIAZ11E51szcMhlNTaf9C8oMtBkbel_yIOjwZN4uIydte09cPT79L9azuf-G6BgeTtSJxgjVl9U6VRS9peQIsb3C1-7MScXy03t13gO6TqhOU2pwzi0RV6kV6x5vD9g_nYvQhmQwEi7rP-1vkEZS1dx_MQS2XrsN-uV9Euimnx4xdUh2HyVvJHXpos",
        "token_type": "Bearer",
        "httpHeaders": [
          {
            "name": "x-authScheme",
            "value": "One"
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
Authorization : Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvOGJkZDYxODA3NWQwNGEwZDgzZTk4NmI4YWE5NGQ3YjIiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NDQ4ODkwMDQsImV4cCI6MTU0NDg5MjYwNCwiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3A3aWRlbnRpdHlzZXJ2ZXI0LmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiJQb3JreVBpZyIsImF1dGhfdGltZSI6MTU0NDg4OTAwNCwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImFwcGxpY2F0aW9uIiwibGltaXRlZCJdLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInNjb3BlIjpbImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.nz5oRlBcfZbptAySRsSnYwkwSpkkMaRHzKgh5YKxb6X-XVvI1FW1CAtl4G23FxP1dMR2O3sUtM5ZYTSiaAfpaG9UupCmKrOGSx9262MN2I2rOutiwmstMC_Koql85s0Yzbzv8wQRu5epksElXiziZOy4js7dsOgPENXDxn0PyJY0BB_NZMBBPGAXTe_6nl6NjKODQrrbt9dpO1KvoWl75eZhvmNU65RX5qDnhO0GHnfMwlbCOwqNLTrLftwHprYVKHov_KJuWdf2gjkrk2xBrX9eQTbzW_bwiBzBnpJVZ99VUhr2ZdBbpdKGyq2zEqHjnVM2spn1Y9wWd_gFb2sYfQ

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
          "value": "1544889004"
        },
        {
          "name": "exp",
          "value": "1544892604"
        },
        {
          "name": "iss",
          "value": "https://p7identityserver4.azurewebsites.net"
        },
        {
          "name": "aud",
          "value": "https://p7identityserver4.azurewebsites.net/resources"
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
          "value": "PorkyPig"
        },
        {
          "name": "auth_time",
          "value": "1544889004"
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
          "value": "offline_access"
        },
        {
          "name": "http://schemas.microsoft.com/claims/authnmethodsreferences",
          "value": "arbitrary_resource_owner"
        },
        {
          "name": "access_token",
          "value": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvOGJkZDYxODA3NWQwNGEwZDgzZTk4NmI4YWE5NGQ3YjIiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NDQ4ODkwMDQsImV4cCI6MTU0NDg5MjYwNCwiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3A3aWRlbnRpdHlzZXJ2ZXI0LmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiJQb3JreVBpZyIsImF1dGhfdGltZSI6MTU0NDg4OTAwNCwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImFwcGxpY2F0aW9uIiwibGltaXRlZCJdLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInNjb3BlIjpbImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.nz5oRlBcfZbptAySRsSnYwkwSpkkMaRHzKgh5YKxb6X-XVvI1FW1CAtl4G23FxP1dMR2O3sUtM5ZYTSiaAfpaG9UupCmKrOGSx9262MN2I2rOutiwmstMC_Koql85s0Yzbzv8wQRu5epksElXiziZOy4js7dsOgPENXDxn0PyJY0BB_NZMBBPGAXTe_6nl6NjKODQrrbt9dpO1KvoWl75eZhvmNU65RX5qDnhO0GHnfMwlbCOwqNLTrLftwHprYVKHov_KJuWdf2gjkrk2xBrX9eQTbzW_bwiBzBnpJVZ99VUhr2ZdBbpdKGyq2zEqHjnVM2spn1Y9wWd_gFb2sYfQ"
        }
      ]
    }
  }
}
```


# Bind
```
query q($input: bind!) {
  bind(input: $input){
    authorization{
      access_token
    	authority
    	expires_in
   	  refresh_token
   	  token_type
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
    "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ4YWRlZDAxMzAwYzY0ZDQwOTBiYTM4ZmExZTY4YTkyIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NDQ5ODU4NDQsImV4cCI6MTU0NDk4NjE0NCwiaXNzIjoiaHR0cHM6Ly9kZW1vLmlkZW50aXR5c2VydmVyLmlvIiwiYXVkIjoibmF0aXZlLmNvZGUiLCJpYXQiOjE1NDQ5ODU4NDQsImF0X2hhc2giOiJ6QXB5SFhZS0twbExfTGNzWldyQXhBIiwic2lkIjoiM2IxM2IxZGFkZDk2ODE3OTU5YTBhMmI2MzA2NzQxMDgiLCJzdWIiOiJmNTUzNWQyMTkwYTlkYzU0NjcyNzA4YzY0NWY1Njg0ZjcxZTBmOGNkMTgzMzM1ZmI4MjdkMzljNGUyYTc1ODljIiwiYXV0aF90aW1lIjoxNTQ0OTg1ODEyLCJpZHAiOiJHb29nbGUiLCJhbXIiOlsiZXh0ZXJuYWwiXX0.G-cioXZffb0MA6GbyhPQ8l6c3Xt1IeDDV-TWaQh085CozccDzRZc5PgPididf5kwx4fq0vSUE9ZtPBqEd15lG_y1PH2xU8MMp3zE8-RqFO1ECpo5QIjOujp5aNmmnTTs6roG2rO126MDfAJTXqQUDwL-87X11g_H71zq1aOg6-M6ciBWEBnFWRfyjX-qwVFZCQ0eIrXlQmo76va6cNy77ktQpR7Z7ADmTjj34fJ1mAoh0OQj7YaI4f7ejL-yEh8e1spJfdt_30ea_Qsu3tg42OdFWtP4m1PB763Ldq5gLo-QvN43XofRLiY7At_ky7AF8P_Plp3fDpP7DsqN3TihOA",
    "tokenScheme": "oidc",
  "authorityKey": "demoidentityserverio"
  }
}
```
Produces...
```
{
  "data": {
    "bind": {
      "authorization": {
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvOGJkZDYxODA3NWQwNGEwZDgzZTk4NmI4YWE5NGQ3YjIiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NDQ5ODU4NzksImV4cCI6MTU0NDk4OTQ3OSwiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3A3aWRlbnRpdHlzZXJ2ZXI0LmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiJmNTUzNWQyMTkwYTlkYzU0NjcyNzA4YzY0NWY1Njg0ZjcxZTBmOGNkMTgzMzM1ZmI4MjdkMzljNGUyYTc1ODljIiwiYXV0aF90aW1lIjoxNTQ0OTg1ODc5LCJpZHAiOiJsb2NhbCIsInJvbGUiOlsiYXBwbGljYXRpb24iLCJsaW1pdGVkIl0sImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiYXJiaXRyYXJ5X3Jlc291cmNlX293bmVyIl19.I8gLLigReBIMOduzORGair4gXG--j-9_Y_ILkFO4xHI4keOUtmRoLxqbuw02nqea59C44JYRvXB9qrcZDOQeTNbF3T3v2kckTfxfpH9WAt4KFTZNue1rh0qlb0i5oxrQxWIYkBCfH7N8TqSRrLVLddxL9ffrO-a48226p-kZXaQYrS5InJN6wtsOqKBPZmghf54C4kDex6dxSzVMXw2aJdUbgB_zjDpqvTuiHHMn-OnLgqb6M_Jgxjs2cROpQDmPWwgAPsPqzpFi5vWKsYUwC91b7A_-922Ggnua1avWIQf0nU9czrlvnDCv3g9Xv8SJI8HNUJn-vvmyjnFBx92hxg",
        "authority": "https://p7identityserver4.azurewebsites.net",
        "expires_in": 3600,
        "refresh_token": "CfDJ8MVlmnGGAk1Ah5xhnpEOsbFmTNqkpv_BleVL8kk39Y_HfCVi6c4NI65r_6HKeZq6pD7zpBcpo-RIR6k16vKYd-6Kfl6GOQ0IVKaLDKPLkuNcHUL9E4sH08-nwf1ljvQtENbeFyapA6B4yjYsIVgawmJOAbTAlMo-AsHaK8bCsKOMGQ7gIRHbTWFekvgfUc3JQ4vWbxg1VdF2GlUJQLU2i-E",
        "token_type": "Bearer",
        "httpHeaders": [
          {
            "name": "x-authScheme",
            "value": "One"
          }
        ]
      }
    }
  }
}
```
