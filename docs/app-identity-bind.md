# App Identity Create

## Query 
```
query q($input: appIdentityCreate!) {
  appIdentityCreate(input: $input){
    authority
    expires_in
    id_token
  }
}


```
## variables  
```
{
  "input": { 
    "appId":"My Cool App 001",
     "machineId":"some guid"
  }
}
```
## Results  
```
{
  "data": {
    "appIdentityCreate": {
      "authority": "https://graphqlplay22.azurewebsites.net",
      "expires_in": 1875449088,
      "id_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6Imh0dHBzOi8vcDdrZXl2YWx1dC52YXVsdC5henVyZS5uZXQva2V5cy9QN0lkZW50aXR5U2VydmVyNFNlbGZTaWduZWQvZjdkODdhNDY3MDhjNGYzZDhkZmU2MTFlOTczNzQ1YzMiLCJ0eXAiOiJKV1QifQ.eyJuYmYiOjE1NTU0NDkwODgsImV4cCI6MTg3NTQ0OTA4OCwiaXNzIjoiaHR0cHM6Ly9ncmFwaHFscGxheTIyLmF6dXJld2Vic2l0ZXMubmV0IiwiYXVkIjoiYXBwLWlkZW50aXR5LWNsaWVudCIsImlhdCI6MTU1NTQ0OTA4OCwiYXRfaGFzaCI6InBfNnMzNFhTcWxJSnRVZDBGd2JEb3ciLCJzdWIiOiIyMzA3Yzg5ZC00YTNjLTQ5ZmEtYjExOC1kYWI0ZDg5YmQzYWIiLCJhdXRoX3RpbWUiOjE1NTU0NDkwODgsImlkcCI6ImxvY2FsIiwiY2xpZW50X25hbWVzcGFjZSI6ImFwcC1pZGVudGl0eS1vcmciLCJhcHBJZCI6Ik15IENvb2wgQXBwIDAwMSIsIm1hY2hpbmVJZCI6InNvbWUgZ3VpZCIsImFtciI6WyJhcmJpdHJhcnlfaWRlbnRpdHkiXX0.voVJZeJUCl9dG473Pep-wBMLRjNs57GAVxZgs76N-qzzQe1TDEb50qkeSdme53LhSH8lPEBOEc7dEc4keouu1bVIUR17oTkHjAxVZdCdOGeqcA-924T6wbu7ktkRDAmiUQuBs_Tfbkos12WppBvtwduzyPFqh7ku7x3t-CpJ9s3fN4u2dS01FJdMFTg20bNsTHVxaD881nrxdQd_YrzBKUAtI5Ir0Zp_CU3ts3pmlQiJx34touuoHkJFEnkXeWoPnKFGyTAxwfl4jjbTaSTvrEPAg6TGLYJvTc1-lctWqmUop2-FaQBJyHO2c6jKBqUjjv5ZmGA0LgGj4kHgrsOJxA"
    }
  }
}
```

# Refreshing your id_token
Not to be confused with refresh_token that are used to refresh an access_token  

## Query 
```
query q($input: appIdentityRefresh!) {
  appIdentityRefresh(input: $input){
    authority
    expires_in
    id_token
  }
}

```
## variables  
```
{
  "input": { 
    "id_token":"eyJhbGciOiJSUzI1NiIsImtpZCI6IjdmYzJkYTUxZTA0YzljYzMyMDNlMjI5ZDdkNTBmZTdmIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQ0MTUwMDYsImV4cCI6MTg3NDQxNTAwNiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOiJhcHAtaWRlbnRpdHktY2xpZW50IiwiaWF0IjoxNTU0NDE1MDA2LCJhdF9oYXNoIjoiUHo5eTUybERpMTZjU0dtMUdDeDZXdyIsInN1YiI6IjNkYjdmZjcyLWZiYmItNGJjOC1iMmYxLWQ1MzViMjBkZWQyZSIsImF1dGhfdGltZSI6MTU1NDQxNTAwNiwiaWRwIjoibG9jYWwiLCJjbGllbnRfbmFtZXNwYWNlIjoiYXBwLWlkZW50aXR5LW9yZyIsImFwcElkIjoiTXkgQ29vbCBBcHAgMDAxIiwibWFjaGluZUlkIjoic29tZSBndWlkIiwiYW1yIjpbImFyYml0cmFyeV9pZGVudGl0eSJdfQ.XqKhy4owDrf_28M3ORxdJoa6cGlm6arVj8Lo8Vp71BYYzJGSXsLjT3BQWTHZkp24EZfPaMVJlYblPbru19K1jvIYEArKNHOwUkqd0FooDp9uvuUBTjjClwReDPb7HXsu5BWHtXafAqYlIDb9VyGeYyKTubrs_s9pYP-4IfBznSrnmjWk-8F6QAwKuhy6T-ZF-zOjfSg6nxbCJpiaIuukWH9w74hHHZNv4xpw-vr-ixLoSXWUoVp4zrc4TIKPFzotCk5J9TQYIB6rBtyzmx45JUcxy20C31bbfR9TYsHZryC9U93XiB73DVYx8SqzR9P6H692jIrIBTa81u_YlVj9SQ"
  }
}
```
## Results  
```
{
  "data": {
    "appIdentityRefresh": {
      "authority": "https://localhost:44371",
      "expires_in": 1874415006,
      "id_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjdmYzJkYTUxZTA0YzljYzMyMDNlMjI5ZDdkNTBmZTdmIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NTQ0MTUxMjYsImV4cCI6MTg3NDQxNTEyNiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEiLCJhdWQiOiJhcHAtaWRlbnRpdHktY2xpZW50IiwiaWF0IjoxNTU0NDE1MTI2LCJhdF9oYXNoIjoidjE5V2FncGlyS1JmYUJyWGNBSXdtZyIsInN1YiI6IjNkYjdmZjcyLWZiYmItNGJjOC1iMmYxLWQ1MzViMjBkZWQyZSIsImF1dGhfdGltZSI6MTU1NDQxNTEyNiwiaWRwIjoibG9jYWwiLCJjbGllbnRfbmFtZXNwYWNlIjoiYXBwLWlkZW50aXR5LW9yZyIsImFwcElkIjoiTXkgQ29vbCBBcHAgMDAxIiwibWFjaGluZUlkIjoic29tZSBndWlkIiwiYW1yIjpbImFyYml0cmFyeV9pZGVudGl0eSJdfQ.a6zXRI9Wv3nR-yvc2EZW3_fs1qfV-WI1B6UR5I-0RbVLkmEzh5XgFQTB9JB8JLyRZuSh8oBll1RxSwkRx0bnZ3FIO4zDhuEyhPuKVA2TUSWMJZjIZAsvhCAKH17GH4NEYaM87AKxPD_aUc9_fs6Z1IXOqqo3uDaPN8vU-S9jNzSuC2xIuhzsVn-TfOowbzSbQBn5K2bJvX4ZQIFZBEs5beuMRSWfExT0N_j9CcrnjEHE3gYQoMcqMohtOlXoitVMO5ZH5mxe1LmBnx4oiUaHLTd79fciVLq70WFX1kfouzJf9SHvhL6OxxguCNJI1mjV7IH5fIipJ0A8y-vgW415Qw"
    }
  }
}
```
