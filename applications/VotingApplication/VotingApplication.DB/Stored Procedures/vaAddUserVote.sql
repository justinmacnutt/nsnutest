IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaAddUserVote')
	BEGIN
		PRINT 'Dropping Procedure vaAddUserVote'
		DROP  Procedure  vaAddUserVote
	END

GO

PRINT 'Creating Procedure vaAddUserVote'
GO
CREATE Procedure vaAddUserVote
	/* Param List */
	(
		@VoteID int,
		@UserID varchar(50),
		@VoteDate datetime
	)
AS

/******************************************************************************
**		File: vaAddUserVote.sql
**		Name: vaAddUserVote
**		Desc: Adds a new vote to the database.
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

-- check to see if this user has previously voted on this vote
IF EXISTS(SELECT * FROM UserVote WHERE VoteID = @voteID AND UserID = @userID)
	RETURN 0;

Declare @ProfileID int
SELECT @ProfileID = Profileid from UserVoteProfile where MemberName = @UserID

-- check for user being a member of the nurses sytem
IF ISNULL(@ProfileID, 0) = 0
BEGIN
	RETURN 1;
END

INSERT INTO UserVote (VoteID, UserID, VoteDate) 
VALUES (@VoteID,
		@ProfileID,
		@VoteDate)	

IF @@ERROR <> 0
BEGIN
	RETURN 0;
END

RETURN 2;	

GO

GRANT EXEC ON vaAddUserVote TO PUBLIC

GO
