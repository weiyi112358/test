/* SCRIPT: CREATE_DB.sql */
/* �������ݿ� */

SET NOCOUNT ON

GO

PRINT '��ʼ�������ݿ�'

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
PRINT '��ʼ�����洢����'
:r CreateSP.sql
PRINT '�洢���̴������'


PRINT '���ݿⴴ�����'

GO