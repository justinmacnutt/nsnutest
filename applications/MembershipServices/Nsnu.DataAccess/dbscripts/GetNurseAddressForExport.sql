/****** Object:  StoredProcedure [dbo].GetNurseAddressForExport    Script Date: 05/17/2013 12:30:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].GetNurseAddressForExport') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].GetNurseAddressForExport
GO


/****** Object:  StoredProcedure [dbo].GetNurseAddressForExport    Script Date: 05/17/2013 12:30:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].GetNurseAddressForExport 
(
	@userids varchar(MAX),
	@addressTypeID int
)

AS


SELECT a.Line1, a.Line2, a.City, a.ProvinceId, a.PostalCode, n.UserId
FROM [Address] a
inner join nurseaddress na ON na.addressid = a.id
inner join nurse n on n.userid = na.nurseid
WHERE na.nurseid in (SELECT CAST(Item AS int) FROM dbo.nsSplit(@userids, N','))
AND a.addressTypeID = @addressTypeID


GO


