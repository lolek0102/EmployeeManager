
# EmployeeManager

## Seeded Credentials

Predefined users:

- **Admin User**
  - Username: `admin`
  - Password: `admin`
  - Role: `Admin`
  
- **Regular User**
  - Username: `user`
  - Password: `user`
  - Role: `User`

## Projects

Solution contains two main projects:

1. **EmployeeManager.Application**
   - This project contains the backend Web API and gRPC services.
   - It handles data management, authentication, and authorization.

2. **EmployeeManager.Client**
   - This project contains the frontend application using Razor Pages.
   - It provides the user interface for interacting with the system.

## Running the Application

To run the application, you need to start both the client and server projects. You need to navigate to Solution properties and select multiple startup projects (Application & Client).

## API Endpoints

### Authentication

- **Login**
  - `POST /api/user/login`
  - Request Body: `{ "username": "admin", "password": "admin" }`

### Employees

- **Get All Employees**
  - `GET /api/employee/all`
  
- **Get Employee by ID**
  - `GET /api/employee/{id}`

- **Create Employee**
  - `POST /api/employee`
  - Request Body: 
    ```json
    {
        "FirstName": "John",
        "LastName": "Smith",
        "DepartmentId": 1,
        "Street": "123 Main St",
        "City": "Anytown",
        "State": "Anystate"
    }
    ```

- **Update Employee**
  - `PUT /api/employee/update`
  - Request Body:
    ```json
    {
        "Id": 1,
        "FirstName": "Jane",
        "LastName": "Doe",
        "DepartmentId": 1,
        "Street": "123 Other st",
        "City": "Sometown",
        "State": "Somestate"
    }
    ```

- **Delete Employee**
  - `DELETE /api/employee/{id}`

### Departments

- **Get All Departments**
  - `GET /api/department/all`

- **Get Department by ID**
  - `GET /api/department/{id}`

- **Create Department**
  - `POST /api/department`
  - Request Body: `"Department Name"`

- **Update Department**
  - `PUT /api/department/{id}`
  - Request Body: `"Updated Department Name"`

- **Delete Department**
  - `DELETE /api/department/{id}`
  - Note: Departments containing employees cannot be deleted.

## Running the Test Script

A shell script `api_test.sh` is provided to test all endpoints of the API using cURL.

The script will:
- Log in as the admin user and obtain a JWT token.
- Perform a series of `GET`, `POST`, `PUT`, and `DELETE` requests on the employees and departments endpoints.
- Display the results in the terminal.
