# Custom Bind

1. Introduce a new scheme in the host apps [appsettings](../src/IdentityServer4-Extension-Grants-App/appsettings.json)
i.e. Copy the google scheme
```
{
  "scheme": "google-my-custom",
  "clientId": "mvc2",
  "authority": "https://accounts.google.com",
  "callbackPath": "/signin-google",
  "additionalEndpointBaseAddresses": [

  ]
}
```
2. Each of the schemes for now requires a custom OIDC Validator that turns the incoming id_token into a principal.  Future work is to have a generic OIDC that is data driven but I wanted to have something in place that allowed ultra fine granular control.
Introduce the [GoogleMyCustomOIDCTokenValidator](../src/Google.Validator/GoogleMyCustomOIDCTokenValidator.cs)  
Make sure you assign the *google-my-custom" in the constructor.  
```
TokenScheme = "google-my-custom";
```

3. Add a registration extension [here](../src/Google.Validator/Extensions/AspNetCoreServiceExtensions.cs)  
```
  public static void AddGoogleMyCustomOIDCTokenValidator(this IServiceCollection services)
  {
      services.AddSingleton<ISchemeTokenValidator, GoogleMyCustomOIDCTokenValidator>();
  }
```  
4. Register the new GoogleMyCustomOIDCTokenValidator in  [Startup.cs](../src/IdentityServer4-Extension-Grants-App/Startup.cs). 
```
services.AddGoogleMyCustomOIDCTokenValidator();
```

**NOTE**: The above makes sure that we have a valid id_token and takes care of rejecting it before we proceed to the next step...  


5. Introduce a new [IdentityPrincipalEvaluator](../src/TokenExchange.Contracts/GoogleMyCustomIdentityPrincipalEvaluator.cs) where you use the scheme name *google-my-custom" in the constructor to name the IdentityPrincipalEvaluator.  
This is where the real customization happens.  The above GoogleMyCustomOIDCTokenValidator was only responsible from decoding the id_token and turning it into a ClaimsPrincipal.  Here you can evaluate the claims, and with an optional string array called "Extras" that is passed in via the bind call, you can decide exactly what the following access_token will look like.
You can reject the bind here as well.  



