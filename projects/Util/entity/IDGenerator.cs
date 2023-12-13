using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.entity;

public interface IDGenerator
{
    public static IDGenerator instance { get; set; } = new MongoIdGenerator();
    public ID Generate();
}

public class MongoIdGenerator : IDGenerator
{
    public ID Generate()
    {
        return (ID) ObjectId.GenerateNewId().ToString();
    }
}

public class GuidGenerator : IDGenerator
{
    public ID Generate()
    {
        return (ID) Guid.NewGuid().ToString();
    }
}