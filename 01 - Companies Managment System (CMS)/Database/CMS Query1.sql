use demoDB;

-- create
CREATE TABLE Admin (
    AdminID INT PRIMARY KEY,
    Name VARCHAR(255),
    Account VARCHAR(255),
    PhoneNumber VARCHAR(20),
	Address VARCHAR(255)
);

CREATE TABLE Company (
    CompID INT PRIMARY KEY,
    Name VARCHAR(255),
    Address VARCHAR(255)
);

CREATE TABLE Department (
    DepID INT PRIMARY KEY,
    Name VARCHAR(255),
	CompID INT,
	FOREIGN KEY (CompID) REFERENCES Company(CompID)
);

CREATE TABLE Product (
    ProdID INT PRIMARY KEY,
    Name VARCHAR(255),
	Description VARCHAR(255),
	Price INT,
	CompID INT,
	FOREIGN KEY (CompID) REFERENCES Company(CompID)
);

EXEC sp_rename 'Product.DepID', 'ProdID', 'COLUMN';

CREATE TABLE Employee (
    EmpID INT PRIMARY KEY,
    Name VARCHAR(255),
    Position VARCHAR(255),
    Account VARCHAR(255),
    PhoneNumber VARCHAR(20),
	Address VARCHAR(255),
    CompID INT,
    FOREIGN KEY (CompID) REFERENCES Company(CompID)
);

-- insert
INSERT INTO Admin(AdminID, Name, Account, PhoneNumber, Address) VALUES 
(0000, 'Khaled', 'admin@mail.com', '0000000000', 'Cairo, EG');

INSERT INTO Company(CompID, Name, Address) VALUES 
(56, 'ABC Corporation', '123 Main Street, Houston, TX'),
(57, 'XYZ Industries', '456 Oak Avenue, San Francisco, CA'),
(58, 'Tech Innovations Ltd.', '789 Elm Street, Chicago, IL'),
(59, 'Global Solutions Inc.', '101 Maple Avenue, New York, NY'),
(60, 'Innovate Enterprises', '202 Pine Street, Los Angeles, CA');

INSERT INTO Department(DepID, Name, CompID) VALUES 
(1, 'Sales', 56),
(2, 'Marketing', 56),
(3, 'Engineering', 57),
(4, 'Human Resources', 57),
(5, 'Research and Development', 58);

INSERT INTO Product(ProdID, Name, Description, Price, CompID) VALUES 
(1, 'Smartphone X', 'A powerful smartphone with advanced features.', 799.99, 56),
(2, 'Laptop Y', 'A lightweight laptop perfect for work and entertainment.', 1299.99, 56),
(3, 'Headphones Z', 'Premium wireless headphones with noise-cancellation technology.', 299.99, 57),
(4, 'Tablet W', 'A versatile tablet for productivity and entertainment on the go.', 499.99, 57),
(5, 'Smartwatch V', 'Stay connected and track your fitness with this stylish smartwatch.', 199.99, 58);

INSERT INTO Employee(EmpID, Name, Position, Account, PhoneNumber, Address, CompID) VALUES 
(0001, 'Clark', 'Sales', 'clark@mail.com', '0000000000', 'Houston, CA', 56),
(0002, 'Emma', 'Marketing', 'emma@mail.com', '1111111111', 'New York, NY', 56),
(0003, 'John', 'Engineer', 'john@mail.com', '2222222222', 'San Francisco, CA', 57),
(0004, 'Sophia', 'HR Manager', 'sophia@mail.com', '3333333333', 'Los Angeles, CA', 57),
(0005, 'William', 'Developer', 'william@mail.com', '4444444444', 'Chicago, IL', 58);

-- display
SELECT * FROM Admin;
SELECT * FROM Company;
SELECT * FROM Department;
SELECT * FROM Employee;
SELECT * FROM Product;