using System;
using System.Threading.Tasks;
using Osiris.Discord;
using Osiris.Discord.Entities;
using Osiris.Storage;

namespace Osiris
{
    public class Osiris
    {
        private readonly IDataStorage _storage;
        private readonly Connection _connection;

        public Osiris(IDataStorage storage, Connection connection)
        {
            _storage = storage;
            _connection = connection;
        }

        public async Task Start()
        {
            await _connection.ConnectAsync(new OsirisBotConfig
            {
                Token = _storage.RestoreObject<string>("Config/BotToken")
            });
        }
    }
}