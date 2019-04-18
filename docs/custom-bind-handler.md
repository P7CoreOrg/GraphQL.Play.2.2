# Custom Bind


1. Introduce a new [ITokenExchangeHandler](../src/TokenExchange.Contracts/Services/GoogleMyCustomIdentityTokenExchangeHandler.cs) where you use the scheme name *google-my-custom" in the constructor to name the IdentityPrincipalEvaluator.  
This is where the real customization happens.  Here you can evaluate the claims, and with an optional string array called "Extras" that is passed in via the bind call, you can decide exactly what the following access_token will look like.
You can reject the bind here as well.  

2. Add a registration extension [here](../src/TokenExchange.Contracts/Extensions/AspNetCoreServiceCollectionExtensions.cs)  
```
public static IServiceCollection AddGoogleMyCustomIdentityPrincipalEvaluator(this IServiceCollection services)
{

    services.AddSingleton<IPrincipalEvaluator, GoogleMyCustomIdentityPrincipalEvaluator>();
    return services;
}
```

7. Register the handler in [Startup.cs](../src/IdentityServer4-Extension-Grants-App/Startup.cs).
```
  services.AddGoogleMyCustomIdentityPrincipalEvaluator();
```

8. GraphQL tokenExchange Call  
## Query  
```
query q($input: tokenExchange!) {
	tokenExchange(input: $input){
		authority
		access_token
		token_type
		httpHeaders
		{
			name
			value
		}
	}
}
```
## Variables  
The **tokenScheme** is a wellknown registered OIDC provider that is used to validate the token that is passsed in.  In this case "google".  
The **exchange** is what custom echange is to be executed to produce the subsequent access_token.  
The **extras** are hints that are only known to the exchange.
```
{
	"input": {
		"exchange": "google-my-custom",
		"extras": ["a", "b", "c"],
		"tokens": [{
			"token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFjN2RmOWU1YjBjYzNkZDI1NmE1MWFiNzcwYmM2ZTAzIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTU0MjkzMTIsImV4cCI6MTg3NTQyOTMxMiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6ImFwcC1pZGVudGl0eS1jbGllbnQiLCJpYXQiOjE1NTU0MjkzMTIsImF0X2hhc2giOiJYTmEzVUdkaWNncWg1UjA0M0JfbnFBIiwic3ViIjoiMTI5YmFiOGEtNTdhOC00NTk4LWJkNTktYzU0MDllOWRjMGRmIiwiYXV0aF90aW1lIjoxNTU1NDI5MzExLCJpZHAiOiJsb2NhbCIsImNsaWVudF9uYW1lc3BhY2UiOiJhcHAtaWRlbnRpdHktb3JnIiwiYXBwSWQiOiJOSVMiLCJtYWNoaW5lSWQiOiJzb21lIGd1aWQiLCJhbXIiOlsiYXJiaXRyYXJ5X2lkZW50aXR5Il19.gwaBsf2WF9oXH9VHKaFEq23D_aCwXnr_9ISGngCzW4WNSM7b78vZ9QFK6l0nf3fiMGFRaRp2cdevbHt9wEN8wa7FRHHz59CwWY_bHFH9diJcHV1-HGKdqdmgIx3TViDpLI3bJs-BIzJwAUowO5GelOcpyXIG1w1I_2ZX0jmLlPfud3OUpibU4q4ECu20jtVqDk_3ajWK0cre8MdMSsuA3MwdtLXmN-Uirra06YXicv7MhMLDvxsUudvh9mYSXyCYvod-qgYkmX79h5pX4sn2VufoXpINVd_yJQjfndeAQwlTD_g14XzHHKL3YAYLFZKlo_TKfoSyrVGh24nipM9Gbg",
			"tokenScheme": "self"
		}]
	}
}

```
## Results
```
{
  "data": {
    "tokenExchange": [
      {
        "authority": "https://localhost:5001",
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFjN2RmOWU1YjBjYzNkZDI1NmE1MWFiNzcwYmM2ZTAzIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTU0MzI3ODcsImV4cCI6MTU1NTQzNjM4NywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo1MDAxL3Jlc291cmNlcyIsImdyYXBoUUxQbGF5Il0sImNsaWVudF9pZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJzdWIiOiIxMjliYWI4YS01N2E4LTQ1OTgtYmQ1OS1jNTQwOWU5ZGMwZGYiLCJhdXRoX3RpbWUiOjE1NTU0MzI3ODcsImlkcCI6ImxvY2FsIiwiY2xpZW50X25hbWVzcGFjZSI6IkRhZmZ5IER1Y2siLCJyb2xlIjpbImEiLCJiIiwiYyIsInVzZXIiXSwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJncmFwaFFMUGxheSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJhcmJpdHJhcnlfcmVzb3VyY2Vfb3duZXIiXX0.j0Ao7O40YixNv6LAst1fphIxMT5JUzX7nVYwQFMhspuUKpicByZzCck269HpLtQfsC6qDcd7dsuWMrGtETmz8Gv0i2Merkj_CZi9v7Pr9gW61hpUyeN10JQbNLKNcxQFesnKwVK_To7bxg2hzwTuDdFtjUGQQlYpP9MGZa4yMQ9JCK_4qpi38dJO3uQY82AhBEg_iKC60N5F56Snh0ODgn0iIe1x831dZeAlYzbFdV23m7jmwNGBDxMeP7CzKNsl0C6ostS6mVpuGWREWnd5-gt0YkHgAE6FVgQsWnHaVoVuXTa2otuWNJOAUeOHmIH15ErhuP0_l8j7Lbz9fSldfg",
        "token_type": "Bearer",
        "httpHeaders": [
          {
            "name": "x-authScheme",
            "value": "self"
          }
        ]
      }
    ]
  }
}
```
[jwt.io](https://jwt.io/#debugger-io?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQyMjQ2OTMsImV4cCI6MTU1NDIyODI5MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDIyNDY5MywiaWRwIjoibG9jYWwiLCJyb2xlIjpbImEiLCJ1c2VyIl0sImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiYXJiaXRyYXJ5X3Jlc291cmNlX293bmVyIl19.MlYPEv0xNe_JjwJJFIw_lJ5ZpxRrnt_1J1WVOXtmAlv3qzM2gXzLRUDiK_geOaO2jz_WM9VFFZvSSEMwuo8QT5Iez5SzTX_nPA5nEDCNJm38cCNgoGDky-j2ZLSojBCqWz3yV6ORQIdRwpxlBGyqyL8Ng25ICkiTOx4gKx0m06SUtk6ezXC_N1BzLO6rHOK1rNTsaeFbJeTk__Mrkzq3nq-9sBhgQVW4vm5giosLkTeEHaamYmkn_9QXtDHYGw5ZPPRHUNlaEJzXHNeSM_suP1B1F4zsdnaB_ixo1xauaRUwix2D7rFeX3V7trDhG1cBazutForWYACqSGQn12nzZA&publicKey=-----BEGIN%20PUBLIC%20KEY-----%0AMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArfiIm%2Bsg%2FHE%2Bk71K5GAj%0A%2FwRm36X87pR9UivWeepzAGVU0aKM9pFs3i%2BJftHhozYq8yrumx%2BwwCgXqi%2FgZN64%0AbDOkt1zM7fopRO6jfgJzMTJFL7jUs1T1yHzXefSQLG6ZPf0KsvAz5L2WdezdVrYi%0A6YZCG873QkAgSfx5ZSR96x5TPqUxh%2BIzdWp62xtLhySzMNpTXHTkRzhdtDKdPahU%0AW3M6LIYcnDvPrNE5adk9eJzpsgUOcUG7JrEY2UqJ9VkCeES2Q%2FDzGpcTGLQoLG4i%0AEJ%2FuGIHMZID66A00MwXbJb8kyri6GoGtWjtxLSFHF%2Feet4mho%2BUd92fpFWxyD4SG%0AxQIDAQAB%0A-----END%20PUBLIC%20KEY-----%0A)  

# Summary
Writing an exchange means you are taking full control over what gets exchanged for what.  The only thing you are using the GraphQLPlay stack for is custodial duties of an OAuth2 stack.  

You can use this exchange to get an access_token, and the access_token can be used as a bearer into any API anywhere.  So if all you use from the GraphQLPlay's offerings is as an exchange, its still pretty significant.

