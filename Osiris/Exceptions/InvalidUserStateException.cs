using System;

namespace Osiris
{
    [Serializable]
    class InvalidUserStateException: Exception
    {
        public InvalidUserStateException()
        {

        }

        public InvalidUserStateException(string type)
            : base($"Invalid character state: {type}")
        {

        }
    }
}