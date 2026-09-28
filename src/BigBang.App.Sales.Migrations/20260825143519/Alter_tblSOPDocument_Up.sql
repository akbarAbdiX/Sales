IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'PaymentTermsType'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD PaymentTermsType int NULL;
END
GO
IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'PaymentTermAmount'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD PaymentTermAmount bigint NULL;
END
GO