namespace Infrastructure.Persistence;

public class ApplicationDbContext
{
    private readonly IMongoDatabase _database;

    public ApplicationDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);

        IMongoCollection<Server> userCollection = GetCollection<Server>();

        var indexOptions = new CreateIndexOptions { Unique = true };

        List<CreateIndexModel<Server>> indexModelList =
        [
            new(Builders<Server>.IndexKeys.Ascending(server => server.Name), indexOptions)
        ];

        userCollection.Indexes.CreateMany(indexModelList);
    }

    public IMongoCollection<T> GetCollection<T>()
    {
        return _database.GetCollection<T>(typeof(T).Name.ToLower() + "s");
    }
}
