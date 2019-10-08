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
                default:
                    card = new VRFamilyCard(true);
                    break;
            }

            return card;
        }
    }
}