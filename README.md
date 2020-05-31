# Setting Up

## Setting up the database locally
1. Go to `../PaymentGateway/PaymentGateway/Payment.Gateway.Database/Scripts`
2. Open the script `Create database.sql`
3. Set the path for the variable '{path}' for the data and log file of the database and execute the script
4. Execute the script `Create database login.sql`

## Setting up the solutions on IIS
Under the **Default Web Site** on IIS, configure the following application:
-   `paymentgateway` which points to `../PaymentGateway/PaymentGateway/Payment.Gateway.Web.Api`
	- This solution contains the APIs for:
		- Processing and validating payment requests received from Merchants
		- Allowing a merchant to retrieve details of a previously made payment
-   `acquiringbank` which points to `../PaymentGateway/PaymentGateway/Payment.Gateway.Acquiring.Bank.Api`
	- This solution stimulates the bank and is used to mock the responses from the bank.
	- The bank payment API is configured on the web.config file of the Payment.Gateway.Web.Api solution and can be swapped to URL of a real bank.
	
## Using the APIs


This APIs uses `POST` request to communicate and HTTP [response codes](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) to indenticate status and errors.

#### Response Codes 
```
200: Success
400: Bad request
401: Unauthorized
404: Cannot be found
405: Method not allowed
422: Unprocessable Entity 
50X: Server Error
```
#### Login
Each merchant is attributed an API key which he uses when making the request. A request made with an unknown API key would result in 401 response code

### Process Payment API

#### Assumption
- A card number is either 14 or 19 length.
- A card CVV number is 4 length.
- Expiry Date of a card number should be in the following format: "yyyy-MM"
- Currency being used for the payment should be known by the Payment Gateway

**Request:**
```json
POST /paymentgateway/api/payment/process HTTP/1.1
Host: localhost
X-ApiKey: F1076038-AC22-4287-8976-4D5079775F0D
Content-Type: application/x-www-form-urlencoded

CardNumber=1234567891234567&CardCvv=1234&ExpiryDate=2025-12&PaymentAmount=1995.69&Currency=USD
```
**Successful Response:**
```json
{
    "Status": "Successful",
    "PaymentUniqueId": "21954b39-5dea-47e8-b0b2-3a85a8e12a8b"
}
```

### Retrieve Payment Details API

**Request:**
```json
POST /paymentgateway/api/payment/detail HTTP/1.1
Host: localhost
X-ApiKey: F1076038-AC22-4287-8976-4D5079775F0D
Content-Type: application/json

"21954b39-5dea-47e8-b0b2-3a85a8e12a8b"
```
**Successful Response:**

```json
{
    "PaymentIdentifier": "21954b39-5dea-47e8-b0b2-3a85a8e12a8b",
    "CardNumber": "X5ymaj8JG4q3eZ62bTrEcn+9siNH1gOX",
    "CardCVV": "1234",
    "Currency": "United States Dollar",
    "PaymentAmount": 1995.6900,
    "PaymentStatus": "Successful"
}
```
The CardNumber received from the response has been encrypted and on the Merchant side there should be the appropriate logic to decrypt the CardNumber. The key  is `F44C657572504E22B1C30107EF3EAAC6`.

## Specifics of Solution

- StructureMap was used for Dependency Injection
- NLog was used for application logging
- Authentication - Each merchant is identified by the API Key that has been allocated to him/her
- Data Storage - Database was created on Sql Server and Dapper was used for CRUD operations
- Unit Tests have been added for the core logic