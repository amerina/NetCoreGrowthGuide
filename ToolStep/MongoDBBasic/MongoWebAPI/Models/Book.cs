using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoWebAPI.Models
{
    public class Book
    {
        [BsonId]
        /*Is annotated with [BsonId] to designate this property as the document's primary key.*/
        [BsonRepresentation(BsonType.ObjectId)]
        /*Is annotated with [BsonRepresentation(BsonType.ObjectId)] to allow passing the parameter as type string 
         * instead of an ObjectId structure. Mongo handles the conversion from string to ObjectId.*/
        public string Id { get; set; }

        [BsonElement("Name")]
        /*The BookName property is annotated with the [BsonElement] attribute. 
         *The attribute's value of Name represents the property name in the MongoDB collection*/
        [JsonProperty("Name")]
        [BsonRequired]
        public string BookName { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        public string Category { get; set; }

        [BsonDefaultValue(false)]
        public string Author { get; set; }
    }
}
