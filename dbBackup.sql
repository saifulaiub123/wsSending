USE [WsSendDB]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 5/22/2024 12:40:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Queue]    Script Date: 5/22/2024 12:40:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Queue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaskId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[RequestedDate] [datetime] NOT NULL,
	[RequestingUser] [nvarchar](250) NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[AttachmentPath] [nvarchar](max) NULL,
	[Sent] [bit] NOT NULL,
	[SentDate] [datetime] NULL,
	[Result] [nvarchar](max) NULL,
 CONSTRAINT [PK_Queue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 5/22/2024 12:40:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL,
	[CompanyId] [int] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Company] ON 

INSERT [dbo].[Company] ([Id], [Name], [Active]) VALUES (1, N'Company 1', 1)
INSERT [dbo].[Company] ([Id], [Name], [Active]) VALUES (2, N'Company 2', 1)
SET IDENTITY_INSERT [dbo].[Company] OFF
GO
SET IDENTITY_INSERT [dbo].[Queue] ON 

INSERT [dbo].[Queue] ([Id], [TaskId], [CompanyId], [RequestedDate], [RequestingUser], [PhoneNumber], [Message], [AttachmentPath], [Sent], [SentDate], [Result]) VALUES (1, 1, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'1', N'8801684882360', N'Welcome message', N'ss', 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Queue] OFF
GO
SET IDENTITY_INSERT [dbo].[Settings] ON 

INSERT [dbo].[Settings] ([Id], [Key], [Value], [Active], [CompanyId]) VALUES (1, N'Twillow.WhatsApp.Token', N'Test', 1, 1)
INSERT [dbo].[Settings] ([Id], [Key], [Value], [Active], [CompanyId]) VALUES (2, N'Twillow.WhatsApp.ApiUrl', N'test', 1, 1)
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[Settings] ADD  CONSTRAINT [DF_Settings_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[Queue]  WITH CHECK ADD  CONSTRAINT [FK_Queue_Queue] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Queue] CHECK CONSTRAINT [FK_Queue_Queue]
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [FK_Settings_Company]
GO
