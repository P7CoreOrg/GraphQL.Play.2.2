# B2B Apis Explained

The GraphQL.Play Kit is meant to minimize the work needed to introduce a new client that is authorized to call apis and the implementation of the final api.

From the perspective of the kit the following has to be introduced;
1. Introduce in a new client
2. Configure in known apis, i.e. 3PApi
3. Configure Authorization rules
4. Write the API
5. Register the API via asp.net core's DI

## Introduce a new client  
A new client is configured in via the OAuth2 component and is currently added in [appsettings.Development.Clients.json](../src/IdentityServer4-Extension-Grants-App/appsettings.Development.Clients.json)  
```
"b2b-client": {
      "namespace": "b2b-org",
      "enabled": true,
      "secrets": [
        "secret"
      ],
      "allowedScopes": [
        "3PApi"
      ],
      "AllowedGrantTypes": [
        "client_credentials"
      ],
      "AccessTokenLifetime": 3600,
      "AuthorizationCodeLifetime": 300,
      "AbsoluteRefreshTokenLifetime": 3600,
      "IdentityTokenLifetime": 2600000,
      "SlidingRefreshTokenLifetime": 900,
      "RefreshTokenUsage": 1,
      "AccessTokenType": 0,
      "AllowOfflineAccess": false,
      "RequireClientSecret": true,
      "RequireRefreshClientSecret": true,
      "ClientClaimsPrefix": null
    }
```

Here you tell what scopes the client is allowed, via the "allowedScopes" entry.  Also you might have many clients that you want to orgainize into a group.  This is done using the "namespace" entry.
The rest are well known settings that are defined by the IdentityServer4 project.

Also we need to configure api resources as well, and these map directly to the "allowedScopes" entry.
The api configuration can be found [here](../src/IdentityServer4-Extension-Grants-App/appsettings.Development.ApiResources.json)  
```
{
  "apiResources": [
    {
      "name": "3PApi"
    }
  ]
}
```

We are now able to use the OAuth2 client_credentials flow;  

```
curl -X POST \
  https://localhost:44371/connect/token \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -H 'Postman-Token: 0216fcce-7237-4b76-8c35-ef6c644de16c' \
  -H 'cache-control: no-cache' \
  -d 'grant_type=client_credentials&client_id=b2b-client&client_secret=secret&undefined='
```
```
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQ1NjQ0MDMsImV4cCI6MTU1NDU2ODAwMywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiM1BBcGkiXSwiY2xpZW50X2lkIjoiYjJiLWNsaWVudCIsImNsaWVudF9uYW1lc3BhY2UiOiJiMmItb3JnIiwic2NvcGUiOlsiM1BBcGkiXX0.jmLDXkv3ZVBDlKMshWHGNanTraNTNuHeFPwAZk9-l0zDI0NE2WM7fLXy9NENU7xgr0vwKL1XuCTEYWq9M2xLgm3A0QAMJjdavU_xDJS0lOuwY-qGljl7Vlu2XQnBu_M46_FE8UBqr7pgWhve4pbiGqAgWytg7OA2pkeiG2xTh-afZjz6n72KXoQu2cvfuoiu-gPSCho9HMBKS1ARZ9N3O75-s-2LkUTnr1bn2eCWQkd_uEE6U14Wjai4DLaNLtcvbr2WDoU2-KmimqbIOrCopnF6LjwDtpGS_GEeULCW_t62LoXdYkgZy3YvY-kfawel3_iqx-PvlHIN_uvoPyXeLw",
    "expires_in": 3600,
    "token_type": "Bearer"
}
```  

