# External Token Exchange  
The simplest way to externalize an exchange is to implement a POST endpoint which can be seen [here](../src/BriarRabbitTokenExchange/BriarRabbitController.cs).  This controller has an endpoint that returns OAuth2 minting instructions which tells the exchange to be our authority and take over the custodial responsiblity of managing tokens.    
```
[HttpPost]
[Authorize]
[Route("briar_rabbit/final-pipeline-exchange")]
public Task<List<ExternalExchangeTokenResponse>> PostFinalPipelineExchangeAsync(ExternalExchangeTokenExchangeHandler.TokenExchangeRequestPackage tokenExchangeRequest)
{...}
```

The exchange has a pipeline implementation where the simplest configuration tells the exchange not to look at the incomming tokens and simply pass them on to the external exchange validator.  The configuration can be seen [here](../src/GraphQLPlayTokenExchangeOnlyApp/appsettings.Development.TokenExchange.json)  

```
"externalExchanges": [
{
"exchangeName": "briar_rabbit",
"mintType": "externalFinalExchangeHandler",
"externalFinalExchangeHandler": {
  "url": "https://localhost:5001/api/token_exchange/briar_rabbit/final-pipeline-exchange",
  "clientId": "arbitrary-resource-owner-client"
},
"passThroughHandler": {
  "url": "https://localhost:5001/api/token_exchange/briar_rabbit/pass-through-handler"
},
"oAuth2_client_credentials": {
  "clientId": "b2b-client",
  "clientSecret": "secret",
  "authority": "https://localhost:5001/",
  "additionalHeaders": [
    {
      "name": "x-authScheme",
      "value": "self-oidc"
    }
  ]
}
}
]
....
 "pipelineExchanges": [
  {
    "exchangeName": "pipeline_briar_rabbit_final_exchange",
    "preprocessors": [ ],
    "finalExchange": "briar_rabbit"
  }
]
      
``` 

You can see that I have configured in where to get my b2b access_token from, and what custom header values I have to add to make the call to;
```
https://localhost:5001/api/token_exchange/briar_rabbit/token-exchange-validator
```
Upon receiving OAuth2 minting instructions from the external handler, I also configured in what OAuth2 client_id I am to use to mint the tokens.  The OAuth2 clients are configured [here](../src/GraphQLPlayTokenExchangeOnlyApp/appsettings.Development.Clients.json)  

```
"clientId": "arbitrary-resource-owner-client"
```


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
## Post Body
```
{
  "mapOpaqueKeyValuePairs": {
    "additionalProp1": [
      {}
    ],
    "additionalProp2": [
      {}
    ],
    "additionalProp3": [
      {}
    ]
  },
  "tokens": [
    {
      "token": "string",
      "tokenScheme": "string"
    }
  ],
  "extras": [
    "string"
  ]
}
```  
where **mapOpaqueKeyValuePairs** is a dictionary of key value pairs.  This set of data is an accumulation of all the pre-processers that fired prior to this final exchange being called.  You may not get anything but the option of a pre-processor to contribute something that gets passed downstresm is there.  
example;
```
"mapOpaqueKeyValuePairs": {
    "additionalProp1": [
      {"a","b"},
      {"c","d"}
    ]
 }
```  
where **tokens** is an array of tokens that have a metadata hint, "tokenScheme".
example;  
```
"extras": [
    "string"
  ]
```  
where **extras** is an open ended array of strings that are hints to the incoming exchange.  It is opaquely passed along.
example;  
```
"tokens": [
    {
      "token": "string",
      "tokenScheme": "string"
    }
  ]
``` 
## Response
```
[
  {
    "customTokenResponse": {
      "hint": "string",
      "type": "string",
      "token": "string",
      "authority": "string",
      "httpHeaders": [
        {
          "name": "string",
          "value": "string"
        }
      ]
    },
    "arbitraryResourceOwnerTokenRequest": {
      "hint": "string",
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
    },
    "arbitraryIdentityTokenRequest": {
      "hint": "string",
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
      "identityTokenLifetime": 0,
      "httpHeaders": [
        {
          "name": "string",
          "value": "string"
        }
      ]
    }
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
	"customTokenResponse": {
		"hint": "briar_rabbit/token-exchange-validator/custom",
		"type": "9bc19743-8c06-4ea7-a111-509def572370",
		"token": "cf3343dc-8698-4ad5-be16-9ffcd42d5bda",
		"authority": "ab0f13dc-2848-4b38-afc6-83611a4f3b9d",
		"httpHeaders": [{
			"name": "36eb35e7-bd8a-4261-8c41-09c153c3e6e9",
			"value": "45eb1260-12d7-44b7-a0db-5303f845369a"
		}]
	},
	"arbitraryResourceOwnerTokenRequest": {
		"hint": "briarRabbitHint_Access",
		"scope": "offline_access graphQLPlay",
		"arbitraryClaims": {
			"role": ["bigFluffy", "fluffyAdmin"]
		},
		"subject": "MrRabbit",
		"accessTokenLifetime": 3600,
		"httpHeaders": [{
			"name": "x-bunnyAuthScheme",
			"value": "BunnyAuthority"
		}]
	},
	"arbitraryIdentityTokenRequest": {
		"hint": "briarRabbitHint_Identity",
		"scope": "briar",
		"arbitraryClaims": {
			"role": ["bigFluffy", "fluffyAdmin"]
		},
		"subject": "MrRabbit",
		"identityTokenLifetime": 3600,
		"httpHeaders": null
	}
}]
```

