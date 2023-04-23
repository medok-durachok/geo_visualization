using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Карта
{
    class Info
    {
       // public string id { get; private set; }
        public string spikerName { get; set; }
        public string titleEN { get; set; }
        public string titleRU { get; set; }
        public string recordDate { get; set; }
        public string language { get; set; }
        public string settlement { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public Info() { }
        public Info(string name, string titleEN, string titleRU, string date, string language, string settlement, string latitude, string longitude)
        {
            //this.id = id;
            spikerName = name;
            this.titleEN = titleEN;
            this.titleRU = titleRU;
            recordDate = date;
            this.language = language;
            this.settlement = settlement;
            this.longitude = longitude;
            this.latitude = latitude;
        }

        public override string ToString()
        {
            return titleEN;
        }
    }
}
