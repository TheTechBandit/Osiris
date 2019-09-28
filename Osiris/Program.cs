using System;
using System.Threading.Tasks;

namespace Osiris
{
    internal class Program
    {
        private static async Task Main()
        {
            var bot = Unity.Resolve<Osiris>();
            await bot.Start();
        }
    }
}