## Configurable Authorization of GraphQL mutation and query  
Mutations are currently hard-coded to require authorization, but you can do fine-grained configuration of what is required in the bearer token to access a specific query or mutation.  
The graphQL configuration can be found [here](../src/IdentityServer4-Extension-Grants-App/appsettings.graphql.json) 
```
"graphQLFieldAuthority": {
    "records": [
      {
        "operationType": "mutation",
        "fieldPath": "/publishState",
        "claims": [
          {
            "Type": "scope",
            "Value": "3PApi"
          }
        ]
      },
      {
        "operationType": "query",
        "fieldPath": "/publishState",
        "claims": [
          {
            "Type": "scope",
            "Value": "3PApi"
          }
        ]
      }
    ]
  }
```
The above states that the bear access_token must have a scope "3PApi" to gain access to the "/publishState" field, and buy nature everything below it.  

## The PublishState APIS

The B2BPublisher api project is [here](../src/B2BPublisher)  
The api contains mutation and query, and there are other example of this in the Orders project brought over from GraphQL.NET.  

## Registering the APIS
As usual, this is  done in our apps [Startup.cs](../src/IdentityServer4-Extension-Grants-App/Startup.cs)  
```
services.AddB2BPublisherTypes();
services.AddInMemoryB2BPlublisherStore();
```

Typically you register the GraphQL stuff, and then all our downstream services.  In this case the only downstream service I have is an InMemoryB2BPlublisherStore.  In production, I would expect to register a store that uses a database.  


# And Thats it.. Lets make the calls.

## Get your client_credentials access_token
```
curl -X POST \
  https://localhost:44371/connect/token \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -H 'Postman-Token: 14f9c200-5218-4a9d-80f4-86017904c476' \
  -H 'cache-control: no-cache' \
  -d 'grant_type=client_credentials&client_id=b2b-client&client_secret=secret&undefined='
```
```
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjI1YmM0M2NjYzdiODFkYjgxZjU3NWYwY2M2OWU3YWQ4IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQ1Njc2NjksImV4cCI6MTU1NDU3MTI2OSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEvcmVzb3VyY2VzIiwiM1BBcGkiXSwiY2xpZW50X2lkIjoiYjJiLWNsaWVudCIsImNsaWVudF9uYW1lc3BhY2UiOiJiMmItb3JnIiwic2NvcGUiOlsiM1BBcGkiXX0.aet6J1b-WWzsIGvhgjtURbko2biBryidzRkv6-1X0leNSJ8bKTzAPEqJFamUTy5kSEqPnzrZq7mTisrguLf_8OREm-XnqCLdFE8pFDEHrIxyu-qOpqEZY3Ir2x9p8hnyMBA3t7qPRJflzIiSUe-MM_NdIr53kAVT6O_QUvKFJbSWJVXw81LSZug6OHpJ7lgUtlpUoMktzrKCs6Rx831Jndzdv5YMPQOMagxrx_gsYGrY9IPbdX-K63B9x-n_I9e2elkdSxawwtujU1yFe3ENrqJLQuOC0lHf1vnMw0QZ4mDgM-TSJWX2vBKWkdpOVJCduODIrQbOpHVGWmNqaMDSTg",
    "expires_in": 3600,
    "token_type": "Bearer"
}
```

## In Altair add the following via "Set Headers"
Authorization : Bearer {access_token}
x-authScheme : self

## Mutation
```
mutation q($input:publishStateInput!){
  
   publishState(input: $input){
    status
    client_id
    client_namespace
  }
}
```
```
{
    "input": {
      "key":"key123",
       "category":"cat 001",
       "version":"version 001",
         "state":"{'a':{'b':['c','d']}}"
    }
}
```
### result  
```
{
  "data": {
    "publishState": {
      "status": "ACCEPTED",
      "client_id": "b2b-client",
      "client_namespace": "b2b-org"
    }
  }
}
```

## Query
```
query{
  publishState{
    category
    key
    state
    version
  }
}
```
### result  
```
{
  "data": {
    "publishState": {
      "category": "cat 001",
      "key": "key123",
      "state": "{'a':{'b':['c','d']}}",
      "version": "version 001"
    }
  }
}
```

