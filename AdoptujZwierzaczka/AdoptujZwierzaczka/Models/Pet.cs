using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoptujZwierzaczka.Models
{
    class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Rasa { get; set; }
        public int Wiek { get; set; }
        public string Umaszczenie { get; set; }
        public string PathToImg { get; set; }
    }
}
