# Original
[graphql-dotnet/example-orders](https://github.com/graphql-dotnet/example-orders)  
[baseline](https://github.com/P7CoreOrg/GraphQL.Play.2.2/tree/4c7398f24d62c72bb32519fadb10278b7ec1c8aa/src/graphql-dotnet/example-orders/Orders)  

# Final Conversion
[conversion result](https://github.com/P7CoreOrg/GraphQL.Play.2.2/tree/5d8622c91f25375ec57cac44f0975de1f99ac22f/src/graphql-dotnet/example-orders/Orders)


The original contains a [schema](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/4c7398f24d62c72bb32519fadb10278b7ec1c8aa/src/graphql-dotnet/example-orders/Orders/Schema/OrdersSchema.cs) which will be removed because P7Core.GraphQLCore owns the master schema.
Mutation, Query and Subscriptions now have to be registered with the DI using the following interfaces;  
[IMutationFieldRegistration](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/master/src/P7Core.GraphQLCore/IMutationFieldRegistration.cs)  
[IQueryFieldRegistration](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/master/src/P7Core.GraphQLCore/IQueryFieldRegistration.cs)  
[ISubscriptionFieldRegistration](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/master/src/P7Core.GraphQLCore/ISubscriptionFieldRegistration.cs)  

# Mutation Converstion
[original](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/4c7398f24d62c72bb32519fadb10278b7ec1c8aa/src/graphql-dotnet/example-orders/Orders/Schema/OrdersMutation.cs)  
You will notice that the original registers all the fields in the constructor.  
We will inherit this class from [IMutationFieldRegistration](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/master/src/P7Core.GraphQLCore/IMutationFieldRegistration.cs) and move those registrations into the required method.  In the constructor we do our normal storing away of injected services.

[converted](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/8e8492e03b3844ef189c4bc5f314c72652a18356/src/graphql-dotnet/example-orders/Orders/Schema/OrdersMutation.cs)  

# Query Converstion
[original](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/4c7398f24d62c72bb32519fadb10278b7ec1c8aa/src/graphql-dotnet/example-orders/Orders/Schema/OrdersQuery.cs)  
You will notice that the original registers all the fields in the constructor.  
We will inherit this class from [IQueryFieldRegistration](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/master/src/P7Core.GraphQLCore/IQueryFieldRegistration.cs) and move those registrations into the required method.  In the constructor we do our normal storing away of injected services.

[converted](https://github.com/P7CoreOrg/GraphQL.Play.2.2/blob/8e8492e03b3844ef189c4bc5f314c72652a18356/src/graphql-dotnet/example-orders/Orders/Schema/OrdersQuery.cs)  

 
