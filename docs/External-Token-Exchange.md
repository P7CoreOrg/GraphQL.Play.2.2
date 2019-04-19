# External Token Exchange  

A custom implementation of an [ITokenExchangeHandler](../src/TokenExchange.Contracts/ITokenExchangeHandler.cs) which lets you [configure](../src/GraphQLPlayTokenExchangeOnlyApp/appsettings.Development.TokenExchange.json) in an external REST based handler.  
A simple [implementation](../src/BriarRabbitTokenExchange) that supports the 2 required endpoints can be seen [here](../src/BriarRabbitTokenExchange).  

## GraphQLPlayTokenExchangeOnlyApp

### About
| Url                   | Version | Contact                     | Terms of Service        | License        |
| --------------------- | ------- | --------------------------- | ----------------------- | -------------- |
| [](http:// "API url") | v1      | [](mailto: "Contact Email") | []( "Terms of Service") | []( "License") |

### Schemes
| Scheme |
| ------ |

## Endpoints
## api/token_exchange/briar_rabbit/full_ownership
## POST
### PostProcessFullOwnesrshipTokenExchangeAsync


### Expected Response Types
| Response | Reason  |
| -------- | ------- |
|          |         |
| 200      | Success |

### Parameters
| Name                 | In     | Description | Required? | Type                                                     |
| -------------------- | ------ | ----------- | --------- | -------------------------------------------------------- |
| tokenExchangeRequest | body   |             | false     | [TokenExchangeRequest](#tokenexchangerequest-definition) |
| Authorization        | header |             | true      | string                                                   |

### Content Types Produced
| Produces         |
| ---------------- |
| text/plain       |
| application/json |
| text/json        |

### Content Types Consumed
| Consumes                    |
| --------------------------- |
| application/json-patch+json |
| application/json            |
| text/json                   |
| application/*+json          |

### Security
| Id   | Scopes |
| ---- | ------ |
| None | None   |
## api/token_exchange/briar_rabbit/half_ownership
## POST
### PostProcessHalfOwnesrshipTokenExchangeAsync


### Expected Response Types
| Response | Reason  |
| -------- | ------- |
|          |         |
| 200      | Success |

### Parameters
| Name                 | In     | Description | Required? | Type                                                     |
| -------------------- | ------ | ----------- | --------- | -------------------------------------------------------- |
| tokenExchangeRequest | body   |             | false     | [TokenExchangeRequest](#tokenexchangerequest-definition) |
| Authorization        | header |             | true      | string                                                   |

### Content Types Produced
| Produces         |
| ---------------- |
| text/plain       |
| application/json |
| text/json        |

### Content Types Consumed
| Consumes                    |
| --------------------------- |
| application/json-patch+json |
| application/json            |
| text/json                   |
| application/*+json          |

### Security
| Id   | Scopes |
| ---- | ------ |
| None | None   |
## api/v1/GraphQL
## GET
### GetAsync


### Expected Response Types
| Response | Reason  |
| -------- | ------- |
|          |         |
| 200      | Success |

### Parameters
| Name          | In    | Description | Required? | Type   |
| ------------- | ----- | ----------- | --------- | ------ |
| operationName | query |             | false     | string |
| query         | query |             | false     | string |

### Content Types Produced
| Produces         |
| ---------------- |
| application/json |

### Content Types Consumed
| Consumes |
| -------- |
| None     |

### Security
| Id   | Scopes |
| ---- | ------ |
| None | None   |
## POST
### PostAsync


### Expected Response Types
| Response | Reason  |
| -------- | ------- |
|          |         |
| 200      | Success |

### Parameters
| Name | In | Description | Required? | Type |
| ---- | -- | ----------- | --------- | ---- |

### Content Types Produced
| Produces         |
| ---------------- |
| application/json |

### Content Types Consumed
| Consumes |
| -------- |
| None     |

### Security
| Id   | Scopes |
| ---- | ------ |
| None | None   |
## Security Definitions
| Id | Type | Flow | Authorization Url | Name | In | Scopes |
| -- | ---- | ---- | ----------------- | ---- | -- | ------ |

| Scope | Description |
| ----- | ----------- |
## Definitions
### HttpHeader Definition
| Property | Type   | Format |
| -------- | ------ | ------ |
| name     | string |        |
| value    | string |        |
### ResourceOwnerTokenRequest Definition
| Property            | Type    | Format |
| ------------------- | ------- | ------ |
|                     |         |        |
| scope               | string  |        |
| arbitraryClaims     | object  |        |
| subject             | string  |        |
| accessTokenLifetime | integer | int32  |
| clientId            | string  |        |
### TokenExchangeRequest Definition
| Property | Type  | Format |
| -------- | ----- | ------ |
| tokens   | array |        |
| extras   | array |        |
### TokenExchangeResponse Definition
| Property      | Type    | Format |
| ------------- | ------- | ------ |
| access_token  | string  |        |
| expires_in    | integer | int32  |
| token_type    | string  |        |
| refresh_token | string  |        |
| authority     | string  |        |
| httpHeaders   | array   |        |
### TokenWithScheme Definition
| Property    | Type   | Format |
| ----------- | ------ | ------ |
| token       | string |        |
| tokenScheme | string |        |
## Additional Resources
[]( "External Documentation")
