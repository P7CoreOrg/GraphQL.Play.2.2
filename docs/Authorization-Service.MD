# Building Out An Authorization Service

The TokenExhange service is really a pipeline where tokens come in, a series of pre-processors evaluates them and then finally a decision is made on what tokens are to be minted.  The name of the exchange depends on what is in the pipeline.

Lets look at an example of a user's id_token being sent through the exchange where the final result is an access_token to a service that the user purchased.

The Entry condition to the exchange would be the following;
1. The users id_token
2. A hint as to what exchange we want the token sent through.  This could also be interpreted as what API group, or what Service that downstream tokens are being requested for.

So I am user Daffy-Duck and I want access to the AnalyticsServce which I have paid for!

