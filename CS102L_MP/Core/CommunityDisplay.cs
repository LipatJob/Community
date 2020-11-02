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

namespace CS102L_MP
{
    class CommunityDisplay
    {

        CommunityLogic Logic;

        public CommunityDisplay(CommunityLogic logic)
        {
            this.Logic = logic;
        }

        public void MainMenu()
        {
            IEnumerable<UserPost> posts = Logic.GetMainFeed();
            EnumerableDisplay<UserPost> display = new EnumerableDisplay<UserPost>(posts, 100, UserPostDisplay);

            while (true)
            {
                Console.Clear();

                // Display List of Posts
                display.Display();

                // Display and Input Selection
                Console.WriteLine(
                 "-----------------------------------------------------\n"+
                $"{(display.HasPreviousPage ? "[Q] Previous Page " : "")}" +
                $"{(display.HasNextPage ? "[W] Next page \n" : "")}" +
                "[1] Users " +
                "[2] Communities " +
                "[X] Logout and Exit");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true, validator: e => e.In("Q", "W", "1", "2", "3", "X"));

                // Perform selection
                if (display.HasPreviousPage && selection == "Q") { display.PreviousPage();  }
                else if (display.HasNextPage && selection == "W") { display.NextPage();  }
                else if (selection == "1") { UsersMenu(); }
                else if (selection == "2") { CommunitiesMenu(); }
                else if (selection == "X") { break; }
            }
            JHelper.ExitPrompt();
        }



        // USERS MODULE
        public void UsersMenu()
        {
            IEnumerable<UserPost> posts = Logic.GetUserFeed();
            EnumerableDisplay<UserPost> display = new EnumerableDisplay<UserPost>(posts, 10, UserPostDisplay);
            while (true)
            {
                Console.Clear();
                display.Display();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine(
                $"{(display.HasPreviousPage ? "[Q] Previous Page " : "")}" +
                $"{(display.HasNextPage ? "[W] Next Page \n" : "")}" +
                "[1] See Recommended Users " +
                "[2] Search Users " +
                "[3] See Followed Users " +
                "[X] Back");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true, validator: e => e.In("Q", "W", "1", "2", "3", "X"));

                if (display.HasPreviousPage && selection == "Q") { display.PreviousPage(); }
                else if (display.HasNextPage && selection == "W") { display.NextPage(); }
                else if (selection == "1") { SeeRecommendedUsers(); }
                else if (selection == "2") { SearchUsers(); }
                else if (selection == "3") { SeeFollowedUsers(CommunityModel.GetInstace().LoggedinUser); }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
            
        }

        public string UserPostDisplay(UserPost post)
        {
            Console.WriteLine(
                $"{post.User.Name} posted to {post.Community.Name} on {post.DatePosted.ToShortDateString()} \n" +
                $"{post.Text}\n");
            return "";
        }

