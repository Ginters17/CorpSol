# CorpSol Backend API Development Task
Used .NET 8 with SQL server. 
.sqlproj contains required packages. Need to adjust 'Server' and 'Database' in sql connection string inside appsettings.json and run 'dotnet ef database update'. Launch app with dotnet run or through VS.
Request base url: https://localhost:[port]
After /login use returned JWT token in request header (For postman: Authorization tab -> Type: Bearer Token -> paste token) to have access to products api. Swagger has some API documentation.

# TODO:
Better handling in controllers with try/catch exceptions and meaningful responses for some requests like update product.
