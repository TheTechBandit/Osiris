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
                default:
                    card = new VRFamilyCard(true);
                    break;
            }

            return card;
        }
    }
}