# Upserting a customer
You must create an access_token that has "role":"admin"  
## Headers
```
Authorization: Bearer {{access_token}}
x-authScheme: One
```
```
mutation q($input: customerMutationInput!) {
   customer(input: $input){
    iD
    loyaltyPointBalance
  }
}
```
```
{
    "input": {
      "iD":"BugsBunny",
        "loyaltyPointBalance":10
    }
}
```

# Check you loyalty points
You must have an access_token where the subject is a known customer.  i.e. PorkyPig  
## Headers
```
Authorization: Bearer {{access_token}}
x-authScheme: One
```
```
query{
  customerLoyalty{
     iD
    loyaltyPointBalance
  }
}
```
```
{
  "data": {
    "customerLoyalty": {
      "iD": "PorkyPig",
      "loyaltyPointBalance": 20
    }
  }
}
```
