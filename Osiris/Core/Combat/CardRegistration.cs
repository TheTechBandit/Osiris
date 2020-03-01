using Osiris.Discord;

namespace Osiris
{
    public static class CardRegistration
    {
        static CardRegistration()
        {

        }

        public static BasicCard RegisterCard(string str)
        {
            BasicCard card;
            str = str.ToLower();

            switch(str)
            {
                case "vrfamily":
                    card = new VRFamilyCard(true);
                    break;
                case "touched":
                    card = new TouchedCard(true);
                    break;
                case "ghub":
                    card = new GhubCard(true);
                    break;
                case "ika":
                    card = new IkaCard(true);
                    break;
                case "sugar ghubby":
                    card = new SugarGhubbyCard(true);
                    break;
                case "fluffy angora":
                    card = new FluffyAngoraCard(true);
                    break;
                case "speedy hare":
                    card = new SpeedyHareCard(true);
                    break;
                case "angry jackalope":
                    card = new AngryJackalopeCard(true);
                    break;
                case "cute bunny":
                    card = new CuteBunnyCard(true);
                    break;
                case "archer":
                    card = new OArcherCard(true);
                    break;
                case "warrior":
                    card = new OWarriorCard(true);
                    break;
                case "kegmaster":
                    card = new OKegmasterCard(true);
                    break;
                case "battle bard":
                    card = new OBattleBardCard(true);
                    break;
                case "trojan soldier":
                    card = new OTrojanSoldierCard(true);
                    break;
                case "hector, champion of troy":
                    card = new OHectorCard(true);
                    break;
                case "odysseus, king of ithaca":
                    card = new OdysseusKingCard(true);
                    break;
                case "odysseus, exiled":
                    card = new OdysseusExileCard(true);
                    break;
                case "odysseus, old":
                    card = new OdysseusOldCard(true);
                    break;
                case "achilles":
                    card = new AchillesCard(true);
                    break;
                case "priam":
                    card = new PriamCard(true);
                    break;
                case "polyphemus, son of poseidon":
                    card = new PolyphemusCard(true);
                    break;
                case "polyphemus, blinded":
                    card = new PolyphemusBlindCard(true);
                    break;
                case "circe, temptress of the mountain":
                    card = new CirceCard(true);
                    break;
                case "pig":
                    card = new OPigCard(true);
                    break;
                case "snake":
                    card = new OSnakeCard(true);
                    break;
                case "lion":
                    card = new OLionCard(true);
                    break;
                case "kingfisher":
                    card = new OKingfisherCard(true);
                    break;
                case "scylla head, terror of the cave":
                    card = new ScyllaCard(true);
                    break;
                case "charybdis, maw of the deep" :
                    card = new CharybdisCard(true);
                    break;
                case "twelve axe heads":
                    card = new TwelveAxesCard(true);
                    break;
                case "suitor":
                    card = new SuitorCard(true);
                    break;
                default:
                    card = new VRFamilyCard(true);
                    break;
            }
            
            return card;
        }
    }
}