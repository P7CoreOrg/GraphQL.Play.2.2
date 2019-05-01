# External Token Exchange  

A custom implementation of an [ITokenExchangeHandler](../src/TokenExchange.Contracts/ITokenExchangeHandler.cs) which lets you [configure](../src/GraphQLPlayTokenExchangeOnlyApp/appsettings.Development.TokenExchange.json) in an external REST based handler.  
A simple [implementation](../src/BriarRabbitTokenExchange) that supports the 2 required endpoints can be seen [here](../src/BriarRabbitTokenExchange).  


## token-exchange-validator
```
POST
https://localhost:5001/api/token_exchange/briar_rabbit/token-exchange-validator
```
## Headers
```
Authorization:Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTU2NTExMDcsImV4cCI6MTU1NTY1NDcwNywiaXNzIjoiaHR0cHM6Ly9ncmFwaHFscGxheTIyLmF6dXJld2Vic2l0ZXMubmV0IiwiYXVkIjpbImh0dHBzOi8vZ3JhcGhxbHBsYXkyMi5henVyZXdlYnNpdGVzLm5ldC9yZXNvdXJjZXMiLCIzUEFwaSJdLCJjbGllbnRfaWQiOiJiMmItY2xpZW50IiwiY2xpZW50X25hbWVzcGFjZSI6ImIyYi1vcmciLCJzY29wZSI6WyIzUEFwaSJdfQ.d_S_JP8hAF9JGtA08qW7VralJeTGc-DlmeJNg-EsG_dGjvfbpPw-ufUS4sBySsWm5RbF0BKCyv_veTKfaZzpivOTVhhPOaXnaWADwUq9bnQWN3LOLgSScj5GVwA6vy4d00p4vYqeaUw8uy59rc_Nk5ZZ4H55VH091dpKg1_zgzGELrzK8G2neACdeHt1wsp1MSUo1dk4crj0sTqdubx9Iseztv4_Zbw-1OChHcDy1sX_xrKH8WUxDaegBel1uX9HxxYf5qa61U9sqKEr0jn5PJyStH33Y-8JOD7pb_yd1BZq91qJgvzu9fLf4Wt5zBMl-FJp3EnDkb7WwQmivS8l6A
x-authScheme:self
Accept:application/json
Content-Type:application/json

```
## Resonse
```
[
  {
    "scope": "string",
    "arbitraryClaims": {
      "additionalProp1": [
        "string"
      ],
      "additionalProp2": [
        "string"
      ],
      "additionalProp3": [
        "string"
      ]
    },
    "subject": "string",
    "accessTokenLifetime": 0,
    "httpHeaders": [
      {
        "name": "string",
        "value": "string"
      }
    ]
  }
]
```
where **scope** is a space separated string.  


| scope  | description |
| ------------- | ------------- |
| offline_access  | Use if you want refresh_token(s)  |
| any_thing  | This is open ended  |  

example;
```
"scope": "offline_access my_custom_scope my_second_custom_scope"
```
where **arbitraryClaims** is a dictionary of arrays.  
This can be anything.  

example;
```
"arbitraryClaims": {
            "role": [
                "bigFluffy",
                "fluffyAdmin"
            ]
        }
```
where **subject** is a string.  
This can be anything.  
example;
```
 "subject": "MrRabbit"
```
where **accessTokenLifetime** is a int.  
This value is configured as a high end value, and here you get to pull it back.  You can't make it bigger than what is configured on the back end.  
example;
```
"accessTokenLifetime": 3600
```
where **httpHeaders** is an open ended name value pair that you can tell clients what they need to put as headers when they make authorized calls using the access_tokens.
This can be anything.  

example;
```
 "httpHeaders": [
          {
            "name": "x-bunnyAuthScheme",
            "value": "BunnyAuthority"
          }
        ]
```
```
[
    {
        "scope": "offline_access graphQLPlay",
        "arbitraryClaims": {
            "role": [
                "bigFluffy",
                "fluffyAdmin"
            ]
        },
        "subject": "MrRabbit",
        "accessTokenLifetime": 3600,
        "httpHeaders": [
          {
            "name": "x-bunnyAuthScheme",
            "value": "BunnyAuthority"
          }
        ]
    }
]
```
