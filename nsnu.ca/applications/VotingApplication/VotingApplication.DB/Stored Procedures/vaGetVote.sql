IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'vaGetVote')
	BEGIN
		PRINT 'Dropping Procedure vaGetVote'
		DROP  Procedure  vaGetVote
	END

GO

PRINT 'Creating Procedure vaGetVote'
GO
CREATE Procedure vaGetVote
	/* Param List */
	(
		@voteID	int
	)
AS

/******************************************************************************
**		File: vaGetVote.sql
**		Name: vaGetVote
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
	LastModifiedDate
FROM Vote V
--INNER JOIN VoteCulture EC ON EC.VoteID = ID
--LEFT OUTER JOIN mod_ecCategory C ON C.CategoryID = CategoryID
--LEFT OUTER JOIN mod_ecCategoryCulture CC ON CC.CategoryID = C.CategoryID
--WHERE EC.Culture = COALESCE(@culture, N'')
--AND (CC.Culture = COALESCE(@culture, N'') OR CC.Culture is null)
WHERE ID = @voteID

GO

GRANT EXEC ON vaGetVote TO PUBLIC

GO
