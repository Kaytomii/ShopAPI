using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shop.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;

        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}