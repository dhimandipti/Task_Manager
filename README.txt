# Task Management System (ASP.NET Core Web API)

A full-stack **Task Management System** built using **.NET 10 Web API**, designed to manage users and tasks efficiently with secure authentication and role-based access control. The system supports Admin and Employee roles with different levels of permissions.

---

## Features

### Authentication & Authorization
- JWT-based secure authentication
- Refresh Token mechanism for seamless session handling
- Role-Based Access Control (RBAC)

### Roles

#### Admin
- Create, update, delete, and view users
- Create, assign, edit, and delete tasks
- View all tasks in the system

#### Employee
- View only assigned tasks
- Update task status (Pending / In Progress / Completed)

---

## Task Fields

- Title
- Description
- Priority (Low / Medium / High)
- Status (Pending / In Progress / Completed)
- Assigned User
- Created Date

---

## Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 10)
- **Database:** SQL Server
- **ORM:** Entity Framework Core (Code-First)
- **Authentication:** JWT (JSON Web Tokens)
- **API Documentation:** Swagger / OpenAPI
- **Testing Tool:** Postman

---

## API Features

- Complete CRUD operations for Users & Tasks
- Pagination support for task listing
- Search & filtering functionality
- Global exception handling middleware
- Swagger UI for API testing and documentation

---

## Project Setup

### 1. Prerequisites

Make sure you have installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [SQL Server Express / LocalDB](https://www.microsoft.com/en-us/sql-server/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- [Postman](https://www.postman.com/) (optional for API testing)

---

### 2. Clone the Repository

```bash
git clone https://github.com/dhimandipti/Task_Manager
cd task-management-system