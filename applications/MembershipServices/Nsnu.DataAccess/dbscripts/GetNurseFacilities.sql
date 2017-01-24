/****** Object:  StoredProcedure [dbo].GetNurseFacilities    Script Date: 05/17/2013 12:30:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].GetNurseFacilities') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].GetNurseFacilities
GO


/****** Object:  StoredProcedure [dbo].GetNurseFacilities    Script Date: 05/17/2013 12:30:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].GetNurseFacilities 
(
	@userids varchar(MAX)
)

AS



SELECT userid,firstname,lastname, ISNull([Full time], '') AS FullTimeFacility, ISNull([part time], '') AS PartTimeFacility, ISNull([casual], '') As CasualFacility FROM 
(
SELECT n.userid,n.firstname,n.lastname,f.facilityname, pt.employmentTypename
FROM Nurse n
INNER JOIN Nursefacility nf
ON nf.nurseid = n.userid
INNER JOIN facility f
ON f.id = nf.facilityid
INNER JOIN refEmploymentType pt
ON pt.id = nf.EmploymentTypeid
WHERE n.userid in (SELECT CAST(Item AS int) FROM dbo.nsSplit(@userids, N','))
) A
PIVOT (MAX(facilityname) FOR employmentTypename IN ([Full time], [part time], [casual])) T

GO


