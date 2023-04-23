using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace извлечение_имен
{
    class Program
    {
        public static string getText(string txt)
        {
            string ans = ""; bool flag = true; int i = 0;
            int index = txt.IndexOf("_", 0), num = 1;
            while (index > -1)
            {
                string res = "1" + txt[index + 1];
                if (num == 2 && !int.TryParse(res, out int result1)) { ans = txt.Remove(0, index + 1); i = index + 1; }
                if (num == 3 && ans != "") { ans = ans.Remove(index - i, ans.Length - (index - i)); flag = false; }
                if (num == 3 && flag) { ans = txt.Remove(0, index + 1); i = index + 1; }
                if (num == 4 && flag) ans = ans.Remove(index - i, ans.Length - (index - i));
                index = txt.IndexOf("_", index + 1);
                num++;
            }
            return ans;
        }

        public static string getDate(string txt)
        {
            string ans = ""; bool flag = true;
            int index = txt.IndexOf("_", 0), num = 1;
            while (index > -1)
            {
                string res = "1" + txt[index + 1];
                if (num == 1 && int.TryParse(res, out int result1)) { ans = txt.Remove(0, index + 1); ans = ans.Remove(4, ans.Length - 4); flag = false; }
                if (num == 2 && flag) { ans = txt.Remove(0, index + 1); ans = ans.Remove(4, ans.Length - 4); }
                index = txt.IndexOf("_", index + 1);
                num++;
            }
            return ans;
        }

        public static void AllNames(List<string> d, int dir, List<Names> names, string s)
        {
            XmlDocument doc = new XmlDocument();
            int begin = 0; bool samename = true; string path = "", language="";
            switch (dir)
            {
                case 0:
                    {
                        path = "SelkupCorpus-v01\\" + d[0] + "\\" + d[1] + "\\" + d[2] + "\\" + d[3];
                        language="selkup";
                        doc.Load(path); break;
                    }
                case 1:
                    {
                        path = "dolgan-noAudio\\" + d[0] + "\\" + d[1] + "\\" + d[2];
                        language = "dolgan";
                        doc.Load(path); break;
                    }
                case 2:
                    {
                        path = "kamas_noAudio\\" + d[0] + "\\" + d[1] + "\\" + d[2];
                        language = "kamas";
                        doc.Load(path); break;
                    }
            }
            XmlNodeList xnodeList = doc.GetElementsByTagName("tier");
            XmlNodeList nodeList = doc.GetElementsByTagName("event");
            int i = 0; List<string> speaker = new List<string>();
            foreach (XmlNode xnode in xnodeList)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (xnode.Attributes["category"].Value.Contains("ps") && childnode.Name.Contains("event") && childnode.InnerText == s)
                    {
                        names.Add(new Names());
                        begin++;
                        names[names.Count - 1].start = childnode.Attributes["start"].InnerText;
                        names[names.Count - 1].end = childnode.Attributes["end"].InnerText;
                        names[names.Count - 1].speaker = xnode.Attributes["speaker"].Value;
                        names[names.Count - 1].language = language;
                        if (speaker.Count == 0) speaker.Add(xnode.Attributes["speaker"].Value);
                        if (xnode.Attributes["speaker"].Value != speaker[speaker.Count - 1]) speaker.Add(xnode.Attributes["speaker"].Value);
                    }
                }
            }
            i = names.Count - begin;
            if (begin != 0)
            {
                foreach (string sp in speaker)
                {
                    foreach (XmlNode xnode in xnodeList)
                    {
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            if (xnode.Attributes["category"].Value == "gr" && childnode.Name.Contains("event") && childnode.Attributes["start"].InnerText == names[i].start && names[i].end == childnode.Attributes["end"].InnerText && names[i].speaker == sp)
                            {
                                string name = childnode.InnerText; int indexdot = 0, indexdash = 0;
                                int index = 0; List<int> ind = new List<int>();
                                if (name.Contains("."))
                                {
                                    while ((index = name.IndexOf(".", index)) != -1)
                                    {
                                        ind.Add(index);
                                        index++;
                                    }
                                    index = 0;
                                    foreach (int tt in ind)
                                    {
                                        if (name[tt + 1] >= 'A' && name[tt + 1] <= 'Z' || name[tt + 1] == '[')
                                        {
                                            if (index == 0 || tt < index) index = tt;
                                        }
                                    }
                                    names[i].name = name.Remove(name.IndexOf("."));
                                    indexdot = name.IndexOf(".");
                                }
                                string res = "1" + name[name.IndexOf("-") + 1];
                                if (name.Contains("-") && (name[name.IndexOf("-") + 1] >= 'A' && name[name.IndexOf("-") + 1] <= 'Z') || int.TryParse(res, out int result1))
                                {
                                    names[i].name = name.Remove(name.IndexOf("-"));
                                    indexdash = name.IndexOf("-");
                                }
                                if (indexdot == 0 && indexdash == 0)
                                    names[i].name = name;
                                if (indexdot != 0 && indexdash != 0)
                                {
                                    if (indexdot < indexdash) names[i].name = name.Remove(name.IndexOf("."));
                                    if (indexdash < indexdot) names[i].name = name.Remove(name.IndexOf("-"));
                                }
                                if (names[i].name.Contains("?")) names[i].name = name.Remove(name.IndexOf("?"));
                                for (int var = names.Count - begin; var < names.Count - 1; var++)
                                {
                                    if (names[i].name == names[var].name && i != var && names[i].speaker == names[var].speaker) samename = false;
                                }
                                if (!samename)
                                {
                                    names.RemoveAt(i);
                                    i--; begin--;
                                }
                                samename = true;
                                if (i + 1 < names.Count || names.Count == 0) i++;
                            }
                        }
                    }
                }
                i = names.Count - begin;
                for (; i < names.Count; i++)
                {
                    foreach (string sp in speaker)
                    {
                        foreach (XmlNode xnode in xnodeList)
                        {
                            foreach (XmlNode childnode in xnode.ChildNodes)
                            {
                                string start = childnode.Attributes["start"].InnerText.TrimStart('T');
                                string end = childnode.Attributes["end"].InnerText.TrimStart('T');
                                if (xnode.Attributes["category"].Value == "ref" && childnode.Name.Contains("event") && int.Parse(names[i].start.TrimStart('T')) >= int.Parse(start) && int.Parse(names[i].start.TrimStart('T')) + 1 <= int.Parse(end) && names[i].speaker == sp)
                                {
                                    string refname = childnode.InnerText;
                                    int index = 0; List<int> ind = new List<int>();
                                    while ((index = refname.IndexOf(".", index)) != -1)
                                    {
                                        ind.Add(index);
                                        index++;
                                    }
                                    index = 0;
                                    foreach (int tt in ind)
                                    {
                                        string res = "1" + refname[tt + 1];
                                        if (int.TryParse(res, out int result11))
                                        {
                                            if (index == 0 || tt < index) index = tt;
                                        }
                                    }
                                    names[i].refname = refname.Remove(index);
                                    string refnum = childnode.InnerText.Substring(index + 1);
                                    if (refnum.Contains(" ")) names[i].refnum = int.Parse(refnum.Remove(refnum.IndexOf(" ")));
                                }
                                if (xnode.Attributes["category"].Value == "ts" && childnode.Name.Contains("event") && int.Parse(names[i].start.TrimStart('T')) >= int.Parse(start) && int.Parse(names[i].start.TrimStart('T')) + 1 <= int.Parse(end) && names[i].speaker == sp)
                                {
                                    names[i].ts = childnode.InnerText;
                                }
                                if (xnode.Attributes["category"].Value == "fr" && childnode.Name.Contains("event") && int.Parse(names[i].start.TrimStart('T')) >= int.Parse(start) && int.Parse(names[i].start.TrimStart('T')) + 1 <= int.Parse(end) && names[i].speaker == sp)
                                {
                                    names[i].fr = childnode.InnerText;
                                }
                                if (xnode.Attributes["category"].Value == "ltr" && names[i].fr == null && childnode.Name.Contains("event") && int.Parse(names[i].start.TrimStart('T')) >= int.Parse(start) && int.Parse(names[i].start.TrimStart('T')) + 1 <= int.Parse(end) && names[i].speaker == sp)
                                {
                                    names[i].fr = childnode.InnerText;
                                }
                            }
                        }
                    }
                }
            }
            begin = 0;
        }
        static void Main(string[] args)
        {
            List<Names> names = new List<Names>();
            DirectoryInfo directoryInfo = new DirectoryInfo("SelkupCorpus-v01");
            DirectoryInfo directoryInfo1 = new DirectoryInfo("dolgan-noAudio");
            DirectoryInfo directoryInfo2 = new DirectoryInfo("kamas_noAudio");
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            directories.Add(directoryInfo);
            directories.Add(directoryInfo1);
            directories.Add(directoryInfo2);
            for (int dir = 0; dir < directories.Count; dir++)
            {
                foreach (var file in directories[dir].GetDirectories())
                {
                    if (dir == 0)
                    {
                        foreach (var file1 in file.GetDirectories())
                        {
                            foreach (var file2 in file1.GetDirectories())
                            {
                                foreach (var file3 in file2.GetFiles())
                                {
                                    List<string> d = new List<string>();
                                    d.Add(file.Name); d.Add(file1.Name); d.Add(file2.Name); d.Add(file3.Name);
                                    AllNames(d, dir, names, "nprop");
                                }
                            }
                        }
                    }
                    else if (file.Name != "documentation")
                    {
                        foreach (var file1 in file.GetDirectories())
                        {
                            foreach (var file2 in file1.GetFiles())
                            {
                                if (file2.Name.Contains(".exb"))
                                {
                                    List<string> d = new List<string>();
                                    d.Add(file.Name); d.Add(file1.Name); d.Add(file2.Name);
                                    AllNames(d, dir, names, "propr");
                                }
                            }
                        }
                    }
                }
            }
            int[] nodelete = { 15, 18, 21, 22, 24, 25, 26, 27, 28, 29, 30, 31, 34, 35, 37, 42, 44, 45, 46, 47, 48, 49, 52, 54, 55, 56, 57, 58, 61, 62, 63, 64, 65, 69, 79, 84, 89, 92, 93, 98, 99, 100, 101, 106, 115, 116, 127, 134, 141, 142, 143, 146, 147, 148, 149, 153, 154, 156, 159, 161, 162, 163, 175, 205, 207, 209, 215, 220, 224, 232, 234, 235, 237, 239, 240, 251, 252, 253, 259, 261, 265, 266, 270, 271, 273, 274, 285, 291, 293, 294, 295, 296, 301, 302, 304, 305, 307, 308, 309, 310, 314, 315, 318, 319, 325, 326, 329, 330, 332, 335, 338, 339, 340, 341, 342, 344, 345, 348, 354, 357, 358, 361, 362, 371, 372, 373, 374, 378, 379, 380, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 406, 407, 408, 434, 438, 444, 447, 448, 452, 454, 455, 464, 471, 477, 478, 479, 480, 481, 482, 483, 484, 485, 487, 499, 503, 504, 505, 510, 512, 515, 520, 521, 522, 528, 530, 531, 532, 534, 535, 536, 539, 540, 549, 554, 555, 558, 559, 560, 563, 564, 573, 574, 575, 576, 577, 578, 579, 580, 581, 582, 583, 584, 586, 587, 588, 589, 590, 593, 599, 600, 601, 602, 609, 610, 611, 612, 614, 617, 622, 654, 655, 656, 659, 660, 661, 662, 663, 664, 665, 666, 668, 673, 681, 682, 683, 693, 699, 701, 704, 720, 727, 731, 732, 736, 739, 748, 749, 750, 765, 766, 767, 768, 771, 776, 777, 781, 785, 786, 787, 788, 789, 793, 794, 795, 805, 807, 814, 815, 816, 838, 841, 842, 846, 847, 848, 852, 853, 857, 862, 877, 878, 879, 880, 881, 882, 883, 884, 885, 886, 887, 891, 892, 896, 899, 901, 902, 904, 906, 907, 908, 914, 916, 917, 918, 920, 923, 924, 926, 932, 933, 941, 942, 943, 944, 945, 946, 947, 948, 949, 950, 952, 953, 954, 960, 961, 967, 968, 969, 970, 971, 976, 977, 978, 979, 980, 981, 982, 983, 984, 990, 991, 992, 993, 995, 996, 999, 1000, 1003, 1004, 1005, 1009, 1010, 1012, 1013, 1014 };
            for (int i = names.Count - 1, j = nodelete.Length - 1; i >= 0; i--)
            {
                if (j < 0) names.RemoveAt(i);
                else
                {
                    if (i != nodelete[j]) names.RemoveAt(i);
                    else if (i == nodelete[j]) j--;
                }
            }
            foreach (Names cons in names)
            {
                Console.WriteLine(cons.name);
                Console.WriteLine(cons.refname + " " + cons.refnum);
                Console.WriteLine(cons.speaker+" "+cons.language);
                Console.WriteLine(cons.ts);
                Console.WriteLine(cons.language);
                Console.WriteLine(cons.fr + "\n");
            }

            /*XmlDocument namesInfo = new XmlDocument();
            namesInfo.Load("XMLFile1.xml");
            foreach (Names cons in names)
            {
                XmlElement highlightStyle = namesInfo.CreateElement("TextName");
                highlightStyle.SetAttribute("name", getText(cons.refname));
                namesInfo["Document"].AppendChild(highlightStyle);
                XmlElement PlaceNode = namesInfo.CreateElement("Place");
                highlightStyle.AppendChild(PlaceNode);
                if (cons.name == "Ленинград")
                    cons.name = "Санкт-Петербург";
                XmlText PlaceTxt = namesInfo.CreateTextNode(cons.name);
                PlaceNode.AppendChild(PlaceTxt);
                XmlElement DateNode = namesInfo.CreateElement("RecordDate");
                DateNode.SetAttribute("date", getDate(cons.refname));
                highlightStyle.AppendChild(DateNode);
                XmlElement SpeakerNode = namesInfo.CreateElement("Speaker");
                SpeakerNode.SetAttribute("speaker", cons.speaker);
                highlightStyle.AppendChild(SpeakerNode);
                XmlElement LanNode = namesInfo.CreateElement("Language");
                LanNode.SetAttribute("lang", cons.language);
                highlightStyle.AppendChild(LanNode);
            }
            namesInfo.Save("namesInfo.xml");*/
            Console.ReadLine();
        }
    }
}