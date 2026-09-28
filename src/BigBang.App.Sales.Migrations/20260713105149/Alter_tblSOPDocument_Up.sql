IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[tblSOPDocument]') 
      AND name = N'SellerID'
)
BEGIN
    ALTER TABLE [dbo].[tblSOPDocument] ADD [SellerID] int NULL; 
END
GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblSOPDocument_tblSOPSeller]') 
      AND parent_object_id = OBJECT_ID(N'[dbo].[tblSOPDocument]')
)
BEGIN
    ALTER TABLE [dbo].[tblSOPDocument] WITH CHECK 
    ADD CONSTRAINT [FK_tblSOPDocument_tblSOPSeller] FOREIGN KEY([SellerID])
    REFERENCES [dbo].[tblSOPSeller] ([Id]);

    ALTER TABLE [dbo].[tblSOPDocument] CHECK CONSTRAINT [FK_tblSOPDocument_tblSOPSeller];
END
GO
