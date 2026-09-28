--SQL UP SCRIPTIF OBJECT_ID(N'[dbo].[tblSOPChangeStateHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tblSOPChangeStateHistory]
    (
        [Id] INT IDENTITY(1,1) NOT NULL,
		[EntityTypeId] TINYINT  NOT NULL,
		[EntityTypeTitle] NVARCHAR(200) NOT NULL,
		[EntityId] bigint NOT NULL,
		[EntityStateTypeId] TINYINT  NOT NULL,
		[EntityStateTypeTitle] NVARCHAR(200) NOT NULL,
        [UserId] bigint NOT NULL,
        IsCurrent BIT NOT NULL,
        [CreatedDate] dateTime NOT NULL DEFAULT(GETDATE()),
        
        CONSTRAINT [PK_ChangeStateHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
        );
END