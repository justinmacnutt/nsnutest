IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'DeleteMemberProfile')
	BEGIN
		PRINT 'Dropping Trigger DeleteMemberProfile'
		DROP  Trigger DeleteMemberProfile
	END
GO

/******************************************************************************
**		File: 
**		Name: DeleteMemberProfile
**		Desc: Trigger to remove records form the UserVoteprofile table when a member is Deleted from the extranet.
**				Without this, a nsnu member could not logon to the site again.
**
**		This template can be customized:
**              
**
**		Auth: Simon Anderson
**		Date: January 27, 2005
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------		--------				-------------------------------------------
**    
*******************************************************************************/

PRINT 'Creating Trigger DeleteMemberProfile'
GO
CREATE Trigger DeleteMemberProfile 
ON dbo.Member

FOR DELETE
AS


DECLARE @MemberName varchar(50)
SET @MemberName = (SELECT MemberName FROM deleted)  

DELETE FROM UserVoteProfile
WHERE MemberName =  @MemberName

GO