6. Add a registration extension [here](../src/TokenExchange.Contracts/Extensions/AspNetCoreServiceCollectionExtensions.cs)  
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
    "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImE0MzEzZTdmZDFlOWUyYTRkZWQzYjI5MmQyYTdmNGU1MTk1NzQzMDgiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIxMDk2MzAxNjE2NTQ2LWVkYmw2MTI4ODF0N3JrcGxqcDNxYTNqdW1pbnNrdWxvLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiMTA5NjMwMTYxNjU0Ni1lZGJsNjEyODgxdDdya3BsanAzcWEzanVtaW5za3Vsby5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsIm5vbmNlIjoiNjM2ODk4MTI0MzU1MzAyNzQ3Lk5EQXpOekUyTkRrdFltVXpZeTAwWmprekxUa3lNV0V0WkdVd05qUm1NVE16WldKa1ltTXdaRGMxTkRVdFlUUXdNaTAwTlRNekxXRXhPR1F0WXpVMll6STRPVEZpTkRBeSIsIm5hbWUiOiJIZXJiIFN0YWhsIiwicGljdHVyZSI6Imh0dHBzOi8vbGg0Lmdvb2dsZXVzZXJjb250ZW50LmNvbS8tdXZPc3RBRzhUcWsvQUFBQUFBQUFBQUkvQUFBQUFBQUFGVTAvaU9OSWpKbjNkZHMvczk2LWMvcGhvdG8uanBnIiwiZ2l2ZW5fbmFtZSI6IkhlcmIiLCJmYW1pbHlfbmFtZSI6IlN0YWhsIiwibG9jYWxlIjoiZW4iLCJpYXQiOjE1NTQyMTU2MzUsImV4cCI6MTU1NDIxOTIzNSwianRpIjoiZTBiOGRjMWQ2MjhmOTkzMDkwOTE2MmVlN2VmZmM4MzI5MmNjMzc3NyJ9.ZSnnfnDBJwi1UlRdt1BjzAho_HEraRJpN24LxXulf4iH7xvkHYPGB6gUXYzl-cDgKSICls0oRKx03jhezy1KgTp51EYTL7dUnXEsZEWPUuSvbUJ6E1Lvmanx8wqFYCT_dIZn1P6XcjXXG8cF9neAluJks2nulYFYhnM3n2VC0JNH66FPgtUcMhVl1Uk7GSA1LqvyuV8avkOi6rRKkyl9t28BTFJvE-ouR_LFtKfzIzXoPgpglspqhMmzMfUE-CELDJg74rR7oR0yG7HXGi6fd_JUGWLonjd3-z4ymoOCzyfrpeUiBWQj_ymQCpdDgYP5L343Bva72JhxHcAoc_k-Rw",
    "tokenScheme": "google-my-custom",
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
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQyMTc2MTYsImV4cCI6MTU1NDIyMTIxNiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDIxNzYxNiwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImEiLCJ1c2VyIl0sImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiYXJiaXRyYXJ5X3Jlc291cmNlX293bmVyIl19.n-G0PAj9-LgDXYNv8V2dVcKMxu-ipFFW7p2GcGNoggchWQ9o_J-xjb2y1SRq3ok2tWaffqx4C0dGjT3C-Kr__xkQ0OgNrhM7GjY7pRJTM8KyIoVMpTAxDlh9belpIkUuKKqvbtGNysEe3IqFGZLK09q2sbDgDWoFmIf9eEkTNvbeWI09D2Y0Mghqtf6FphFuJuMJstttFdopEFNCuo9NbH0HPs7vGNe07L3QRNxRpyTyigNtj0-rGfwyv2uP4rN5slSBmG1akvIqCWoIII6y3BFf4JCSoxZ-x2NnDWnmj1MC6ybCteFmT3f2IZ-IHTR9C1LkECaIo1kMN4lC7VfXZg",
        "authority": "https://localhost:44371",
        "expires_in": 3600,
        "refresh_token": "CfDJ8AxolQtSBd5DoqmD23krznhY6imu-BniQkEf2dXrn0J2P_izxtVwKQ-gq-SbbXRgCgoihIWiJfxncofy6vGDD2cWjBN7FFar4U781aYdyf4lzJjoD9tQc1OIUQMwak0boirix2cqRm8iCPkvhjP3ysYoUFbBz6d4kwxdLgykxqTYx4QUqmC5q6igfdWBy90tRIpm-uAA8EedtrrIhwDaEK8",
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
[jwt.io](https://jwt.io/#debugger-io?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQyMTc2MTYsImV4cCI6MTU1NDIyMTIxNiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6IjEwNDc1ODkyNDQyODAzNjY2Mzk1MSIsImF1dGhfdGltZSI6MTU1NDIxNzYxNiwiaWRwIjoibG9jYWwiLCJyb2xlIjpbImEiLCJ1c2VyIl0sImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwic2NvcGUiOlsiZ3JhcGhRTFBsYXkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiYXJiaXRyYXJ5X3Jlc291cmNlX293bmVyIl19.n-G0PAj9-LgDXYNv8V2dVcKMxu-ipFFW7p2GcGNoggchWQ9o_J-xjb2y1SRq3ok2tWaffqx4C0dGjT3C-Kr__xkQ0OgNrhM7GjY7pRJTM8KyIoVMpTAxDlh9belpIkUuKKqvbtGNysEe3IqFGZLK09q2sbDgDWoFmIf9eEkTNvbeWI09D2Y0Mghqtf6FphFuJuMJstttFdopEFNCuo9NbH0HPs7vGNe07L3QRNxRpyTyigNtj0-rGfwyv2uP4rN5slSBmG1akvIqCWoIII6y3BFf4JCSoxZ-x2NnDWnmj1MC6ybCteFmT3f2IZ-IHTR9C1LkECaIo1kMN4lC7VfXZg&publicKey=-----BEGIN%20PUBLIC%20KEY-----%0AMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArfiIm%2Bsg%2FHE%2Bk71K5GAj%0A%2FwRm36X87pR9UivWeepzAGVU0aKM9pFs3i%2BJftHhozYq8yrumx%2BwwCgXqi%2FgZN64%0AbDOkt1zM7fopRO6jfgJzMTJFL7jUs1T1yHzXefSQLG6ZPf0KsvAz5L2WdezdVrYi%0A6YZCG873QkAgSfx5ZSR96x5TPqUxh%2BIzdWp62xtLhySzMNpTXHTkRzhdtDKdPahU%0AW3M6LIYcnDvPrNE5adk9eJzpsgUOcUG7JrEY2UqJ9VkCeES2Q%2FDzGpcTGLQoLG4i%0AEJ%2FuGIHMZID66A00MwXbJb8kyri6GoGtWjtxLSFHF%2Feet4mho%2BUd92fpFWxyD4SG%0AxQIDAQAB%0A-----END%20PUBLIC%20KEY-----%0A)
