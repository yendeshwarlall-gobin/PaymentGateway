USE [master]
GO

CREATE LOGIN [payment.gateway.user] WITH PASSWORD=N'Asdcxz1+', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [payment.gateway.user] ENABLE
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [payment.gateway.user]
GO


