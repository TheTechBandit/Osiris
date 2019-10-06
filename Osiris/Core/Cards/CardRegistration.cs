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
                    card = new VRFamilyCard();
                    break;
                default:
                    card = new VRFamilyCard();
                    break;
            }

            return card;
        }
    }
}