use [aspnet-DevCodeGroupCapstone-20191110123838]

INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'1b2ba4a0-ac41-4d16-b76b-0c61a1147ac2', N'test3@noreply.com', 0, N'AIkO47d+Oz45hEomLw1lbJTXZvgtQehCjVNQaTfUu76ItqBY2q5Wwx44E99MT3Tvvw==', N'd98dc094-cda8-4697-8fbe-09f98f6e76de', NULL, 0, 0, NULL, 0, 0, N'test3@noreply.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'708fc9ff-96b7-4dcc-a78a-71663ac38fd2', N'test2@noreply.com', 0, N'AIkO47d+Oz45hEomLw1lbJTXZvgtQehCjVNQaTfUu76ItqBY2q5Wwx44E99MT3Tvvw==', N'4a448de1-b350-4321-a9d0-a45ca1a9cbf2', NULL, 0, 0, NULL, 0, 0, N'test2@noreply.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'bee0ae46-f729-46c9-892d-7d994efcf6e0', N'test4@noreply.com', 0, N'AIkO47d+Oz45hEomLw1lbJTXZvgtQehCjVNQaTfUu76ItqBY2q5Wwx44E99MT3Tvvw==', N'3cdafc2a-6d0f-4474-85bc-965c32730a72', NULL, 0, 0, NULL, 0, 0, N'test4@noreply.com')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'e22021ed-a00f-467b-9671-fcec4a42daa2', N'test1@noreply.com', 0, N'AIkO47d+Oz45hEomLw1lbJTXZvgtQehCjVNQaTfUu76ItqBY2q5Wwx44E99MT3Tvvw==', N'00ac752f-7a46-48c1-a7fc-bdca784dea5d', NULL, 0, 0, NULL, 0, 0, N'test1@noreply.com')
GO

SET IDENTITY_INSERT [dbo].[People] ON
INSERT INTO [dbo].[People] ([PersonId], [firstName], [lastName], [subjects], [phoneNumber], [ApplicationId], [LocationId]) VALUES (1, N'Trevor', N'Clements', N'awesomeness', N'5419138650', N'e22021ed-a00f-467b-9671-fcec4a42daa2', 1)
INSERT INTO [dbo].[People] ([PersonId], [firstName], [lastName], [subjects], [phoneNumber], [ApplicationId], [LocationId]) VALUES (2, N'Gabe', N'Kunkel', N'javascript', N'4145079038', N'708fc9ff-96b7-4dcc-a78a-71663ac38fd2', 2)
INSERT INTO [dbo].[People] ([PersonId], [firstName], [lastName], [subjects], [phoneNumber], [ApplicationId], [LocationId]) VALUES (3, N'Alex', N'Hardy', N'bass', N'7472068258', N'1b2ba4a0-ac41-4d16-b76b-0c61a1147ac2', 3)
INSERT INTO [dbo].[People] ([PersonId], [firstName], [lastName], [subjects], [phoneNumber], [ApplicationId], [LocationId]) VALUES (4, N'Adam', N'Neujahr', N'guitar', N'3093176370', N'bee0ae46-f729-46c9-892d-7d994efcf6e0', 4)
SET IDENTITY_INSERT [dbo].[People] OFF
GO

SET IDENTITY_INSERT [dbo].[Locations] ON
INSERT INTO [dbo].[Locations] ([LocationId], [lat], [lng], [address1], [address2], [city], [state], [zip], [description]) VALUES (1, N'43.034169', N'-87.9119333', N'313 N Plankinton Ave', NULL, N'Milwaukee', 54, N'53203', NULL)
INSERT INTO [dbo].[Locations] ([LocationId], [lat], [lng], [address1], [address2], [city], [state], [zip], [description]) VALUES (2, N'43.1383663', N'-87.9773432', N'5222 W Hassel Ln', NULL, N'Milwaukee', 54, N'53223', NULL)
INSERT INTO [dbo].[Locations] ([LocationId], [lat], [lng], [address1], [address2], [city], [state], [zip], [description]) VALUES (3, N'43.0018617', N'-88.0324269', N'9627 W National Ave', NULL, N'West Allis', 54, N'53227', NULL)
INSERT INTO [dbo].[Locations] ([LocationId], [lat], [lng], [address1], [address2], [city], [state], [zip], [description]) VALUES (4, N'43.2948857', N'-87.9190001', N'1024 Lakefield Rd', NULL, N'Grafton', 54, N'53024', NULL)
SET IDENTITY_INSERT [dbo].[Locations] OFF
GO

SET IDENTITY_INSERT [dbo].[TeacherAvails] ON
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (1, 0, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 1)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (2, 1, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 1)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (3, 2, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 1)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (4, 3, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 1)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (5, 4, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 1)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (6, 5, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 1)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (7, 6, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 1)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (8, 0, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 2)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (9, 1, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 2)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (10, 2, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 2)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (11, 3, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 2)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (12, 4, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 2)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (13, 5, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 2)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (14, 6, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 2)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (15, 0, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 3)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (16, 1, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 3)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (17, 2, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 3)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (18, 3, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 3)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (19, 4, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 3)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (20, 5, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 3)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (21, 6, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 3)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (22, 0, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 4)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (23, 1, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 4)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (24, 2, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 4)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (25, 3, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 4)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (26, 4, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 4)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (27, 5, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 4)
INSERT INTO [dbo].[TeacherAvails] ([availId], [weekDay], [start], [end], [PersonId]) VALUES (28, 6, N'2019-11-15 08:00:00', N'2019-11-15 17:00:00', 4)
SET IDENTITY_INSERT [dbo].[TeacherAvails] OFF
GO

SET IDENTITY_INSERT [dbo].[TeacherPreferences] ON
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [perHourRate], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (2, 60, 7.77, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 2, 0)
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [perHourRate], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (2, 60, 7.11, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 2, 0)
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [perHourRate], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (3, 60, 11.00, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 3, 0)
INSERT INTO [dbo].[TeacherPreferences] ([TeacherPreferenceId], [defaultLessonLength], [perHourRate], [distanceType], [maxDistance], [incrementalCost], [TimeBeforeCancellation], [teacherId], [NumberOfProximalLessons]) VALUES (4, 60, 13.00, 0, 5, CAST(0.25 AS Decimal(18, 2)), 24, 4, 0)
SET IDENTITY_INSERT [dbo].[TeacherPreferences] OFF
GO

