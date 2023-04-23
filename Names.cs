using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace извлечение_имен
{
    public class Names
    {
        public string name { get; set; }            //название места
        public string start { get; set; }
        public string end { get; set; }
        public string refname { get; set; }        //название текста
        public int refnum { get; set; }               //номер предложения
        public string ts { get; set; }               //предложение в оригинале
        public string fr { get; set; }                //перевод предложения
        public string speaker { get; set; }           //кто записывал этот текст
        public string language { get; set; }
        public Names() { }
        public Names(string name, string refname, int refnum, string ts, string fr, string speaker, string language)
        {
            this.name = name;
            this.refname = refname;
            this.refnum = refnum;
            this.ts = ts;
            this.fr = fr;
            this.speaker = speaker;
            this.language = language;
        }

    }
}
