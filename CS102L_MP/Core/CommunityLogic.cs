﻿using CS102L_MP.Lib;
using CS102L_MP.Lib.Concrete;
using CS102L_MP.Models;
using JobLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP
{
    class CommunityLogic
    {
        AVLTree<Community> Communities;
        ListDirectedGraph<User> Users;
        User LoggedinUser;

        public CommunityLogic()
        {
            Communities = CommunityModel.GetInstace().Communities;
            Users = CommunityModel.GetInstace().Users;
            LoggedinUser = CommunityModel.GetInstace().LoggedinUser;
        }

        public void Login()
        {
            bool loggedin = false;
            while (!loggedin)
            {
                string username = JHelper.InputString("Enter Username: ", toUpper: true);
                string password = JHelper.InputString("Enter Password: ");
                foreach (var user in Users)
                {
                    if (user.Name.ToUpper() == username && user.Password == password)
                    {
                        LoggedinUser = user;
                        loggedin = true;
                        break;
                    }
                }
                Console.WriteLine("Incorrect username or password try again");
            }

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
        }

        public void UnfollowUser(User user)
        {
            Users.RemoveEdge(LoggedinUser, user);
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

        public void SearchUser(string key)
        {
            var users = Users.ToList();
            Algorithms.QuickSort(users, UserSearchComparator);
        }

        private int UserSearchComparator(User user1, User user2)
        {
            return Algorithms.EditDistance(user1.Name, user2.Name);
        }

        public void SeeCommunities()
        {
            var communities = Communities.Inorder();
        }

        public void JoinCommunity(Community community)
        {
            LoggedinUser.Communities.Insert(community);
        }

        public void LeaveCommunity(Community community)
        {
            LoggedinUser.Communities.Remove(community);
        }

        public void PostToCommunity(Community community, UserPost post)
        {
            community.Posts.Push(post);
        }

        public void SeeCommunityPosts(Community community)
        {
            var posts = community.Posts.AsEnumerable();
        }

        public void CreateCommunity()
        {
            Community community = new Community(); ;
            Communities.Insert(community);
        }
    }
}