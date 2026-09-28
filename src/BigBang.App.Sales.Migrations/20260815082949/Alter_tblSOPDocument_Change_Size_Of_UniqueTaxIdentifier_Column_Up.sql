IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'UniqueTaxIdentifier'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ALTER COLUMN UniqueTaxIdentifier NVARCHAR(20) NULL;
END
GO