/****** Object:  StoredProcedure [dbo].[GetFacilitiesForExport]    Script Date: 05/17/2013 12:30:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFacilitiesForExport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetFacilitiesForExport]
GO


/****** Object:  StoredProcedure [dbo].[GetFacilitiesForExport]    Script Date: 05/17/2013 12:30:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE procedure [dbo].[GetFacilitiesForExport] 

as

SELECT f.facilityName, f.casualCoverage, f.lpnCoverage 
	,d.DistrictName, r.regionname
	,lr.firstname + ' ' + lr.lastname As 'Labour Rep', ft.facilityTypeName, eg.EmployerGroupName
	,a.line1, a.line2, a.city, a.provinceid, a.postalCode
	,p.PhoneNumber
FROM Facility f
INNER JOIN District d ON d.id = f.districtid
INNER JOIN Region r ON r.id = d.regionId
INNER JOIN LabourRepresentative lr ON lr.id = f.labourRepId
INNER JOIN refFacilityType ft ON ft.id = f.facilityTypeID
INNER JOIN refEmployerGroup eg On eg.id = f.employergroupId
INNER JOIN facilityPhone fp On fp.facilityid = f.id
INNER JOIN Phone p ON p.id = fp.phoneid

INNER JOIN facilityAddress fa ON fa.facilityid = f.id
INNER JOIN [Address] a ON a.id = fa.addressid

WHERE p.PhoneTypeId = 2 -- work phone
AND a.addressTypeid = 1 --mailing address

GO


