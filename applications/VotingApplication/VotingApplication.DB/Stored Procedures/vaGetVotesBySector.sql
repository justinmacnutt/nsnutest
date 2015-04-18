IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVotesBySector')
	BEGIN
		PRINT 'Dropping Procedure vaGetVotesBySector'
		DROP  Procedure  vaGetVotesBySector
	END

GO

PRINT 'Creating Procedure vaGetVotesBySector'
GO
CREATE Procedure vaGetVotesBySector
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaGetVotesBySector.sql
**		Name: vaGetVotesBySector
**		Desc:
**
**		This template can be customized:
**              
**		Return values:
** 
**		Called by:   
**              
**		Parameters:
**		Input							Output
**     ----------							-----------
**
**		Auth: Simon Anderson
**		Date: Feb 1, 2013
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**
*******************************************************************************/
SET NOCOUNT ON

DECLARE @temptable TABLE (TotalVote int, FacilityTypeName varchar(100));

	INSERT INTO @temptable
	  SELECT count(*) As TotalVote, ft.facilityTypeName
	  FROM nsnu_cm70.dbo.UserVote uv
	  INNER JOIN nsnu_membership.dbo.Nurse n on uv.userId = n.userId   
	  INNER JOIN nsnu_membership.dbo.NurseFacility nf on n.userId = nf.nurseId
	  INNER JOIN nsnu_membership.dbo.Facility f on nf.facilityid = f.id
	  INNER JOIN nsnu_membership.dbo.refFacilityType ft on f.facilityTypeID = ft.id

	  WHERE uv.voteid = @voteID
	  AND nf.Priority = 1 -- primary facility 
	  GROUP BY ft.facilityTypeName

declare @TotalVotes int
select  @TotalVotes = sum(TotalVote) from @temptable
select  TotalVote, TotalVote * 100 / @TotalVotes as PercentVote, FacilityTypeName from @temptable
GROUP BY TotalVote, FacilityTypeName
ORDER BY TotalVote DESC

GO

GRANT EXEC ON vaGetVotesBySector TO PUBLIC

GO
