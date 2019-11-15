			//SEED USERS

            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("Test!123");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "test1@noreply.com",
                    PasswordHash = password,
                    Email = "test1@noreply.com",
                    SecurityStamp = "00ac752f-7a46-48c1-a7fc-bdca784dea5d"
                },
                new ApplicationUser
                {
                    UserName = "test2@noreply.com",
                    PasswordHash = password,
                    Email = "test2@noreply.com",
                    SecurityStamp = "4a448de1-b350-4321-a9d0-a45ca1a9cbf2"
                },
                new ApplicationUser
                {
                    UserName = "test3@noreply.com",
                    PasswordHash = password,
                    Email = "test3@noreply.com",
                    SecurityStamp = "d98dc094-cda8-4697-8fbe-09f98f6e76de"
                },
                new ApplicationUser
                {
                    UserName = "test4@noreply.com",
                    PasswordHash = password,
                    Email = "test4@noreply.com",
                    SecurityStamp = "3cdafc2a-6d0f-4474-85bc-965c32730a72"
                }
                );


            //SEED LOCATIONS

            context.Locations.AddOrUpdate(
                new Location
                {
                    address1 = "313 N Plankinton Ave",
                    city = "Milwaukee",
                    state = StateList.WI,
                    zip = "53203",
                    lat = "43.034169",
                    lng = "-87.9119333"
                },
                new Location
                {
                    address1 = "5222 W Hassel Ln",
                    city = "Milwaukee",
                    state = StateList.WI,
                    zip = "53223",
                    lat = "43.1383663",
                    lng = "-87.9773432"
                },
                new Location
                {
                    address1 = "9627 W National Ave",
                    city = "West Allis",
                    state = StateList.WI,
                    zip = "53227",
                    lat = "43.0018617",
                    lng = "-88.0324269"
                },
                new Location
                {
                    address1 = "1024 Lakefield Rd",
                    city = "Grafton",
                    state = StateList.WI,
                    zip = "53024",
                    lat = "43.2948857",
                    lng = "-87.9190001"
                }
                );

            //SEED PEOPLE

            var trevor = context.Users.FirstOrDefault(u => u.Email == "test1@noreply.com");
            var gabe = context.Users.FirstOrDefault(u => u.Email == "test2@noreply.com");
            var alex = context.Users.FirstOrDefault(u => u.Email == "test3@noreply.com");
            var adam = context.Users.FirstOrDefault(u => u.Email == "test4@noreply.com");
            var trevorLocation = context.Locations.FirstOrDefault(l => l.address1 == "313 N Plankinton Ave");
            var gabeLocation = context.Locations.FirstOrDefault(l => l.address1 == "5222 W Hassel Ln");
            var alexLocation = context.Locations.FirstOrDefault(l => l.address1 == "9627 W National Ave");
            var adamLocation = context.Locations.FirstOrDefault(l => l.address1 == "1024 Lakefield Rd");
            context.People.AddOrUpdate(
                new Person
                {
                    firstName = "Trevor",
                    lastName = "Clements",
                    subjects = "awesomeness",
                    phoneNumber = "5419138650",
                    LocationId = trevorLocation.LocationId,
                    ApplicationId = trevor.Id
                },
                new Person
                {
                    firstName = "Gabe",
                    lastName = "Kunkel",
                    subjects = "javascript",
                    phoneNumber = "4145079038",
                    LocationId = gabeLocation.LocationId,
                    ApplicationId = gabe.Id
                },
                new Person
                {
                    firstName = "Alex",
                    lastName = "Hardy",
                    subjects = "bass",
                    phoneNumber = "7472068258",
                    LocationId = alexLocation.LocationId,
                    ApplicationId = alex.Id
                },
                new Person
                {
                    firstName = "Adam",
                    lastName = "Neujahr",
                    subjects = "guitar",
                    phoneNumber = "3093176370",
                    LocationId = adamLocation.LocationId,
                    ApplicationId = adam.Id
                }
                );

            //SEED PREFERENCES
            var trevor = context.People.FirstOrDefault(p => p.firstName == "Trevor");
            var gabe = context.People.FirstOrDefault(p => p.firstName == "Gabe");
            var alex = context.People.FirstOrDefault(p => p.firstName == "Alex");
            var adam = context.People.FirstOrDefault(p => p.firstName == "Adam");
            decimal cost = Convert.ToDecimal(0.25);
            context.Preferences.AddOrUpdate(
                new TeacherPreference
                {
                    teacherId = trevor.PersonId,
                    TimeBeforeCancellation = 24,
                    incrementalCost = cost,
                    maxDistance = 5,
                    distanceType = RadiusOptions.Miles,
                    defaultLessonLength = 60
                },
                new TeacherPreference
                {
                    teacherId = gabe.PersonId,
                    TimeBeforeCancellation = 24,
                    incrementalCost = cost,
                    maxDistance = 5,
                    distanceType = RadiusOptions.Miles,
                    defaultLessonLength = 60
                },
                new TeacherPreference
                {
                    teacherId = alex.PersonId,
                    TimeBeforeCancellation = 24,
                    incrementalCost = cost,
                    maxDistance = 5,
                    distanceType = RadiusOptions.Miles,
                    defaultLessonLength = 60
                },
                new TeacherPreference
                {
                    teacherId = adam.PersonId,
                    TimeBeforeCancellation = 24,
                    incrementalCost = cost,
                    maxDistance = 5,
                    distanceType = RadiusOptions.Miles,
                    defaultLessonLength = 60
                }
                );

            //SEED AVAILABILITIES
            var trevor = context.People.FirstOrDefault(p => p.firstName == "Trevor");
            var gabe = context.People.FirstOrDefault(p => p.firstName == "Gabe");
            var alex = context.People.FirstOrDefault(p => p.firstName == "Alex");
            var adam = context.People.FirstOrDefault(p => p.firstName == "Adam");
            DateTime start = new DateTime(2019, 11, 18, 8, 0, 0);
            DateTime end = new DateTime(2019, 11, 18, 17, 0, 0);
            context.TeacherAvailabilities.AddOrUpdate(
                new TeacherAvail
                {
                    PersonId = trevor.PersonId,
                    weekDay = DayOfWeek.Sunday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = trevor.PersonId,
                    weekDay = DayOfWeek.Monday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = trevor.PersonId,
                    weekDay = DayOfWeek.Tuesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = trevor.PersonId,
                    weekDay = DayOfWeek.Wednesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = trevor.PersonId,
                    weekDay = DayOfWeek.Thursday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = trevor.PersonId,
                    weekDay = DayOfWeek.Friday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = trevor.PersonId,
                    weekDay = DayOfWeek.Saturday,
                    start = start,
                    end = end
                },

                new TeacherAvail
                {
                    PersonId = gabe.PersonId,
                    weekDay = DayOfWeek.Sunday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = gabe.PersonId,
                    weekDay = DayOfWeek.Monday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = gabe.PersonId,
                    weekDay = DayOfWeek.Tuesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = gabe.PersonId,
                    weekDay = DayOfWeek.Wednesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = gabe.PersonId,
                    weekDay = DayOfWeek.Thursday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = gabe.PersonId,
                    weekDay = DayOfWeek.Friday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = gabe.PersonId,
                    weekDay = DayOfWeek.Saturday,
                    start = start,
                    end = end
                },

                new TeacherAvail
                {
                    PersonId = alex.PersonId,
                    weekDay = DayOfWeek.Sunday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = alex.PersonId,
                    weekDay = DayOfWeek.Monday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = alex.PersonId,
                    weekDay = DayOfWeek.Tuesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = alex.PersonId,
                    weekDay = DayOfWeek.Wednesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = alex.PersonId,
                    weekDay = DayOfWeek.Thursday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = alex.PersonId,
                    weekDay = DayOfWeek.Friday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = alex.PersonId,
                    weekDay = DayOfWeek.Saturday,
                    start = start,
                    end = end
                },

                new TeacherAvail
                {
                    PersonId = adam.PersonId,
                    weekDay = DayOfWeek.Sunday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = adam.PersonId,
                    weekDay = DayOfWeek.Monday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = adam.PersonId,
                    weekDay = DayOfWeek.Tuesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = adam.PersonId,
                    weekDay = DayOfWeek.Wednesday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = adam.PersonId,
                    weekDay = DayOfWeek.Thursday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = adam.PersonId,
                    weekDay = DayOfWeek.Friday,
                    start = start,
                    end = end
                },
                new TeacherAvail
                {
                    PersonId = adam.PersonId,
                    weekDay = DayOfWeek.Saturday,
                    start = start,
                    end = end
                }

                );

            //SEED LESSONS
            var trevor = context.People.FirstOrDefault(p => p.firstName == "Trevor");
            var gabe = context.People.FirstOrDefault(p => p.firstName == "Gabe");
            var alex = context.People.FirstOrDefault(p => p.firstName == "Alex");
            DateTime startTime = new DateTime(2019, 11, 18, 13, 0, 0);
            DateTime endTime = new DateTime(2019, 11, 18, 14, 0, 0);
            context.Lessons.AddOrUpdate(
                new Lesson
                {
                    subject = "awesomeness",
                    Length = 60,
                    Price = 20,
                    teacherId = trevor.PersonId,
                    studentId = gabe.PersonId,
                    LessonType = "In-Studio",
                    LocationId = trevor.LocationId,
                    cost = 20,
                    start = startTime,
                    end = endTime

                },
                new Lesson
                {
                    subject = "javascript",
                    Length = 60,
                    Price = 25,
                    teacherId = gabe.PersonId,
                    studentId = alex.PersonId,
                    LessonType = "Online",
                    LocationId = gabe.LocationId,
                    cost = 25,
                    start = startTime,
                    end = endTime
                },
                new Lesson
                {
                    subject = "bass",
                    Length = 60,
                    Price = 30,
                    teacherId = alex.PersonId,
                    studentId = trevor.PersonId,
                    LessonType = "In-Studio",
                    LocationId = trevor.LocationId,
                    cost = 30,
                    start = startTime,
                    end = endTime
                }
                );