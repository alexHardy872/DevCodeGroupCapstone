			//SEED USERS

			var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("Test!123");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "test1@noreply.com",
                    PasswordHash = password,
                    Email = "test1@noreply.com"
                },
                new ApplicationUser
                {
                    UserName = "test2@noreply.com",
                    PasswordHash = password,
                    Email = "test2@noreply.com"
                },
                new ApplicationUser
                {
                    UserName = "test3@noreply.com",
                    PasswordHash = password,
                    Email = "test3@noreply.com"
                },
                new ApplicationUser
                {
                    UserName = "test4@noreply.com",
                    PasswordHash = password,
                    Email = "test4@noreply.com"
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
                    LocationId = trevorLocation.LocationId
                },
                new Person
                {
                    firstName = "Gabe",
                    lastName = "Kunkel",
                    subjects = "javascript",
                    phoneNumber = "4145079038",
                    LocationId = gabeLocation.LocationId
                },
                new Person
                {
                    firstName = "Alex",
                    lastName = "Hardy",
                    subjects = "bass",
                    phoneNumber = "7472068258",
                    LocationId = alexLocation.LocationId
                },
                new Person
                {
                    firstName = "Adam",
                    lastName = "Neujahr",
                    subjects = "guitar",
                    phoneNumber = "3093176370",
                    LocationId = adamLocation.LocationId
                }
                );


            //SEED LESSONS
            var trevor = context.People.FirstOrDefault(p => p.firstName == "Trevor");
            var gabe = context.People.FirstOrDefault(p => p.firstName == "Gabe");
            var alex = context.People.FirstOrDefault(p => p.firstName == "Alex");
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
                    cost = 20
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
                    cost = 25
                },
                new Lesson
                {
                    subject = "bass",
                    Length = 60,
                    Price = 30,
                    teacherId = alex.PersonId,
                    studentId = trevor.PersonId,
                    LessonType = "In-Home",
                    LocationId = trevor.LocationId,
                    cost = 30
                }
                );