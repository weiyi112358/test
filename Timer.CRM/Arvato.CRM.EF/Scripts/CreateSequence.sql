USE [Kidsland_CRM]
GO

USE [Kidsland_CRM]
GO

/****** Object:  Sequence [dbo].[MemberCodeSequence]    Script Date: 2015/10/28 14:14:33 ******/
DROP SEQUENCE [dbo].[MemberCodeSequence]
GO

USE [Kidsland_CRM]
GO

/****** Object:  Sequence [dbo].[MemberCodeSequence]    Script Date: 2015/10/28 14:14:33 ******/
CREATE SEQUENCE [dbo].[MemberCodeSequence] 
 AS [int]
 START WITH 100000000
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO


USE [Kidsland_CRM]
GO

USE [Kidsland_CRM]
GO

/****** Object:  Sequence [dbo].[CouponSequence]    Script Date: 2015/10/28 14:15:16 ******/
DROP SEQUENCE [dbo].[CouponSequence]
GO

USE [Kidsland_CRM]
GO

/****** Object:  Sequence [dbo].[CouponSequence]    Script Date: 2015/10/28 14:15:16 ******/
CREATE SEQUENCE [dbo].[CouponSequence] 
 AS [int]
 START WITH 1000000
 INCREMENT BY 1
 MINVALUE -2147483648
 MAXVALUE 2147483647
 CACHE 
GO


USE [master]
GO

/****** Object:  LinkedServer [42.159.249.149]    Script Date: 2015/11/5 16:49:11 ******/
EXEC master.dbo.sp_dropserver @server=N'42.159.249.149', @droplogins='droplogins'
GO

/****** Object:  LinkedServer [42.159.249.149]    Script Date: 2015/11/5 16:49:11 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'42.159.249.149', @srvproduct=N'SQL Server'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'42.159.249.149',@useself=N'False',@locallogin=NULL,@rmtuser=NULL,@rmtpassword=NULL
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'42.159.249.149',@useself=N'False',@locallogin=N'sa',@rmtuser=N'sa',@rmtpassword='########'

GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'42.159.249.149', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO







