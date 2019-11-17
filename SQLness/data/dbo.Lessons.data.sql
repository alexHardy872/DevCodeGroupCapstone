SET IDENTITY_INSERT [dbo].[Lessons] ON
INSERT INTO [dbo].[Lessons] ([LocationId], [LessonId], [subject], [start], [end], [cost], [teacherApproval], [studentId], [teacherId], [Length], [Price], [LessonType], [travelDuration], [requiresMakeup]) VALUES (1, 1, N'awesomeness', N'2019-11-05 13:00:00', N'2019-11-05 14:00:00', CAST(20.00 AS Decimal(18, 2)), 0, 2, 1, 60, CAST(20.00 AS Decimal(18, 2)), N'In-Studio', 0, 0)
INSERT INTO [dbo].[Lessons] ([LocationId], [LessonId], [subject], [start], [end], [cost], [teacherApproval], [studentId], [teacherId], [Length], [Price], [LessonType], [travelDuration], [requiresMakeup]) VALUES (2, 2, N'javascript', N'2019-11-05 13:00:00', N'2019-11-05 14:00:00', CAST(25.00 AS Decimal(18, 2)), 0, 3, 2, 60, CAST(25.00 AS Decimal(18, 2)), N'Online', 0, 0)
INSERT INTO [dbo].[Lessons] ([LocationId], [LessonId], [subject], [start], [end], [cost], [teacherApproval], [studentId], [teacherId], [Length], [Price], [LessonType], [travelDuration], [requiresMakeup]) VALUES (1, 3, N'bass', N'2019-11-05 13:00:00', N'2019-11-05 14:00:00', CAST(30.00 AS Decimal(18, 2)), 0, 1, 3, 60, CAST(30.00 AS Decimal(18, 2)), N'In-Studio', 0, 0)
SET IDENTITY_INSERT [dbo].[Lessons] OFF
