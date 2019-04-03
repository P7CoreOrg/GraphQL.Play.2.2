# Extensibility Points

# APIs
External facing apis are based upon the work of the the [GraphQL.NET Project](https://github.com/graphql-dotnet/graphql-dotnet)  

An example conversion can be found [here](orders-conversion.md).  

# OAuth Token Exchange 
[draft-ietf-oauth-token-exchange-11](https://tools.ietf.org/html/draft-ietf-oauth-token-exchange-11)  

Basically this describes a process of exchanging one token for another.  In our flows it is exchanging an id_token for an access_token to downstream services.  The downstream services are apis.  id_tokens are **NEVER** to be used as a means to access apis.  id_tokens are injested into a rules engine that determines what level of access is to be granted, and that results in a brand new access_token being minted.  That new access_token is our bearer token.  

An example of implementing a custom exchange can be found [here](custom-bind-handler.md)  

