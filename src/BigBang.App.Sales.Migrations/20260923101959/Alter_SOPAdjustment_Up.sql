IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPAdjustment')
      AND name = N'FromDate'
)
BEGIN
    ALTER TABLE dbo.tblSOPAdjustment
    ADD FromDate DateTime NOT NULL 
        CONSTRAINT DF_tblSOPAdjustment_FromDate DEFAULT (GETDATE());
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.tblSOPAdjustment')
      AND name = N'ToDate'
)
BEGIN
    ALTER TABLE dbo.tblSOPAdjustment
    ADD ToDate DateTime NULL
        CONSTRAINT DF_tblSOPAdjustment_ToDate DEFAULT (NULL);
END
GO