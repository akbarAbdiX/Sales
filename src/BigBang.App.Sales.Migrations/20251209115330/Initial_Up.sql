--SQL UP SCRIPT

/****** Object:  Table [dbo].[tblSOPAddendum]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPAddendum](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NOT NULL,
	[AddendumTypeID] [int] NOT NULL,
	[AmendmentDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Amount] [decimal](18, 0) NULL,
	[Notes] [nvarchar](max) NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_tblSOPAddendum] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPAddendumType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPAddendumType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[TimeChange] [bit] NOT NULL,
	[AmountChange] [bit] NOT NULL,
 CONSTRAINT [PK_tblSOPAddendumType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPAdjustment]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPAdjustment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[AdjustmentTypeID] [int] NOT NULL,
	[Rate] [decimal](5, 2) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[IsAppliedTax] [bit] NOT NULL,
 CONSTRAINT [PK_tblSOPAdjustment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPAdjustmentSign]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPAdjustmentSign](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [smallint] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tblSOPAdjustmentSign] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPAdjustmentType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPAdjustmentType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[AdjustmentSignID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_tblSOPAdjustmentType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPContractAdditionalInfo]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPContractAdditionalInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NOT NULL,
	[ContractFieldTypeID] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblSOPContractAdditionalInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPContractFieldType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPContractFieldType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[FieldName] [nvarchar](100) NOT NULL,
	[IsRequired] [bit] NOT NULL,
 CONSTRAINT [PK_tblSOPcontractfieldtype] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPContractStage]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPContractStage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [int] NOT NULL,
	[SubContractStageId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
 CONSTRAINT [PK_tblSOPContractStage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPDocument]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPDocument](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[No] [int] NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[DocumentTypeID] [int] NOT NULL,
	[DocumentID] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[DiscountAmount] [decimal](18, 0) NOT NULL,
	[AdditionalChargesAmount] [decimal](18, 0) NOT NULL,
	[TaxAmount] [decimal](18, 0) NOT NULL,
	[TotalAmount] [decimal](18, 0) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[StateID] [int] NULL,
 CONSTRAINT [PK_tblSOPDocument] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPDocumentAdjustment]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPDocumentAdjustment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentItemID] [int] NOT NULL,
	[AdjustmentID] [int] NOT NULL,
	[AdjustmentAmount] [decimal](18, 0) NOT NULL,
	[ParameterAmount] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_tblSOPDocumentAdjustment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPDocumentFlow]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPDocumentFlow](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Level] [int] NOT NULL,
	[DocumentTypeID] [int] NOT NULL,
 CONSTRAINT [PK_tblSOPDocumentFlow] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPDocumentItem]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPDocumentItem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[Amount] [decimal](18, 0)  NULL,
	[Quantity] [int] NOT NULL,
	[ProductAndServiceRateID] [int] NOT NULL,
	[UnitPrice] [decimal](18, 0) NOT NULL,
	[Discount] [decimal](18, 0) NOT NULL,
	[AdditionalCharge] [decimal](18, 0) NOT NULL,
	[Tax] [decimal](18, 0) NOT NULL,
	[TotalPrice] [decimal](18, 0) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblSOPDocumenttIem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPDocumentType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPDocumentType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_tblSOPDocumentType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPDueType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPDueType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tblSOPDueType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPGuaranteeType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPGuaranteeType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tblSOPGuaranteeType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPMainContractStage]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPMainContractStage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tblSOPMainContractStage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPPaymentBreakdown]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPPaymentBreakdown](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [int] NOT NULL,
	[PhaseName] [nvarchar](200) NOT NULL,
	[PerAmount] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_tblSOPPaymentBreakdown] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPPaymentTerm]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPPaymentTerm](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [int] NOT NULL,
	[PaymentTermsTypeId] [int] NOT NULL,
	[DueTypeId] [int] NOT NULL,
	[PerAmount] [decimal](5, 2) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[DueDate] [date] NULL,
	[IsDeductible] [bit] NULL,
	[IsGuarantee] [bit] NULL,
	[GuaranteeTypeId] [int] NULL,
	[ReleaseCondition] [nvarchar](max) NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_tblSOPPaymentTerm] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPPaymentTermsType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPPaymentTermsType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_tblSOPPaymentTermsType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPProductAndServiceRate]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPProductAndServiceRate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[BasePrice] [decimal](18, 0) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_tblSOPProductAndServiceRate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPProductDetailTaxExemption]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPProductDetailTaxExemption](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductDetailId] [int] NOT NULL,
	[TaxExemptionTypeId] [int] NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NULL,
 CONSTRAINT [PK_tblSOPProductDetailTaxExemption] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPSubContractStage]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPSubContractStage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[MainContractStageId] [int] NOT NULL,
 CONSTRAINT [PK_tblSOPSubContractStage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSOPTaxExemptionType]    Script Date: 12/2/2025 10:12:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSOPTaxExemptionType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tblSOPTaxExemptionType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblSOPAddendumType] ON 
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (1, 1, N'تمدید مدت قرارداد', 1, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (2, 2, N'افزایش یا کاهش مبلغ قرارداد', 0, 1)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (3, 3, N'تغییر شرایط پرداخت', 0, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (4, 4, N'تغییر موضوع قرارداد', 0, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (5, 5, N'تغییر مشخصات فنی یا کیفی', 0, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (6, 6, N'تغییر مکان یا محل اجرای قرارداد', 0, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (7, 7, N'تغییر اشخاص طرف قرارداد', 0, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (8, 8, N'فسخ یا خاتمه پیش از موعد', 0, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (9, 9, N'اضافه کردن ضوابط و مقررات تکمیلی', 0, 0)
GO
INSERT [dbo].[tblSOPAddendumType] ([ID], [Code], [Title], [TimeChange], [AmountChange]) VALUES (10, 10, N'تغییر در ضمانت‌ها یا تضامین', 0, 0)
GO
SET IDENTITY_INSERT [dbo].[tblSOPAddendumType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPAdjustmentSign] ON 
GO
INSERT [dbo].[tblSOPAdjustmentSign] ([ID], [Value], [Name], [Title]) VALUES (1, 1, N'Increment', N'افزاینده')
GO
INSERT [dbo].[tblSOPAdjustmentSign] ([ID], [Value], [Name], [Title]) VALUES (2, -1, N'Decrement', N'کاهنده')
GO
SET IDENTITY_INSERT [dbo].[tblSOPAdjustmentSign] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPAdjustmentType] ON 
GO
INSERT [dbo].[tblSOPAdjustmentType] ([ID], [Code], [Title], [AdjustmentSignID], [Name]) VALUES (1, 1, N'تخفیف', 2, N'Discount')
GO
INSERT [dbo].[tblSOPAdjustmentType] ([ID], [Code], [Title], [AdjustmentSignID], [Name]) VALUES (2, 2, N'مالیات', 1, N'Tax')
GO
INSERT [dbo].[tblSOPAdjustmentType] ([ID], [Code], [Title], [AdjustmentSignID], [Name]) VALUES (3, 3, N'هزینه های اضافی', 1, N'AdditionalCharges')
GO
SET IDENTITY_INSERT [dbo].[tblSOPAdjustmentType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPContractFieldType] ON 
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (1, 1, N'محل اجرای پروژه', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (2, 2, N'روش پرداخت', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (3, 3, N'ضمانت‌ نامه بانکی', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (4, 4, N'جریمه دیرکرد', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (5, 5, N'مدت قرارداد', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (6, 6, N'تاریخ ابلاغ', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (7, 7, N'نماینده حقوقی طرف اول', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (8, 8, N'نماینده حقوقی طرف دوم', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (9, 9, N'شرایط پرداخت', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (10, 10, N'درصد پیش پرداخت', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (11, 11, N'ضمانت حسن انجام کار', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (12, 12, N'شرایط تحویل', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (13, 13, N'گارانتی', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (14, 14, N'پشتیبانی', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (15, 15, N'مرجع حل اختلاف', 0)
GO
INSERT [dbo].[tblSOPContractFieldType] ([ID], [Code], [FieldName], [IsRequired]) VALUES (16, 16, N'شرایط فسخ قرارداد', 0)
GO
SET IDENTITY_INSERT [dbo].[tblSOPContractFieldType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPDocumentType] ON 
GO
INSERT [dbo].[tblSOPDocumentType] ([ID], [Code], [Title], [Name]) VALUES (1, 1, N'پیش فاکتور', N'ProformaInvoice')
GO
INSERT [dbo].[tblSOPDocumentType] ([ID], [Code], [Title], [Name]) VALUES (2, 2, N'سفارش', N'Order')
GO
INSERT [dbo].[tblSOPDocumentType] ([ID], [Code], [Title], [Name]) VALUES (3, 3, N'قرارداد', N'Contract')
GO
INSERT [dbo].[tblSOPDocumentType] ([ID], [Code], [Title], [Name]) VALUES (4, 4, N'فاکتور', N'Invoice')
GO
SET IDENTITY_INSERT [dbo].[tblSOPDocumentType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPDueType] ON 
GO
INSERT [dbo].[tblSOPDueType] ([Id], [Code], [Description]) VALUES (1, 1, N'هنگام امضا')
GO
INSERT [dbo].[tblSOPDueType] ([Id], [Code], [Description]) VALUES (2, 2, N'بعد از تحویل فاز مرتبط')
GO
INSERT [dbo].[tblSOPDueType] ([Id], [Code], [Description]) VALUES (3, 3, N'بعد از صدور مفاصاحساب')
GO
INSERT [dbo].[tblSOPDueType] ([Id], [Code], [Description]) VALUES (4, 4, N'پایان پروژه')
GO
SET IDENTITY_INSERT [dbo].[tblSOPDueType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPGuaranteeType] ON 
GO
INSERT [dbo].[tblSOPGuaranteeType] ([Id], [Code], [Description]) VALUES (1, 1, N'ضمانتنامه بانکی')
GO
INSERT [dbo].[tblSOPGuaranteeType] ([Id], [Code], [Description]) VALUES (2, 2, N'چک')
GO
INSERT [dbo].[tblSOPGuaranteeType] ([Id], [Code], [Description]) VALUES (3, 3, N'سفته')
GO
SET IDENTITY_INSERT [dbo].[tblSOPGuaranteeType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPMainContractStage] ON 
GO
INSERT [dbo].[tblSOPMainContractStage] ([Id], [Code], [Description]) VALUES (1, 1, N'عقد قرارداد')
GO
INSERT [dbo].[tblSOPMainContractStage] ([Id], [Code], [Description]) VALUES (2, 2, N'اجرای قرارداد')
GO
INSERT [dbo].[tblSOPMainContractStage] ([Id], [Code], [Description]) VALUES (3, 3, N'اتمام قرارداد')
GO
INSERT [dbo].[tblSOPMainContractStage] ([Id], [Code], [Description]) VALUES (4, 4, N'پایان غیرعادی')
GO
SET IDENTITY_INSERT [dbo].[tblSOPMainContractStage] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPPaymentTermsType] ON 
GO
INSERT [dbo].[tblSOPPaymentTermsType] ([Id], [Code], [Description]) VALUES (1, 1, N'پیش‌پرداخت')
GO
INSERT [dbo].[tblSOPPaymentTermsType] ([Id], [Code], [Description]) VALUES (2, 2, N'حسن انجام کار')
GO
INSERT [dbo].[tblSOPPaymentTermsType] ([Id], [Code], [Description]) VALUES (3, 3, N'تضمین انجام تعهدات')
GO
INSERT [dbo].[tblSOPPaymentTermsType] ([Id], [Code], [Description]) VALUES (4, 4, N'بیمه')
GO
INSERT [dbo].[tblSOPPaymentTermsType] ([Id], [Code], [Description]) VALUES (5, 5, N'صورت‌وضعیت دوره‌ای')
GO
SET IDENTITY_INSERT [dbo].[tblSOPPaymentTermsType] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPSubContractStage] ON 
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (1, 1,   N'دریافت درخواست قرارداد', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (2, 2,   N'تهیه پیش‌نویس قرارداد', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (3, 3,   N'بررسی حقوقی داخل', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (4, 4,   N'ارسال پیش‌نویس برای مشتری', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (5, 5,   N'دریافت اصلاحات مشتری', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (6, 6,   N'نهایی‌سازی قرارداد', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (7, 7,   N'امضای طرفین', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (8, 8,   N'ثبت رسمی', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (9, 9,   N'دریافت نامه ابلاغ شروع قرارداد', 1)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (10, 10, N'آغاز کار', 2)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (11, 11, N'اجرای عملیات', 2)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (12, 12, N'تحویل موقت', 2)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (13, 13, N'دریافت پرداخت‌های مرحله‌ای', 2)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (14, 14, N'تحویل دائم', 2)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (15, 15, N'ثبت درخواست تسویه مالی نهایی', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (16, 16, N'نامه به کارفرما جهت اخذ مفاصاحساب بیمه', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (17, 17, N'معرفی کارفرما به بیمه', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (18, 18, N'تهیه و ارسال مدارک بیمه', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (19, 19, N'صدور مفاصاحساب بیمه', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (20, 20, N'ارائه مفاصاحساب به مشتری', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (21, 21, N'آزاد سازی بیمه', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (22, 22, N'آزاد سازی ضمانتنامه ها', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (23, 23, N'بستن حساب‌های پروژه', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (24, 24, N'بستن قرارداد', 3)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (25, 25, N'خاتمه زودهنگام قرارداد', 4)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (26, 26, N'خاتمه زودهنگام قرارداد', 4)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (27, 27, N'فسخ قرارداد توسط شرکت', 4)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (28, 28, N'تسویه میانی / تعیین تکلیف حقوقی', 4)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (29, 29, N'ثبت دلایل و مستندات فسخ', 4)
GO
INSERT [dbo].[tblSOPSubContractStage] ([Id], [Code], [Description], [MainContractStageId]) VALUES (30, 30, N'تحلیل ریسک و ثبت تجربیات', 4)
GO
SET IDENTITY_INSERT [dbo].[tblSOPSubContractStage] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSOPTaxExemptionType] ON 
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (1, 1, N'محصول دانش‌ بنیان')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (2, 2, N'محصول فرهنگی و رسانه‌ای')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (3, 3, N'محصول هنری و صنایع‌دستی')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (4, 4, N'تولید در مناطق آزاد تجاری – صنعتی')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (5, 5, N'تولید در مناطق ویژه اقتصادی')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (6, 6, N'تولید در پارک‌های علم و فناوری و پردیس‌ها')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (7, 7, N'تولید در مناطق محروم و کمترتوسعه‌یافته')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (8, 8, N'صنعت و معدن')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (9, 9, N'انرژی‌های نو و زیست‌ محیطی')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (10, 10, N'طرح‌های تحقیق و توسعه')
GO
INSERT [dbo].[tblSOPTaxExemptionType] ([ID], [Code], [Description]) VALUES (11, 11, N'محصول دانش‌بنیان / صنعت و معدن')
GO
SET IDENTITY_INSERT [dbo].[tblSOPTaxExemptionType] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_tblSOPAdjustmentSign_Name]    Script Date: 12/2/2025 10:12:48 AM ******/
ALTER TABLE [dbo].[tblSOPAdjustmentSign] ADD  CONSTRAINT [UQ_tblSOPAdjustmentSign_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblSOPAdjustment] ADD  CONSTRAINT [DF_tblSOPAdjustment_Rate]  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[tblSOPAdjustment] ADD  CONSTRAINT [DF_tblSOPAdjustment_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[tblSOPDocument] ADD  CONSTRAINT [DF_tblSOPDocument_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[tblSOPDocument] ADD  CONSTRAINT [DF_tblSOPDocument_DiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[tblSOPDocument] ADD  CONSTRAINT [DF_tblSOPDocument_AdditionalChargesAmount]  DEFAULT ((0)) FOR [AdditionalChargesAmount]
GO
ALTER TABLE [dbo].[tblSOPDocument] ADD  CONSTRAINT [DF_tblSOPDocument_TaxAmount]  DEFAULT ((0)) FOR [TaxAmount]
GO
ALTER TABLE [dbo].[tblSOPDocument] ADD  CONSTRAINT [DF_tblSOPDocument_TotalAmount]  DEFAULT ((0)) FOR [TotalAmount]
GO
ALTER TABLE [dbo].[tblSOPDocumentAdjustment] ADD  CONSTRAINT [DF_tblSOPDocumentAdjustment_AdjustmentAmount]  DEFAULT ((0)) FOR [AdjustmentAmount]
GO
ALTER TABLE [dbo].[tblSOPDocumentAdjustment] ADD  CONSTRAINT [DF_tblSOPDocumentAdjustment_ParameterAmount]  DEFAULT ((0)) FOR [ParameterAmount]
GO
ALTER TABLE [dbo].[tblSOPDocumentItem] ADD  CONSTRAINT [DF_tblSOPDocumentItem_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
GO
ALTER TABLE [dbo].[tblSOPDocumentItem] ADD  CONSTRAINT [DF_tblSOPDocumentItem_Discount]  DEFAULT ((0)) FOR [Discount]
GO
ALTER TABLE [dbo].[tblSOPDocumentItem] ADD  CONSTRAINT [DF_tblSOPDocumentItem_AdditionalCharge]  DEFAULT ((0)) FOR [AdditionalCharge]
GO
ALTER TABLE [dbo].[tblSOPDocumentItem] ADD  CONSTRAINT [DF_tblSOPDocumentItem_Tax]  DEFAULT ((0)) FOR [Tax]
GO
ALTER TABLE [dbo].[tblSOPDocumentItem] ADD  CONSTRAINT [DF_tblSOPDocumentItem_TotalPrice]  DEFAULT ((0)) FOR [TotalPrice]
GO
ALTER TABLE [dbo].[tblSOPAddendum]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPAddendum_tblSOPAddendumType] FOREIGN KEY([AddendumTypeID])
REFERENCES [dbo].[tblSOPAddendumType] ([ID])
GO
ALTER TABLE [dbo].[tblSOPAddendum] CHECK CONSTRAINT [FK_tblSOPAddendum_tblSOPAddendumType]
GO
ALTER TABLE [dbo].[tblSOPAddendum]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPAddendum_tblSOPDocument] FOREIGN KEY([DocumentID])
REFERENCES [dbo].[tblSOPDocument] ([ID])
GO
ALTER TABLE [dbo].[tblSOPAddendum] CHECK CONSTRAINT [FK_tblSOPAddendum_tblSOPDocument]
GO
ALTER TABLE [dbo].[tblSOPAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPAdjustment_tblSOPAdjustmentType] FOREIGN KEY([AdjustmentTypeID])
REFERENCES [dbo].[tblSOPAdjustmentType] ([ID])
GO
ALTER TABLE [dbo].[tblSOPAdjustment] CHECK CONSTRAINT [FK_tblSOPAdjustment_tblSOPAdjustmentType]
GO
ALTER TABLE [dbo].[tblSOPAdjustmentType]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPAdjustmentType_tblSOPAdjustmentSign] FOREIGN KEY([AdjustmentSignID])
REFERENCES [dbo].[tblSOPAdjustmentSign] ([ID])
GO
ALTER TABLE [dbo].[tblSOPAdjustmentType] CHECK CONSTRAINT [FK_tblSOPAdjustmentType_tblSOPAdjustmentSign]
GO
ALTER TABLE [dbo].[tblSOPContractAdditionalInfo]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPContractAdditionalInfo_tblSOPcontractfieldtype] FOREIGN KEY([ContractFieldTypeID])
REFERENCES [dbo].[tblSOPContractFieldType] ([ID])
GO
ALTER TABLE [dbo].[tblSOPContractAdditionalInfo] CHECK CONSTRAINT [FK_tblSOPContractAdditionalInfo_tblSOPcontractfieldtype]
GO
ALTER TABLE [dbo].[tblSOPContractAdditionalInfo]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPContractAdditionalInfo_tblSOPDocument] FOREIGN KEY([DocumentID])
REFERENCES [dbo].[tblSOPDocument] ([ID])
GO
ALTER TABLE [dbo].[tblSOPContractAdditionalInfo] CHECK CONSTRAINT [FK_tblSOPContractAdditionalInfo_tblSOPDocument]
GO
ALTER TABLE [dbo].[tblSOPContractStage]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPContractStage_tblSOPDocument] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[tblSOPDocument] ([ID])
GO
ALTER TABLE [dbo].[tblSOPContractStage] CHECK CONSTRAINT [FK_tblSOPContractStage_tblSOPDocument]
GO
ALTER TABLE [dbo].[tblSOPContractStage]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPContractStage_tblSOPSubContractStage] FOREIGN KEY([SubContractStageId])
REFERENCES [dbo].[tblSOPSubContractStage] ([Id])
GO
ALTER TABLE [dbo].[tblSOPContractStage] CHECK CONSTRAINT [FK_tblSOPContractStage_tblSOPSubContractStage]
GO
ALTER TABLE [dbo].[tblSOPDocument]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPDocument_tblSOPDocument] FOREIGN KEY([DocumentID])
REFERENCES [dbo].[tblSOPDocument] ([ID])
GO
ALTER TABLE [dbo].[tblSOPDocument] CHECK CONSTRAINT [FK_tblSOPDocument_tblSOPDocument]
GO
ALTER TABLE [dbo].[tblSOPDocument]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPDocument_tblSOPDocumentType] FOREIGN KEY([DocumentTypeID])
REFERENCES [dbo].[tblSOPDocumentType] ([ID])
GO
ALTER TABLE [dbo].[tblSOPDocument] CHECK CONSTRAINT [FK_tblSOPDocument_tblSOPDocumentType]
GO
ALTER TABLE [dbo].[tblSOPDocumentAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPDocumentAdjustment_tblSOPAdjustment] FOREIGN KEY([AdjustmentID])
REFERENCES [dbo].[tblSOPAdjustment] ([ID])
GO
ALTER TABLE [dbo].[tblSOPDocumentAdjustment] CHECK CONSTRAINT [FK_tblSOPDocumentAdjustment_tblSOPAdjustment]
GO
ALTER TABLE [dbo].[tblSOPDocumentAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPDocumentAdjustment_tblSOPDocumentItem] FOREIGN KEY([DocumentItemID])
REFERENCES [dbo].[tblSOPDocumentItem] ([ID])
GO
ALTER TABLE [dbo].[tblSOPDocumentAdjustment] CHECK CONSTRAINT [FK_tblSOPDocumentAdjustment_tblSOPDocumentItem]
GO
ALTER TABLE [dbo].[tblSOPDocumentFlow]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPDocumentFlow_tblSOPDocumentType] FOREIGN KEY([DocumentTypeID])
REFERENCES [dbo].[tblSOPDocumentType] ([ID])
GO
ALTER TABLE [dbo].[tblSOPDocumentFlow] CHECK CONSTRAINT [FK_tblSOPDocumentFlow_tblSOPDocumentType]
GO
ALTER TABLE [dbo].[tblSOPDocumentItem]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPDocumentItem_tblSOPDocument] FOREIGN KEY([DocumentID])
REFERENCES [dbo].[tblSOPDocument] ([ID])
GO
ALTER TABLE [dbo].[tblSOPDocumentItem] CHECK CONSTRAINT [FK_tblSOPDocumentItem_tblSOPDocument]
GO
ALTER TABLE [dbo].[tblSOPDocumentItem]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPDocumenttIem_tblSOPProductAndServiceRate] FOREIGN KEY([ProductAndServiceRateID])
REFERENCES [dbo].[tblSOPProductAndServiceRate] ([ID])
GO
ALTER TABLE [dbo].[tblSOPDocumentItem] CHECK CONSTRAINT [FK_tblSOPDocumenttIem_tblSOPProductAndServiceRate]
GO
ALTER TABLE [dbo].[tblSOPPaymentBreakdown]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPPaymentBreakdown_tblSOPDocument] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[tblSOPDocument] ([ID])
GO
ALTER TABLE [dbo].[tblSOPPaymentBreakdown] CHECK CONSTRAINT [FK_tblSOPPaymentBreakdown_tblSOPDocument]
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPDocument] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[tblSOPDocument] ([ID])
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm] CHECK CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPDocument]
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPDueType] FOREIGN KEY([DueTypeId])
REFERENCES [dbo].[tblSOPDueType] ([Id])
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm] CHECK CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPDueType]
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPGuaranteeType] FOREIGN KEY([GuaranteeTypeId])
REFERENCES [dbo].[tblSOPGuaranteeType] ([Id])
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm] CHECK CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPGuaranteeType]
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPPaymentTermsType] FOREIGN KEY([PaymentTermsTypeId])
REFERENCES [dbo].[tblSOPPaymentTermsType] ([Id])
GO
ALTER TABLE [dbo].[tblSOPPaymentTerm] CHECK CONSTRAINT [FK_tblSOPPaymentTerm_tblSOPPaymentTermsType]
GO
ALTER TABLE [dbo].[tblSOPProductDetailTaxExemption]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPProductDetailTaxExemption_tblSOPTaxExemptionType] FOREIGN KEY([TaxExemptionTypeId])
REFERENCES [dbo].[tblSOPTaxExemptionType] ([ID])
GO
ALTER TABLE [dbo].[tblSOPProductDetailTaxExemption] CHECK CONSTRAINT [FK_tblSOPProductDetailTaxExemption_tblSOPTaxExemptionType]
GO
ALTER TABLE [dbo].[tblSOPSubContractStage]  WITH CHECK ADD  CONSTRAINT [FK_tblSOPSubContractStage_tblSOPMainContractStage] FOREIGN KEY([MainContractStageId])
REFERENCES [dbo].[tblSOPMainContractStage] ([Id])
GO
ALTER TABLE [dbo].[tblSOPSubContractStage] CHECK CONSTRAINT [FK_tblSOPSubContractStage_tblSOPMainContractStage]
GO
