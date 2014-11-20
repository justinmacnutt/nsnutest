IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaSaveVote')
	BEGIN
		PRINT 'Dropping Procedure vaSaveVote'
		DROP  Procedure  vaSaveVote
	END

GO

PRINT 'Creating Procedure vaSaveVote'
GO
CREATE Procedure vaSaveVote
	/* Param List */
	(
		@voteID int,
		@siteID int,
		@Title varchar(500),
		@Lure varchar(2000),
		@Question varchar(4000),
		@Answer1 varchar(1000),
		@Answer2 varchar(1000),
		@DisplayDate datetime,
		@ExpiryDate datetime,
		@LastModifiedBy varchar(50),
		@LastModifiedDate datetime
	)
AS

/******************************************************************************
**		File: vaSaveVote.sql
**		Name: vaSaveVote
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
**		Date: Feb 1, 2013
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**
*******************************************************************************/
SET NOCOUNT ON

IF NOT EXISTS(SELECT * FROM Vote WHERE ID = @voteID)
	RETURN 0;
	
---- ensure that the vote's active date range does not overlap with another votes.
--IF dbo.vaCheckDates (@displayDate, @expirydate, @siteID) = 0
--	RETURN 2;

UPDATE Vote
SET SiteID = @siteID,
	Title = @Title,
	Lure = @Lure,
	Question = @Question,
	Answer1 = @Answer1,
	Answer2 = @Answer2,
	DisplayDate = @DisplayDate,
	ExpiryDate = @ExpiryDate,
	LastModifiedBy = @LastModifiedBy,
	LastModifiedDate = @LastModifiedDate
WHERE ID = @voteID;

IF @@ERROR <> 0
BEGIN
	RETURN 0;
END

RETURN 1;

GO

GRANT EXEC ON vaSaveVote TO PUBLIC

GO
