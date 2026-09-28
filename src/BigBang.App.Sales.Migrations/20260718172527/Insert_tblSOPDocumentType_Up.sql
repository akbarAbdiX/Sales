IF NOT EXISTS (SELECT 1 FROM [dbo].[tblSOPDocumentType] WHERE [ID] = 5)
BEGIN
    SET IDENTITY_INSERT [dbo].[tblSOPDocumentType] ON;

    INSERT INTO [dbo].[tblSOPDocumentType] ([ID], [Code], [Title], [Name])
    VALUES (5, 5, N'درخواست صدور فاکتور', N'InvoiceIssuanceRequest');

    SET IDENTITY_INSERT [dbo].[tblSOPDocumentType] OFF;
END
ELSE
BEGIN
    UPDATE [dbo].[tblSOPDocumentType]
    SET [Code] = 5,
        [Title] = N'درخواست صدور فاکتور',
        [Name] = N'InvoiceIssuanceRequest'
    WHERE [ID] = 5;
END
