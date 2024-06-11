#!/bin/bash

API_URL="https://localhost:7291"
CLIENT_URL="https://localhost:7072"

get_token() {
    echo "Logging in to get JWT token..."
    response=$(curl -s -X POST "$API_URL/api/user/login" \
        -H "Content-Type: application/json" \
        -d '{"username":"admin","password":"admin"}')
    TOKEN=$(echo $response | grep -oP '(?<="token":")[^"]*')
    echo -e "\nToken: $TOKEN"
}

test_employees() {
    echo -e "\n\nTesting Employees endpoints..."

    # Create a new employee
    echo -e "\n\nCreating a new employee..."
    curl -s -X POST "$API_URL/api/employee" \
        -H "Authorization: Bearer $TOKEN" \
        -H "Content-Type: application/json" \
        -d '{"FirstName":"John","LastName":"Doe","DepartmentId":1,"Street":"123 Main St","City":"Anytown","State":"CA"}'

    # Get all employees
    echo -e "\n\nGetting all employees..."
    curl -s -X GET "$API_URL/api/employee/all" \
        -H "Authorization: Bearer $TOKEN"

    # Get existing employee by ID
    echo -e "\n\nGetting employee with ID 1..."
    curl -s -X GET "$API_URL/api/employee/1" \
        -H "Authorization: Bearer $TOKEN"

    # Get non-existing employee by ID
    echo -e "\n\nGetting employee with ID 99..."
    curl -s -X GET "$API_URL/api/employee/99" \
        -H "Authorization: Bearer $TOKEN"

    # Update employee
    echo -e "\n\nUpdating employee with ID 1..."
    curl -s -X PUT "$API_URL/api/employee/update" \
        -H "Authorization: Bearer $TOKEN" \
        -H "Content-Type: application/json" \
        -d '{"Id":1,"FirstName":"Jane","LastName":"Doe","DepartmentId":1,"Street":"123 Main St","City":"Anytown","State":"CA"}'

    # Delete employee
    echo -e "\n\nDeleting employee with ID 1..."
    curl -s -X DELETE "$API_URL/api/employee/1" \
        -H "Authorization: Bearer $TOKEN"

     echo -e "\n\n----------------------Employees tests finished"

}

# Function to test departments endpoints
test_departments() {
    echo -e "\n\nTesting Departments endpoints..."

    # Create a new department
    echo -e "\n\nCreating a new department..."
    curl -s -X POST "$API_URL/api/department" \
        -H "Authorization: Bearer $TOKEN" \
        -H "Content-Type: application/json" \
        -d '"Research and Development"'

    # Get all departments
    echo -e "\n\nGetting all departments..."
    curl -s -X GET "$API_URL/api/department/all" \
        -H "Authorization: Bearer $TOKEN"

    # Get department by ID
    echo -e "\n\nGetting department with ID 1..."
    curl -s -X GET "$API_URL/api/department/1" \
        -H "Authorization: Bearer $TOKEN"

    # Update department
    echo -e "\n\nUpdating department with ID 1..."
    curl -s -X PUT "$API_URL/api/department/1" \
        -H "Authorization: Bearer $TOKEN" \
        -H "Content-Type: application/json" \
        -d '"Human Resources Updated"'

    # Delete department with employees (should fail)
    echo -e "\n\nDeleting department with ID 1...(should fail)"
    curl -s -X DELETE "$API_URL/api/department/1" \
        -H "Authorization: Bearer $TOKEN"

    # Delete department without employees (should succeed)
    echo -e "\n\nDeleting department with ID 3...(should succeed)"
    curl -s -X DELETE "$API_URL/api/department/3" \
        -H "Authorization: Bearer $TOKEN"
    echo -e "\n\n----------------------Departments tests finished"
}

# Run the tests
get_token
test_employees
test_departments

echo -e "\nAll tests completed."