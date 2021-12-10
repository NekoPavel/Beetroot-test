DROP TABLE IF EXISTS employees
DROP TABLE IF EXISTS languages

CREATE TABLE employees(
employee_number int identity(1,1) NOT NULL PRIMARY KEY, 
first_name VARCHAR(255) NOT NULL,
last_name VARCHAR(255) NOT NULL,
age INT NOT NULL,
);
CREATE TABLE languages(
id int identity(1,1) NOT NULL PRIMARY KEY,
employee_number int NOT NULL, 
language_name VARCHAR(255) NOT NULL,
proficiency_level INT NOT NULL,
);

INSERT INTO employees(first_name, last_name,age) VALUES('Pavel','Kuzminov',21)
