ALTER TABLE dbo.tblContact NOCHECK CONSTRAINT ALL;

truncate table dbo.tblContact

ALTER TABLE dbo.tblContact CHECK CONSTRAINT ALL;





ALTER SEQUENCE tblContactSeq RESTART WITH 1;



-------------
DELETE FROM dbo.tblContact;
DBCC CHECKIDENT ('dbo.tblContact', RESEED, 0);
ALTER SEQUENCE dbo.FeedbackID_Seq RESTART WITH 1;