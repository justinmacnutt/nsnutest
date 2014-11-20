declare @rc bit
declare @siteID int
declare @displayDate datetime
declare @expirydate datetime

set @displayDate = '05/02/2013'
set @expirydate = '06/02/2013'
set @siteID = 1

if dbo.vaCheckdates (@displayDate, @expirydate,  @siteID) = 1
print 'ok'
else print 'invalid dates'
