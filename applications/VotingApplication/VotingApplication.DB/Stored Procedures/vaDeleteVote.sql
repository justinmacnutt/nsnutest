IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaDeleteVote')
	BEGIN
		PRINT 'Dropping Procedure vaDeleteVote'
		DROP  Procedure  vaDeleteVote
	END

GO

PRINT 'Creating Procedure vaDeleteVote'
GO
CREATE Procedure vaDeleteVote
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaDeleteVote.sql
**		Name: vaDeleteVote
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
**		Date:Feb 1, 2013
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**    
*******************************************************************************/
SET NOCOUNT ON

IF NOT EXISTS(SELECT COUNT(*) FROM Vote WHERE ID = @voteID)
	RETURN 0;


UPDATE Vote
SET IsDeleted = 1
WHERE ID = @voteID;

IF @@ERROR <> 0
BEGIN
	RETURN 0;
END

RETURN 1;


GO

GRANT EXEC ON vaDeleteVote TO PUBLIC

GO
