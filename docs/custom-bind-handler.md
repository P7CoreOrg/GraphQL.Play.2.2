# Custom Bind


1. Introduce a new [IdentityPrincipalEvaluator](../src/TokenExchange.Contracts/GoogleMyCustomIdentityPrincipalEvaluator.cs) where you use the scheme name *google-my-custom" in the constructor to name the IdentityPrincipalEvaluator.  
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

8. GraphQL Bind Call  
## Query  
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
## Variables  
```
{
  "input": {
    "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImE0MzEzZTdmZDFlOWUyYTRkZWQzYjI5MmQyYTdmNGU1MTk1NzQzMDgiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIxMDk2MzAxNjE2NTQ2LWVkYmw2MTI4ODF0N3JrcGxqcDNxYTNqdW1pbnNrdWxvLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiMTA5NjMwMTYxNjU0Ni1lZGJsNjEyODgxdDdya3BsanAzcWEzanVtaW5za3Vsby5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsIm5vbmNlIjoiNjM2ODk4MjE0Nzc0MDc0NTU2LlpHWTVaVEU0WldNdE1tVmpNeTAwTmpBMExUbGlaV1l0TkdZeVlqTmhPR015TURJM09XSmpaR1pqWVdVdE5qUmxaUzAwWkRsbExXRXhNbVV0TnpJek9EZGxaRFF6WXpsaCIsIm5hbWUiOiJIZXJiIFN0YWhsIiwicGljdHVyZSI6Imh0dHBzOi8vbGg0Lmdvb2dsZXVzZXJjb250ZW50LmNvbS8tdXZPc3RBRzhUcWsvQUFBQUFBQUFBQUkvQUFBQUFBQUFGVTAvaU9OSWpKbjNkZHMvczk2LWMvcGhvdG8uanBnIiwiZ2l2ZW5fbmFtZSI6IkhlcmIiLCJmYW1pbHlfbmFtZSI6IlN0YWhsIiwibG9jYWxlIjoiZW4iLCJpYXQiOjE1NTQyMjQ2NzcsImV4cCI6MTU1NDIyODI3NywianRpIjoiMmU0MWM4YzU0MjczMDQ5NjFlNGJhYmQ0MjQxN2NiMjc5MTQ2ZTIyMSJ9.WC9HFH4oogfdoVwpPX5XVzckoS6pwrFmYKXWJpAG73-GZwQEq8x7bd-UneWWsb-K_SMG0YTcyHfV5gK0cnan3ElrTtN7xv4THPmzVLo570zEYlmmQZlEUgfOg2X4xraF2hPnr0G0eOkdKhmWloTZzopgngrDaFbs3xrnDVRNPM9A8max6eSaZHNBM6IZhJZVHMnkY9B3AlOOyhts8Wv0kWXMpT8WzrpHf8wlhdpDmMJkBEKDvdefE6Octea4ExNRL9r6kW9LKUgQAse71KioaVUQ8gD5VysoMM_W3pXaAKP_oAJge_tDRhKc8fdS3csAsiYyYdqRyyEk_5hdep0jYw",
    "tokenScheme": "google",
    "exchange": "google-my-custom",
     "extras":["a"]
  }
}
```
## Results
```
{
  "data": {
    "bind": {
      "authorization": {
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQyMjQ2OTMsImV4cCI6MTU1NDIyODI5MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDIyNDY5MywiaWRwIjoibG9jYWwiLCJyb2xlIjpbImEiLCJ1c2VyIl0sImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiYXJiaXRyYXJ5X3Jlc291cmNlX293bmVyIl19.MlYPEv0xNe_JjwJJFIw_lJ5ZpxRrnt_1J1WVOXtmAlv3qzM2gXzLRUDiK_geOaO2jz_WM9VFFZvSSEMwuo8QT5Iez5SzTX_nPA5nEDCNJm38cCNgoGDky-j2ZLSojBCqWz3yV6ORQIdRwpxlBGyqyL8Ng25ICkiTOx4gKx0m06SUtk6ezXC_N1BzLO6rHOK1rNTsaeFbJeTk__Mrkzq3nq-9sBhgQVW4vm5giosLkTeEHaamYmkn_9QXtDHYGw5ZPPRHUNlaEJzXHNeSM_suP1B1F4zsdnaB_ixo1xauaRUwix2D7rFeX3V7trDhG1cBazutForWYACqSGQn12nzZA",
        "authority": "https://localhost:44371",
        "expires_in": 3600,
        "refresh_token": "CfDJ8AxolQtSBd5DoqmD23krzng6GgHwvamQ4CRZbrvdhB3Id5wAhnFpPJXhUkvErWFYW3EJpUcmWEXRV0SMkL_stTSTwGJ8RZ38mvDnNrIw5ygRmBFCE56_BUn0GO6V3dkxmMhE3pX1VY4hdcL0xMvpEXtHvddRAkxP113AnQ715d2w_tmqMy_11RFXcpWRe6LaVWENZ5DAQ5mLy4O124Vi4nU",
        "token_type": "Bearer",
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
[jwt.io](https://jwt.io/#debugger-io?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQyMjQ2OTMsImV4cCI6MTU1NDIyODI5MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDIyNDY5MywiaWRwIjoibG9jYWwiLCJyb2xlIjpbImEiLCJ1c2VyIl0sImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiYXJiaXRyYXJ5X3Jlc291cmNlX293bmVyIl19.MlYPEv0xNe_JjwJJFIw_lJ5ZpxRrnt_1J1WVOXtmAlv3qzM2gXzLRUDiK_geOaO2jz_WM9VFFZvSSEMwuo8QT5Iez5SzTX_nPA5nEDCNJm38cCNgoGDky-j2ZLSojBCqWz3yV6ORQIdRwpxlBGyqyL8Ng25ICkiTOx4gKx0m06SUtk6ezXC_N1BzLO6rHOK1rNTsaeFbJeTk__Mrkzq3nq-9sBhgQVW4vm5giosLkTeEHaamYmkn_9QXtDHYGw5ZPPRHUNlaEJzXHNeSM_suP1B1F4zsdnaB_ixo1xauaRUwix2D7rFeX3V7trDhG1cBazutForWYACqSGQn12nzZA&publicKey=-----BEGIN%20PUBLIC%20KEY-----%0AMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArfiIm%2Bsg%2FHE%2Bk71K5GAj%0A%2FwRm36X87pR9UivWeepzAGVU0aKM9pFs3i%2BJftHhozYq8yrumx%2BwwCgXqi%2FgZN64%0AbDOkt1zM7fopRO6jfgJzMTJFL7jUs1T1yHzXefSQLG6ZPf0KsvAz5L2WdezdVrYi%0A6YZCG873QkAgSfx5ZSR96x5TPqUxh%2BIzdWp62xtLhySzMNpTXHTkRzhdtDKdPahU%0AW3M6LIYcnDvPrNE5adk9eJzpsgUOcUG7JrEY2UqJ9VkCeES2Q%2FDzGpcTGLQoLG4i%0AEJ%2FuGIHMZID66A00MwXbJb8kyri6GoGtWjtxLSFHF%2Feet4mho%2BUd92fpFWxyD4SG%0AxQIDAQAB%0A-----END%20PUBLIC%20KEY-----%0A)
