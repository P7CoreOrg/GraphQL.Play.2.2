# Building Out An Authorization Service

The TokenExhange service is really a pipeline where tokens come in, a series of pre-processors evaluates them and then finally a decision is made on what tokens are to be minted.  The name of the exchange depends on what is in the pipeline.

Lets look at an example of a user's id_token being sent through the exchange where the final result is an access_token to a service that the user purchased.

The Entry condition to the exchange would be the following;
1. The users id_token
2. A hint as to what exchange we want the token sent through.  This could also be interpreted as what API group, or what service that downstream tokens are being requested for.

So I am user **Daffy-Duck** and I want access to the **analytics-service** which I have paid for!


![PlantUML model](http://www.plantuml.com/plantuml/png/5SnB2i90303GlQVG0pI1RWItBjm8rYF4JDefpcV89CZSNleEy2vxuUH1NDCvchL-GrJwmRPRz8ztKQx5f1XmpRXSxrNbfHi-b5fyFs59D6266JDr4yLGkMEWTHrJSFvEnm3DSphz0G00)  

The GraphQL Query to the exchange would look like the following;
## Query
```
query q($input: tokenExchange!) {
  tokenExchange(input: $input){
    identityToken{
      authority
      expires_in
      hint
      httpHeaders{
        name
        value
      }
      id_token
    }
    customToken{
      authority
      hint
      httpHeaders{
        name
        value
      }
      token
      type
    }
    accessToken{
      hint
      authority
      expires_in
      access_token
      refresh_token
      token_type
      httpHeaders
      {
        name
        value
      }
    }
  }
}
```
## Input 
```
{
  "input": {
    "exchange": "pipeline_briar_rabbit_final_exchange",
    "extras": ["analytics-service", "backup-service", "superadmin-so-I-can-hack-you-service"],
    "tokens": [{
      "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFjN2RmOWU1YjBjYzNkZDI1NmE1MWFiNzcwYmM2ZTAzIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTgyMjY2NTUsImV4cCI6MTg3ODIyNjY1NSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6ImFwcC1pZGVudGl0eS1jbGllbnQiLCJpYXQiOjE1NTgyMjY2NTUsImF0X2hhc2giOiJHTGNiY0F2S25ibUlfc3RHZ0ZwUUdRIiwic3ViIjoiNGJhMmY2ODAtMjY1NC00ODcwLWI0YTYtMjdjN2ZjMmQwMDk4IiwiYXV0aF90aW1lIjoxNTU4MjI2NjU1LCJpZHAiOiJEZW1vIiwiY2xpZW50X25hbWVzcGFjZSI6ImFwcC1pZGVudGl0eS1vcmciLCJhcHBJZCI6Ik5JUyIsIm1hY2hpbmVJZCI6InNvbWUgZ3VpZCIsImFtciI6WyJhcmJpdHJhcnlfaWRlbnRpdHkiXX0.eCdXVVctM-gFfXpZOY4eRi_xesLu6poLwZp_u8aufYCFM8OAnLdwidkJP9XPE1fmfHSGkRH0G4aYgtaJNpdtzAkChutcafziitui_QLMHdgJWIvzhrZyRa0BiEtTJzkCyrFWawQkTy3mjlA6iwz_DFNDjyQK6Fdb8hOC0HXAGi1xuG2Rn2eBgIlaFFrMxF_ad0_crB-7eaYxxnkx3P045z54_zdSltxQmNpUsbaV2OJr2f56Mm_yYDr8tKAKNofCKBzHnthWIlLmapeAPOguJZLKZPAZan4suIL1DJr6oOygI4ZZIq3BdeNjWOGhfzmpeeGwsHJk1jXo5refnYIG-A",
      "tokenScheme": "self-oidc"
    }]
  }
}
```  
In the input you are telling the exchanger to use **pipeline_briar_rabbit_final_exchange**.  
The exchanger can accept additional hints in the form of a string array, which is about as generic as I could come up with.  These hints can be mapped to resources that only mean something to the exchange.  
The exchanger can accept an array of **tokens** along with the metadata **tokenScheme** which tells the backend what this token is.  Usually the **tokenScheme** maps to a wellknown OAuth2 authority.  

What we want returned is access_token(s) to the requested services.  
## Response  
```
{
  "data": {
  "tokenExchange": [{
      "identityToken": null,
      "customToken": {
        "authority": "454d23e2-aff4-4cde-b091-b56062a95174",
        "hint": "briar_rabbit/token-exchange-validator/custom",
        "httpHeaders": [{
          "name": "f7bfaa16-524d-4463-99b4-737683c18ac8",
          "value": "298cef26-f779-4c35-b121-6e66393636eb"
        }],
        "token": "6acdd653-3fed-43f9-81e0-71b9ebeb0ea6",
        "type": "5fa98272-9a0f-44ea-8182-179fa8f3a3b4"
      },
      "accessToken": null
    },
    {
      "identityToken": {
        "authority": "https://localhost:5001",
        "expires_in": 3600,
        "hint": "briarRabbitHint_Identity",
        "httpHeaders": null,
        "id_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFjN2RmOWU1YjBjYzNkZDI1NmE1MWFiNzcwYmM2ZTAzIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTg0NTA3NDQsImV4cCI6MTU1ODQ1NDM0NCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJpYXQiOjE1NTg0NTA3NDQsImF0X2hhc2giOiJJNXI5SGxRTThPMGZtU2k4TTNCQ1d3Iiwic3ViIjoiTXJSYWJiaXQiLCJhdXRoX3RpbWUiOjE1NTg0NTA3NDQsImlkcCI6IkRlbW8iLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInJvbGUiOlsiYmlnRmx1ZmZ5IiwiZmx1ZmZ5QWRtaW4iXSwiYW1yIjpbImFyYml0cmFyeV9pZGVudGl0eSJdfQ.FFK-3ZF_u8P0mOBXaQcNSQgmqBIbv7FycWErMumuLnV_C_l2OEmYZ__PDtOX7W28O0lxGP4aozcXaKdDRUXLaXOxK8vLyNKGmY59ElwwFmbq7Doql-pEsAUmHzIonYPqggQI6Ub7G_GGeSUvu3MHZrVcgg9YpEdYMFP4XUxZ90houGMh79iK0vZNijatGp7f-eT6rmuKzsyL5WMkF16baHTmQBhNJLXQp1uLnHIqdKm3psOje6IDEPi8-LbGR1kcE_1kIwhjtNQxLREb6x86hWdhIkEBo1mVzd8D2lFWKcFCfoI5_HpeKPyq6vurokJV90fNg9BVGnQ0iW875C_8tQ"
      },
      "customToken": null,
      "accessToken": null
    },
    {
      "identityToken": null,
      "customToken": null,
      "accessToken": {
        "hint": "briarRabbitHint_Access",
        "authority": "https://localhost:5001",
        "expires_in": 3600,
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFjN2RmOWU1YjBjYzNkZDI1NmE1MWFiNzcwYmM2ZTAzIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTg0NTA3NDQsImV4cCI6MTU1ODQ1NDM0NCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo1MDAxL3Jlc291cmNlcyIsImJyaWFyIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6Ik1yUmFiYml0IiwiYXV0aF90aW1lIjoxNTU4NDUwNzQ0LCJpZHAiOiJsb2NhbCIsImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwicm9sZSI6WyJiaWdGbHVmZnkiLCJmbHVmZnlBZG1pbiJdLCJzY29wZSI6WyJicmlhciIsImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.OBfkqcY2UjQNt6FAz-oTqNjfW5807hVxtVZIhCX-X2bEicnLuRzhZHauVrmPH2XJps0qrjlNeb2Q3fi48VrX8Ys9Ck1uQeeE1Psuu0aiPVoGd-r2v57zfZvXLplS1rTqVGjnyiaFzh5j8kjjeRhnWg-VpgFqvcg6FmjNqeY9prS_SJacO9CJQq4sa37pus2PSGswb6yvYE93X4cQj1qjZynFsc_7lG0l59e3IZVVNwzqagb2PekUVT-cg77vCGs721tCZutunDGlDMrquyL0FP_0rvBEVR-6yj_B8vHlRMSKYUS8hrQIO5XCffjGKTlUhbDyTN-aDUulEwS32XdX4w",
        "refresh_token": "CfDJ8GiyAO4NV5xKr2rVHroAkh_NRv-rYmlyVHkXpfAW-Um9gGSeavd-c2ZxfB582E6xv8zza4CEgWDrvrlV2CAEaKbloJ-ETqQWl9Dj6n7SjTYdLKJ7-wzpmgEhMVZbDRSUKy1fgGX8qoMJlv2Dwl7G3sdh6dq3X7RH0hEe14a5sUpVjxUVGc6ab3nO93l-nKIBCzKRY5vfBP_1_3PZivOSWMA",
        "token_type": "Bearer",
        "httpHeaders": [{
          "name": "x-bunnyAuthScheme",
          "value": "BunnyAuthority"
        }]
      }
    }
    ]
  }
}
```

The backend exchange handler will make the determination if you actually are getting any of the requested data.  Even if you pass all validation, you may not ever get the **refresh_token**, etc.  

Based on the exchange configuration on the backend the token(s) are sent through a pipeline, which can reject the request at any stage.  
![PlantUML model](http://www.plantuml.com/plantuml/png/5Sqn3i8m30NGdLF00MfBNP2OAZ6m856EODAl9Y9r8iSHS7jWUkERcejvt_Bka3Jalg1RtaVtsaxC9YzQayV-w0qMYZhKAPIDv-Ev66wsyiManlkLvYmV6cdaJPh3U2cXiPSdTC0xHD4LGqqLEIce_iS9klpy0W00)  

What makes this an **Authorization Exchange** is what happens in the pre-processors and final exchange.  The role of the pre-processers are as follows;  
1. Validate the tokens.  i.e. if these are OIDC tokens, than validate the signature and expiration, etc.  
2. Validate that the **subject** in the token exists in our user database.  
3. Validate that the user has paid for the services being requested.  
  ```
   "extras": ["analytics-service", "backup-service", "superadmin-so-I-can-hack-you-service"],
  ```
4. The final exchange determines what downstream tokens are to be issued.  i.e. access_token(s) with specific claims, etc.  

Here is a contrived example of a pre-processor that validates the OIDC token and stips off the signature to make it smaller when we want to send it out via a backchannel request.  [ValidateAndStripSignatureTokenExchangeHandlerPreProcessor](../src/TokenExchange.Contracts/Services/ValidateAndStripSignatureTokenExchangeHandlerPreProcessor.cs)  and it [DI Registration](../src/TokenExchange.Contracts/Extensions/AspNetCoreServiceCollectionExtensions.cs)  

An example configuration of a pipeline can be seen [here](../src/GraphQLPlayTokenExchangeOnlyApp/appsettings.Development.TokenExchange.json)  

The pipeline **pipeline_briar_rabbit** has some pre-processor(s)..
```
 "preprocessors": [ "validate-strip-signature" ],
``` 
and a final echange  
```
 "finalExchange": "briar_rabbit"
```
where the final exchange is an [external backchannel callout](../src/BriarRabbitTokenExchange) that responds back with token minting instructions.  The pipeline here wants the host to perform the **custodial duties** of minting an OAuth2 token.  


```
{
  "tokenExchange": {
    "externalExchanges": [
      {
        "exchangeName": "briar_rabbit",
        "mintType": "externalExchangeHandler",
        "externalExchangeHandler": {
          "url": "https://localhost:5001/api/token_exchange/briar_rabbit/token-exchange-validator",
          "clientId": "arbitrary-resource-owner-client"
        },
        "passThroughHandler": {
          "exchangeUrl": "https://localhost:5001/api/token_exchange/briar_rabbit/pass-through-handler"
        },
        "oAuth2_client_credentials": {
          "clientId": "b2b-client",
          "clientSecret": "secret",
          "authority": "https://localhost:5001/",
          "additionalHeaders": [
            {
              "name": "x-authScheme",
              "value": "self"
            }
          ]
        }
      }
    ],
    "pipelineExchanges": [
      {
        "exchangeName": "pipeline_briar_rabbit",
        "preprocessors": [ "validate-strip-signature" ],
        "finalExchange": "briar_rabbit"
      }
    ]
  }
}
```

