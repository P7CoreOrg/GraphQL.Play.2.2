# GraphQL.Play.2.2

3 IDP's are currently supported  

[demoidentityserverio](https://demo.identityserver.io/Account/Login)  
To get an id_token, run the following client app.
[oidc-client-js](https://github.com/IdentityModel/oidc-client-js)  

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
    "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvOGJkZDYxODA3NWQwNGEwZDgzZTk4NmI4YWE5NGQ3YjIiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NDQ5NzkzMTMsImV4cCI6MTU0NTAxNTMxMywiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJhcmJpdHJhcnktcmVzb3VyY2Utb3duZXItY2xpZW50IiwiY2F0IiwiZG9nIl0sImlhdCI6MTU0NDk3OTMxMywiYXRfaGFzaCI6InhXTVlWbE9fTjB0VTM5Z0d2QmUzdGciLCJzdWIiOiJQb3JreVBpZyIsImF1dGhfdGltZSI6MTU0NDk3OTMxMiwiaWRwIjoibG9jYWwiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJ0ZWRAdGVkLmNvbSIsIm5hbWUiOiJ0ZWRAdGVkLmNvbSIsImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwiYW1yIjpbImFyYml0cmFyeV9pZGVudGl0eSIsImFnZW50OnVzZXJuYW1lOmFnZW50MEBzdXBwb3J0dGVjaC5jb20iLCJhZ2VudDpjaGFsbGVuZ2U6ZnVsbFNTTiIsImFnZW50OmNoYWxsZW5nZTpob21lWmlwIl0sImN1c3RvbV9wYXlsb2FkIjp7ImEiOnsiMCI6eyJTdHJlZXQxIjoiMCBNb250YW5hIEF2ZSIsIlN0cmVldDIiOm51bGwsIlN0cmVldDMiOm51bGwsIlppcCI6IjkwNDAzIiwiQ2l0eSI6IlNhbnRhIE1vbmljYSIsIlN0YXRlIjoiQ2FsaWZvcm5pYSIsIkNvdW50cnkiOiJVU0EifSwiMSI6eyJTdHJlZXQxIjoiMSBNb250YW5hIEF2ZSIsIlN0cmVldDIiOm51bGwsIlN0cmVldDMiOm51bGwsIlppcCI6IjkwNDAzIiwiQ2l0eSI6IlNhbnRhIE1vbmljYSIsIlN0YXRlIjoiQ2FsaWZvcm5pYSIsIkNvdW50cnkiOiJVU0EifX19fQ.KgwEp-Y7zNUEBDf5wgp06mBB3yeLyhaGcztmrjov3ZqBmwu48-f-aQnIvLSB7rllY7SA7KjanK7tbiD8nV4zC0yY_pGaqvBavpo99-3a2oi5Gw1AfSLj1jbdPSS9r5VLrjsJdzezEqAvUHR1eGDlRzwk1HeDpDKscC250UBLPJP3QbI_MXcVxK2uphlaFl_ig7lHVvP3EKbJHhSK3cPFx6sAuhE0QkgGrZIn4vNhkQJAUtVwTvtn5P_o_xw1wFrM8kZI9M3sMfTgJKqhxBtfuhNiO8y2RwGq_vKU7XRHTku8mGHf_jfbzmla3T9wokU3fS-WLeskoreIs7kszDrURw",
    "tokenScheme": "oidc",
    "authorityKey": "p7identityserver4"
  }
}
```
Produces...
```
{
  "data": {
    "bind": {
      "authorization": {
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvOGJkZDYxODA3NWQwNGEwZDgzZTk4NmI4YWE5NGQ3YjIiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NDQ5ODQ5NjIsImV4cCI6MTU0NDk4ODU2MiwiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3A3aWRlbnRpdHlzZXJ2ZXI0LmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiJQb3JreVBpZyIsImF1dGhfdGltZSI6MTU0NDk4NDk2MiwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImFwcGxpY2F0aW9uIiwibGltaXRlZCJdLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInNjb3BlIjpbImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.JZ5R1LMO56vh3kKY4AGPl_GYJeeEoJKT_7BL3WvdMtM6fvNthKHu1eQRHrMJDxNz6lQy_wNDVdAWkxjO7U2A7lgkKcxwstV7xsW4dDragQXJORGNb2JaaqxKeEpEz4EMD2Lzt-awAlBLp_p7yXeBYaKwzwbmd5k59-19EMxHmxs8bfUOPtrAhOJ10RFVooghZ7ZlwQZSDYJ_kNUxZFlJ9kjzXtWWO02fcWXEcIj3T1KW7sAdSDC8qe-QPwK5PKQwzwhNeAjepnjoQrSMPu-SR1u_94Sa7Olv0eyGQ1fzWwk8qn1DvJKYX7Jpnm8-KR77c8Rc5EJTtJciLyAgF1RujQ",
        "authority": "https://p7identityserver4.azurewebsites.net",
        "expires_in": 3600,
        "refresh_token": "CfDJ8MVlmnGGAk1Ah5xhnpEOsbECzobfc6EHpUktaORa9gthRJe6_paSdcct6BjVVE4iW_xAndJbf0JqQjQydgqN0uDL1z0-2BKrRpzNxJJTraLKXd8opCz1e3R_gNHgwnLHyaWAl35fC5kS7M6Ghgh-xVgjErhxzu_9RselWvGDMzhSMNMTmJgr6YekZs4hpx0j8BTXbYOkPbdHwzhuyfsMjq0",
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
    access_token
    authority
    expires_in
    refresh_token
    token_type
  }
}

```
```
{
  "input": {
    "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ4YWRlZDAxMzAwYzY0ZDQwOTBiYTM4ZmExZTY4YTkyIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NDQ4OTMzMjYsImV4cCI6MTU0NDg5MzYyNiwiaXNzIjoiaHR0cHM6Ly9kZW1vLmlkZW50aXR5c2VydmVyLmlvIiwiYXVkIjoiaW1wbGljaXQiLCJub25jZSI6IjMzZjM4Mjg2OTlhNjQ4ODY5OTU0NjIwNTA1NzNkYWJmIiwiaWF0IjoxNTQ0ODkzMzI2LCJhdF9oYXNoIjoiWHlUSy1IZnZ1Z0hXQnZiclpWUDlsZyIsInNpZCI6IjJhMjg4ZjM0NGJkYzZjYjMxMDE2MmUzMDlhNWNhMWNmIiwic3ViIjoiZjU1MzVkMjE5MGE5ZGM1NDY3MjcwOGM2NDVmNTY4NGY3MWUwZjhjZDE4MzMzNWZiODI3ZDM5YzRlMmE3NTg5YyIsImF1dGhfdGltZSI6MTU0NDg5MTM0OSwiaWRwIjoiR29vZ2xlIiwiYW1yIjpbImV4dGVybmFsIl19.JEXtBGaqTgrG0xHlogebITvzWtFG6caEQYMNiW5fuYa8cdxWbijhQko7fslhf6X3gN2WnIUVyXxkPSkAZISkaU8nbQxsDR1NdwxZ9ZUykLr6IPTd5gZ0p5KaTdk7stP-Su5PriwxA_tbgruD2tLhfkXOvCVSclos76r9deFH0G-bSd8iA19BnydcFwcOMKWrN4XYTtfjXZvwE_VSQeA5wQfm1RSnBy1AbYBwc-OcCBTwENkZmBRtEF6Vaso5egFxCjef5R3NeUpPfXkdX5e-9svqHZ2n2FpWms0nvRTwKIMe_ktcjDXBknmBymXjz8mvlxy9SHvuKPaFUwYBfEqA2g",
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
      "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvOGJkZDYxODA3NWQwNGEwZDgzZTk4NmI4YWE5NGQ3YjIiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NDQ4OTM0MTcsImV4cCI6MTU0NDg5NzAxNywiaXNzIjoiaHR0cHM6Ly9wN2lkZW50aXR5c2VydmVyNC5henVyZXdlYnNpdGVzLm5ldCIsImF1ZCI6WyJodHRwczovL3A3aWRlbnRpdHlzZXJ2ZXI0LmF6dXJld2Vic2l0ZXMubmV0L3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiJmNTUzNWQyMTkwYTlkYzU0NjcyNzA4YzY0NWY1Njg0ZjcxZTBmOGNkMTgzMzM1ZmI4MjdkMzljNGUyYTc1ODljIiwiYXV0aF90aW1lIjoxNTQ0ODkzNDE3LCJpZHAiOiJsb2NhbCIsInJvbGUiOlsiYXBwbGljYXRpb24iLCJsaW1pdGVkIl0sImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiYXJiaXRyYXJ5X3Jlc291cmNlX293bmVyIl19.PjWNxp_RXwiKxF4y6JsiPg3UmkWeCcFpjjZeiv3p-tdmzZOVm0NZW3BIKRckLp_TO65rnk_390H1lMZoc-Igk3vdSetBCz2x-raLeLqKO74nzR9dH3pZIa54qCZsSBzZA-yZtzejO-tH9xrOTiNDrK1ZwBN6vHmLC7x97-gqhC7U2IYJgKensX-B9FYQLfP64tUPGIYuTqDfhLHo5bkBIz8Zdh58UWtcBnGAFflYC-Rs-a1XruJ1DNjfj7pk75ymY3-KadbCbnLOC6LsmushvXhJV7fMW-7AjCXi6lghEuWH9eWh601_uh0BwDiseoR4agak1f0xK4-XUc6kz7Jt9w",
      "authority": "https://p7identityserver4.azurewebsites.net",
      "expires_in": 3600,
      "refresh_token": "CfDJ8MVlmnGGAk1Ah5xhnpEOsbGjS_lAysG7wiGjsqFbiE9wbtxrUja5gymiGLCjN8oL1B0ZGGRIgS2wxZO-x81V08sGNEGgQXRczin_mhN9s2DT7pBrYcmvwMsrqyWOQWjUKDFhjwN43Dd13RbqbWvt-rujBd89QwSgZGPJjdHSU5xB4ocfujRIrX80pfjhAaOqmPWAOajghn4iGYYEalL8nBU",
      "token_type": "Bearer"
    }
  }
}
```
