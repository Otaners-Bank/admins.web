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
        [DisplayName("Client")]
        public string id { get; set; }
        [DisplayName("Account Number")]
        public string ACCOUNT { get; set; }
        public string CPF { get; set; }
        [DisplayName("Name")]
        public string NAME { get; set; }
        [DisplayName("E-mail")]
        public string EMAIL { get; set; }
        [DisplayName("Password")]
        public string PASSWORD { get; set; }
        [DisplayName("Balance")]
        public string BALANCE { get; set; }
        [DisplayName("Last Access")]
        public string LAST_ACCESS { get; set; }
        [DisplayName("Saving Balance")]
        public string BALANCE_EARNED { get; set; }
        [DisplayName("Manager Name")]
        public string MANAGER_NAME { get; set; }
        [DisplayName("Manager E-mail")]
        public string MANAGER_EMAIL { get; set; }
    }
}
