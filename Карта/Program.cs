using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System.Runtime.Serialization.Formatters.Binary;


namespace Карта
{
    class Program
    {
        public static void Create(XmlDocument Map, XmlNode xnode, string coText, string xcoText, string speaker)
        {
            XmlElement NewX = Map.CreateElement("Placemark");
            Map["kml"]["Document"].AppendChild(NewX);

            XmlElement nameX = Map.CreateElement("name");
            NewX.AppendChild(nameX);
            XmlText nameText = Map.CreateTextNode(speaker);
            nameX.AppendChild(nameText);

            XmlElement styleNode = Map.CreateElement("styleUrl");
            NewX.AppendChild(styleNode).AppendChild(Map.CreateTextNode("#dlg"));

            XmlElement data = Map.CreateElement("Data");
            data.SetAttribute("name", "Type");
            XmlElement value = Map.CreateElement("value");
            XmlText valueText = Map.CreateTextNode("Path");
            NewX.AppendChild(data).AppendChild(value).AppendChild(valueText);

            XmlElement lineNode = Map.CreateElement("LineString");
            NewX.AppendChild(lineNode);

            XmlElement exNode = Map.CreateElement("extrude");
            lineNode.AppendChild(exNode).AppendChild(Map.CreateTextNode("1"));
            XmlElement teNode = Map.CreateElement("tessellate");
            lineNode.AppendChild(teNode).AppendChild(Map.CreateTextNode("1")); 
            XmlElement alNode = Map.CreateElement("altitudeMode");
            lineNode.AppendChild(alNode).AppendChild(Map.CreateTextNode("clampToGround"));
            XmlElement coNode = Map.CreateElement("coordinates");
            lineNode.AppendChild(coNode);
            coNode.AppendChild(Map.CreateTextNode(xcoText));
            coNode.AppendChild(Map.CreateTextNode(" "));
            coNode.AppendChild(Map.CreateTextNode(coText));
        }

