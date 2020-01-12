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
        [DisplayName("ACCOUNT")]
        public string conta { get; set; }
        [DisplayName("CPF")]
        public string CPF { get; set; }
        [DisplayName("NAME")]
        public string nome { get; set; }
        [DisplayName("EMAIL")]
        public string email { get; set; }
        [DisplayName("PASSWORD")]
        public string senha { get; set; }
        [DisplayName("BALANCE")]
        public string saldo { get; set; }
        [DisplayName("LAST ACCESS")]
        public string ultimoAcesso { get; set; }
        [DisplayName("BALANCE WARNED")]
        public string rendaGerada { get; set; }
        [DisplayName("MANAGER NAME")]
        public string nomeGerenteResponsavel { get; set; }
        [DisplayName("MANAGER EMAIL")]
        public string emailGerenteResponsavel { get; set; }
    }
}
