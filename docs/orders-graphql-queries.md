# Orders 

# Mutation  

## Create Order
```
mutation q($input: OrderInput!){
  createOrder(order: $input){
    created
    customer{
      id
      name
    }
    description
    id
    name
    status
  }
}
```
```
{
    "input": {
      "name":"Glenn",
       "description":"Test",
        "customerId":"1",
         "created":"03/16/2018"
        
    }
}
```
## result
```
{
  "data": {
    "createOrder": {
      "created": "2018-03-16",
      "customer": {
        "id": 1,
        "name": "KinetEco"
      },
      "description": "Test",
      "id": "d5370a81-e801-4e91-aa5b-c756ccee3b85",
      "name": "Glenn",
      "status": "CREATED"
    }
  }
}
```
