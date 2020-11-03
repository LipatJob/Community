using CS102L_MP.Lib;
using CS102L_MP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Core
{
    class CommunityModelSerializer
    {
        string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        CommunityModel model;
        HashMap<string, User> Users;
        HashMap<string, Community> Communities;
        HashMap<int, UserPost> Posts;


        public CommunityModelSerializer()
        {
            Directory.CreateDirectory(dir+"/data");
            Directory.CreateDirectory(dir + "/data/communityposts");
            Directory.CreateDirectory(dir + "/data/userposts");

            model = CommunityModel.GetInstance();
            Users = new HashMap<string, User>();
            Communities = new HashMap<string, Community>();
            Posts = new HashMap<int, UserPost>();
        }

        public void Serialize()
        {

            SerializeCommunitites();
            SerializeUsers();
            SerializeFollowedCommunities();
            SerializeFollowedUsers();
            SerializeAllPosts();
        }

        public void SerializeCommunitites()
        {// Serialize Communitites
            using (StreamWriter writer = new StreamWriter(dir+"/data/communities.txt"))
            {
                foreach (var community in model.Communities.Inorder())
                {
                    writer.WriteLine($"{community.Id}|{community.Name}");
                }
            }
        }
        public void SerializeUsers()
        {// Serialize Users
            using (StreamWriter writer = new StreamWriter(dir + "/data/users.txt"))
            {
                foreach (var user in model.Users)
                {
                    writer.WriteLine($"{user.Id}|{user.Name}|{user.Password}");
                }
            }
        }
        public void SerializeFollowedCommunities()
        {// Serialize Followed Communities
            using (StreamWriter writer = new StreamWriter(dir + "/data/usercommunity.txt"))
            {
                foreach (var user in model.Users)
                {
                    writer.WriteLine(user.Name + "|" + string.Join(",", user.Communities.Inorder().Select(e => e.Name)));
                }
            }
        }
        public void SerializeFollowedUsers()
        {// Serialize FollowedUsers
            using (StreamWriter writer = new StreamWriter(dir + "/data/userfollowed.txt"))
            {
                foreach (var user in model.Users)
                {
                    writer.WriteLine(user.Name + "|" + string.Join(",", user.FollowedUsers.Select(e => e.Name)));
                }
            }
        }
        public void SerializeUserPosts(User user) 
        {
            using (StreamWriter writer = new StreamWriter(dir + $"/data/userposts/{user.Name}.txt") )
            {
                foreach (var item in user.Posts.Reverse())
                {
                    writer.WriteLine($"{item.ID}|{item.DatePosted}|{item.Community.Name}|{item.Text}");
                }
            }
        }

        public void SerializeCommunityPosts(Community community)
        {
            using (StreamWriter writer = new StreamWriter(dir + $"/data/communityposts/{community.Name}.txt"))
            {
                foreach (var item in community.Posts.Reverse())
                {
                    writer.WriteLine($"{item.ID}|{item.DatePosted}|{item.Community.Name}|{item.Text}");
                }
            }
        }


        public void SerializeAllPosts()
        {
            foreach (var user in model.Users)
            {
                SerializeUserPosts(user);
            }
            foreach (var community in model.Communities.Inorder())
            {
                SerializeCommunityPosts(community);
            }
        }


        

        public void Deserialize()
        {
            DeserializeCommunitites();
            DeserializeUsers();
            DeserializeFollowedCommunities();
            DeserializeFollowedUsers();
            DeserializeAllPosts();
        }

        public void DeserializeCommunitites()
        { // Deserialize Communitites
            using (var writer = new StreamWriter(dir + "/data/communities.txt", true)) { }; // Create if doesn't exist
            using (var writer = new StreamReader(dir + "/data/communities.txt"))
            {
                while (!writer.EndOfStream)
                {
                    var values = writer.ReadLine().Split('|');
                    var community = new Community() { Id = int.Parse(values[0]), Name = values[1] };
                    Communities[community.Name] = community;
                    model.Communities.Insert(community);
                }
            }
        }
        public void DeserializeUsers()
        { // Deserialize Communitites
            using (var writer = new StreamWriter(dir + "/data/users.txt", true)) { }; // Create if doesn't exist
            using (var writer = new StreamReader(dir + "/data/users.txt"))
            {
                while (!writer.EndOfStream)
                {
                    var values = writer.ReadLine().Split('|');
                    var user = new User() { Id = int.Parse(values[0]), Name = values[1], Password = values[2]};
                    Users[user.Name] = user;
                    model.Users.AddVertex(user);
                }
            }
        }
        public void DeserializeFollowedCommunities()
        {// Deserialize Followed Communities
            using (var writer = new StreamWriter(dir + "/data/usercommunity.txt", true)) { }; // Create if doesn't exist
            using (var writer = new StreamReader(dir+"/data/usercommunity.txt"))
            {
                while (!writer.EndOfStream)
                {
                    var values = writer.ReadLine().Split('|');
                    if (values[1] == "") { continue; }
                    var user = Users[values[0]];
                    foreach (var community in values[1].Split(','))
                    {   
                        user.Communities.Insert(Communities[community]);
                    }
                }
            }
        }
        public void DeserializeFollowedUsers()
        {// Deserialize FollowedUsers
            using (var writer = new StreamWriter(dir+"/data/userfollowed.txt", true)) { }; // Create if doesn't exist
            using (var writer = new StreamReader(dir + "/data/userfollowed.txt"))
            {
                while (!writer.EndOfStream)
                {
                    var values = writer.ReadLine().Split('|');
                    var user = Users[values[0]];
                    if(values[1] == "") { continue; }
                    foreach (var followedUser in values[1].Split(','))
                    {
                        model.Users.AddEdge(user, Users[followedUser], 1);
                    }
                }
            }
        }
        public void DeserializeUserPosts(User user)
        {
            using (var writer = new StreamWriter(dir+$"/data/userposts/{user.Name}.txt", true)) { }; // Create if doesn't exist
            using (var reader = new StreamReader(dir + $"/data/userposts/{user.Name}.txt"))
            {

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split('|');
                    var post = new UserPost() { ID = int.Parse(values[0]), DatePosted = DateTime.Parse(values[1]), Community = Communities[values[2]], Text = values[3], User = user };
                    Posts[post.ID] = post;
                    user.Posts.Push(post);

                }
            }
        }



        public void DeserializeCommunityPosts(Community community)
        {
            using (var writer = new StreamWriter(dir + $"/data/communityposts/{community.Name}.txt", true)) { }; // Create if doesn't exist
            using (var reader = new StreamReader(dir + $"/data/communityposts/{community.Name}.txt"))
            {

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split('|');
                    Communities[values[2]].Posts.Push(Posts[int.Parse(values[0])]);
                }
            }
        }



        public void DeserializeAllPosts()
        { // Deserialize Posts
            foreach (var user in model.Users)
            {
                DeserializeUserPosts(user);
            }
            foreach (var community in model.Communities.Inorder())
            {
                DeserializeCommunityPosts(community);
            }
        }
    }
}
