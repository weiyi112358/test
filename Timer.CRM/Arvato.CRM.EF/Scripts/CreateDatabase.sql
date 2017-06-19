/* SCRIPT: CREATE_DB.sql */
/* 创建数据库 */

SET NOCOUNT ON

GO

PRINT '开始创建数据库'

IF EXISTS (SELECT 1 FROM SYS.DATABASES WHERE NAME = 'Kidsland_CRM')
DROP DATABASE Kidsland_CRM
GO

CREATE DATABASE Kidsland_CRM
GO

:On Error ignore
PRINT '开始创建别名表'
:r CreateFieldAlias.sql
PRINT '别名表创建完毕'
PRINT '开始创建Option与用户表'
:r CreateOptionAndUser.sql
PRINT 'Option与用户表创建完毕'
PRINT '开始创建表'
:r CreateTable.sql
PRINT '表创建完毕'
PRINT '开始创建视图'
:r CreateView.sql
PRINT '视图创建完毕'
PRINT '开始创建基础数据'
:r CreateBaseData.sql
PRINT '基础数据创建完毕'
PRINT '开始创建区域数据'
:r CreateRegionData.sql
PRINT '区域数据创建完毕'
PRINT '开始创建存储过程'
:r CreateSP.sql
PRINT '存储过程创建完毕'



PRINT '数据库创建完毕'

GO