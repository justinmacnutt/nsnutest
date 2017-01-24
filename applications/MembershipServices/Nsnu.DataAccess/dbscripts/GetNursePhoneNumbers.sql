/****** Object:  StoredProcedure [dbo].GetNursePhoneNumbers    Script Date: 05/17/2013 12:30:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].GetNursePhoneNumbers') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].GetNursePhoneNumbers
GO


/****** Object:  StoredProcedure [dbo].GetNursePhoneNumbers    Script Date: 05/17/2013 12:30:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].GetNursePhoneNumbers 
(
	@userids varchar(MAX)
)

AS


SELECT userid,firstname,lastname, ISNull([Work], '') AS WorkPhone, ISNull([Home], '') AS HomePhone, ISNull([Cell], '') As Cell, ISNull([Fax], '') As Fax
FROM
(
	SELECT n.userid, n.firstname, n.lastname, p.phonenumber,pt.phonetypename
	FROM Nurse n
	INNER JOIN NursePhone up
	ON up.Userid = n.userid
	INNER JOIN Phone p
	ON p.id = up.phoneid
	INNER JOIN refPhoneType pt
	ON pt.id = p.phonetypeid
	WHERE n.userid in (SELECT CAST(Item AS int) FROM dbo.nsSplit(@userids, N','))
)t
PIVOT(MAX(phonenumber) FOR phonetypename IN ([Work],[Home],[Cell],[Fax]))p

GO


