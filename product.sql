USE [ProductDB]
GO
/****** Object:  Table [dbo].[brand]    Script Date: 13/05/2025 6:45:19 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[brand](
	[brand_id] [int] IDENTITY(1,1) NOT NULL,
	[brand_name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[is_active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[brand_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[category]    Script Date: 13/05/2025 6:45:19 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[image_url] [varchar](300) NULL,
	[is_active] [bit] NULL,
	[parent_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product]    Script Date: 13/05/2025 6:45:19 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[product_id] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [nvarchar](200) NOT NULL,
	[category_id] [int] NULL,
	[brand_id] [int] NULL,
	[price] [decimal](18, 2) NULL,
	[discount] [decimal](5, 2) NULL,
	[stock] [int] NULL,
	[description] [nvarchar](max) NULL,
	[specs] [nvarchar](max) NULL,
	[is_active] [bit] NULL,
	[is_featured] [bit] NULL,
	[sold_quantity] [int] NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product_image]    Script Date: 13/05/2025 6:45:19 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product_image](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NULL,
	[image_url] [varchar](300) NULL,
	[is_main] [bit] NULL,
	[sort_order] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[brand] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[category] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[product] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[product] ADD  DEFAULT ((0)) FOR [is_featured]
GO
ALTER TABLE [dbo].[product] ADD  DEFAULT ((0)) FOR [sold_quantity]
GO
ALTER TABLE [dbo].[product] ADD  DEFAULT (getdate()) FOR [created_at]
GO
