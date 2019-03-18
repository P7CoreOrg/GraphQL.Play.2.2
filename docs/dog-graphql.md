# Dog Mutations and Query
You need to be authenticated to mutate anything.   Changing this later to be configurable.
# Mutation  
## Headers
```
Authorization: Bearer {{access_token}}
x-authScheme: self
```
```
mutation q($input: DogInputType!){
  mutateDog(dog: $input){
    name
  }
}
```
```
{ 
  "input":{
    "name":"Ralph"
  }
}
```  
## Produces
```
{
  "data": {
    "mutateDog": {
      "name": "Ralph"
    }
  }
}
```

# Query  
```
query{
  dogName
  dogName2
}
```  
## Produces
```
{
  "data": {
    "dogName": "Ralph",
    "dogName2": "Heidi"
  }
}
```
