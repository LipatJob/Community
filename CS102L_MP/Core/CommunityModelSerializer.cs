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
        Dictionary<string, User> Users;
        Dictionary<string, Community> Communities;

        public CommunityModelSerializer()
        {
            System.IO.Directory.CreateDirectory(dir+"/data");
            model = CommunityModel.GetInstance();
            Users = new Dictionary<string, User>();
            Communities = new Dictionary<string, Community>();
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
        public void SerializePosts(User user) 
        {
            using (StreamWriter writer = new StreamWriter(dir + $"/data/userposts-{user.Name}.txt") )
            {
                foreach (var item in user.Posts)
                {
                    writer.WriteLine($"{item.ID}|{item.DatePosted}|{item.Community.Name}|{item.Text}");
                }
            }
        }
        public void SerializeAllPosts()
        {
            foreach (var user in model.Users)
            {
                SerializePosts(user);
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
        public void DeserializePosts(User user)
        {
            using (var writer = new StreamWriter(dir+$"/data/userposts-{user.Name}.txt", true)) { }; // Create if doesn't exist
            using (var reader = new StreamReader(dir + $"/data/userposts-{user.Name}.txt"))
            {

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split('|');
                    var post = new UserPost() { ID = int.Parse(values[0]), DatePosted = DateTime.Parse(values[1]), Community = Communities[values[2]], Text = values[3], User = user };
                    Communities[values[2]].Posts.Push(post);
                    user.Posts.Push(post);
                }
            }
        }
        public void DeserializeAllPosts()
        { // Deserialize Posts
            foreach (var user in model.Users)
            {
                DeserializePosts(user);
            }
        }
    }
}
