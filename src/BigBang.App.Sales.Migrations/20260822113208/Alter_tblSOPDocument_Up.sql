IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'Color'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD Color int NULL;
END
GO