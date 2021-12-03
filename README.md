This application was made using .NET Core 2.1.

To run this, pull the solution and launch in Visual Studio. It will load up a Swagger UI with all of the endpoints there. No seeding or scripts are needed as this uses and In Memory Database.

This means each time the app is run the data is seeded at runtime and will be wiped when the app stops. I took this approach to reduce the complexity of having to use a SQL Database to run this.
