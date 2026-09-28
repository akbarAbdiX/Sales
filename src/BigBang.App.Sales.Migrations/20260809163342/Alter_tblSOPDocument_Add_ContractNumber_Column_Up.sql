IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[tblSOPDocument]')
      AND name = N'ContractNumber'
)
BEGIN
    ALTER TABLE [dbo].[tblSOPDocument]
    ADD [ContractNumber] NVARCHAR(20) NULL;
END
GO