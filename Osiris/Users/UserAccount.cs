using System;
using System.Collections;
using System.Collections.Generic;
using Osiris.Discord;
using Newtonsoft.Json;

namespace Osiris
{
    public class UserAccount
    {
        public ulong UserId { get; set; }
        public ulong DmId { get; set; }
        public string Mention { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public int PromptState { get; set; }

        public UserAccount()
        {

        }
        public UserAccount(bool newuser)
        {
            PromptState = -1;
        }

        public string DebugString()
        {
            string str = $"UserID: {UserId}\nMention: {Mention}\nName: {Name}\nAvatarUrl: {AvatarUrl}\nPromptState: {PromptState}";
            return str;
        }
        
    }
}