USE [WsSendDB]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 5/26/2024 12:28:57 AM ******/
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
/****** Object:  Table [dbo].[Queue]    Script Date: 5/26/2024 12:28:58 AM ******/
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
/****** Object:  Table [dbo].[Settings]    Script Date: 5/26/2024 12:28:58 AM ******/
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
GO
INSERT [dbo].[Company] ([Id], [Name], [Active]) VALUES (1, N'Company 1', 1)
GO
INSERT [dbo].[Company] ([Id], [Name], [Active]) VALUES (2, N'Company 2', 1)
GO
SET IDENTITY_INSERT [dbo].[Company] OFF
GO
SET IDENTITY_INSERT [dbo].[Queue] ON 
GO
INSERT [dbo].[Queue] ([Id], [TaskId], [CompanyId], [RequestedDate], [RequestingUser], [PhoneNumber], [Message], [AttachmentPath], [Sent], [SentDate], [Result]) VALUES (1, 1, 1, CAST(N'2024-05-22T00:00:00.000' AS DateTime), N'1', N'50762887499', N'Here is your file', N'cops/1/permit-portal/files/craiyon_015533_Illustration_of_Ansible_event_driven_automation_in_large_it_infrastructures__add_a_de.png', 0, CAST(N'2024-05-26T00:00:00.000' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Queue] OFF
GO
SET IDENTITY_INSERT [dbo].[Settings] ON 
GO
INSERT [dbo].[Settings] ([Id], [Key], [Value], [Active], [CompanyId]) VALUES (1, N'Twilio.WhatsApp.Token', N'5240fe42c3e244a248c35eceee15feea', 1, 1)
GO
INSERT [dbo].[Settings] ([Id], [Key], [Value], [Active], [CompanyId]) VALUES (2, N'Twilio.WhatsApp.AccountSid', N'AC15f38aa26a555b70455d724f8aa9f8ed', 1, 1)
GO
INSERT [dbo].[Settings] ([Id], [Key], [Value], [Active], [CompanyId]) VALUES (1002, N'Twilio.WhatsApp.MessagingServiceSid', N'MG19c41ee6b18b83ba38c13ae1d78a1d5a', 1, 1)
GO
INSERT [dbo].[Settings] ([Id], [Key], [Value], [Active], [CompanyId]) VALUES (1003, N'Twilio.WhatsApp.FileAttachmentSid', N'HX7ec4bf97214b5e0f3f0492620a3b677b', 1, 1)
GO
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
