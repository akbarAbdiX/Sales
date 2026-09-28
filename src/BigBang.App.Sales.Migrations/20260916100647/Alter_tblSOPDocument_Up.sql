IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'CustomerAddress'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD CustomerAddress int NULL;
END