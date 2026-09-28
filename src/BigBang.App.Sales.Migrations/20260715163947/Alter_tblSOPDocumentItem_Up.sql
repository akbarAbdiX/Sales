IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblSOPDocumentItem]') 
      AND name = N'PrintDescription'
)
BEGIN
    ALTER TABLE [dbo].[tblSOPDocumentItem] ADD [PrintDescription] NVARCHAR(MAX) NULL; 
END
GO
