IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPDocument')
      AND name = N'RejectionReason'
)
BEGIN
    ALTER TABLE dbo.tblSOPDocument
    ADD RejectionReason NVARCHAR(500) NULL;
END
GO
