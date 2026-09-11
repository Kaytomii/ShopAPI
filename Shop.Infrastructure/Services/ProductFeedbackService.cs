using MongoDB.Driver;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Services;

public class ProductFeedbackService
{
    private readonly IMongoCollection<ProductFeedback> _collection;

    public ProductFeedbackService(MongoDbService mongo)
    {
        _collection = mongo.GetCollection<ProductFeedback>("ProductFeedback");
    }

    public async Task AddAsync(ProductFeedback feedback)
    {
        await _collection.InsertOneAsync(feedback);
    }
}