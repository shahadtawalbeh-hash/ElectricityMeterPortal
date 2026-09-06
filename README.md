Electricity Meter Portal
1.Project Overview

Electricity Meter Portal is an ASP.NET Core MVC web application designed to manage electricity meter requests and related services.

The system provides two main types of users:

Citizen – can register, log in, submit electricity meter requests, upload required documents, and view their requests.
Employee – can log in and manage electricity meter requests and related information.


2.Technologies Used:

* ASP.NET Core MVC
* .NET 10
* C#
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* HTML / CSS
* Bootstrap
* JavaScript
* jQuery

3. Project Structure

The project contains:

* Controllers – Handle application requests and business logic.
* Models – Represent the application’s data.
* Views – MVC user interface pages.
* Data – Database contexts and Identity configuration.
* Migrations – Entity Framework Core database migrations.
* wwwroot – Static files such as CSS, JavaScript, and libraries.
* ECDB.sql – SQL script for creating and setting up the database.

4. Requirements

Before running the project, make sure you have:

1. Visual Studio 2022 or later
2. .NET 10 SDK
3. SQL Server
4. SQL Server Management Studio (SSMS)
5. Git (if cloning the project from GitHub)

5. Database Setup

The application uses:

* SQL Server
* Server: localhost\SQLEXPRESS
* Database: ElectricityMeterDB

Option 1 – Using ECDB.sql

1. Open SQL Server Management Studio (SSMS).
2. Connect to:
    localhost\SQLEXPRESS
3. Open the file:
    ECDB.sql
4. Execute the script.
5. Make sure the database ElectricityMeterDB is created successfully.

Option 2 – Using Entity Framework Migrations

The project also contains Entity Framework Core migrations.

If needed, open the Package Manager Console in Visual Studio and run the appropriate migration commands for the project.

6.Connection String

The application is configured to use:

localhost\SQLEXPRESS

with Windows Authentication.

The connection string is located in:

appsettings.json

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ElectricityMeterDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

If SQL Server is installed using a different server/instance name, update the connection string accordingly.

7.How to Run the Project

1. Clone the Repository

Clone the GitHub repository to your computer.

2. Open the Solution

Open:

ElectricityMeterportal.slnx

using Visual Studio.

3. Restore Dependencies

Visual Studio should automatically restore the required NuGet packages.

If needed, run:

dotnet restore

4. Check the Database

Make sure the ElectricityMeterDB database exists and the connection string is correct.

5. Run the Application

Run the project from Visual Studio using:

Ctrl + F5

or

F5

The application will open in the browser.

8. Demo Accounts

The project includes demo accounts for testing.

Employee Account

Employee ID: EMP001
Password: Employee@123

Citizen Account

National ID: 1222222222
Password: 12345Aa&

These accounts can be used to test the different user roles and application features.

9. User Roles

Citizen

A citizen can:

* Register an account
* Log in
* Submit a new electricity meter request
* Enter request information
* Upload required documents
* View submitted requests
* Track request status

Employee

An employee can:

* Log in using an employee account
* View electricity meter requests
* Manage requests
* Update request information
* Manage meter-related information

10.Required Documents

When submitting an electricity meter request, the citizen can provide the required documents, including:

* Identity Document
* Proof of Ownership
* Clearance Certificate

11. Notes

* The application uses ASP.NET Core MVC.
* SQL Server is required for the database.
* The connection string may need to be changed depending on the local SQL Server instance.
* Uploaded user documents are excluded from the Git repository for privacy and security reasons.
* The bin, obj, and .vs folders are excluded from the repository because they are generated files.
  
12.Test Documents

Sample PDF files are included in the TestDocuments folder for testing the document upload functionality.

Note: These PDF files are intentionally empty and are provided for testing purposes only.

13.Developer

Shahad Tawalbeh
Bachelor’s Degree in Computer Engineering
Yarmouk University – Al-Hijjawi Faculty of Engineering Technology
2026
