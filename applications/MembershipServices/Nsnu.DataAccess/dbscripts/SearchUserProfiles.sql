/****** Object:  StoredProcedure [dbo].[SearchUserProfiles]    Script Date: 05/17/2013 12:30:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchUserProfiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchUserProfiles]
GO

USE [NSNU]
GO

/****** Object:  StoredProcedure [dbo].[SearchUserProfiles]    Script Date: 05/17/2013 12:30:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--SearchUserProfiles 'jmacnutt', 'justin', 'macnutt', 'justin@gmail.com', 1, 1
--SearchUserProfiles null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null

CREATE procedure [dbo].[SearchUserProfiles] 
@username varchar(100),
@firstName varchar(100), 
@lastName varchar(100),
@email varchar(100),
@designationId tinyint,
@employmentStatusId tinyint,
@sectorId tinyint,
@facilityId int,
@districtId tinyint,
@regionId tinyint,
@committeeId tinyint,
@positionId tinyint,
@communicationOptionId int,
@employerGroupId tinyint,
@facilityCasualCoverage bit,
@facilityLpnCoverage bit, 
@letterFilter varchar(1),
@isAlternate bit,
@tableOfficerPosition tinyint,
@localPosition tinyint
as



select n.userId, n.firstName, n.lastName, n.initial, n.nickname, n.genderId, n.birthdate, up.username, (select top 1 phoneNumber from Phone p join NursePhone np on p.id = np.phoneId where n.userId = np.userId and p.phoneTypeId = 1) as phone, up.email, up.secondaryemail, n.nurseDesignationId, n.employmentStatusId, (select top 1 f.facilityName from Facility f join NurseFacility nf on f.id = nf.facilityId where nf.priority = 1 and nf.nurseId = n.userId) as primaryFacility, a.line1, a.line2, a.city, a.provinceId, a.postalCode
from Nurse n 
JOIN UserProfile up on n.userId = up.id
LEFT OUTER JOIN NurseAddress na on n.userId = na.nurseId
LEFT OUTER JOIN Address a on na.addressId = a.id
where n.userId in 
(
select distinct n.userId
from Nurse n
JOIN UserProfile up on n.userId = up.id
LEFT OUTER JOIN NurseFacility nf on n.userId = nf.nurseId
LEFT OUTER JOIN NurseCommittee nc on n.userId = nc.nurseId
LEFT OUTER JOIN Facility f on nf.facilityId = f.id
LEFT OUTER JOIN District d on f.districtId = d.id
where n.isDeleted = 0 
and (@username is null or up.username like '%' + @username + '%')
and (@firstName is null or n.firstName like '%' + @firstName + '%')
and (@lastName is null or n.lastName like '%' + @lastName + '%')
and (@email is null or up.email like '%' + @email + '%')
and (@designationId is null or n.nurseDesignationId = @designationId)
and (@employmentStatusId is null or n.employmentStatusId = @employmentStatusId)
and (@sectorId is null or f.facilityTypeId = @sectorId)
and (@facilityId is null or f.id = @facilityId)
and (@districtId is null or f.districtId = @districtId)
and (@regionId is null or d.regionId = @regionId)
and (@employerGroupId is null or f.employerGroupId = @employerGroupId)
and (@facilityCasualCoverage is null or f.casualCoverage = @facilityCasualCoverage)
and (@facilityLpnCoverage is null or f.lpnCoverage = @facilityLpnCoverage)
and (@letterFilter is null or n.lastName like @letterFilter + '%')
and (@communicationOptionId is null or (NOT EXISTS (select nurseId from NurseOptOut where optOutID = @communicationOptionId and nurseId = n.userId)))
and (@employerGroupId is null or 
	EXISTS (select nurseid from nursefacility nf
	INNER JOIN facility f ON nf.facilityid = f.id
	WHERE f.employerGroupID = @employerGroupID
	))
and (@committeeId is null or (nc.committeeid = @committeeId) 
	--BOARD OF DIRECTOR EXCEPTIONS
	or (@committeeId = 1 and ( (nc.committeeId = 6 and nc.positionId = 2) -- finance chair
								or (nc.committeeId = 9 and nc.positionId = 2) -- bursary chair
								or (nc.committeeId = 2 and nc.positionId in (1,4)) -- negotiating pres and vp
								)
								))
and (@positionId is null or (nc.positionid = @positionId and nc.isAlternate = @isAlternate and nc.committeeId = @committeeId)
							or (@committeeId = 1 and nc.committeeId = 6 and nc.positionId = 2 and nc.isAlternate = @isAlternate and @positionId = 7) --Finance Chair
							or (@committeeId = 1 and nc.committeeId = 9 and nc.positionId = 2 and nc.isAlternate = @isAlternate and nc.regionId = 1 and @positionId = 3) --Central Bursary Chair
							or (@committeeId = 1 and nc.committeeId = 9 and nc.positionId = 2 and nc.isAlternate = @isAlternate and nc.regionId = 2 and @positionId = 6) --Western Bursary Chair
							or (@committeeId = 1 and nc.committeeId = 9 and nc.positionId = 2 and nc.isAlternate = @isAlternate and nc.regionId = 3 and @positionId = 4) --Northern Bursary Chair
							or (@committeeId = 1 and nc.committeeId = 9 and nc.positionId = 2 and nc.isAlternate = @isAlternate and nc.regionId = 4 and @positionId = 5) --Eastern Bursary Chair
							or (@committeeId = 1 and nc.committeeId = 2 and nc.positionId = 1 and nc.isAlternate = @isAlternate and @positionId = 1) --Negotiating Pres
							or (@committeeId = 1 and nc.committeeId = 2 and nc.positionId = 4 and nc.isAlternate = @isAlternate and @positionId = 2) --Negotiating Vice Pres
							)
and (@localPosition is null or 
	EXISTS (Select nurseId from FacilityLocalPosition flp
			where flp.positionId = @localPosition
			and (@facilityId is null or flp.facilityId = @facilityId)
			and nurseId = n.userId
			))
and (@tableOfficerPosition is null or 
	EXISTS (Select nurseId from FacilityTableOfficer fto
			where fto.positionId = @tableOfficerPosition
			and (@facilityId is null or fto.facilityId = @facilityId)
			and nurseId = n.userId
			))
) 
order by n.lastName, n.firstName

GO