        public void SeeRecommendedUsers()
        {
            IList<User> users = Logic.GetRecommendedUsers().ToList();
            while (true)
            {
                Console.Clear();

                int i = 1;
                foreach (var user in users)
                {
                    Console.WriteLine($"{i}. {user.Name}");
                    i++;
                }

                Console.WriteLine(
                "[1] View User Profile" +
                "[X] Back");
                string selection = JHelper.InputString("Enter Selection (-1 to go Back): ", validator: e => e.In("1", "X"));
                if (selection == "1")
                {
                    int userNumber = JHelper.InputInt("Enter Number: ", validator: e => e > 0 && e <= users.Count);
                    if(userNumber != -1)
                    {
                        UserProfile(users[userNumber - 1]);
                    }
                }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
        }

        public void UserProfile(User user)
        {
            IEnumerable<Community> communities = Logic.CommonCommunities(user).Take(5);
            while (true)
            {
                Console.Clear();
                // Display Common Communities
                Console.WriteLine("Common Communities: " + string.Join(", ", communities.Select(e => e.Name)));

                Console.WriteLine("\nPosts: ");
                // Display Posts
                foreach (var post in user.Posts)
                {
                    Console.WriteLine(
                        $"{post.User.Name}/{post.Community.Name}/{post.DatePosted.ToShortDateString()}\n" +
                        $"{post.Text}");
                }

                bool followed = false;
                Console.WriteLine("-----------------------------------------------------");

                if (followed)
                {
                    Console.WriteLine(
                    "[1] Unfollow User " +
                    "[2] See Followed Communities " +
                    "[3] See Followed Users " +
                    "[X] Back");
                    string selection = JHelper.InputString("Enter Selection: ");
                    if (selection == "1") { Logic.UnfollowUser(user); }
                    if (selection == "2") { SeeFollowedCommunities(user); }
                    if (selection == "3") { SeeFollowedUsers(user); }
                    else if (selection == "X") { break; }
                }
                else
                {
                    Console.WriteLine(
                    "[1] Follow User " +
                    "[2] See Followed Communities" +
                    "[3] See Followed Users " +
                    "[X] Back");
                    string selection = JHelper.InputString("Enter Selection: ");
                    if (selection == "1") { }
                    else if (selection == "X") { break; }
                }
            
               
            }
        }

        private void SeeFollowedCommunities(User user)
        {
            while (true)
            {
                IList<Community> followedCommunities = user.Communities.Inorder().ToList();

                // Dispay List of Communities
                int i = 1;
                foreach (var followedCommunity in followedCommunities)
                {
                    Console.WriteLine($"{i}. {followedCommunity.Name}");
                    i++;
                }

                // Input Selection
                Console.WriteLine(
                "[1] Select and View Community " +
                "[X] Back");
                string selection = JHelper.InputString("Enter Selection (-1 to go Back): ");

                // Perform Action
                if (selection == "1")
                {
                    int userNumber = JHelper.InputInt("Enter Number: ", validator: e => e > 0 && e <= followedCommunities.Count);
                    if (userNumber != -1)
                    {
                        SeeCommunityPosts(followedCommunities[userNumber - 1]);
                    }
                }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
        }

        public void SearchUsers()
        {
            string key = JHelper.InputString("Enter Search Key: ");
            while (true)
            {
                IList<User> users = null;
                
                // Display list of users
                int i = 0;
                foreach (var user in users)
                {
                    Console.WriteLine($"{i}. {user.Name}");
                    i++;
                }

                // Input Selection
                Console.WriteLine(
                "1. View User Profile\n" +
                "X. Back");
                string selection = JHelper.InputString("Enter Selection (-1 to go Back): ");

                // Perform Action
                if (selection == "1")
                {
                    int userNumber = JHelper.InputInt("Enter Number: ", validator: e => e > 0 && e <= users.Count);
                    if (userNumber != -1)
                    {
                        UserProfile(users[userNumber - 1]);
                    }
                }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }

        }

        public void SeeFollowedUsers(User user)
        {
            while(true)
            {
                IList<User> followedUsers = null;

                // Dispay List of Users
                int i = 0;
                foreach (var followedUser in followedUsers)
                {
                    Console.WriteLine($"{i}. {followedUser.Name}");
                    i++;
                }

                // Input Selection
                Console.WriteLine(
                "1. View User Profile\n" +
                "X. Back");
                string selection = JHelper.InputString("Enter Selection (-1 to go Back): ");

                // Perform Action
                if (selection == "1")
                {
                    int userNumber = JHelper.InputInt("Enter Number: ", validator: e => e > 0 && e <= followedUsers.Count);
                    if (userNumber != -1)
                    {
                        UserProfile(followedUsers[userNumber - 1]);
                    }
                }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
        }



        // COMMUNITIES MODULE
        void CommunitiesMenu()
        {
            while (true)
            {
                Console.Clear();
                SeeCommunitiesFeed();
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine(
                "[Q] Scroll Up " +
                "[W] Scroll Down\n" +
                "[1] See Communities " +
                "[2] See Followed Communities " +
                "[X] Back");
                string selection = JHelper.InputString("Enter Selection: ", toUpper: true);

                if (selection == "1") { }
                if (selection == "2") { }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
        }

        public void SeeCommunitiesFeed()
        {
            IEnumerable<UserPost> posts = Logic.GetCommunityFeed(); ;
            foreach (var post in posts)
            {
                Console.WriteLine(
                    $"{post.User.Name}/{post.Community.Name}/{post.DatePosted.ToShortDateString()}" +
                    $"{post.Text}");
            }
        }

        public void SeeCommunities()
        {
            while (true)
            {
                int i = 0;
                IList<Community> communities = null;
                foreach (var community in communities)
                {
                    Console.WriteLine($"{i} {community.Name}");
                }
                Console.WriteLine("[1] Select Community [X] Back");
            }
        }

        public void SeeCommunityPosts(Community community)
        {
            while (true)
            {
                foreach (var post in community.Posts)
                {
                    Console.WriteLine(
                    $"{post.User.Name}/{post.Community.Name}/{post.DatePosted.ToShortDateString()}" +
                    $"{post.Text}");
                }

                bool following = false;
                string followSelection = following ? "Unfollow" : "Follow";
                Console.WriteLine(

                $"{community.Name}:\n" +
                $"[Q] Scroll Up " +
                $"[W] Scroll Down\n" +
                $"[1] {followSelection} " +
                 "[X] Back");
                
                string selection = JHelper.InputString("Enter Selection: ");
                if (selection == "1") { }
                else if (selection == "X") { break; }
                else { Console.WriteLine("Please Enter a Valid Selection."); }
            }
        }
    }
}