        static void Main(string[] args)         
        {
            string style = "", furstyle="",titleEN = "", titleRU = "", date = "", language = "", settlement = "", latitude = "", longitude = "", name="", furname = "", furdate = "", furtitleEN = "",coTexts="", xcoTexts="", speaker = "", furspeaker="", ages=""; int num = 1, agei=0; double lat =0, longi=0, furlat=0, furlongi=0, change=0.01;bool ll = false,notxt=false;

            XmlDocument doc = new XmlDocument();
            XmlText coText = doc.CreateTextNode(""), xcoText = doc.CreateTextNode("");
            doc.Load("allKML.kml");
            XmlNodeList xnodeList = doc.GetElementsByTagName("Placemark");

            XmlDocument Map = new XmlDocument();
            Map.Load("start.xml");

            foreach (XmlNode xnode in xnodeList)
            {
                foreach (XmlNode node in xnode.ChildNodes)
                {
                    if (node.Name == "Style" && node.Attributes["id"].Value == "highlightPlacemark")
                        ll = true;
                    if (node.Name == "Point" && ll)
                        coTexts = node["coordinates"].InnerText;
                    if (node.Name == "name")
                    {
                        name = node.InnerText;
                        if (node.InnerText.Contains(" "))
                            name = node.InnerText.Replace(" ", "");
                    }
                    if (node.Name == "ExtendedData" && ll)
                    {
                        foreach (XmlNode el in node.ChildNodes)
                        {
                            if (el.Attributes["name"].Value == "Type"&&el.InnerText!="Text") ll = false;
                            if (el.Attributes["name"].Value == "language" && el.InnerText == "xas") ll = false;
                            if (el.Attributes["name"].Value == "language" && el.InnerText == "sel") ll = false;
                            if (el.Attributes["name"].Value == "speakers")
                                speaker = el.InnerText;
                        }
                    }
                }
                if (ll)
                {
                    foreach (XmlNode nod in xnodeList)
                    {
                        foreach (XmlNode no in nod.ChildNodes)
                        {
                            /*if (no.Name == "styleUrl" && no.InnerText == "#namesPlacemark")
                                notxt = true;*/
                            if (no. Name == "Point")
                                xcoTexts = no["coordinates"].InnerText;
                            if (no.Name == "ExtendedData")
                            {
                                foreach (XmlNode el in no.ChildNodes)
                                {
                                    if (el.Attributes["name"].Value == "Type" && el.InnerText != "Text" && el.InnerText != "From Text") notxt = true;
                                    if (el.Attributes["name"].Value == "language" && el.InnerText == "xas") ll = false;
                                    if (el.Attributes["name"].Value == "language" && el.InnerText == "sel") ll = false;
                                    if (el.Attributes["name"].Value == "speakers")
                                        furspeaker = el.InnerText;
                                    if (el.Attributes["name"].Value == "TitleText")
                                        furname = el.InnerText;
                                }
                            }
                        }
                        if (speaker == furspeaker && coTexts != xcoTexts && furspeaker != "" && notxt)
                        {
                            Create(Map, xnode, coTexts, xcoTexts, speaker);
                        }
                        notxt = false;
                    }
                }
                ll = false;
            }
            Map.Save("attempt.kml");

            ////добавление путей/////
            /*XmlDocument doc = new XmlDocument();
            XmlText coText = doc.CreateTextNode(""), xcoText = doc.CreateTextNode("");
            doc.Load("sel_link.kml");
            XmlNodeList xnodeList = doc.GetElementsByTagName("Placemark");

            XmlDocument Map = new XmlDocument();
            Map.Load("start.xml");

            foreach (XmlNode xnode in xnodeList)
            {
                foreach (XmlNode node in xnode.ChildNodes)
                {
                    if (node.Name == "Style" && node.Attributes["id"].Value == "highlightPlacemark")
                        ll = true;
                    if (node.Name == "Point" && ll)
                        coTexts = node["coordinates"].InnerText;
                    if (node.Name == "name" ) 
                        name = node.InnerText;
                    if (node.Name == "ExtendedData" && ll)
                    {
                        foreach (XmlNode el in node.ChildNodes)
                        {
                            if (el.Attributes["name"].Value == "Type") ll = false;
                            if (el.Attributes["name"].Value == "speakers")
                                speaker = el.InnerText;
                        }
                    }
                }
                if (ll)
                {
                    foreach (XmlNode nod in xnodeList)
                    {
                        foreach (XmlNode no in nod.ChildNodes)
                        {
                            if (no.Name == "styleUrl" && no.InnerText == "#namesPlacemark")
                                notxt = true;
                            if (no.Name == "Point")
                                xcoTexts = no["coordinates"].InnerText;
                            if (no.Name == "ExtendedData")
                            {
                                foreach (XmlNode el in no.ChildNodes)
                                {
                                    //if (el.Attributes["name"].Value == "Type") notxt = true;
                                    if (el.Attributes["name"].Value == "speakers")
                                        furspeaker = el.InnerText;
                                    if (el.Attributes["name"].Value == "TitleText")
                                        furname = el.InnerText;
                                }
                            }
                        }
                        if (name == furname && coTexts != xcoTexts && furspeaker != "" && notxt)
                        {
                            Create(Map, xnode, coTexts, xcoTexts, speaker);
                        }
                        notxt = false;
                    }
                }
                ll = false;
            }
            Map.Save("sel_link.kml");*/

            ////добавление тэга ages/////
            /*            XmlDocument doc = new XmlDocument();
            doc.Load("xas_link.kml");
            XmlNodeList xnodeList = doc.GetElementsByTagName("ExtendedData");
            foreach (XmlNode xnode in xnodeList)
            {
                foreach (XmlNode node in xnode.ChildNodes)
                {
                    //if (node.Attributes["name"].Value == "Date of birth" && node.InnerText != "...") ll = true;
                    if (node.Attributes["name"].Value == "age" && ll)
                    {
                        node["value"].InnerText = "0";
                    }
                    if (node.Attributes["name"].Value == "age" && ll)
                    {
                        node["value"].InnerText = "0";
                    }
            if (node.Attributes["name"].Value == "Type")
                ll = true;
            if (node.Attributes["name"].Value == "Date of birth" && node.InnerText != "...")
            {
                if (node.InnerText.Length > 4)
                {
                    if (!node.InnerText.Remove(4).Contains("."))
                        ages = node.InnerText.Remove(4);
                    else ages = node.InnerText.Remove(0, node.InnerText.Length - 4);
                    agei = int.Parse(ages);
                }
                else agei = int.Parse(node.InnerText);
            }
            if (node.Attributes["name"].Value == "age")
            {
                if (ages == "" && agei == 0) agei = 0;
                else agei = 2019 - agei;
                if (agei == 0) ages = "...";
                else ages = agei.ToString();
                node["value"].InnerText = ages;
            }
        }
                if (ll)
                {
                    if (ages == "" && agei == 0) agei = 0;
                    else agei = 2019 - agei;
                    if (agei == 0) ages = "...";
                    else ages = agei.ToString();
                    XmlElement data = doc.CreateElement("Data");
        data.SetAttribute("name", "age");
                    XmlElement value = doc.CreateElement("value");
        XmlText valueText = doc.CreateTextNode(ages);

        xnode.AppendChild(data).AppendChild(value).AppendChild(valueText);
        doc.Save("try.xml");
                }
    ll = false; ages = ""; agei = 0;
            }
doc.Save("xas_link.kml");*/

                                      ////Сдвиг координат/////
            /*XmlDocument doc = new XmlDocument();
            doc.Load("AllMap.kml");
            XmlNodeList xnodeList = doc.GetElementsByTagName("Placemark");

            foreach (XmlNode xnode in xnodeList)
            {
                foreach (XmlNode node in xnode.ChildNodes)
                {
                    if(node.Name=="name") 
                    {
                        name = node.InnerText;
                    }
                    if (node.Name == "styleUrl")
                    {
                        style = node.InnerText;
                    }
                    if (node.Name == "Style")
                    {
                        style = node.Attributes["id"].Value;
                    }
                    if (node.Name == "ExtendedData")
                    {
                        foreach(XmlNode el in node.ChildNodes)
                        {
                            if (el.Attributes["name"].Value == "titleEN")
                            {
                                titleEN = node.InnerText;
                            }
                            if (el.Attributes["name"].Value == "recordDate")
                            {
                                date = node.InnerText;
                            }
                            if (el.Attributes["name"].Value == "speakerName")
                            {
                                speaker = node.InnerText;
                            }
                        }
                    }
                    if (node.Name=="Point")
                    {
                        latitude = node["coordinates"].InnerText.Remove(0, node["coordinates"].InnerText.IndexOfAny("0123456789".ToCharArray()));
                        longitude = latitude.Remove(0, latitude.IndexOf(",") + 1);
                        if (longitude.Contains(","))
                            longitude = longitude.Remove(longitude.IndexOf(","));
                        latitude = latitude.Remove(latitude.IndexOf(","));
                        lat = Convert.ToDouble(latitude.Replace(".", ","));
                        longi = Convert.ToDouble(longitude.Replace(".", ","));
                        foreach (XmlNode xxnode in xnodeList)
                        {
                            foreach (XmlNode nod in xxnode.ChildNodes)
                            {
                                if (nod.Name == "name")
                                {
                                    furname = nod.InnerText;
                                }
                                if (node.Name == "styleUrl")
                                {
                                    furstyle = node.InnerText;
                                }
                                if (node.Name == "Style")
                                {
                                    furstyle = node.Attributes["id"].Value;
                                }
                                if (node.Name == "ExtendedData")
                                {
                                    foreach (XmlNode el in node.ChildNodes)
                                    {
                                        if (el.Attributes["name"].Value == "titleEN")
                                        {
                                            furtitleEN = node.InnerText;
                                        }
                                        if (el.Attributes["name"].Value == "recordDate")
                                        {
                                            furdate = node.InnerText;
                                        }
                                        if (el.Attributes["name"].Value == "speakerName")
                                        {
                                            furspeaker = node.InnerText;
                                        }
                                    }
                                }
                                if (furname != name|| titleEN!= furtitleEN|| date!= furdate|| speaker!= furspeaker|| style!= furstyle)
                                {
                                    if (nod.Name == "Point")
                                    {
                                        latitude = nod["coordinates"].InnerText.Remove(0, nod["coordinates"].InnerText.IndexOfAny("0123456789".ToCharArray()));
                                        longitude = latitude.Remove(0, latitude.IndexOf(",") + 1);
                                        if(longitude.Contains(","))
                                            longitude = longitude.Remove(longitude.IndexOf(","));
                                        latitude = latitude.Remove(latitude.IndexOf(","));
                                        furlat = Convert.ToDouble(latitude.Replace(".", ","));
                                        furlongi = Convert.ToDouble(longitude.Replace(".", ","));
                                        if (lat == furlat && longi == furlongi)
                                        {
                                            if (ll)
                                            {
                                                if (num == 1)
                                                    furlongi += change;
                                                if (num == 2)
                                                    furlat += change;
                                            }
                                            if (!ll)
                                            {
                                                if (num == 3)
                                                    furlongi -= change;
                                                if (num == 4)
                                                    furlat -= change;
                                            }
                                            if (num == 2)
                                            {
                                                ll = false;
                                            }
                                            if (num == 4)
                                            {
                                                change += 0.01;
                                                ll = true;
                                                num = 1;
                                            }
                                            else num++;
                                            string txt = String.Format("{0},{1}", furlat.ToString().Replace(",", "."), furlongi.ToString().Replace(",", "."));
                                            nod["coordinates"].InnerText = txt;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    num = 1;
                    ll = true;
                }
            }
            doc.Save("allMap.kml");
                        
            ////Я записала файлы/////
            XmlDocument d2 = new XmlDocument();
            d2.Load("namesInfo.xml");
            XmlNodeList xnodeList = d2.GetElementsByTagName("TextName");

            XmlDocument namesMap = new XmlDocument();
            //namesMap.Load("map.kml");
            namesMap.Load("namesInfo.xml");
            XmlElement highlightStyle = namesMap.CreateElement("Style");
            highlightStyle.SetAttribute("id", "namesPlacemark");
            XmlElement IconStyleH = namesMap.CreateElement("IconStyle");
            XmlElement IconH = namesMap.CreateElement("Icon");
            XmlElement link = namesMap.CreateElement("href");
            namesMap["kml"]["Document"].AppendChild(highlightStyle).AppendChild(IconStyleH).AppendChild(IconH).AppendChild(link);
            XmlText linkText = namesMap.CreateTextNode("name.png");
            link.AppendChild(linkText);

            foreach (XmlNode xnode in xnodeList)
            {
                XmlElement placeNode = namesMap.CreateElement("Placemark");
                namesMap["kml"]["Document"].AppendChild(placeNode);

                if (xnode.InnerText.Contains("%")) name = xnode["Place"].InnerText.Remove(0, 1);
                else name = xnode["Place"].InnerText;
                if (xnode.InnerText=="Черное") name = "Черное море";
                else name = xnode["Place"].InnerText;

                XmlElement nameNode = namesMap.CreateElement("name");
                XmlText nameText = namesMap.CreateTextNode(name);
                placeNode.AppendChild(nameNode);
                nameNode.AppendChild(nameText);

                XmlElement descNode = namesMap.CreateElement("description");
                placeNode.AppendChild(descNode);

                XmlElement StyleNode= namesMap.CreateElement("styleUrl");
                placeNode.AppendChild(StyleNode);

                XmlText styleText = namesMap.CreateTextNode("namesPlacemark");
                StyleNode.AppendChild(styleText);

                XmlElement pointNode = namesMap.CreateElement("Point");
                placeNode.AppendChild(pointNode);

                PointLatLng? pointLatLng = GMapProviders.OpenStreetMap.GetPoint(name, out GeoCoderStatusCode status);
                if (status == GeoCoderStatusCode.OK)
                {
                    longitude = pointLatLng.Value.Lng.ToString().Replace(",", ".");
                    latitude = pointLatLng.Value.Lat.ToString().Replace(",", ".");
                }
                XmlElement coordNode = namesMap.CreateElement("coordinates");
                XmlText coordText = namesMap.CreateTextNode(String.Format("{0},{1}", longitude, latitude));
                pointNode.AppendChild(coordNode);
                coordNode.AppendChild(coordText);

                XmlElement exData = namesMap.CreateElement("ExtendedData");
                placeNode.AppendChild(exData);

                XmlElement data = namesMap.CreateElement("Data");
                data.SetAttribute("name", "TitleText");
                XmlElement value = namesMap.CreateElement("value");
                XmlText valueText = namesMap.CreateTextNode(xnode.Attributes["name"].Value);
                exData.AppendChild(data).AppendChild(value).AppendChild(valueText);

                XmlElement data1 = namesMap.CreateElement("Data");
                data1.SetAttribute("name", "SpeakerName");
                XmlElement value1 = namesMap.CreateElement("value");
                XmlText value1Text = namesMap.CreateTextNode(xnode["Speaker"].Attributes["speaker"].Value);
                exData.AppendChild(data1).AppendChild(value1).AppendChild(value1Text);

                XmlElement data2 = namesMap.CreateElement("Data");
                data2.SetAttribute("name", "RecordDate");
                XmlElement value2 = namesMap.CreateElement("value");
                XmlText value2Text = namesMap.CreateTextNode(xnode["RecordDate"].Attributes["date"].Value);
                exData.AppendChild(data2).AppendChild(value2).AppendChild(value2Text);

                XmlElement data3 = namesMap.CreateElement("Data");
                data3.SetAttribute("name", "Language");
                XmlElement value3 = namesMap.CreateElement("value");
                XmlText value3Text = namesMap.CreateTextNode(xnode["Language"].Attributes["lang"].Value);
                exData.AppendChild(data3).AppendChild(value3).AppendChild(value3Text);

                XmlElement data4 = namesMap.CreateElement("Data");
                data4.SetAttribute("name", "Longitude");
                XmlElement value4 = namesMap.CreateElement("value");
                XmlText value4Text = namesMap.CreateTextNode(longitude);
                exData.AppendChild(data4).AppendChild(value4).AppendChild(value4Text);

                XmlElement data5 = namesMap.CreateElement("Data");
                data5.SetAttribute("name", "Latitude");
                XmlElement value5 = namesMap.CreateElement("value");
                XmlText value5Text = namesMap.CreateTextNode(latitude);
                exData.AppendChild(data5).AppendChild(value5).AppendChild(value5Text);
            }*/
        }
    }
}