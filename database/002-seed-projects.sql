SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM dbo.Projects)
BEGIN
    INSERT INTO dbo.Projects
        (ProjectNumber, Name, CustomerName, Status, CreatedDate, DueDate, IsActive)
    VALUES
        ('PRJ-1001', 'North Campus Modernization', 'Northwind Industries', 'Active', DATEADD(DAY, -18, SYSUTCDATETIME()), DATEADD(DAY, 30, SYSUTCDATETIME()), 1),
        ('PRJ-1002', 'Customer Portal Upgrade', 'Contoso Services', 'Active', DATEADD(DAY, -14, SYSUTCDATETIME()), DATEADD(DAY, 21, SYSUTCDATETIME()), 1),
        ('PRJ-1003', 'Operations Reporting Refresh', 'Fabrikam Group', 'Planning', DATEADD(DAY, -10, SYSUTCDATETIME()), DATEADD(DAY, 45, SYSUTCDATETIME()), 1),
        ('PRJ-1004', 'Legacy Workflow Migration', 'Adventure Works', 'Active', DATEADD(DAY, -8, SYSUTCDATETIME()), DATEADD(DAY, 60, SYSUTCDATETIME()), 1),
        ('PRJ-1005', 'Field Services Integration', 'Tailspin Services', 'On Hold', DATEADD(DAY, -6, SYSUTCDATETIME()), NULL, 1),
        ('PRJ-1006', 'Document Automation Pilot', 'Northwind Industries', 'Completed', DATEADD(DAY, -90, SYSUTCDATETIME()), DATEADD(DAY, -15, SYSUTCDATETIME()), 0),
        ('PRJ-1007', 'Enterprise Search Prototype', 'Contoso Services', 'Completed', DATEADD(DAY, -75, SYSUTCDATETIME()), DATEADD(DAY, -7, SYSUTCDATETIME()), 0),
        ('PRJ-1008', 'Data Reconciliation Initiative', 'Fabrikam Group', 'Active', DATEADD(DAY, -4, SYSUTCDATETIME()), DATEADD(DAY, 28, SYSUTCDATETIME()), 1);
END;
GO
