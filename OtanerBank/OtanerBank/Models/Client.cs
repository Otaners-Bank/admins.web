using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtanerBank.Models
{
    public class Client
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string conta { get; set; }
        public string CPF { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string saldo { get; set; }

        public string ultimoAcesso { get; set; }
        public string rendaGerada { get; set; }

        public string nomeGerenteResponsavel { get; set; }
        public string emailGerenteResponsavel { get; set; }
    }
}
