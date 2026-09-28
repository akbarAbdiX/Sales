IF COL_LENGTH('dbo.tblSOPPaymentBreakdown', 'PerAmount') IS NOT NULL
    ALTER TABLE [dbo].[tblSOPPaymentBreakdown] DROP COLUMN [PerAmount];
IF COL_LENGTH('dbo.tblSOPPaymentBreakdown', 'Title') IS NULL
    ALTER TABLE [dbo].[tblSOPPaymentBreakdown] ADD [Title] NVARCHAR(300) NOT NULL DEFAULT '';
IF COL_LENGTH('dbo.tblSOPPaymentBreakdown', 'Amount') IS NULL
    ALTER TABLE [dbo].[tblSOPPaymentBreakdown] ADD [Amount] DECIMAL(18, 2) NOT NULL DEFAULT 0;
IF COL_LENGTH('dbo.tblSOPPaymentBreakdown', 'DueDate') IS NULL
    ALTER TABLE [dbo].[tblSOPPaymentBreakdown] ADD [DueDate] DATE NOT NULL DEFAULT GETDATE();
