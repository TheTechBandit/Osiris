using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Osiris.Discord;
using Osiris.Storage;
using Osiris.Storage.Implementations;
using Discord.WebSocket;

namespace Osiris
{
    public static class UserHandler
    {
        public static readonly string filepath;
        private static Dictionary<ulong, UserAccount> _dic;
        private static JsonStorage _jsonStorage;

        static UserHandler()
        {
            System.Console.WriteLine("Loading User Accounts...");
            
            //Access JsonStorage to load user list into memory
            filepath = "Users/UserList";

            _dic = new Dictionary<ulong, UserAccount>();
            _jsonStorage = new JsonStorage();

            foreach(KeyValuePair<ulong, UserAccount> entry in _jsonStorage.RestoreObject<Dictionary<ulong, UserAccount>>(filepath))
            {
                _dic.Add(entry.Key, (UserAccount)entry.Value);
            }

            System.Console.WriteLine($"Successfully loaded {_dic.Count} users.");
        }

        public static UserAccount GetUser(ContextIds ids)
        {
            return GetUser(ids.UserId);
        }

        public static UserAccount GetUser(ulong id)
        {
            if(DoesUserExist(id))
            {
                return _dic[id];
            }
            else
            {
                CreateNewUser(id);
                return _dic[id];
            }
        }

        public static UserAccount CreateNewUser(ulong id)
        {
            System.Console.WriteLine($"Creating new user with ID: {id}");

            UserAccount acc = new UserAccount(true)
            {
                UserId = id
            };
            _dic.Add(id, acc);
            SaveUsers();
            return acc;
        }

        public static void UpdateUserInfo(ulong id, ulong dm, string name, string mention, string avatar)
        {
            var user = GetUser(id);
            user.DmId = dm;
            user.Name = name;
            user.Mention = mention;
            user.AvatarUrl = avatar;
            SaveUsers();
        }

        public static void SaveUsers()
        {
            System.Console.WriteLine("Saving users...");
            _jsonStorage.StoreObject(_dic, filepath);
        }

        public static void ClearUserData()
        {
            System.Console.WriteLine("Deleting all users.");
            Dictionary<ulong, UserAccount> emptyDic = new Dictionary<ulong, UserAccount>();
            emptyDic.Add(0, new UserAccount(true)
            {
                UserId = 0
            });
            _jsonStorage.StoreObject(emptyDic, filepath);
        }

        public static bool DoesUserExist(ulong id)
        {
            return _dic.ContainsKey(id);
        }

        public static async Task UserInCombat(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if(user.CombatID != -1)
            {
                await MessageHandler.UserInCombat(ids);
                throw new InvalidUserStateException("user in combat");
            }
        }

        public static async Task UserNotInCombat(ContextIds ids)
        {
            var user = GetUser(ids.UserId);
            if(user.CombatID == -1)
            {
                await MessageHandler.UserNotInCombat(ids);
                throw new InvalidUserStateException("user not in combat");
            }
        }

        public static async Task OtherUserInCombat(ContextIds ids, UserAccount otherUser)
        {
            var user = GetUser(ids.UserId);
            if(otherUser.CombatID != -1)
            {
                await MessageHandler.OtherUserInCombat(ids);
                throw new InvalidUserStateException("other user in combat");
            }
        }

        public static async Task OtherUserNotInCombat(ContextIds ids, UserAccount otherUser)
        {
            var user = GetUser(ids.UserId);
            if(otherUser.CombatID == -1)
            {
                await MessageHandler.OtherUserNotInCombat(ids);
                throw new InvalidUserStateException("other user not in combat");
            }
        }

        public static async Task UserHasNoCards(ContextIds ids, UserAccount user)
        {
            if(user.ActiveCards.Count <= 0)
            {
                await MessageHandler.UserHasNoCards(ids, user);
                throw new InvalidUserStateException("user has no card");
            }
        }

        public static async Task OtherUserHasNoCards(ContextIds ids, UserAccount user, UserAccount otherUser)
        {
            if(otherUser.ActiveCards.Count <= 0)
            {
                await MessageHandler.OtherUserHasNoCards(ids, user);
                throw new InvalidUserStateException("other user has no card");
            }
        }

    }
}