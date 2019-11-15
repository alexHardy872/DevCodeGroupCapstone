SET IDENTITY_INSERT [dbo].[TeacherPreferences] ON
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (1, 60, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 1, NULL)
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (2, 60, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 2, NULL)
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (3, 60, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 3, NULL)
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (4, 60, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 4, NULL)
SET IDENTITY_INSERT [dbo].[TeacherPreferences] OFF
