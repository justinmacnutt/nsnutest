IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVotes')
	BEGIN
		PRINT 'Dropping Procedure vaGetVotes'
		DROP  Procedure  vaGetVotes
	END

GO

PRINT 'Creating Procedure vaGetVotes'
GO
CREATE Procedure vaGetVotes
	/* Param List */
	(
		@siteID		int
	)
AS

/******************************************************************************
**		File: vaGetVotes.sql
**		Name: vaGetVotes
**		Desc: Get all the votes for a site
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


SELECT ID,
	SiteID,
	Title,
	Lure,
	Question,
	Answer1,
	Answer2,
	Answer1Count,
	Answer2Count,
	DisplayDate,
	ExpiryDate,
	LastModifiedBy,
	LastModifiedDate,
	IsDeleted
FROM Vote V
--INNER JOIN VoteCulture EC ON EC.VoteID = ID
--LEFT OUTER JOIN mod_ecCategory C ON C.CategoryID = CategoryID
--LEFT OUTER JOIN mod_ecCategoryCulture CC ON CC.CategoryID = C.CategoryID
--WHERE EC.Culture = COALESCE(@culture, N'')
--AND (CC.Culture = COALESCE(@culture, N'') OR CC.Culture is null)
WHERE (COALESCE(SiteID, 0) = 0 OR COALESCE(SiteID, 0) = COALESCE(@siteID, 0) OR COALESCE(@siteID, 0) = 0);

GO

GRANT EXEC ON vaGetVotes TO PUBLIC

GO
