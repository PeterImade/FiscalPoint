# 📊 Personal Finance API

A secure and extensible ASP.NET Core Web API for managing personal finances — track income, expenses, budgets, and get intelligent dashboard summaries. Built with **Clean Architecture**, powered by **ASP.NET Identity**, **JWT**, and **Serilog** for production-grade logging.

---

## 🚀 Features

- 🔐 User Authentication (JWT + ASP.NET Identity)
- 📂 Clean Architecture (Domain, Application, Infrastructure, Presentation)
- 🧾 Track Income & Expenses
- 📊 Dashboard Insights
  - Income vs Expense
  - Category Breakdown
  - Savings Rate
- 🧮 Monthly Budgets
- 🔍 Filtering and Pagination
- 📌 Logging with Serilog
- ⏳ Background Services Support
- 🔄 AutoMapper & FluentValidation

---

## 🧱 Tech Stack

- ASP.NET Core Web API 8
- Entity Framework Core
- ASP.NET Identity
- JWT Bearer Authentication
- Serilog
- AutoMapper
- FluentValidation
- SQL Server

---

## 📁 Project Structure (Clean Architecture)

├── Domain
│ └── Entities, Enums, Interfaces
├── Application
│ └── DTOs, Interfaces, Services, Validators
├── Infrastructure
│ └── Repositories, DbContext, Identity Config
├── Presentation
│ └── Controllers, Middleware, Program.cs


---

## 🔐 Authentication

- **Register:** `POST /api/auth/register`
- **Login:** `POST /api/auth/login`
- **Refresh Token:** `POST /api/auth/refresh`
- **Change Password:** `POST /api/auth/change-password`
- **Logout:** `POST /api/auth/logout`
- **Get current user info:** `GET /api/auth/me`

---

## 📊 Dashboard Endpoints

- `GET /api/dashboard/summary`  
  _Total income, expenses, and balance._

- `GET /api/dashboard/income-vs-expense`  
  _Monthly trends comparing income to expenses._

- `GET /api/dashboard/category-breakdown`  
  _Breakdown of spending per category._

- `GET /api/dashboard/savings-rate`  
  _Savings as a percentage of income._

---

## 🧾 Transaction Endpoints

- `GET /api/transactions`
- `GET /api/transactions/{id}`
- `POST /api/transactions`
- `PUT /api/transactions/{id}`
- `DELETE /api/transactions/{id}`
- ✅ Supports **filtering** and **pagination**

---

## 💰 Budget Endpoints

- `GET /api/budgets`
- `POST /api/budgets`
- `PUT /api/budgets/{id}`
- `DELETE /api/budgets/{id}`

---
## 📈 Reports Endpoints

- `GET /api/reports/monthly?month=&year=`
 _Get monthly report data._
- `GET /api/reports/annual?year=`
_Get annual report data._
- `GET /api/reports/export?format=&month=&year=` 
_Export report (PDF/Excel)._

---

## 🧪 Testing & Documentation

- Swagger UI: `https://localhost:{port}/swagger`
- Unit tested: Services and Repositories

---

## 🛠️ Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/PeterImade/FiscalPoint.git
   cd FiscalPoint
   ```
   
2. **Update appsettings.json**
 JWT Secret
 Connection string

3. **Apply Migrations**
        ```
            dotnet ef database update
        ```
4. **Run the Application**
```dotnet run```


📦 Tools Used
Identity
EF Core
Hangfire
Serilog
FluentValidation
JWT.io

🤝 Contribution
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
