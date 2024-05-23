USE AdventureWorks2017;
GO

SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';
GO

SELECT *
FROM Sales.Customer



SELECT TOP 1
    c.CustomerId, c.ModifiedDate
FROM Sales.Customer c
ORDER BY c.ModifiedDate DESC;

SELECT *
FROM Sales.Customer
WHERE ModifiedDate = (SELECT MAX(ModifiedDate)
FROM Sales.Customer);