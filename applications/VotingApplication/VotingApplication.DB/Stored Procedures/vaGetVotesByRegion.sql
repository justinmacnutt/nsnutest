IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVotesByRegion')
	BEGIN
		PRINT 'Dropping Procedure vaGetVotesByRegion'
		DROP  Procedure  vaGetVotesByRegion
	END

GO

PRINT 'Creating Procedure vaGetVotesByRegion'
GO
CREATE Procedure vaGetVotesByRegion
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaGetVotesByRegion.sql
**		Name: vaGetVotesByRegion
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

DECLARE @temptable TABLE (TotalVote int, RegionName varchar(100));

	INSERT INTO @temptable
	  select count(*) As TotalVote, r.regionname
	  FROM nsnu_cm70.dbo.UserVote uv
	  INNER JOIN nsnu_membership.dbo.Nurse n on uv.userId = n.userId   
	  INNER JOIN nsnu_membership.dbo.NurseFacility nf on n.userId = nf.nurseId
	  INNER JOIN nsnu_membership.dbo.Facility f on nf.facilityid = f.id
	  INNER JOIN nsnu_membership.dbo.District d on f.districtid = d.id
	  INNER JOIN nsnu_membership.dbo.Region r on d.regionid = r.id

	  WHERE uv.voteid = @voteID
	  AND nf.Priority = 1 -- primary facility 
	  GROUP BY r.regionname

DECLARE @TotalVotes int
SELECT  @TotalVotes = sum(TotalVote) FROM @temptable
SELECT  TotalVote, TotalVote * 100 / @TotalVotes as PercentVote, RegionName FROM @temptable
GROUP BY TotalVote, RegionName
ORDER BY TotalVote DESC
  

GO

GRANT EXEC ON vaGetVotesByRegion TO PUBLIC

GO
