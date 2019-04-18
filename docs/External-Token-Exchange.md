# External Token Exchange  

A custom implementation of an [ITokenExchangeHandler](../src/TokenExchange.Contracts/ITokenExchangeHandler.cs) which lets you [configure](../src/GraphQLPlayTokenExchangeOnlyApp/appsettings.Development.TokenExchange.json) in an external REST based handler.  
A simple [implementation](../src/BriarRabbitTokenExchange) that supports the 2 required endpoints can be seen [here](../src/BriarRabbitTokenExchange).  

