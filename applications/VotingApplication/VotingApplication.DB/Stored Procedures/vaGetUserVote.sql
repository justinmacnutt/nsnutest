IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetUserVote')
	BEGIN
		PRINT 'Dropping Procedure vaGetUserVote'
		DROP  Procedure  vaGetUserVote
	END

GO

PRINT 'Creating Procedure vaGetUserVote'
GO
CREATE Procedure vaGetUserVote
	/* Param List */
	(
		@voteID	int,
		@userID varchar(50)
	)
AS

/******************************************************************************
**		File: vaGetUserVote.sql
**		Name: vaGetUserVote
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
**		Date: Feb 5, 2013
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**
*******************************************************************************/
SET NOCOUNT ON


Declare @ProfileID int
SELECT @ProfileID = Profileid from UserVoteProfile where MemberName = @UserID

-- check for user being a member of the nurses sytem
--IF ISNULL(@ProfileID, 0) = 0
--BEGIN
--	RETURN ;
--END

SELECT VoteID,
	UserID,
	VoteDate
FROM UserVote
WHERE VoteID = @voteID
AND UserID = @ProfileID

GO

GRANT EXEC ON vaGetUserVote TO PUBLIC

GO
