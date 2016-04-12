using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace ConsoleSkimur
{
    class Program
    {
        static void Main(string[] args)
        {
            //sets up the site for reading
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(new WebClient().DownloadString("https://skimur.com"));
            var aTags = doc.DocumentNode.SelectNodes("//a");

            //create the variables for holding links and text
            string insideText;
            string hrefLink;

            //if there are a tags
            if (aTags != null)
            {
                //loop through each tag
                foreach (var tag in aTags)
                {
                    //if the class and href values are present
                    if (tag.Attributes["class"] != null && tag.Attributes["href"] != null)
                    {
                        //check if they are links to flocks or articles
                        if (tag.Attributes["href"].Value.Contains("http") || tag.Attributes["href"].Value.Contains(System.Text.RegularExpressions.Regex.Unescape("/s/")))
                        {
                            //get inside text and links
                            insideText = tag.InnerText;
                            hrefLink = tag.Attributes["href"].Value;

                            /*for some reason this isnt working
                            if (tag.InnerText.Contains("&quot"))
                            {
                                insideText = tag.InnerText;
                                insideText.Replace("&quot;", "``");
                            }*/

                            //if the href contains a flock then add https://skimur.com to it
                            if (hrefLink.Contains(System.Text.RegularExpressions.Regex.Unescape("/s/")))
                            {
                                hrefLink = "https://skimur.com" + hrefLink;
                            }

                            //output the inner text and the link
                            Console.WriteLine(tag.InnerText + "-" + hrefLink);
                        }
                    }
                }
            }
            //keep the process from stopping
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
