IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaAddVote')
	BEGIN
		PRINT 'Dropping Procedure vaAddVote'
		DROP  Procedure  vaAddVote
	END

GO

PRINT 'Creating Procedure vaAddVote'
GO
CREATE Procedure vaAddVote
	/* Param List */
	(
		@SiteID int,
		@Title varchar(500),
		@Lure varchar(2000),
		@Question varchar(4000),
		@Answer1 varchar(1000),
		@Answer2 varchar(1000),
		@DisplayDate datetime,
		@ExpiryDate datetime,
		@LastModifiedBy varchar(50),
		@LastModifiedDate datetime,
		@voteID int OUTPUT
	)
AS

/******************************************************************************
**		File: vaAddVote.sql
**		Name: vaAddVote
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

-- ensure that the vote's active date range does not overlap with another votes.
IF dbo.vaCheckDates (@displayDate, @expirydate, @siteID) = 0
	RETURN 2;
	
INSERT INTO Vote (SiteID, Title, Lure, Question, Answer1, Answer2, DisplayDate, ExpiryDate, LastModifiedBy, LastModifiedDate, IsDeleted) 
VALUES (@SiteID,
		@Title,
		@Lure,
		@Question,
		@Answer1,
		@Answer2,
		@DisplayDate,
		@ExpiryDate,
		@LastModifiedBy,
		@LastModifiedDate,
		0)	
IF @@ERROR <> 0
BEGIN
	RETURN 0;
END

SET @voteID = @@IDENTITY;

RETURN 1;	

GO

GRANT EXEC ON vaAddVote TO PUBLIC

GO
