/****** Object:  UserDefinedFunction [dbo].[nsSplit]    Script Date: 06/17/2013 13:57:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[nsSplit]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[nsSplit]
GO

/****** Object:  UserDefinedFunction [dbo].[nsSplit]    Script Date: 06/17/2013 13:57:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].[nsSplit]
	/* Param List */
	(
		@string varchar(1000),
		@delimeter char(1)
	)
RETURNS
	/* Return type */
	@result TABLE (Item varchar(1000))
AS

/******************************************************************************
**		File: nsSplit.sql
**		Name: nsSplit
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
**		Auth: Travis Musika
**		Date: Splits a varchar string
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------		--------				-------------------------------------------
**    
*******************************************************************************/
BEGIN
	DECLARE @index int;
	DECLARE @start int;
	--DECLARE ;

	SET @index = CHARINDEX(@delimeter, @string);
	SET @start = 1;
	WHILE @index > 0
	BEGIN
		INSERT INTO @result (Item) VALUES (SUBSTRING(@string, @start, @index - @start))
		SET @start = @index + 1
		SET @index = CHARINDEX(@delimeter, @string, @start);
	END
	INSERT INTO @result (Item) VALUES (SUBSTRING(@string, @start, LEN(@string) + 1 - @start))

	RETURN;
END



GO


