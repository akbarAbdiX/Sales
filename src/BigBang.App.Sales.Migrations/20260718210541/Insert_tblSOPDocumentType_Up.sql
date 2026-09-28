IF NOT EXISTS (SELECT 1 FROM [dbo].[tblSOPDocumentType] WHERE [ID] = 6)
BEGIN
    SET IDENTITY_INSERT [dbo].[tblSOPDocumentType] ON;

    INSERT INTO [dbo].[tblSOPDocumentType] ([ID], [Code], [Title], [Name])
    VALUES (6, 6, N'پیشنهاد قیمت', N'PriceProposal');

    SET IDENTITY_INSERT [dbo].[tblSOPDocumentType] OFF;
END
ELSE
BEGIN
    UPDATE [dbo].[tblSOPDocumentType]
    SET [Code] = 6,
        [Title] = N'پیشنهاد قیمت',
        [Name] = N'PriceProposal'
    WHERE [ID] = 6;
END
