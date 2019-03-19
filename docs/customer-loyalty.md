# Upserting a customer
You must create an access_token that has "role":"admin"  
```
curl -X POST \
  https://localhost:44371/connect/token \
  -H 'Content-Type: application/x-www-form-urlencoded' \
  -H 'Postman-Token: 3d154658-60bb-4454-a2a6-efeca19ccc8a' \
  -H 'cache-control: no-cache' \
  -d 'grant_type=arbitrary_resource_owner&client_id=arbitrary-resource-owner-client&client_secret=secret&scope=offline_access%20wizard&arbitrary_claims=%7B%22top%22%3A%5B%22TopDog%22%5D%2C%22role%22%3A%20%5B%22admin%22%2C%22limited%22%5D%2C%22query%22%3A%20%5B%22dashboard%22%2C%20%22licensing%22%5D%2C%22seatId%22%3A%20%5B%228c59ec41-54f3-460b-a04e-520fc5b9973d%22%5D%2C%22piid%22%3A%20%5B%222368d213-d06c-4c2a-a099-11c34adc3579%22%5D%7D&subject=BugsBunny&access_token_lifetime=3600&arbitrary_amrs=%5B%22agent%3Ausername%3Aagent0%40supporttech.com%22%2C%22agent%3Achallenge%3AfullSSN%22%2C%22agent%3Achallenge%3AhomeZip%22%5D&custom_payload=%7B%22some_string%22%3A%20%22data%22%2C%22some_number%22%3A%201234%2C%22some_object%22%3A%20%7B%22some_string%22%3A%20%22data%22%2C%22some_number%22%3A%201234%7D%2C%22some_array%22%3A%20%5B%7B%22a%22%3A%20%22b%22%7D%2C%7B%22b%22%3A%20%22c%22%7D%5D%7D&undefined='
```
## Headers
```
Authorization: Bearer {{access_token}}
x-authScheme: self
```
```
mutation q($input:customerMutationInput!){
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
You must have an access_token where the subject is a known customer.  i.e. BugsBunny  
## Headers
```
Authorization: Bearer {{access_token}}
x-authScheme: self
```
```
query{
  customerLoyalty{
    iD
    loyaltyPointBalance
    prizes{
      iD
      loyaltyPointsCost
    }
  }
}
```
```
{
  "data": {
    "customerLoyalty": {
      "iD": "BugsBunny",
      "loyaltyPointBalance": 110,
      "prizes": []
    }
  }
}
```
