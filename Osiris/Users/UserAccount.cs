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
        public bool Celestial { get; set; }
        public int PromptState { get; set; }
        public ulong CombatRequest { get; set; }
        public int CombatID { get; set; }
        public int TeamNum { get; set; }
        public bool Dead { get; set; }
        public List<BasicCard> ActiveCards { get; set; }
        public bool Blocked { get; set; }

        public UserAccount()
        {

        }

        public UserAccount(bool newuser)
        {
            Celestial = false;
            PromptState = -1;
            CombatRequest = 0;
            CombatID = -1;
            TeamNum = -1;
            Dead = false;
            ActiveCards = new List<BasicCard>();
            Blocked = false;
        }

        public void ResetCombatFields(bool duel)
        {
            CombatID = -1;
            TeamNum = -1;
            foreach(BasicCard card in ActiveCards)
            {
                card.IsTurn = false;
                card.CooldownsReset();
                card.RemoveAllBuffs();
                card.RemoveAllMarkers();
                
                if(duel)
                {
                    card.CurrentHP = card.TotalHP;
                    card.Dead = false;
                }
            }
        }

        public string DebugString()
        {
            string str = $"UserID: {UserId}\nDmId: {DmId}\nMention: {Mention}\nName: {Name}\nAvatarUrl: {AvatarUrl}\nCelestial: {Celestial}\nPromptState: {PromptState}";
            str += $"\nCombatRequest: {CombatRequest}\nCombatID: {CombatID}\nTeamNum: {TeamNum}\nDead: {Dead}";
            str += "\n**Cards**";
            foreach(BasicCard card in ActiveCards)
            {
                str += $"\n**{card.Name}**";
            }
            
            return str;
        }
        
    }
}