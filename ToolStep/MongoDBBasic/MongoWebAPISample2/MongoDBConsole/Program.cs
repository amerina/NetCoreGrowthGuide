using MongoDB.Bson;
using MongoDB.Driver;
using System;
/// <summary>
/// Demo from 
/// https://www.c-sharpcorner.com/article/getting-started-with-mongodb-mongodb-with-c-sharp/
/// </summary>
namespace MongoDBConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");

            //Database List  
            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases are :");
            foreach (var item in dbList)
            {
                Console.WriteLine(item);
            }

            //Get Database and Collection  
            IMongoDatabase db = dbClient.GetDatabase("test");
            var collList = db.ListCollections().ToList();
            Console.WriteLine("The list of collections are :");
            foreach (var item in collList)
            {
                Console.WriteLine(item);
            }

            //不推荐使用BsonDocument,如果系统较为简单可以考虑,最好使用强类型DTO或领域实体Entity
            //Create
            var thingsCollect = db.GetCollection<BsonDocument>("things");

            //CREATE  
            BsonDocument personDoc = new BsonDocument();

            //Method 1  
            BsonElement personFirstNameElement = new BsonElement("PersonFirstName", "Sankhojjal");
            personDoc.Add(personFirstNameElement);

            //Method 2  
            personDoc.Add(new BsonElement("PersonAge", 23));

            thingsCollect.InsertOne(personDoc);

            //Read
            var resultDoc = thingsCollect.Find(new BsonDocument()).ToList();
            foreach (var item in resultDoc)
            {
                Console.WriteLine(item.ToString());
            }

            //Update
            BsonElement updatePersonFirstNameElement = new BsonElement("PersonFirstName", "Souvik");

            BsonDocument updatePersonDoc = new BsonDocument();
            updatePersonDoc.Add(updatePersonFirstNameElement);
            updatePersonDoc.Add(new BsonElement("PersonAge", 24));

            BsonDocument findPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sankhojjal"));
            var updateDoc = thingsCollect.FindOneAndReplace(findPersonDoc, updatePersonDoc);
            Console.WriteLine(updateDoc);

            //Delete
            BsonDocument findAnotherPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sourav"));
            thingsCollect.FindOneAndDelete(findAnotherPersonDoc);


            //

            Console.WriteLine("Hello World!");
        }
    }
}
