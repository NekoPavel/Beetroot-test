CREATE TABLE employees(
employee_number int identity(1,1) NOT NULL PRIMARY KEY, 
first_name VARCHAR(255) NOT NULL,
last_name VARCHAR(255) NOT NULL,
age INT NOT NULL,
);
CREATE TABLE languages(
employee_number int NOT NULL, 
language_name VARCHAR(255) NOT NULL,
proficiency_level INT NOT NULL,
);