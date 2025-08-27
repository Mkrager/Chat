# Real-Time Chat Application

## Project Description
This is a simple real-time chat application that allows users to register, log in, and communicate with each other. The project is implemented using ASP.NET Core for the backend, as well as HTML, CSS, and Blazor for the frontend, providing an intuitive interface and high performance.

## Technologies Used
- **Frontend:** 
  - **MVC** — for creating interactive web components.
  - **HTML** — for structuring the pages.
  - **CSS** — for styling the interface.

- **Backend:**
  - **ASP.NET Core 8** — the server-side part of the application for request handling and routing.
  - **C#** — the primary programming language used for developing the backend logic, implementing business rules, handling user requests, and interacting with the database through Entity Framework Core.
  - **Entity Framework Core** — for database interaction.
  - **Database** — MSSQL.

- **Other Technologies:**
  - **SignalR (ASP.NET Core)** — for real-time communication between users.

## Functionality

1. **User Registration:**
   - Users can register by providing their first name, last name, email, username, and password.

2. **Login:**
   - After registration, users can log in using their email and password.

3. **Real-Time Chat:**
   - Users can communicate with each other in real-time by sending messages through SignalR.
   - The chat supports private messaging between users.

4. **User Interface:**
   - A simple and intuitive interface for easy chat usage.
   - Ability to view message history.

## How to Run the Project Locally

### Getting Started 
1. Clone the repository to your local machine.
```bash
git clone https://github.com/Mkrager/Chat.git
```
2. Set up your development environment. Make sure you have the necessary tools and packages installed.

3. Configure the project settings and dependencies. You may need to create configuration files for sensitive information like API keys and database connection strings.

4. Install the required packages using your package manager of choice (e.g., npm, yarn, NuGet).

5. Run the application locally for development and testing.
```bash
dotnet run
```
6.Access the application in your web browser at http://localhost:port.
