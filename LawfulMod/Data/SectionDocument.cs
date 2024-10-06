using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulMod.Data
{
    public class SectionDocument
    {
        [BsonId] // This attribute indicates that this field is the primary key
        public int Id { get; set; } // Automatically generated ID
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserDescription { get; set; }
        public string JSON { get; set; }
    }
}
