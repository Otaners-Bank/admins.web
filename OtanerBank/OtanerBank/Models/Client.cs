using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OtanerBank.Models
{
    public class Client
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [DisplayName("CLIENT ID")]
        public string id { get; set; }
        public string ACCOUNT { get; set; }
        public string CPF { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string BALANCE { get; set; }
        [DisplayName("LAST ACCESS")]
        public string LAST_ACCESS { get; set; }
        [DisplayName("BALANCE EARNED")]
        public string BALANCE_EARNED { get; set; }
        [DisplayName("MANAGER NAME")]
        public string MANAGER_NAME { get; set; }
        [DisplayName("MANAGER EMAIL")]
        public string MANAGER_EMAIL { get; set; }
    }
}
