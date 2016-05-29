using Answer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using System.Web.Security;

namespace Answer
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            // GetCategories().ForEach(c => context.Categories.Add(c));
            const string adminRole = "Administrator";
            const string adminName = "Admin";
            const string adminPass = "password";
            if (!Roles.RoleExists(adminRole))
            {
                Roles.CreateRole(adminRole);
            }
            if (!WebSecurity.UserExists(adminName))
            {
                WebSecurity.CreateUserAndAccount(adminName, adminPass, new
                {
                    Password = adminPass,
                    FirstName = "Samer",
                    LastName = "Naoura",
                    Country = "Syria",
                    City = "Damascus",
                    Email = "samer_n91@hotmail.com",
                    BirthDate = DateTime.Parse("1/1/1991"),
                    CreationDate = DateTime.Now,
                    Balance = 1000,
                    Salary = 2
                }
                 );
                Roles.AddUserToRole(adminName, adminRole);
            }
        }

    }

    public class DBContext : DbContext
    {
        public DBContext()
            : base("DefaultConnection")
        {
        }
        static DBContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DBContext>());
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<MajorModels> Majors { get; set; }
        public DbSet<QuestionModels> Questions { get; set; }
        public DbSet<AnswerModels> Answers { get; set; }
        public DbSet<RateModels> Rates { get; set; }


        public UserProfile GetUser(string un)
        {
            foreach (var item in UserProfiles)
                if (un == item.UserName)
                    return item;
            return null;
        }

        public void DeleteUser(UserProfile user)
        {
            while (user.Questions.Count > 0)
                DeleteQuestion(user.Questions[0]);

            while (user.RefferdQuestions.Count > 0)
                DeleteQuestion(user.RefferdQuestions[0]);

            UserProfiles.Remove(user);
        }

        public void DeleteQuestion(QuestionModels question)
        {
            UserProfile user = question.User;

            while (question.Answers.Count > 0)
                DeleteAnswer(question.Answers[0]);
            question.Answers.Clear();

            if (question.ReferredUser != null)
                question.ReferredUser.RefferdQuestions.Remove(question);
            if (user != null)
                user.Questions.Remove(question);
            if (question != null)
                Questions.Remove(question);
        }

        public void DeleteAnswer(AnswerModels answer)
        {
            UserProfile user = answer.User;
            QuestionModels question = answer.Question;

            if (user != null)
                user.Answers.Remove(answer);
            if (question != null)
                question.Answers.Remove(answer);
            Answers.Remove(answer);
        }

        public void DeleteMajor(MajorModels major)
        {
            var users = UserProfiles.Where(u => u.Major == major).ToList();
            var questions = Questions.Where(q => q.Major == major).ToList();
            foreach (var u in users)
            {
                DeleteUser(u);
            }
            foreach (var q in questions)
            {
                DeleteQuestion(q);
            }
            Majors.Remove(major);
        }

        public void drop()
        {
            foreach (var item in Answers)
            {
                Answers.Remove(item);
            }
            foreach (var item in Questions)
            {
                Questions.Remove(item);
            }
            foreach (var item in UserProfiles)
            {
                UserProfiles.Remove(item);
            }
            foreach (var item in Majors)
            {
                Majors.Remove(item);
            }
            foreach (var item in Rates)
            {
                Rates.Remove(item);
            }


        }

        public void AddingMajor()
        {
            Majors.Add(new MajorModels(47, "Accounting"));
            Majors.Add(new MajorModels(94, "Airlines/Aviation"));
            Majors.Add(new MajorModels(120, "Alternative Dispute Resolution"));
            Majors.Add(new MajorModels(125, "Alternative Medicine"));
            Majors.Add(new MajorModels(127, "Animation"));
            Majors.Add(new MajorModels(19, "Apparel & Fashion"));
            Majors.Add(new MajorModels(50, "Architecture & Planning"));
            Majors.Add(new MajorModels(111, "Arts and Crafts"));
            Majors.Add(new MajorModels(53, "Automotive"));
            Majors.Add(new MajorModels(52, "Aviation & Aerospace"));
            Majors.Add(new MajorModels(41, "Banking"));
            Majors.Add(new MajorModels(12, "Biotechnology"));
            Majors.Add(new MajorModels(36, "Broadcast Media"));
            Majors.Add(new MajorModels(49, "Building Materials"));
            Majors.Add(new MajorModels(138, "Business Supplies and Equipment"));
            Majors.Add(new MajorModels(129, "Capital Markets"));
            Majors.Add(new MajorModels(54, "Chemicals"));
            Majors.Add(new MajorModels(90, "Civic & Social Organization"));
            Majors.Add(new MajorModels(51, "Civil Engineering"));
            Majors.Add(new MajorModels(128, "Commercial Real Estate"));
            Majors.Add(new MajorModels(118, "Computer & Network Security"));
            Majors.Add(new MajorModels(109, "Computer Games"));
            Majors.Add(new MajorModels(3, "Computer Hardware"));
            Majors.Add(new MajorModels(5, "Computer Networking"));
            Majors.Add(new MajorModels(4, "Computer Software"));
            Majors.Add(new MajorModels(48, "Construction"));
            Majors.Add(new MajorModels(24, "Consumer Electronics"));
            Majors.Add(new MajorModels(25, "Consumer Goods"));
            Majors.Add(new MajorModels(91, "Consumer Services"));
            Majors.Add(new MajorModels(18, "Cosmetics"));
            Majors.Add(new MajorModels(65, "Dairy"));
            Majors.Add(new MajorModels(1, "-----"));
            Majors.Add(new MajorModels(2, "Defense & Space"));
            Majors.Add(new MajorModels(99, "Design"));
            Majors.Add(new MajorModels(69, "Education Management"));
            Majors.Add(new MajorModels(132, "E-Learning"));
            Majors.Add(new MajorModels(112, "Electrical/Electronic Manufacturing"));
            Majors.Add(new MajorModels(28, "Entertainment"));
            Majors.Add(new MajorModels(86, "Environmental Services"));
            Majors.Add(new MajorModels(110, "Events Services"));
            Majors.Add(new MajorModels(76, "Executive Office"));
            Majors.Add(new MajorModels(122, "Facilities Services"));
            Majors.Add(new MajorModels(63, "Farming"));
            Majors.Add(new MajorModels(43, "Financial Services"));
            Majors.Add(new MajorModels(38, "Fine Art"));
            Majors.Add(new MajorModels(66, "Fishery"));
            Majors.Add(new MajorModels(34, "Food & Beverages"));
            Majors.Add(new MajorModels(23, "Food Production"));
            Majors.Add(new MajorModels(101, "Fund-Raising"));
            Majors.Add(new MajorModels(26, "Furniture"));
            Majors.Add(new MajorModels(29, "Gambling & Casinos"));
            Majors.Add(new MajorModels(145, "Glass, Ceramics & Concrete"));
            Majors.Add(new MajorModels(75, "Government Administration"));
            Majors.Add(new MajorModels(148, "Government Relations"));
            Majors.Add(new MajorModels(140, "Graphic Design"));
            Majors.Add(new MajorModels(124, "Health, Wellness and Fitness"));
            Majors.Add(new MajorModels(68, "Higher Education"));
            Majors.Add(new MajorModels(14, "Hospital & Health Care"));
            Majors.Add(new MajorModels(31, "Hospitality"));
            Majors.Add(new MajorModels(137, "Human Resources"));
            Majors.Add(new MajorModels(134, "Import and Export"));
            Majors.Add(new MajorModels(88, "Individual & Family Services"));
            Majors.Add(new MajorModels(147, "Industrial Automation"));
            Majors.Add(new MajorModels(84, "Information Services"));
            Majors.Add(new MajorModels(96, "Information Technology and Services"));
            Majors.Add(new MajorModels(42, "Insurance"));
            Majors.Add(new MajorModels(74, "International Affairs"));
            Majors.Add(new MajorModels(141, "International Trade and Development"));
            Majors.Add(new MajorModels(6, "Internet"));
            Majors.Add(new MajorModels(45, "Investment Banking"));
            Majors.Add(new MajorModels(46, "Investment Management"));
            Majors.Add(new MajorModels(73, "Judiciary"));
            Majors.Add(new MajorModels(77, "Law Enforcement"));
            Majors.Add(new MajorModels(9, "Law Practice"));
            Majors.Add(new MajorModels(10, "Legal Services"));
            Majors.Add(new MajorModels(72, "Legislative Office"));
            Majors.Add(new MajorModels(30, "Leisure, Travel & Tourism"));
            Majors.Add(new MajorModels(85, "Libraries"));
            Majors.Add(new MajorModels(116, "Logistics and Supply Chain"));
            Majors.Add(new MajorModels(143, "Luxury Goods & Jewelry"));
            Majors.Add(new MajorModels(55, "Machinery"));
            Majors.Add(new MajorModels(11, "Management Consulting"));
            Majors.Add(new MajorModels(95, "Maritime"));
            Majors.Add(new MajorModels(80, "Marketing and Advertising"));
            Majors.Add(new MajorModels(97, "Market Research"));
            Majors.Add(new MajorModels(135, "Mechanical or Industrial Engineering"));
            Majors.Add(new MajorModels(126, "Media Production"));
            Majors.Add(new MajorModels(17, "Medical Devices"));
            Majors.Add(new MajorModels(13, "Medical Practice"));
            Majors.Add(new MajorModels(139, "Mental Health Care"));
            Majors.Add(new MajorModels(71, "Military"));
            Majors.Add(new MajorModels(56, "Mining & Metals"));
            Majors.Add(new MajorModels(35, "Motion Pictures and Film"));
            Majors.Add(new MajorModels(37, "Museums and Institutions"));
            Majors.Add(new MajorModels(115, "Music"));
            Majors.Add(new MajorModels(114, "Nanotechnology"));
            Majors.Add(new MajorModels(81, "Newspapers"));
            Majors.Add(new MajorModels(100, "Nonprofit Organization Management"));
            Majors.Add(new MajorModels(57, "Oil & Energy"));
            Majors.Add(new MajorModels(113, "Online Media"));
            Majors.Add(new MajorModels(123, "Outsourcing/Offshoring"));
            Majors.Add(new MajorModels(87, "Package/Freight Delivery"));
            Majors.Add(new MajorModels(146, "Packaging and Containers"));
            Majors.Add(new MajorModels(61, "Paper & Forest Products"));
            Majors.Add(new MajorModels(39, "Performing Arts"));
            Majors.Add(new MajorModels(15, "Pharmaceuticals"));
            Majors.Add(new MajorModels(131, "Philanthropy"));
            Majors.Add(new MajorModels(136, "Photography"));
            Majors.Add(new MajorModels(117, "Plastics"));
            Majors.Add(new MajorModels(107, "Political Organization"));
            Majors.Add(new MajorModels(67, "Primary/Secondary Education"));
            Majors.Add(new MajorModels(83, "Printing"));
            Majors.Add(new MajorModels(105, "Professional Training & Coaching"));
            Majors.Add(new MajorModels(102, "Program Development"));
            Majors.Add(new MajorModels(79, "Public Policy"));
            Majors.Add(new MajorModels(98, "Public Relations and Communications"));
            Majors.Add(new MajorModels(78, "Public Safety"));
            Majors.Add(new MajorModels(82, "Publishing"));
            Majors.Add(new MajorModels(62, "Railroad Manufacture"));
            Majors.Add(new MajorModels(64, "Ranching"));
            Majors.Add(new MajorModels(44, "Real Estate"));
            Majors.Add(new MajorModels(40, "Recreational Facilities and Services"));
            Majors.Add(new MajorModels(89, "Religious Institutions"));
            Majors.Add(new MajorModels(144, "Renewables & Environment"));
            Majors.Add(new MajorModels(70, "Research"));
            Majors.Add(new MajorModels(32, "Restaurants"));
            Majors.Add(new MajorModels(27, "Retail"));
            Majors.Add(new MajorModels(121, "Security and Investigations"));
            Majors.Add(new MajorModels(7, "Semiconductors"));
            Majors.Add(new MajorModels(58, "Shipbuilding"));
            Majors.Add(new MajorModels(20, "Sporting Goods"));
            Majors.Add(new MajorModels(33, "Sports"));
            Majors.Add(new MajorModels(104, "Staffing and Recruiting"));
            Majors.Add(new MajorModels(22, "Supermarkets"));
            Majors.Add(new MajorModels(8, "Telecommunications"));
            Majors.Add(new MajorModels(60, "Textiles"));
            Majors.Add(new MajorModels(130, "Think Tanks"));
            Majors.Add(new MajorModels(21, "Tobacco"));
            Majors.Add(new MajorModels(108, "Translation and Localization"));
            Majors.Add(new MajorModels(92, "Transportation/Trucking/Railroad"));
            Majors.Add(new MajorModels(59, "Utilities"));
            Majors.Add(new MajorModels(106, "Venture Capital & Private Equity"));
            Majors.Add(new MajorModels(16, "Veterinary"));
            Majors.Add(new MajorModels(93, "Warehousing"));
            Majors.Add(new MajorModels(133, "Wholesale"));
            Majors.Add(new MajorModels(142, "Wine and Spirits"));
            Majors.Add(new MajorModels(119, "Wireless"));
            Majors.Add(new MajorModels(103, "Writing and Editing"));
        }

        public void AddingUser()
        {

            WebSecurity.CreateUserAndAccount("sam", "asdfgh", new
            {
                Password = "asdfgh",
                FirstName = "Samer",
                LastName = "Naoura",
                Country = "Syria",
                City = "Damascus",
                Email = "samer_n91@hotmail.com",
                Rate = 0,
                BirthDate = DateTime.Parse("1/1/1991"),
                CreationDate = DateTime.Now,
                Balance = 1000,
                Major = Majors.Find(5),
                Salary = 2
            }
            );

            WebSecurity.CreateUserAndAccount("sam0", "asdfgh", new
            {
                Password = "asdfgh",
                FirstName = "Samer",
                LastName = "Naoura",
                Country = "Syria",
                City = "Damascus",
                Email = "samer@hotmail.com",
                Rate = 0,
                BirthDate = DateTime.Parse("1/1/1991"),
                CreationDate = DateTime.Now,
                Balance = 1000,
                Major = Majors.Find(5),
                Salary = 2
            }
            );

            WebSecurity.CreateUserAndAccount("sam1", "asdfgh", new
            {
                Password = "asdfgh",
                FirstName = "Samer",
                LastName = "Naoura",
                Country = "Syria",
                City = "Damascus",
                Email = "samer91@hotmail.com",
                Rate = 0,
                BirthDate = DateTime.Parse("1/1/1991"),
                CreationDate = DateTime.Now,
                Balance = 1000,
                Major = Majors.Find(4),
                Salary = 2
            }
            );

            WebSecurity.CreateUserAndAccount("sam2", "asdfgh", new
            {
                Password = "asdfgh",
                FirstName = "Samer",
                LastName = "Naoura",
                Country = "Syria",
                City = "Damascus",
                Email = "samern91@hotmail.com",
                Rate = 0,
                BirthDate = DateTime.Parse("1/1/1991"),
                CreationDate = DateTime.Now,
                Balance = 1000,
                Major = Majors.Find(4),
                Salary = 2
            }
            );


        }

        public MajorModels GetMajor(string un)
        {
            foreach (var item in Majors)
                if (un == item.MajorType)
                    return item;
            return null;
        }
    }
}