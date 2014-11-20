IF EXISTS (SELECT * FROM sysobjects WHERE type = 'FN' AND name = 'vaTrimTimeFromDateTime')
	BEGIN
		PRINT 'Dropping FUNCTION vaTrimTimeFromDateTime'
		DROP  FUNCTION  vaTrimTimeFromDateTime
	END

GO

PRINT 'Creating FUNCTION vaTrimTimeFromDateTime'
GO
CREATE FUNCTION vaTrimTimeFromDateTime
	/* Param List */
	(
		@date as datetime
	)
RETURNS
	/* Return type */
	datetime with schemabinding 
AS

/******************************************************************************
**		File: vaTrimTimeFromDateTime.sql
**		Name: vaTrimTimeFromDateTime
**		Desc: 
**
**		This template can be customized:
**              
**
**		Auth: Simon Anderson
**		Date: Feb 06th, 2013
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------		--------				-------------------------------------------
**    
*******************************************************************************/

BEGIN

    --- Convert to a float, and get the integer that represents it.
    --- And then convert back to datetime.
    return cast(floor(cast(@date as float)) as datetime)

END

GO

GRANT EXEC ON vaTrimTimeFromDateTime TO PUBLIC

GO
