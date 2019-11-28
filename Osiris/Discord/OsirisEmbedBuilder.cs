using System;
using System.Collections.Generic;
using Discord;

namespace Osiris.Discord
{
    public static class OsirisEmbedBuilder
    {
        static OsirisEmbedBuilder()
        {
            
        }

        public static Embed CardList(BasicCard card)
        {
            var builder = new EmbedBuilder();

            if(card.Name.Equals(card.Signature))
                builder.WithAuthor($"{card.Name}");
            else
	            builder.WithAuthor($"{card.Name} {card.Signature}");

            builder.WithImageUrl(card.Picture);

            foreach(BasicMove move in card.Moves)
            {
                string cooldownText = "";
                if(move.Cooldown != 0)
                    cooldownText += $"**{move.CooldownText}**";

                string ult = "";
                if(move.IsUltimate)
                    ult = "(Ultimate)";
                
                builder.AddField($"**{move.Name} {ult}**", $"{move.Description} {cooldownText}");
            }

            if(card.HasPassive)
                    builder.AddField($"**{card.Passive.Name} (Passive)**", $"{card.Passive.Description}");

            int r = card.HPGradient()[0];
            int g = card.HPGradient()[1];
            int b = card.HPGradient()[2];
        	builder.WithColor(r, g, b);
            var embed = builder.Build();

            return embed;
        }

        public static List<Embed> RoundStart(CombatInstance inst)
        {
            Console.WriteLine("a");
            List<Embed> embeds = new List<Embed>();
            var builder = new EmbedBuilder()
	        .WithAuthor($"Round: {inst.RoundNumber}");
            Console.WriteLine("b");

            var players = "";
            List<string> effectTitles = new List<string>();
            List<List<string>> effects = new List<List<string>>();

            foreach(BasicCard card in inst.CardList)
            {
                List<string> currentPlayerEffects = new List<string>();
                string shields = "";

                if(card.Effects.Count > 0 || card.Markers.Count > 0 || card.HasPassive)
                    effectTitles.Add($"({card.Signature})");

                var light = 0;
                var medium = 0;
                var heavy = 0;    
                foreach(BuffDebuff eff in card.Effects)
                {
                    light += eff.LightShield;
                    medium += eff.MediumShield;
                    heavy += eff.HeavyShield;
                    
                    currentPlayerEffects.Add($"\n{eff.ToString()}");
                }
                if(card.HasPassive)
                    currentPlayerEffects.Add($"\n**{card.Passive.Name} ({card.Signature}'s Passive)**- {card.Passive.Description}");

                if(light > 0)
                    shields += $"{light} light shields. ";
                if(medium > 0)
                    shields += $"{medium} medium shields. ";
                if(heavy > 0)
                    shields += $"{heavy} heavy shields.";
                    
                foreach(Marker mark in card.Markers)
                {
                    currentPlayerEffects.Add($"\n{mark.ToString()}");
                }
                //if(card.Effects.Count > 0 || card.Markers.Count > 0 || card.HasPassive)
                    //currentPlayerEffects.Add("\n.\n");

                if(card.Name.Equals(card.Signature))
                    players += $"**[{card.Name}]:** {card.CurrentHP}/{card.TotalHP} HP {shields}\n";
                else
                    players += $"**[{card.Name}] {card.Signature}:** {card.CurrentHP}/{card.TotalHP} HP {shields}\n";
                
                if(currentPlayerEffects.Count == 0)
                {
                    currentPlayerEffects.Add("none");
                }

                effects.Add(currentPlayerEffects);
            }
            Console.WriteLine("c");

            Console.WriteLine("d");
            builder.WithDescription(players);
            Console.WriteLine("e");
            //builder.AddField("ACTIVE EFFECTS", finalEffects, false);
            Console.WriteLine("f");
            builder.WithFooter($"It is {inst.Players[inst.TurnNumber].Name}'s turn.");
            Console.WriteLine("g");
        	builder.WithColor(62, 62, 255);
            Console.WriteLine("h");
            var embed = builder.Build();
            Console.WriteLine("i");
            embeds.Add(embed);
            Console.WriteLine("j");

            effects.Reverse();
            effectTitles.Reverse();
            while(effects.Count > 0)
            {
                Console.WriteLine("k");
                var bail = false;
                var effBuilder = new EmbedBuilder()
                .WithAuthor($"**ACTIVE EFFECTS**")
                .WithFooter($"It is {inst.Players[inst.TurnNumber].Name}'s turn.");
                Console.WriteLine("l");

                for (int i = effects.Count - 1; i >= 0; i--)
                {
                    Console.WriteLine("m");
                    var playerEffects = "";
                    int totalChars = 0;
                    for (int j = effects[i].Count - 1; j >= 0; j--)
                    {
                        Console.WriteLine("n");
                        totalChars += effects[i][j].Length;
                        Console.WriteLine("o");
                        if(totalChars < 1024)
                        {
                            Console.WriteLine("p");
                            playerEffects += $"{effects[i][j]}\n";
                            Console.WriteLine("q");
                            effects[i].RemoveAt(j);
                            Console.WriteLine("r");
                        }
                        else
                        {
                            Console.WriteLine("s");
                            effBuilder.AddField($"{effectTitles[i]}", $"{playerEffects}", false);
                            Console.WriteLine("t");
                            bail = true;
                            Console.WriteLine("u");
                            break;
                        }
                    }
                    Console.WriteLine("v");
                    if(bail)
                        break;
                    Console.WriteLine("w");
                    effBuilder.AddField($"{effectTitles[i]}", $"{playerEffects}", false);
                    Console.WriteLine("x");
                    effectTitles.RemoveAt(i);
                    effects.RemoveAt(i);
                    Console.WriteLine("y");
                }
                Console.WriteLine("z");
                embeds.Add(effBuilder.Build());
                Console.WriteLine("za");
            }
            Console.WriteLine("zb");

            return embeds;
        }
        

