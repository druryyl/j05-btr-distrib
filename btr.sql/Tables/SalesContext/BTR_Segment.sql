CREATE TABLE [dbo].[BTR_Segment]
(
	SegmentId VARCHAR(3) NOT NULL CONSTRAINT DF_BTR_Segment_SegmentId DEFAULT (''),
	SegmentName VARCHAR(255) NOT NULL CONSTRAINT DF_BTR_Segment_SegmentName DEFAULT (''),

	CONSTRAINT PK_BTR_Segment PRIMARY KEY CLUSTERED (SegmentId)
)
GO

--INSERT INTO BTR_Segment (SegmentId, SegmentName) 
--SELECT 'GT', 'General Trade' UNION ALL
--SELECT 'MM', 'Mini Market' UNION ALL
--SELECT 'SPM', 'Supermarket' UNION ALL
--SELECT 'NKA', 'National Key Account'
--GO
