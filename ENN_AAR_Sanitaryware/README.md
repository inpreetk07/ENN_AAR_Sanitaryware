ENN AAR Sanitaryware:

A premium sanitaryware product catalogue and store management website built using ASP.NET Core MVC.

=> About the Project

ENN AAR Sanitaryware is a web application designed to showcase sanitaryware products and help the store owner manage products, categories and customer enquiries through a secure admin dashboard.

=> Features

Customer Website
- Premium, responsive homepage
- Product catalogue with images and prices
- Product search and category filters
- Contact information and store location
- Customer enquiry submission form
- Enquiry success popup
- WhatsApp contact integration

Admin Dashboard
- Secure admin authentication
- Product management (Add, Edit, Delete)
- Product image uploads
- Product availability management
- Dynamic product statistics
- Category management (Add, Edit, Delete)
- Automatic category updates in products
- Customer enquiry management
- Enquiry status tracking (Pending, Contacted, Completed)

=> Technology Stack

- C#
- ASP.NET Core MVC
- .NET 10
- Entity Framework Core
- SQL Server LocalDB
- ASP.NET Core Identity
- HTML5
- CSS3
- Bootstrap 5
- JavaScript

=> Project Structure

- Controllers – Application controllers
- Models – Product, Category and Enquiry models
- Data – Database context and admin seeding
- Migrations – Entity Framework database migrations
- Views – Razor views
- wwwroot – CSS, JavaScript and images

=> Getting Started

1. Clone this repository.
2. Open the solution in Visual Studio 2026.
3. Restore NuGet packages.
4. Configure the database connection in appsettings.json.
5. Configure the AdminSeed:Password using ASP.NET Core User Secrets.
6. Run Entity Framework migrations using Update-Database.
7. Run the application.

=> Security

- Admin pages are protected using ASP.NET Core Identity and role-based authorization.
- Admin credentials are configured through User Secrets.
- Private credentials and customer database records should not be committed.

=> Author

Developed as a real-world web development project.
Project: ENN AAR Sanitaryware
