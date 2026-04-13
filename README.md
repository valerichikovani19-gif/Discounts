
Read this before reviewing.

The application is built using Clean Architecture, with the Repository Pattern implemented, and it also follows OOP and SOLID principles.

The project is divided into MVC (Web) and API parts, both of which use the shared Application and Infrastructure layers.

Instructions

When running the project, the database is automatically generated and the initial predefined seed data is inserted through DbInitializer.

To avoid database concurrency issues, the DbInitializer is placed only in the Web project.

In addition to the core requirements, several BONUS FEATURES have been added:

Security & Authorization + Rate Limiting

In addition to standard Identity and JWT authorization, the API also has Refresh Token functionality implemented.

For security purposes, the Access Token has a short expiration time, while the user session continues seamlessly with the help of the Refresh Token stored in the database.

Additionally, Rate Limiting is configured on the Login and Register endpoints to protect against brute-force attacks.

Performance & Logging

For faster data retrieval, Memory Caching is used on heavily loaded endpoints (in our case, only the Homepage).

For full application monitoring, I have also configured Serilog, which stores detailed logs (Information, Error, Fatal) in local text files on a daily basis.

Error Handling

Both in MVC and API, there is a Global Exception Middleware.

It automatically catches any unexpected exceptions, hides the system stack trace from the user, and returns clean, standardized HTTP responses
(for example: 404 NotFound when an object is not found, or 401 Unauthorized in case of expired tokens).

Export to CSV

In the Merchant panel, on the Sales History page, functionality has been added to download sales history in CSV format, which I think is useful for financial reporting.

Coupon Redeem

The merchant can enter the customer’s code and change its status to "Redeemed".

Privacy Policy

A corresponding Privacy page is also implemented on the website :)

Paging

Paging has also been added to display offers on the homepage.

If on the first run you start only the API project, the database will not be created.

Therefore, on the first run, either:

run both Web and API projects simultaneously, or
run the MVC project first

so that the database can be created and seeded with the accounts and pre-created offers listed below.

Seeded Test Credentials

Admin Account
Email: admin@discounts.ge
Password: Admin_123!

Merchant Account
Email: merchant@discounts.ge
Password: Merchant_123!

Customer Account
Email: customer@discounts.ge
Password: Customa_123!
