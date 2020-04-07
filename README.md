# ShoppingBasket

Onion multi-layered architecture has been implemented where: 

 - ShoppingBasket.Service - contains business logic/rules
 
 - ShoppingBasket.Repository - data access layer and should only communicate to persistence storage
 
 - ShoppingBasket.Infrastructure - logging or usually utility/infrastructure classes 
 
 - ShoppingBasket.Model - domain models  