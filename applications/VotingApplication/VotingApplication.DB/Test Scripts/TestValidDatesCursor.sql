DECLARE @DisplayDate datetime, @ExpiryDate datetime
DECLARE @NewDisplayDate datetime, @NewExpiryDate datetime
	
set @NewDisplayDate = '04/30/2013'
set @NewExpiryDate = '06/01/2013'
	
	
	-- copy the page templates
	DECLARE dates CURSOR FOR
		SELECT DisplayDate, ExpiryDate 
		FROM Vote
		WHERE SiteID = 1;

	OPEN dates;
	FETCH NEXT FROM dates INTO @DisplayDate, @ExpiryDate;

	-- get next page template to copy
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		--  if (closeDate == null && newCloseDate == null) {
		if @ExpiryDate Is Null And @NewExpiryDate Is Null
			print 'both null'
		
		-- if (openDate < newOpenDate && closeDate > newOpenDate)	
		if 	@DisplayDate < @NewDisplayDate AND @ExpiryDate > @NewDisplayDate
			print 'new display date falls within existing period'
		
		--  if (openDate < newCloseDate && (closeDate == null || closeDate > newCloseDate))	
		if 	@DisplayDate < @NewExpiryDate AND (@ExpiryDate Is Null Or  @ExpiryDate > @NewExpiryDate)
			print 'new close date falls within existing period'
			
		-- if (openDate > newOpenDate && ((closeDate != null && closeDate < newCloseDate) || newCloseDate == null))	
		if 	@DisplayDate > @NewDisplayDate AND ((@ExpiryDate Is Not Null And  @ExpiryDate < @NewExpiryDate) Or @NewExpiryDate Is null)
			print 'new period envelops existing period"'			
			

	
		print @DisplayDate --+ ' ' + @ExpiryDate
		
		FETCH NEXT FROM dates INTO @DisplayDate, @ExpiryDate;
	END
	CLOSE dates;
	DEALLOCATE dates;
	