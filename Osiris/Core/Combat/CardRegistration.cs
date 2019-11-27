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
                default:
                    card = new CuteBunnyCard(true);
                    break;
            }

            return card;
        }
    }
}