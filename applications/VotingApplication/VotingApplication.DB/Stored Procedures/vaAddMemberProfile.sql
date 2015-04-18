IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaAddMemberProfile')
	BEGIN
		PRINT 'Dropping Procedure vaAddMemberProfile'
		DROP  Procedure  vaAddMemberProfile
	END

GO

PRINT 'Creating Procedure vaAddMemberProfile'
GO
CREATE Procedure vaAddMemberProfile
	/* Param List */
	(
		@ProfileID int,
		@MemberName varchar(50)
	)
AS

/******************************************************************************
**		File: vaAddMemberProfile.sql
**		Name: vaAddMemberProfile
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

-- check to see if this member has already been added
IF EXISTS(SELECT * FROM UserVoteProfile WHERE ProfileId = @ProfileID AND MemberName = @MemberName)
	RETURN 0;

INSERT INTO UserVoteProfile (ProfileId, MemberName) 
VALUES (@ProfileID,
		@MemberName)	

IF @@ERROR <> 0
BEGIN
	RETURN 0;
END

RETURN 1;	

GO

GRANT EXEC ON vaAddMemberProfile TO PUBLIC

GO