The Kit exposes the following OAuth2 extension grants, and this reponse is telling the exchange to use these extension grants to mint the tokens.

## OAuth2
[IdentityServer4](https://github.com/IdentityServer/IdentityServer4) is used as the OAuth2 engine.  So out of the box you get stuff like **client_credentials** flows.  I use the **client_credentials** flow when I want to allow B2B access to apis.  Being also a compliant OAuth2 service, thanks to **IdentityServer4**, you get all the discovery and token endpoints you would expect. 

There are also extension grants available that are basically a la carte apis to the OAuth2 token endpoint to mint arbitrary tokens.   
### Extension Grants  
[arbitrary_no_subject](./arbitrary_no_subject.md)  
[arbitrary_identity](./arbitrary_identity.md)  
[arbitrary_resource_owner](./arbitrary_resource_owner.md)  


# Walkthrough  
1. Get an identity.
[AppIdentityCreate](https://localhost:5001/api/v1/GraphQL?query=query%20q%28%24input%3A%20appIdentityCreate%21%29%20%7B%20appIdentityCreate%28input%3A%20%24input%29%7B%20authority%20expires_in%20id_token%7D%7D&variables=%7B%22input%22%3A%20%7B%20%22appId%22%3A%22app.0900%22%2C%22machineId%22%3A%2210101010%22%20%7D%7D)

## Produces
```
{
	"data": {
		"appIdentityCreate": {
			"authority": "https://localhost:5001",
			"expires_in": 1878378033,
			"id_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTgzNzgwMzMsImV4cCI6MTg3ODM3ODAzMywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6ImFwcC1pZGVudGl0eS1jbGllbnQiLCJpYXQiOjE1NTgzNzgwMzMsImF0X2hhc2giOiI5bDliaHY4R1FqcmVfTFpGS2Z3dVZnIiwic3ViIjoiNTcyYmMzN2ItNjRmMS00N2JjLTgzZGUtZWVlNmNjOThmYTkwIiwiYXV0aF90aW1lIjoxNTU4Mzc4MDMzLCJpZHAiOiJEZW1vIiwiY2xpZW50X25hbWVzcGFjZSI6ImFwcC1pZGVudGl0eS1vcmciLCJhcHBJZCI6ImFwcC4wOTAwIiwibWFjaGluZUlkIjoiMTAxMDEwMTAiLCJhbXIiOlsiYXJiaXRyYXJ5X2lkZW50aXR5Il19.Nvct3eZBw_Frqy5fOyyeprPUCIy_R2qxjdAOIrI0TNXA6SbZKgW2INvgfdqKydTHnrOtS2Rp-4TBA4jADxyEL7t1tkFo5Y0YcuAw9c1Jv4H1zENqFlCAqdDrYFgTy6bqGUxjiTC30vJ-zi7fXSjJBLxJm6RoFpwNpLQShgMU3abcu_U99w0lUcW3y8O3wRs-B4uWWMMT0LIvOw65gddeE55CGFgJjrnVdUKm_Skpl2r-_vR-nguVmY9Wg00aeKH47f5xmey9r3gf1UMBQVaom7BWNNzPPMEZ8MugYx15czxmwsfTgsK7FbGsUm97OC7RgAQpTSJrArVlDlJBXl2tOQ"
		}
	}
}
```

2. Using GraphQL run it though the **pipeline_briar_rabbit_passThrough** exhange.
## Query
```
query q($input: tokenExchange!) {
	tokenExchange(input: $input){
    customToken{
      hint
      authority  
      token
      type
      httpHeaders
      {
	name
        value
      }
    }
    accessToken{
      hint
      authority
      access_token
      expires_in
      token_type
      httpHeaders
      {
        name
        value
      }
    }
    identityToken{
      authority
      hint
      expires_in
      id_token
    }
  }
}
```
## Variables  
```
{
	"input": {
		"exchange": "pipeline_briar_rabbit_passThrough",
		"extras": ["a", "b", "c"],
		"tokens": [{
			"token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTgxMzAwOTEsImV4cCI6MTg3ODEzMDA5MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6ImFwcC1pZGVudGl0eS1jbGllbnQiLCJpYXQiOjE1NTgxMzAwOTEsImF0X2hhc2giOiJjOTZjazdkSVBNbHQxSk1teUpDbExnIiwic3ViIjoiMmRlMjkzM2QtM2Y0Ny00N2M5LThmMWYtNmM1MDAwYTE0YWVlIiwiYXV0aF90aW1lIjoxNTU4MTMwMDkwLCJpZHAiOiJEZW1vIiwiY2xpZW50X25hbWVzcGFjZSI6ImFwcC1pZGVudGl0eS1vcmciLCJhcHBJZCI6ImFwcC4wOTAwIiwibWFjaGluZUlkIjoiMTAxMDEwMTAiLCJhbXIiOlsiYXJiaXRyYXJ5X2lkZW50aXR5Il19.dNHss9zA8JYUx57BtnXtN2639skMJepCCCk5WTIF5LAsE6No1J0IQGrIm9FUsEqbWoGhsIsZFlWQqGykoLMuK2X2537NQc3b9LkB5mgQXaw8-dcWq-BujOQCOWJEr63l5Hb5O3CE1XErm_klYM-WI0lMZ4YRMnCsxFAu38Vl2nc1M0KkTTmmXp4t0gS_7Vh_EfPi82w-WMk72w5cyZ6r8K07YWoLQn63CZrQjC7Ri-o-XCzEEHGJSqBNn7gWXXr7bUYdFxNa7JtDO4M_qbT5rnLwan6c90kiXUKV6EfMqey_PeaFVsuMkucgIEPHJNmXik7MCpG2BQBCS-_fl6ySRg",
			"tokenScheme": "self"
		}]
	}
}
```

## Result
```
{
  "data": {
    "tokenExchange": [
      {
        "customToken": {
          "hint": "briar_rabbit/token-exchange-validator/custom",
          "authority": "88d65a29-194f-4593-94be-cd3ac47487bf",
          "token": "0f1dbe73-6fac-4d44-b80b-4b14d5b9c885",
          "type": "f70c2ff5-d48e-41ee-9de8-3b0bd467efe9",
          "httpHeaders": [
            {
              "name": "548d9383-efd7-47d1-9d33-e473480fea35",
              "value": "182f5eb0-723d-4414-9ec8-96bf28b7a5be"
            }
          ]
        },
        "accessToken": null,
        "identityToken": null
      },
      {
        "customToken": null,
        "accessToken": null,
        "identityToken": {
          "authority": "https://localhost:5001",
          "hint": "briarRabbitHint_Identity",
          "expires_in": 3600,
          "id_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTgzNzgzMzAsImV4cCI6MTU1ODM4MTkzMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6ImFyYml0cmFyeS1yZXNvdXJjZS1vd25lci1jbGllbnQiLCJpYXQiOjE1NTgzNzgzMzAsImF0X2hhc2giOiI4OUk2ODBNa0Y2bjdUY3hzQl9oNVhBIiwic3ViIjoiTXJSYWJiaXQiLCJhdXRoX3RpbWUiOjE1NTgzNzgzMjksImlkcCI6IkRlbW8iLCJjbGllbnRfbmFtZXNwYWNlIjoiRGFmZnkgRHVjayIsInJvbGUiOlsiYmlnRmx1ZmZ5IiwiZmx1ZmZ5QWRtaW4iXSwiYW1yIjpbImFyYml0cmFyeV9pZGVudGl0eSJdfQ.UI3kJdXb1wVeXnKgZziILNFvTTaQY7yTyrVPB027T6x-xefeqKh4d2wubWAe-pNV-8o85jbSKgmcA7m4MjwLlmfkIi95fRe6tl8gMrMZiheYeCv5hSPi9DtVvGgV-Kx7ksWc4a4MLIdSiDuY_L4mvg0owuqFc-pRnbcvER7Ps-IKodrPy56gcZuo2jBSUWe5ESB6matRZOFDblxKpn05hMSFjxBsM4DZCgEzANEe_gXaQhF3nglcxysxaqGrGUPw5GJIszzPMY4lMvH72CZqTPJmAeInF3QLb4VrixFAzekXrCAMZrNVk-TSEgPg1R_lOSTDVJVsF_R5_YiUT94tTg"
        }
      },
      {
        "customToken": null,
        "accessToken": {
          "hint": "briarRabbitHint_Access",
          "authority": "https://localhost:5001",
          "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTgzNzgzMzAsImV4cCI6MTU1ODM4MTkzMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo1MDAxL3Jlc291cmNlcyIsImJyaWFyIiwiZ3JhcGhRTFBsYXkiXSwiY2xpZW50X2lkIjoiYXJiaXRyYXJ5LXJlc291cmNlLW93bmVyLWNsaWVudCIsInN1YiI6Ik1yUmFiYml0IiwiYXV0aF90aW1lIjoxNTU4Mzc4MzMwLCJpZHAiOiJsb2NhbCIsImNsaWVudF9uYW1lc3BhY2UiOiJEYWZmeSBEdWNrIiwicm9sZSI6WyJiaWdGbHVmZnkiLCJmbHVmZnlBZG1pbiJdLCJzY29wZSI6WyJicmlhciIsImdyYXBoUUxQbGF5Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImFyYml0cmFyeV9yZXNvdXJjZV9vd25lciJdfQ.LjcjybTgHMsZR0xegqlerL7vmhCKevrcGN1gqdRSF-6uQsJinVevRrcMlv4bWMJLcNS7i5kwO6bg0TqA3yF2PN9ZvcKPdRo6veQBwDguRklrMBzx9nVRrla-zvzMtcJ-4qGblG1vzX_IXRrebtSqih_cajKeYkO8-7CNrLZuwg5fUBWs9NCaYAIbNiY3s9bvkau_0EXt9TDu-RlVtyw7PWl_L67n6xND-xdmXnzb_Igczs-CL3rPwCAcUHRffwrwWCFIlx0XaOe3PqAcJPPcB01M4YuROSXAsTjLctkZf11VMfZGjY2qnVrugdd4Mv4breKo6Lspo3OOv0zpyWkZBw",
          "expires_in": 3600,
          "token_type": "Bearer",
          "httpHeaders": [
            {
              "name": "x-bunnyAuthScheme",
              "value": "BunnyAuthority"
            }
          ]
        },
        "identityToken": null
      }
    ]
  }
}
```


