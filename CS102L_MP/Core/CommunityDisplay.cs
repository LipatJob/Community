using CS102L_MP.Lib.Concrete;
using CS102L_MP.Lib;
using CS102L_MP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JobLib;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;

namespace CS102L_MP
{
    class CommunityDisplay
    {

        CommunityLogic Logic;

        public CommunityDisplay(CommunityLogic logic)
        {
            this.Logic = logic;
        }

        public void WelcomeScreen()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(
                "Welcome to Community\n" +
                "[1] Login\n" +
                "[2] Register\n" +
                "[X] Exit"
                );
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true, validator: e=>e.In("1", "2", "X"));
                if (selection == "1") { Login(); }
                else if (selection == "2") { Register(); }
                else if (selection == "X") { break; }
            }
            JHelper.ExitPrompt();
        }

        public void Login()
        {
            while (true)
            {
                Console.Clear();
                // Enter Username
                string username = JHelper.InputString("Enter Username: ");
                string password = JHelper.InputPassword("Enter Password: ");

                // Check if correct username and password
                if (Logic.Login(username, password))
                {
                    break;
                }

                // If invalid username and password
                Console.WriteLine("Please Enter A Valid Username and Password");
                JHelper.ContinuePrompt();
            }
            MainMenu();
        }

        public void Register()
        {
            Console.Clear();
            // Enter Username
            string username = JHelper.InputString("Enter Username: ", validator: ValidateNewUsername);
            string password = JHelper.InputPassword("Enter Password: ", validator: ValidateNewPassword);

            Logic.Register(username, password);

            Console.WriteLine("User Registered. You may now log in to your account.");
            JHelper.ContinuePrompt();
        }

        public bool ValidateNewUsername(string e)
        {
            foreach (var letter in e)
            {
                if(!char.IsLetterOrDigit(letter)) 
                {
                    Console.WriteLine("Username may only conatin letters or digits");
                    return false;
                }
            }
            if(e.Length < 5) { Console.WriteLine("> Username must be atleast 5 characters"); return false; }
            if (Logic.ExistingUser(e)) { Console.WriteLine("> Username is taken"); return false; }

            return true;
        }

        public bool ValidateNewPassword(string e)
        {
            if (e.Length < 5) { Console.WriteLine("> password must be atleast 5 characters"); return false; }

            return true;
        }



        public void MainMenu()
        {
            IEnumerable<UserPost> posts = Logic.GetMainFeed();
            EnumerableDisplay<UserPost> display = new EnumerableDisplay<UserPost>(posts, 7, PostDisplay);

            while (true)
            {
                display.items = Logic.GetMainFeed().ToList();
                Console.Clear();

                Title("Home");

                // Display List of Posts
                display.Display();

                // Display and Input Selection
                Console.WriteLine(
                 bar() +
                $"{(display.HasPreviousPage ? "[Q] Previous Page " : "")}" +
                $"{(display.HasNextPage ? "[W] Next page" : "")}\n" +
                "[1] Create Post " +
                "[2] Users " +
                "[3] Communities " +
                "[X] Logout");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true, validator: e => e.In("Q", "W", "1", "2", "3", "X"));

                // Perform selection
                if (display.HasPreviousPage && selection == "Q") { display.PreviousPage();  }
                else if (display.HasNextPage && selection == "W") { display.NextPage();  }
                else if (selection == "1") { CreatePost(); }
                else if (selection == "2") { UsersMenu(); }
                else if (selection == "3") { CommunitiesMenu(); }
                else if (selection == "X") { break; }
            }
            
        }

        private void CreatePost()
        {
            Console.Clear();

            Title("Create Post");

            IList<Community> communities = Logic.GetFolowedCommunities().ToList();

            // Display Communities
            DisplayEnumeratedList(communities, e => e.Name);
            Console.WriteLine("[-1] Go Back");

            Console.Write(bar());
            // Input Community
            int communityNumber = JHelper.InputInt("Enter Number: ", validator: e => e == -1 || (e > 0 && e <= communities.Count));
            if (communityNumber == -1) { return; }
            var community = communities[communityNumber - 1];

            // Input text
            Console.WriteLine();
            Console.WriteLine($"Posting to \"{community.Name}\": ");
            string text = JHelper.InputString("Enter Text: ", validator: e => !string.IsNullOrWhiteSpace(e));
            Logic.PostToCommunity(text, community);
        }

        // USERS MODULE
        public void UsersMenu()
        {
            IEnumerable<UserPost> posts = Logic.GetUserFeed();
            EnumerableDisplay<UserPost> display = new EnumerableDisplay<UserPost>(posts, 7, PostDisplay);
            while (true)
            {
                display.items = Logic.GetUserFeed().ToList();
                Console.Clear();

                Title("Users Feed");

                display.Display();
                Console.WriteLine(
                bar() +
                $"{(display.HasPreviousPage ? "[Q] Previous Page " : "")}" +
                $"{(display.HasNextPage ? "[W] Next Page" : "")}\n" +
                "[1] See Recommended Users " +
                "[2] Search Users " +
                "[3] See Followed Users " +
                "[X] Back");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true, validator: e => e.In("Q", "W", "1", "2", "3", "X"));

                if (display.HasPreviousPage && selection == "Q") { display.PreviousPage(); }
                else if (display.HasNextPage && selection == "W") { display.NextPage(); }
                else if (selection == "1") { SeeRecommendedUsers(); }
                else if (selection == "2") { SearchUsers(); }
                else if (selection == "3") { SeeFollowedUsers(CommunityModel.GetInstance().LoggedinUser); }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
            
        }

        public void SeeRecommendedUsers()
        {
            while (true)
            {
                IList<User> users = Logic.GetRecommendedUsers().ToList();

                Console.Clear();

                Title("Recommended Users");

                // Display Users
                Console.WriteLine("Select Recommended User:");
                DisplayEnumeratedList(users, e => e.Name);
                Console.WriteLine("[-1] Go Back");
                Console.Write(bar());
                
                int userNumber = JHelper.InputInt("Enter Number: ", validator: e => e == -1 || (e > 0 && e <= users.Count));
                if(userNumber == -1) { break; }
                UserProfile(users[userNumber - 1]);
            }
        }

        public void UserProfile(User user)
        {
            IEnumerable<UserPost> posts = user.Posts;
            EnumerableDisplay<UserPost> display = new EnumerableDisplay<UserPost>(posts, 5, PostDisplay);
            while (true)
            {
                Console.Clear();
                IEnumerable<Community> communities = Logic.CommonCommunities(user).Take(5);
                display.items = user.Posts.ToList();

                // Display Profile Description
                Title($"Profile of {user.Name}");

                // Display Common Communities
                Console.WriteLine("Common Communities: " + string.Join(", ", communities.Select(e => e.Name)));

                // Display Posts
                Console.WriteLine("\nPosts: ");
                display.Display();
                // Get Input
                bool followed = Logic.IsFollowed(user);
                Console.WriteLine(
                    bar() +
                   $"{(display.HasPreviousPage ? "[Q] Previous Page " : "")}" +
                   $"{(display.HasNextPage ? "[W] Next Page" : "")}\n" +
                   $"[1]  {(followed ? "Unfollow User" : "Follow User ")} " +
                    "[2] See Followed Communities " +
                    "[3] See Followed Users " +
                    "[X] Back");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true, validator: e => e.In("Q", "W", "1", "2", "3", "X"));
                if (display.HasPreviousPage && selection == "Q") { display.PreviousPage(); }
                else if (display.HasNextPage && selection == "W") { display.NextPage(); }
                else if (selection == "1") { if (followed) { Logic.UnfollowUser(user); } else { Logic.FollowUser(user); }  }
                else if (selection == "2") { SeeFollowedCommunities(user); }
                else if (selection == "3") { SeeFollowedUsers(user); }
                else if (selection == "X") { break; }
            }
        }

        private void SeeFollowedCommunities(User user)
        {
            while (true)
            {
                IList<Community> followedCommunities = user.Communities.Inorder().ToList();
                Console.Clear();

                Title($"Communities Followed by {user.Name}");

                // Dispay List of Communities
                DisplayEnumeratedList(followedCommunities, e => e.Name);

                Console.Write(bar());
                int userNumber = JHelper.InputInt("Enter Number (-1 to go Back): ", validator: e => e == -1 || (e > 0 && e <= followedCommunities.Count));
                if (userNumber == -1) { break; }
                SeeCommunityPosts(followedCommunities[userNumber - 1]);

            }
        }

        public void SearchUsers()
        {
            while (true)
            {
                Console.Clear();

                Title("Search for Users");
                string key = JHelper.InputString("Enter Search Key: ");
                IList<User> users = Logic.SearchUser(key);


                Console.Clear();
                Title($"Search Results for \"{key}\"");

                // Display list of users
                DisplayEnumeratedList(users, e => e.Name);

                // Input Selection
                Console.WriteLine(
                bar()+
                "[1] View User Profile " +
                "[X] Back");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true ,validator: e=>e.In("1", "X"));

                // Perform Action
                if (selection == "1")
                {
                    int userNumber = JHelper.InputInt("Enter Number (-1 to go Back): ", validator: e => e == -1 || (e > 0 && e <= users.Count));
                    if (userNumber == - 1) { continue; }
                    UserProfile(users[userNumber - 1]);
                }
                else if (selection == "X") { break; }
            }

        }

        public void SeeFollowedUsers(User user)
        {
            IList<User> followedUsers = user.FollowedUsers.ToList();
            while (true)
            {
                Console.Clear();

                Title($"Users Followed by {user.Name}");

                // Dispay List of Users
                DisplayEnumeratedList(followedUsers, e => e.Name);
                Console.Write(bar());
                // Input Selection
                int userNumber = JHelper.InputInt("Enter Number (-1 to go Back): ", validator: e => e == -1 || (e > 0 && e <= followedUsers.Count));
                if(userNumber == -1) { break; }
                UserProfile(followedUsers[userNumber - 1]);
            }
        }



        // COMMUNITIES MODULE
        void CommunitiesMenu()
        {
            IEnumerable<UserPost> posts = Logic.GetCommunityFeed();
            EnumerableDisplay<UserPost> display = new EnumerableDisplay<UserPost>(posts, 5, PostDisplay);
            while (true)
            {
                Console.Clear();
                display.items = Logic.GetCommunityFeed().ToList();


                Title("Communities Feed");

                display.Display();
                Console.WriteLine(
                bar()+
                $"{(display.HasPreviousPage ? "[Q] Previous Page " : "")}" +
                $"{(display.HasNextPage ? "[W] Next Page \n" : "")}" +
                "[1] See Communities " +
                "[2] See Followed Communities " +
                "[X] Back");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true);

                if (display.HasPreviousPage && selection == "Q") { display.PreviousPage(); }
                else if (display.HasNextPage && selection == "W") { display.NextPage(); }
                else if (selection == "1") { SeeCommunities(); }
                else if (selection == "2") { SeeFollowedCommunities(); }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
        }

        public void SeeCommunities()
        {
            while (true)
            {
                var communities = Logic.GetCommunities().ToList();
                Console.Clear();

                Title("Communities");

                DisplayEnumeratedList(communities, e => e.Name);
                Console.Write(bar());

                int communityNumber = JHelper.InputInt("Enter Number (-1 to go Back): ", validator: e => e == -1 || (e > 0 && e <= communities.Count));
                if (communityNumber == -1) { break; }
                SeeCommunityPosts(communities[communityNumber - 1]);
            }
        }

        public void SeeCommunityPosts(Community community)  
        {
            var display = new EnumerableDisplay<UserPost>(community.Posts, 7, PostDisplay);
            while (true)
            {
                display.items = community.Posts.ToList();
                Console.Clear();

                Title($"{community.Name} Posts");

                display.Display();

                bool following = Logic.IsFollwingCommunity(community);
                Console.WriteLine(
                bar() +
                $"{(display.HasPreviousPage ? "[Q] Previous Page " : "")}" +
                $"{(display.HasNextPage ? "[W] Next page" : "")}\n" +
                $"[1] {(following ? "Unfollow" : "Follow")} " +
                $"[2] Post " +
                 "[X] Back");
                
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true, validator: e=>e.In("1", "2" ,"X"));
                if (selection == "1") 
                {
                    if (following)
                    {
                        Logic.UnfollowCommunity(community);
                    }
                    else
                    {
                        Logic.FollowCommunity(community);
                    }
                }
                else if (selection == "2") { PostToCommunity(community);  }
                else if (selection == "X") { break; }
            }
        }

        private void PostToCommunity(Community community)
        {
            Title($"Post to {community.Name}");
            Console.Clear();
            // Input text
            string text = JHelper.InputString("Enter Text: ", validator: e => !string.IsNullOrWhiteSpace(e));
            Logic.PostToCommunity(text, community);
        }

        public void SeeFollowedCommunities()
        {
            IList<Community> followedCommunities = Logic.FollowedCommunities().ToList();
            while (true)
            {
                Console.Clear();

                Title($"Followed Communities");

                // Dispay List of Communities
                DisplayEnumeratedList(followedCommunities, e => e.Name);
                Console.WriteLine("[-1] Go Back");
                Console.Write(bar());
                // Input Selection

                int userNumber = JHelper.InputInt("Enter Number: ", validator: e => e == -1 || (e > 0 && e <= followedCommunities.Count));
                if (userNumber == -1) { break; }
                SeeCommunityPosts(followedCommunities[userNumber - 1]);

            }
        }

        // Helpers
        private void DisplayEnumeratedList<T>(IEnumerable<T> values, Func<T, string> display)
        {
            int i = 1;
            foreach (var item in values)
            {
                Console.WriteLine($"[{i}] {display(item)}");
                i++;
            }
        }

        private string bar()
        {
            return "-------------------------------------------------------------------------------------------------------------------\n";
        }

        public void PostDisplay(UserPost post)
        {
            var diff = DateTime.Now - post.DatePosted;

            string diffDate = $"{(diff.Days > 0 ? diff.Days + " days ": "")}{diff.Hours} hours ago";

            Console.WriteLine(
                $"{post.User.Name} on {post.Community.Name}:\n" +
                $"{post.Text} ({diffDate}) \n");
        }

        private void Title(string text)
        {
            Console.Write(
                bar() +
                $"Community: {text}\n" +
                bar());
        }
    }
}
