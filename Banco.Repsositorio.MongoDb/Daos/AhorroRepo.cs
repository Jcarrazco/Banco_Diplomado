using Banco.Core.Repositorio.Entidades;
using Banco.Core.Repositorio.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Banco.Repsositorio.MongoDb.Daos
{
    internal class AhorroRepo : IAhorroRepositorio
    {
        private readonly IMongoCollection<AhorroEntidad> _collection;

        public AhorroRepo(IConfiguration configurations)
        {
            var conectionString = configurations.GetConnectionString("MongoDb");
            var mongoClient = new MongoClient(conectionString);
            var nombreDeLaDb = conectionString?.Split("/").Last().Split("?").First();
            var mongoDatabase = mongoClient.GetDatabase(nombreDeLaDb);
            _collection = mongoDatabase.GetCollection<AhorroEntidad>("Ahorros");
        }
        public Task Actualizar(AhorroEntidad entidad)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AgregarAsync(AhorroEntidad item)
        {
            item.Id = (int)await _collection.CountDocumentsAsync(_ => true) + 1;
            await _collection.InsertOneAsync(item);

            return item.Id;
        }

        public async Task<AhorroEntidad> ObtenerAsync(string idEncodedKey)
        {
            if (int.TryParse(idEncodedKey, out int id))
                return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            else
                return await _collection.Find(x => x.Encodedkey == idEncodedKey).FirstOrDefaultAsync();
        }

        public async Task<AhorroEntidad> ObtenerAhorroAsync(string clienteId)
        {
            if (int.TryParse(clienteId, out int id))
                return await _collection.Find(x => x.ClienteId == id).FirstOrDefaultAsync();
            else
                return await _collection.Find(x => x.ClienteEncodedkey == clienteId).FirstOrDefaultAsync();
        }


    }
}
