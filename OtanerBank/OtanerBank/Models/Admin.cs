using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OtanerBank.Models
{
    public class Admin
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [DisplayName("CLIENT ID")]
        public string id { get; set; }
        public string EMAIL { get; set; }
        public string NAME { get; set; }
        public string PASSWORD { get; set; }
    }
}
