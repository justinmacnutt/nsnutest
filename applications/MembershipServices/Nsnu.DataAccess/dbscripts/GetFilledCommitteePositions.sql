/****** Object:  StoredProcedure [dbo].[GetFilledCommitteePositions]    Script Date: 05/17/2013 12:30:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFilledCommitteePositions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetFilledCommitteePositions]
GO


/****** Object:  StoredProcedure [dbo].[GetFilledCommitteePositions]    Script Date: 05/17/2013 12:30:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[GetFilledCommitteePositions] 
@nurseId int
AS
select cp.committeeId, cp.positionId, A.isAlternate, A.regionId, A.districtId, maxPositions, maxAlternates, currentCount
from CommitteePosition cp
JOIN (
select committeeId, positionId, isAlternate, regionId, districtId, COUNT(*) as currentCount
from NurseCommittee
where nurseId != @nurseId
group by committeeId, positionId, isAlternate, regionId, districtId
) A on cp.committeeId = A.committeeId and cp.positionId = A.positionId
where (isAlternate = 0 and currentCount >= maxPositions) OR
(isAlternate = 1 and currentCount >= maxAlternates)

GO
