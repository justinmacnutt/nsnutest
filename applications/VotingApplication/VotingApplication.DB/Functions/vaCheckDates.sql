IF EXISTS (SELECT * FROM sysobjects WHERE type = 'FN' AND name = 'vaCheckDates')
	BEGIN
		PRINT 'Dropping FUNCTION vaCheckDates'
		DROP  FUNCTION  vaCheckDates
	END

GO

PRINT 'Creating FUNCTION vaCheckDates'
GO
CREATE FUNCTION vaCheckDates
	/* Param List */
	(
		@NewDisplayDate	datetime,
		@NewExpiryDate	datetime,
		@SiteId	int
	)
RETURNS
	/* Return type */
	bit
AS

/******************************************************************************
**		File: vaCheckDates.sql
**		Name: vaCheckDates
**		Desc: Returns a bit value indicating if the submitted dates do not overlap any current dates in the Votes table.
**
**		This template can be customized:
**              
**		Return values:
**		bit - 1 for success 0 for failure
** 
**		Called by:   
**              
**		Parameters:
**		Input							Output
**     ----------							-----------
**		@NewDisplayDate		- new display date to test
**		@NewExpiryDate		- new expiry date to test
**		@SiteId				- site id
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
	DECLARE @return bit;
	DECLARE @DisplayDate datetime, @ExpiryDate datetime


	SET @return = 1;
	-- 
	DECLARE dates CURSOR FOR
		SELECT DisplayDate, ExpiryDate 
		FROM Vote
		WHERE SiteID = @SiteId
		AND IsDeleted = 0

	OPEN dates;
	FETCH NEXT FROM dates INTO @DisplayDate, @ExpiryDate;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		if @ExpiryDate Is Null And @NewExpiryDate Is Null
				-- 'both null'
			SET @return = 0;
		
		if @DisplayDate < @NewDisplayDate AND @ExpiryDate > @NewDisplayDate
			-- 'new display date falls within existing period'
			SET @return = 0;
			
		if @DisplayDate < @NewExpiryDate AND (@ExpiryDate Is Null Or  @ExpiryDate > @NewExpiryDate)
			-- 'new close date falls within existing period'
			SET @return = 0;
			
		 if @DisplayDate > @NewDisplayDate AND ((@ExpiryDate Is Not Null And  @ExpiryDate < @NewExpiryDate) Or @NewExpiryDate Is null)
				-- 'new period envelops existing period'	
				SET @return = 0;
	
	
		FETCH NEXT FROM dates INTO @DisplayDate, @ExpiryDate;
	END
	CLOSE dates;
	DEALLOCATE dates;

	RETURN @return;
END

GO

GRANT EXEC ON vaCheckDates TO PUBLIC

GO
