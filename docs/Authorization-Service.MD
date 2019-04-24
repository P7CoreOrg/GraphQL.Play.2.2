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
    authority
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
```
## Input 
```
{
  "input": {
    "exchange": "pipeline_briar_rabbit",
    "extras": ["analytics-service", "backup-service", "superadmin-so-I-can-hack-you-service"],
    "tokens": [{
      "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTU3ODAzNDksImV4cCI6MTg3NTc4MDM0OSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6ImFwcC1pZGVudGl0eS1jbGllbnQiLCJpYXQiOjE1NTU3ODAzNDksImF0X2hhc2giOiJBQlFqRHBXV2J5cFFVSHhwNUg4Ukt3Iiwic3ViIjoiYmNkZTM4OGYtOGUxMC00MzY0LWFjZWEtMWJjYmE1Y2I1ZGFiIiwiYXV0aF90aW1lIjoxNTU1NzgwMzQ4LCJpZHAiOiJsb2NhbCIsImNsaWVudF9uYW1lc3BhY2UiOiJhcHAtaWRlbnRpdHktb3JnIiwiYXBwSWQiOiJOSVMiLCJtYWNoaW5lSWQiOiJzb21lIGd1aWQiLCJhbXIiOlsiYXJiaXRyYXJ5X2lkZW50aXR5Il19.a180J543Vky8xdHne7VeU27UEtalePzQ4W3k4fkn0i1NchVoWOW_rqT-oW-25Mh_wlNeIAlc4kWGtfvYCs0TgERwig8_Mk3Q5Ay16vBgx_T8iLl4oGk8G11FtyCn8pdDDuVPsPggeiRKCbb-7p593WGsjDiFUwJO0bYOOtCPGcIsswZPzMZsKWpqzvSM6ibFlYmkMFdWf1dw7hSkPAs4_dbEg8Ab2PjR4pV6yRsHbTihII9edB4-dWqNIZBsYskjZRxIzifEdkuh-ms3IcXelM_4BEls9GlJPzAQRTQc465siffHYNbZRoR_3dIZPVZqn7P5yv5IX5I2wNnDzVIAuA",
      "tokenScheme": "self"
    }]
  }
}
```  
In the input you are telling stating the exchanger to use **pipeline_briar_rabbit**.  
The exhanger can accept additional hints in the form of a string array, which is about as generic as I could come up with.  
The exchanger can accept an array of **tokens** along with the metadata **tokenScheme** which tells the backend what this token is.  Usually the **tokenScheme** maps to a wellknown OAuth2 authority.  

What we want returned is access_token(s) to the requested services.  
## Response  
```
{
  "data": {
    "tokenExchange": [
      {
        "authority": "https://graphqlplay22.azurewebsites.net",
        "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTYxMTU1ODksImV4cCI6MTU1NjExOTE4OSwiaXNzIjoiaHR0cHM6Ly9ncmFwaHFscGxheTIyLmF6dXJld2Vic2l0ZXMubmV0IiwiYXVkIjpbImh0dHBzOi8vZ3JhcGhxbHBsYXkyMi5henVyZXdlYnNpdGVzLm5ldC9yZXNvdXJjZXMiLCJncmFwaFFMUGxheSJdLCJjbGllbnRfaWQiOiJhcmJpdHJhcnktcmVzb3VyY2Utb3duZXItY2xpZW50Iiwic3ViIjoiTXJSYWJiaXQiLCJhdXRoX3RpbWUiOjE1NTYxMTU1ODksImlkcCI6ImxvY2FsIiwiY2xpZW50X25hbWVzcGFjZSI6IkRhZmZ5IER1Y2siLCJyb2xlIjpbImJpZ0ZsdWZmeSIsImZsdWZmeUFkbWluIl0sInNjb3BlIjpbImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.EoMyhPq0JnZA5vSlYkhpB1NkqaqajeFaeTZcbzME8dOY-QkMqZUKT44hCScPP1N85LSQ_ey-iSZMYPYXpcDcLkZZeTx0stiSb3N5MbFVZHpEJSkTmc_CDLttvbdYGO2mDRo8tfOKlqBVjBkCxSMi3_v3qfQ28FDd7GLZPsVCU-OvCqjLvyI5WmfwpSZN7GkRjXjS6hHJF1kKt49_sqzWojRKu4-liVYb8F9dPhJkN35j5pbFUGJ5sYFwgXXz61Wp36ZUr0avLjlLT4V25_GFBBY9-vKp-DKh4l32iqUKeH6fZDxaq6d-UhNCUl9h-ZYzYv26uys7ekLGOfDoM04Dng",
        "refresh_token": "CfDJ8PIPlTwcUFFNqUfBjjZJF0-bYRoKj001l02QK8jRw8W4yGzN51K6m3qJ2-K14UjXtNRfvTxix8C5KJ0eFPgN3252Buftb4gcUVNTI68-ieyY5WaMexT8v1i44eP2i81YQ01dvJQk_l-CFXxXA9iJNedsQFMDt7W1PTWob9xXHQa4ENnFvXwvNukot-yh-iszfd5xzIIP3bbahAQrnOCirLY",
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

