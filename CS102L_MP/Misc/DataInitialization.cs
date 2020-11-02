using CS102L_MP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Misc
{
    class DataInitialization
    {
        public void Run()
        {
            var model = CommunityModel.GetInstance();

            // Create Communitties
            #region
            var announcements = new Community() { Id = 0, Name = "announcements" };
            var funny = new Community() { Id = 1, Name = "funny" };
            var askcommunity = new Community() { Id = 2, Name = "askcommunity" };
            var gaming = new Community() { Id = 3, Name = "gaming" };
            var aww = new Community() { Id = 4, Name = "aww" };
            var music = new Community() { Id = 5, Name = "music" };
            var science = new Community() { Id = 6, Name = "science" };
            var worldnews = new Community() { Id = 7, Name = "worldnews" };
            var todayilearned = new Community() { Id = 8, Name = "todayilearned" };
            var movies = new Community() { Id = 9, Name = "movies" };
            var tech = new Community() { Id = 10, Name = "tech" };
            var showerthoughts = new Community() { Id = 11, Name = "showerthoughts" };
            var jokes = new Community() { Id = 12, Name = "jokes" };
            var books = new Community() { Id = 13, Name = "books" };
            var mildlyinteresting = new Community() { Id = 14, Name = "mildlyinteresting" };
            var sports = new Community() { Id = 15, Name = "sports" };
            #endregion

            // Add Communities
            #region
            model.Communities.Insert(announcements);
            model.Communities.Insert(funny);
            model.Communities.Insert(askcommunity);
            model.Communities.Insert(gaming);
            model.Communities.Insert(aww);
            model.Communities.Insert(music);
            model.Communities.Insert(science);
            model.Communities.Insert(worldnews);
            model.Communities.Insert(todayilearned);
            model.Communities.Insert(movies);
            model.Communities.Insert(tech);
            model.Communities.Insert(showerthoughts);
            model.Communities.Insert(jokes);
            model.Communities.Insert(books);
            model.Communities.Insert(mildlyinteresting);
            model.Communities.Insert(sports);
            #endregion

            // Create Users
            #region
            var lepegen = new User() { Id = 0, Name = "lepegen", Password = "qwerty"};
            var citylightsbird = new User() { Id = 1, Name = "citylightsbird", Password = "qwerty" };
            var cucumberapple = new User() { Id = 2, Name = "cucumberapple", Password = "qwerty" };
            var pathsofgloryfog = new User() { Id = 3, Name = "pathsofgloryfog", Password = "qwerty" };
            var animaltracksnet = new User() { Id = 4, Name = "animaltracksnet", Password = "qwerty" };
            var lastradawalker = new User() { Id = 5, Name = "lastradawalker", Password = "qwerty" };
            var runningstardust = new User() { Id = 6, Name = "runningstardust", Password = "qwerty" };
            var broccolipotato = new User() { Id = 7, Name = "broccolipotato", Password = "qwerty" };
            var bridgebaseball = new User() { Id = 8, Name = "bridgebaseball", Password = "qwerty" };
            var spiralshapefig = new User() { Id = 9, Name = "spiralshapefig", Password = "qwerty" };
            var marsexpresscane = new User() { Id = 10, Name = "marsexpresscane", Password = "qwerty" };
            var walruspandabird = new User() { Id = 11, Name = "walruspandabird", Password = "qwerty" };
            var rearwindowowl = new User() { Id = 12, Name = "rearwindowowl", Password = "qwerty" };
            var owlsilverberry = new User() { Id = 13, Name = "owlsilverberry", Password = "qwerty" };
            var yogapianosalt = new User() { Id = 14, Name = "yogapianosalt", Password = "qwerty" };
            #endregion

            // Add Communities to Users
            #region
            foreach (var item in new[] { askcommunity, funny, announcements, showerthoughts, tech, todayilearned })
            { lepegen.Communities.Insert(item); }
            foreach (var item in new[] { books, worldnews, todayilearned, gaming }) 
            { citylightsbird.Communities.Insert(item); }
            foreach (var item in new[] { tech, mildlyinteresting, announcements, worldnews, todayilearned }) 
            { cucumberapple.Communities.Insert(item); }
            foreach (var item in new[] { movies, showerthoughts, aww, science, music, sports, todayilearned, announcements, gaming, askcommunity, funny }) 
            { pathsofgloryfog.Communities.Insert(item); }
            foreach (var item in new[] { movies, announcements, books, askcommunity, science }) 
            { animaltracksnet.Communities.Insert(item); }
            foreach (var item in new[] { funny, music, askcommunity, announcements, sports, gaming, aww }) 
            { lastradawalker.Communities.Insert(item); }
            foreach (var item in new[] { books, science, worldnews, movies, music, askcommunity }) 
            { runningstardust.Communities.Insert(item); }
            foreach (var item in new[] { movies, sports, tech, gaming, showerthoughts }) 
            { broccolipotato.Communities.Insert(item); }
            foreach (var item in new[] { movies, books, aww, gaming, science, announcements, tech, askcommunity, jokes }) 
            { bridgebaseball.Communities.Insert(item); }
            foreach (var item in new[] { announcements, gaming, mildlyinteresting, sports, jokes, askcommunity, science, showerthoughts, music, aww, funny, worldnews, todayilearned, books, movies }) 
            { spiralshapefig.Communities.Insert(item); }
            foreach (var item in new[] { mildlyinteresting, gaming, tech, books, funny, jokes, movies, worldnews }) 
            { marsexpresscane.Communities.Insert(item); }
            foreach (var item in new[] { todayilearned, books, showerthoughts, jokes, music, gaming, sports, movies }) 
            { walruspandabird.Communities.Insert(item); }
            foreach (var item in new[] { todayilearned, science, tech, music, gaming, sports, movies, showerthoughts }) 
            { rearwindowowl.Communities.Insert(item); }
            foreach (var item in new[] { music, worldnews, announcements, funny, showerthoughts, books, askcommunity, todayilearned }) 
            { owlsilverberry.Communities.Insert(item); }
            foreach (var item in new[] { movies, funny, books, jokes, mildlyinteresting, music, todayilearned, aww, science, sports, worldnews, askcommunity, showerthoughts }) 
            { yogapianosalt.Communities.Insert(item); }
            #endregion

            // Add Users
            #region
            model.Users.AddVertex(lepegen); // X
            model.Users.AddVertex(citylightsbird); // X
            model.Users.AddVertex(cucumberapple); // X
            model.Users.AddVertex(pathsofgloryfog); // X
            model.Users.AddVertex(animaltracksnet); // X
            model.Users.AddVertex(lastradawalker); // X
            model.Users.AddVertex(runningstardust); // X
            model.Users.AddVertex(broccolipotato); // X
            model.Users.AddVertex(bridgebaseball); // X
            model.Users.AddVertex(spiralshapefig); // X
            model.Users.AddVertex(marsexpresscane); // X
            model.Users.AddVertex(walruspandabird); // X
            model.Users.AddVertex(rearwindowowl); // X
            model.Users.AddVertex(owlsilverberry); // X
            model.Users.AddVertex(yogapianosalt);
            #endregion

            // Follow Other Users
            #region
            model.Users.AddEdge(lepegen, citylightsbird, 1);
            model.Users.AddEdge(lepegen, cucumberapple, 1);
            model.Users.AddEdge(lepegen, pathsofgloryfog, 1);
            model.Users.AddEdge(lepegen, animaltracksnet, 1);
            model.Users.AddEdge(lepegen, lastradawalker, 1);



            model.Users.AddEdge(citylightsbird, walruspandabird, 1);

            model.Users.AddEdge(cucumberapple, yogapianosalt, 1);

            model.Users.AddEdge(pathsofgloryfog, bridgebaseball, 1);
            model.Users.AddEdge(pathsofgloryfog, owlsilverberry, 1);
            model.Users.AddEdge(pathsofgloryfog, broccolipotato, 1);

            model.Users.AddEdge(animaltracksnet, spiralshapefig, 1);
            model.Users.AddEdge(animaltracksnet, owlsilverberry, 1);

            model.Users.AddEdge(bridgebaseball, broccolipotato, 1);
            model.Users.AddEdge(bridgebaseball, marsexpresscane, 1);

            model.Users.AddEdge(spiralshapefig, lepegen, 1);

            model.Users.AddEdge(marsexpresscane, lepegen, 1);
            model.Users.AddEdge(marsexpresscane, bridgebaseball, 1);
            model.Users.AddEdge(marsexpresscane, spiralshapefig, 1);
            model.Users.AddEdge(marsexpresscane, cucumberapple, 1);
            model.Users.AddEdge(marsexpresscane, pathsofgloryfog, 1);
            model.Users.AddEdge(marsexpresscane, citylightsbird, 1);
            model.Users.AddEdge(marsexpresscane, rearwindowowl, 1);

            model.Users.AddEdge(owlsilverberry, rearwindowowl, 1);
            model.Users.AddEdge(owlsilverberry, lastradawalker, 1);
            model.Users.AddEdge(owlsilverberry, lepegen, 1);

            model.Users.AddEdge(yogapianosalt, marsexpresscane, 1);
            #endregion

            // Add Posts to Users
            #region
            var post1 = new UserPost() { ID = 0, DatePosted = DateTime.Parse("9/11/2020"), Text = "Welcome to Community", Community = announcements, User = lepegen };
            lepegen.Posts.Push(post1); announcements.Posts.Push(post1);

            var post2 = new UserPost() { ID = 1, DatePosted = DateTime.Parse("9/12/2020"), Text = "E", Community = funny, User = citylightsbird };
            citylightsbird.Posts.Push(post2); funny.Posts.Push(post2);
            
            var post3 = new UserPost() { ID = 2, DatePosted = DateTime.Parse("9/13/2020"), Text = "What's", Community = askcommunity, User = cucumberapple };
            cucumberapple.Posts.Push(post3); askcommunity.Posts.Push(post3);
            
            var post4 = new UserPost() { ID = 3, DatePosted = DateTime.Parse("9/14/2020"), Text = "Xbox Series X Unboxing", Community = gaming, User = pathsofgloryfog };
            pathsofgloryfog.Posts.Push(post4); gaming.Posts.Push(post4);
            
            var post5 = new UserPost() { ID = 4, DatePosted = DateTime.Parse("9/15/2020"), Text = "It's my dog's 3rd birthday today :D", Community = aww, User = runningstardust };
            runningstardust.Posts.Push(post5); aww.Posts.Push(post5);
            
            var post6 = new UserPost() { ID = 5, DatePosted = DateTime.Parse("9/15/2020"), Text = "The Beatles - Here Comes the Sun", Community = music, User = lastradawalker };
            lastradawalker.Posts.Push(post6); music.Posts.Push(post6);
            
            var post7 = new UserPost() { ID = 6, DatePosted = DateTime.Parse("9/16/2020"), Text = "Water discoverd on the moon", Community = science, User = runningstardust };
            runningstardust.Posts.Push(post7); science.Posts.Push(post7);
            
            var post8 = new UserPost() { ID = 7, DatePosted = DateTime.Parse("9/17/2020"), Text = "Typhoon hit philippines devasted", Community = worldnews, User = broccolipotato };
            broccolipotato.Posts.Push(post8); worldnews.Posts.Push(post8);
            
            var post9 = new UserPost() { ID = 8, DatePosted = DateTime.Parse("9/18/2020"), Text = "TIL Socrates taught Plato, Plato taught Aristotle, and Aristotle taught Alexander the Great", Community = todayilearned, User = bridgebaseball };
            bridgebaseball.Posts.Push(post9); todayilearned.Posts.Push(post9);
            
            var post10 = new UserPost() { ID = 9, DatePosted = DateTime.Parse("9/19/2020"), Text = "Hot Fuzz(2007)", Community = movies, User = spiralshapefig };
            spiralshapefig.Posts.Push(post10); movies.Posts.Push(post10);
            
            var post11 = new UserPost() { ID = 10, DatePosted = DateTime.Parse("10/11/2020"), Text = "IPhone X Released", Community = tech, User = marsexpresscane };
            marsexpresscane.Posts.Push(post11); tech.Posts.Push(post11);
            
            var post12 = new UserPost() { ID = 11, DatePosted = DateTime.Parse("10/13/2020"), Text = "You have proberly seen more of the moons surface than you have earth's", Community = showerthoughts, User = walruspandabird };
            walruspandabird.Posts.Push(post12); showerthoughts.Posts.Push(post12);
            
            var post13 = new UserPost() { ID = 12, DatePosted = DateTime.Parse("10/15/2020"), Text = "I taught a wolf to meditate Now he’s aware wolf", Community = jokes, User = rearwindowowl };
            rearwindowowl.Posts.Push(post13); jokes.Posts.Push(post13);
            
            var post14 = new UserPost() { ID = 13, DatePosted = DateTime.Parse("10/16/2020"), Text = "1984 George Orwell", Community = books, User = owlsilverberry };
            owlsilverberry.Posts.Push(post14); books.Posts.Push(post14);
            
            var post15 = new UserPost() { ID = 14, DatePosted = DateTime.Parse("10/16/2020"), Text = "The A key on my keyboard is half A and half Q.", Community = mildlyinteresting, User = yogapianosalt };
            yogapianosalt.Posts.Push(post15); mildlyinteresting.Posts.Push(post15);
            
            var post16 = new UserPost() { ID = 15, DatePosted = DateTime.Parse("10/17/2020"), Text = "Barca - Alaves", Community = sports, User = lepegen };
            lepegen.Posts.Push(post16); sports.Posts.Push(post16);
            
            var post17 = new UserPost() { ID = 16, DatePosted = DateTime.Parse("10/17/2020"), Text = "v1.1 Now Up :D", Community = announcements, User = citylightsbird };
            citylightsbird.Posts.Push(post17); announcements.Posts.Push(post17);
            
            var post18 = new UserPost() { ID = 17, DatePosted = DateTime.Parse("10/18/2020"), Text = "I ran 3 miles yesterday Eventually I just said “here keep your purse”", Community = funny, User = cucumberapple };
            cucumberapple.Posts.Push(post18); funny.Posts.Push(post18);
            
            var post19 = new UserPost() { ID = 18, DatePosted = DateTime.Parse("10/18/2020"), Text = "How is everybody doing?", Community = askcommunity, User = animaltracksnet };
            animaltracksnet.Posts.Push(post19); askcommunity.Posts.Push(post19);
            
            var post20 = new UserPost() { ID = 19, DatePosted = DateTime.Parse("10/19/2020"), Text = "Watch Dogs: Legion now released", Community = gaming, User = rearwindowowl };
            rearwindowowl.Posts.Push(post20); gaming.Posts.Push(post20);
            #endregion

        }
    }
}
