# SIMRU API | Backend Service
**The robust backbone for SIMRU Room Management System.**

---

## 📝 Description
This is the core API for **SIMRU (Sistem Manajemen Ruangan)**. It handles all business logic, database persistence, and room reservation scheduling for the PENS environment. The backend is engineered to be scalable and secure, ensuring no data conflicts during the room booking process.

## 🏗️ Architecture
To ensure high maintainability and separation of concerns, this API follows the **N-Tier Architecture** pattern:
* **Presentation Layer (Controllers)**: Handles HTTP requests and manages API endpoints.
* **Logic Layer (Services)**: The "brain" of the system where all validation and business rules reside.
* **Data Access Layer**: Manages direct communication with the database using efficient query patterns.

## 🛠️ Tech Stack
* **Framework**: ASP.NET Core (.NET 8/9).
* **Language**: C#.
* **Database**: SQL Server / Relational Database (via Entity Framework Core).
* **Standards**: RESTful API Design & Semantic Versioning.

## ⚙️ Installation & Setup

1. **Clone the repository**
   ```bash
   git clone [https://github.com/alghifarydaffa62/2026-simru-backend.git](https://github.com/alghifarydaffa62/2026-simru-backend.git)

2. **Restore Dependencies**
    ```bash
    dotnet restore

3. **Configure Database**
    ```JSON
    "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER;Database=SIMRU_DB;..."
    }

4. **Run the Application**
    ```bash
    dotnet run

## 🔐 Environtment Variables
Make sure to configure the following in your appsettings.Development.json or environment variables:
* **ConnectionStrings:DefaultConnection**: Your database URI.
* **AllowedHosts**: Set to * or your frontend domain for CORS policy.

## 🤝 Git Workflow
This repository follows professional standards as part of the SIMRU ecosystem:
* **Conventional Commits**: Clear and descriptive commit messages.
* **Feature Branching**: Development happens in isolated branches before merging to main.

## 📄 License
Distributed under the MIT License.

## 👤 Author
M Daffa Al Ghifary