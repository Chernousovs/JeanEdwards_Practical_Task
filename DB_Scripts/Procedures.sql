ALTER PROCEDURE [DBO].[QUERY_HISTORY_UPDATE]
	(@QUERY_STRING AS VARCHAR(255))
AS 
BEGIN
DECLARE @RECORD_COUNT SMALLINT,
@MAX_RECORD_COUNT SMALLINT;
SET @MAX_RECORD_COUNT = 5;
	
INSERT INTO QUERY_HISTORY(QUERY_STRING, QUERY_TIME)
VALUES (@QUERY_STRING, GETDATE());
SELECT @RECORD_COUNT = COUNT(*)
FROM QUERY_HISTORY;

IF @RECORD_COUNT > @MAX_RECORD_COUNT
DELETE FROM QUERY_HISTORY WHERE QUERY_ID NOT IN (
	SELECT TOP(5) QUERY_ID 
	FROM QUERY_HISTORY
	ORDER BY QUERY_TIME DESC
	);

END;