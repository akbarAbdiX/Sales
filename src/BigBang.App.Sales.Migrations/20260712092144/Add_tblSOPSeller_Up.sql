IF OBJECT_ID(N'[dbo].[tblSOPSeller]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tblSOPSeller]
    (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(200) NOT NULL,
        [EconomicCode] INT NULL,
        [BranchCode] int NULL,
        [Address]  NVARCHAR(500) NULL,
        [PostalCode] INT NULL,
        [PhoneNumber] INT NULL,
        [FaxNumber] INT NULL,  
        [RegistrationNumber] INT NULL,  
        IsDefault BIT NOT NULL,
        CONSTRAINT [PK_Seller] PRIMARY KEY CLUSTERED ([Id] ASC),
        );
END

