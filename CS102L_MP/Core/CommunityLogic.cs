﻿using CS102L_MP.Core;
using CS102L_MP.Lib;
using CS102L_MP.Lib.Concrete;
using CS102L_MP.Models;
using JobLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CS102L_MP
{
    class CommunityLogic
    {
        AVLTree<Community> Communities;
        ListDirectedGraph<User> Users;
        User LoggedinUser;
        public CommunityModelSerializer serializer;

        public CommunityLogic()
        {
            Communities = CommunityModel.GetInstance().Communities;
            Users = CommunityModel.GetInstance().Users;
            LoggedinUser = CommunityModel.GetInstance().LoggedinUser;
            serializer = new CommunityModelSerializer();
        }

        // User Module

        public bool Login(string username, string password)
        {
            username = username.ToLower();
            foreach (var user in Users)
            {
                if (user.Name.ToLower() == username && user.Password == password)
                {
                    LoggedinUser = user;
                    return true;
                }
            }
            return false;

        }

        public void Register(string username, string password)
        {
            var user = new User();
            user.Id = Users.Count == 0 ? 0 : Users.Max(e => e.Id) + 1;
            user.Name = username;
            user.Password = password;
            Users.AddVertex(user);
            
            serializer.SerializeUsers();
        }

        public bool ExistingUser(string username)
        {
            return Users.Any(e => e.Name.ToLower() == username.ToLower());
        }

        public IEnumerable<UserPost> GetMainFeed()
        {
            List<IStack<UserPost>> posts = new List<IStack<UserPost>>();

            foreach(var followedUser in Users.Neighbors(LoggedinUser))
            {
                posts.Add(followedUser.Item1.Posts);
            }

            foreach (var followedCommunity in LoggedinUser.Communities.Inorder())
            {
                posts.Add(followedCommunity.Posts);
            }

            posts.Add(LoggedinUser.Posts);

            return Algorithms.MergePosts(posts) ;
        }

        public IEnumerable<UserPost> GetUserFeed()
        {
            List<IStack<UserPost>> posts = new List<IStack<UserPost>>();

            foreach (var followedUser in Users.Neighbors(LoggedinUser))
            {
                posts.Add(followedUser.Item1.Posts);
            }

            return Algorithms.MergePosts(posts);
        }

        public IEnumerable<UserPost> GetCommunityFeed()
        {
            List<IStack<UserPost>> posts = new List<IStack<UserPost>>();

            foreach (var followedCommunity in LoggedinUser.Communities.Inorder())
            {
                posts.Add(followedCommunity.Posts);
            }

            return Algorithms.MergePosts(posts);
        }

        public void FollowUser(User user)
        {
            Users.AddEdge(LoggedinUser, user, 0);
            serializer.SerializeFollowedUsers();
        }

        public void UnfollowUser(User user)
        {
            Users.RemoveEdge(LoggedinUser, user);
            serializer.SerializeFollowedUsers();
        }

        public IEnumerable<User> GetRecommendedUsers()
        {
            var recommendedUsers = Algorithms.GenerateMST(Users, LoggedinUser, UserIndex).ToList();
            Algorithms.QuickSort(recommendedUsers, recommendCmp);
            return recommendedUsers.Select(e=>e.Item1).Where(e=>!LoggedinUser.FollowedUsers.Contains(e) && e!=LoggedinUser);
        }

        private int recommendCmp(Tuple<User, int> arg1, Tuple<User, int> arg2)
        {
            return arg1.Item2.CompareTo(arg2.Item2);
        }

        public IEnumerable<Community> CommonCommunities(User user)
        {
            return user.Communities.Inorder().Where(e => LoggedinUser.Communities.Contains(e));
        }

        public IEnumerable<User> CommonFollowedUsers(User user)
        {
            return user.FollowedUsers.Where(e => LoggedinUser.FollowedUsers.Contains(e));
        }

        public int UserIndex(User user1, User user2)
        {
            int maxCommunitites = LoggedinUser.Communities.Count;
            int count = 0;
            foreach (var community in user1.Communities.Inorder())
            {
                if (user2.Communities.Contains(community)) { count++; }
            }
            return maxCommunitites - count;
        }

        /*
          public IEnumerable<User> CommonFollowedUsers(User user)
        {
            return user.FollowedUsers.Where(e => LoggedinUser.FollowedUsers.Contains(e));
        }


        public double UserIndex(User user1, User user2)
        {
            int communityCount = 0;
            int usersCount = 0;
            double communityWeight = .7;
            double userWeight = .3;
            double maxIndex = (LoggedinUser.Communities.Count * communityWeight) + (LoggedinUser.FollowedUsers.Count() * userWeight);


            foreach (var community in user1.Communities.Inorder())
            {
                if (user2.Communities.Contains(community)) { communityCount++; }
            }
            foreach (var user in user1.FollowedUsers)
            {
                if (user2.FollowedUsers.Contains(user)) { usersCount++; }
            }


            return maxIndex - ((communityCount * communityWeight) + (usersCount * userWeight));
        }
 

         */

        public bool IsFollowed(User user)
        {
            return LoggedinUser.FollowedUsers.Contains(user);
        }

        public IList<User> SearchUser(string key)
        {
            var users = Users.ToList();
            IList<Tuple<User, int>> usersIndex = new List<Tuple<User, int>>();

            int maxLen = users.Max(e => e.Name.Length);
            key = AddPadding(key, maxLen);
            foreach (var user in users)
            {
                if (user == LoggedinUser) { continue; }
                string name = user.Name;
                usersIndex.Add(Tuple.Create(user, Algorithms.EditDistance(key, name)));
            }
            Algorithms.QuickSort(usersIndex, UserSearchComparator);
            return usersIndex.Select(e=>e.Item1).ToList();
        }

        public string AddPadding(string value, int length)
        {
            //
            return value + string.Concat(Enumerable.Repeat("|", length - value.Length));
        }

        private int UserSearchComparator(Tuple<User, int> user1, Tuple<User, int> user2)
        {
            return user1.Item2.CompareTo(user2.Item2);
        }


        // Community Module

        public IEnumerable<Community> GetCommunities()
        {
            return Communities.Inorder();
        }

        public IEnumerable<Community> GetFolowedCommunities()
        {
            return LoggedinUser.Communities.Inorder();
        }

        public void PostToCommunity(string text, Community community)
        {
            var post = new UserPost();
            post.ID = UserPost.Count;
            post.Text = text;
            post.DatePosted = DateTime.Now;
            post.Community = community;
            post.User = LoggedinUser;

            community.Posts.Push(post);
            LoggedinUser.Posts.Push(post);
            serializer.SerializeUserPosts(LoggedinUser);
            serializer.SerializeCommunityPosts(community);
        }

        public bool IsFollowingCommunity(Community community)
        {
            return LoggedinUser.Communities.Contains(community);
        }

        public bool HasCommunity(string name)
        {
            return Communities.Retrieve(name, (e,f)=> e.ToLower().CompareTo(f.Name.ToLower())) != null;
        }

        public void JoinCommunity(Community community)
        {
            LoggedinUser.Communities.Insert(community);
            serializer.SerializeFollowedCommunities();
        }

        public void LeaveCommunity(Community community)
        {
            LoggedinUser.Communities.Remove(community);
            serializer.SerializeFollowedCommunities();
        }


        public void CreateCommunity(string communityName)
        {
            Community community = new Community() { Id = Communities.Count == 0 ? 0 : Communities.Inorder().Max(e => e.Id) + 1, Name= communityName };
            Communities.Insert(community);
            FollowCommunity(community);
            serializer.SerializeCommunitites();
        }

        internal void UnfollowCommunity(Community community)
        {
            LoggedinUser.Communities.Remove(community);
            serializer.SerializeFollowedCommunities();
        }

        internal void FollowCommunity(Community community)
        {
            LoggedinUser.Communities.Insert(community);
            serializer.SerializeFollowedCommunities();
        }

        internal IEnumerable<Community> FollowedCommunities()
        {
            return LoggedinUser.Communities.Inorder();
        }

        internal User GetLoggedinUser()
        {
            return LoggedinUser;
        }
    }
}