        public static Embed PlayerTurnStatus(BasicCard card, int round)
        {
            var builder = new EmbedBuilder();

            string cooldowns = "";
            string effects = "";
            var lightShield = "";
            var mediumShield = "";
            var heavyShield = "";
            foreach(BuffDebuff eff in card.Effects)
            {
                for(int i = 0; i < eff.HeavyShield; i++)
                    heavyShield += "🟥";
                for(int i = 0; i < eff.LightShield; i++)
                    lightShield += "🟦";
                for(int i = 0; i < eff.MediumShield; i++)
                    mediumShield += "🟪";
                
                effects += eff.ToString() + "\n";
            }

            effects += card.Passive.ToString();

            foreach(Marker mark in card.Markers)
            {
                effects += mark.ToString() + "\n";
            }
            if(effects.Length == 0)
                effects += "none";

	        if(card.Name.Equals(card.Signature))
                builder.WithAuthor($"{card.Name}");
            else
	            builder.WithAuthor($"{card.Name} {card.Signature}");

            builder.WithImageUrl(card.Picture);
            
            builder.WithTitle($"HP: {card.CurrentHP}/{card.TotalHP}\n{heavyShield}{mediumShield}{lightShield}\n");

            foreach(BasicMove move in card.Moves)
            {
                string cooldownText = "";
                if(move.Cooldown != 0)
                    cooldownText += $"**{move.CooldownText}**";

                string ult = "";
                if(move.IsUltimate)
                    ult = "(Ultimate)";

                builder.AddField($"**{move.Name} {ult}**", $"{move.Description} {cooldownText}");
            }

            if(card.HasPassive)
                    builder.AddField($"**{card.Passive.Name} (Passive)**", $"{card.Passive.Description}");

            foreach(BasicMove move in card.Moves)
            {
                if(move.OnCooldown)
                    cooldowns += $"{move.Name}- Available on round {move.CurrentCooldown+round}\n";
            }
            if(cooldowns.Length == 0)
                cooldowns += "none";

            builder.AddField("Cooldowns:", cooldowns)
            .AddField("Effects:", effects);

            int r = card.HPGradient()[0];
            int g = card.HPGradient()[1];
            int b = card.HPGradient()[2];
        	builder.WithColor(r, g, b)
            .WithFooter($"Round {round}");

            var embed = builder.Build();
            return embed;
        }

        public static Embed Blinder()
        {
            var builder = new EmbedBuilder()
            .WithImageUrl("https://cdn.discordapp.com/attachments/460357767484407809/648733197726646304/blinding_flash.jpg");

            var embed = builder.Build();
            return embed;
        }

    }
}