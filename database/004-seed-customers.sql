IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Customers' AND schema_id = SCHEMA_ID('dbo'))
   AND NOT EXISTS (SELECT 1 FROM dbo.Customers)
BEGIN
    INSERT dbo.Customers
        (CustomerNumber, CompanyName, ContactName, Email, Phone, City, State, CreatedDate, IsActive)
    VALUES
        ('CUST-1001', 'Northwind Industries', 'Avery Stone', 'avery@example.test', '713-555-0101', 'Houston', 'TX', '2026-08-01', 1),
        ('CUST-1002', 'Contoso Services', 'Jordan Lee', 'jordan@example.test', '512-555-0102', 'Austin', 'TX', '2026-08-02', 1),
        ('CUST-1003', 'Fabrikam Group', 'Morgan Diaz', 'morgan@example.test', '303-555-0103', 'Denver', 'CO', '2026-08-03', 1),
        ('CUST-1004', 'Tailspin Services', 'Casey Reed', 'casey@example.test', '206-555-0104', 'Seattle', 'WA', '2026-08-04', 0);
END;
