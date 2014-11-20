IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaUpdateVoteCounts')
	BEGIN
		PRINT 'Dropping Procedure vaUpdateVoteCounts'
		DROP  Procedure  vaUpdateVoteCounts
	END

GO

PRINT 'Creating Procedure vaUpdateVoteCounts'
GO
CREATE Procedure vaUpdateVoteCounts
	/* Param List */
	(
		@voteID int,
		@Answer1Count int,
		@Answer2Count int
	)
AS

/******************************************************************************
**		File: vaUpdateVoteCounts.sql
**		Name: vaUpdateVoteCounts
**		Desc: Saves the vote.
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
**		Date: Feb 5, 2013
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**
*******************************************************************************/
SET NOCOUNT ON

IF NOT EXISTS(SELECT * FROM Vote WHERE ID = @voteID)
	RETURN 0;
	

UPDATE Vote
SET Answer1Count = @Answer1Count,
	Answer2Count = @Answer2Count
WHERE ID = @voteID;

IF @@ERROR <> 0
BEGIN
	RETURN 0;
END

RETURN 1;

GO

GRANT EXEC ON vaUpdateVoteCounts TO PUBLIC

GO
