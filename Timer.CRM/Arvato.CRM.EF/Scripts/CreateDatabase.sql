/* SCRIPT: CREATE_DB.sql */
/* �������ݿ� */

SET NOCOUNT ON

GO

PRINT '��ʼ�������ݿ�'

IF EXISTS (SELECT 1 FROM SYS.DATABASES WHERE NAME = 'Kidsland_CRM')
DROP DATABASE Kidsland_CRM
GO

CREATE DATABASE Kidsland_CRM
GO

:On Error ignore
PRINT '��ʼ����������'
:r CreateFieldAlias.sql
PRINT '�����������'
PRINT '��ʼ����Option���û���'
:r CreateOptionAndUser.sql
PRINT 'Option���û��������'
PRINT '��ʼ������'
:r CreateTable.sql
PRINT '�������'
PRINT '��ʼ������ͼ'
:r CreateView.sql
PRINT '��ͼ�������'
PRINT '��ʼ������������'
:r CreateBaseData.sql
PRINT '�������ݴ������'
PRINT '��ʼ������������'
:r CreateRegionData.sql
PRINT '�������ݴ������'
PRINT '��ʼ�����洢����'
:r CreateSP.sql
PRINT '�洢���̴������'



PRINT '���ݿⴴ�����'

GO