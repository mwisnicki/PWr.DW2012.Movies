GO
/****** Object:  Table [dbo].[dimCountryContinents]    Script Date: 05/29/2012 07:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[dimCountryContinents](
	[Country] [varchar](64) NOT NULL,
	[Continent] [varchar](32) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'', N'')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'\GB', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'\Ge', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'Australia', N'Australia')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'CA', N'North America')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'CA,USA', N'North America')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'China', N'Asia')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'Cuba', N'North America')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'D.R.Germany', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'Denmark', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'England', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'F.R.Germany', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'France', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'G.B.', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'GB', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'Germany', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'Holland', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'India', N'Asia')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'Italy', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'NJ', N'North America')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'NY', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'P.R.China', N'Asia')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'The Netherlands', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'U SA', N'North America')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'U.S.A.', N'North America')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'UK', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'UK?', N'Europe')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'USA', N'North America')
INSERT [dbo].[dimCountryContinents] ([Country], [Continent]) VALUES (N'USSR', N'Europe')
