using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Wordstudyweb.Models
{
    public class Common
    {


        public string Dateconverter(DateTime date)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

     


    }
    public class BibleService
    {
        public HttpClient client = new HttpClient();
        public static string ScriptureUrl = "https://api.scripture.api.bible/v1/bibles/de4e12af7f28f599-01/verses/Gen.1.22?content-type=json&include-notes=false&include-titles=false&include-verse-spans=false&use-org-id=false";
        //   public static string BaseUrl = "https://api.scripture.api.bible/v1/bibles/";
        public static string BaseUrl = "https://bible-api.com/";
        public BibleService()
        {

            //Passing service base url  
            client.BaseAddress = new Uri(BaseUrl);
            //   key: "api-key" Value: "5963c463a5479385d10bf2b4849fdfdd"
            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           //client.DefaultRequestHeaders.Add("api-key" , "5963c463a5479385d10bf2b4849fdfdd");
            // check if user is logged in
         
        }

        public async Task<string> GetScriptureKJV(Anchor _anchor)
        {
            if (_anchor.VersrTo > _anchor.VerseFrom)
            {
                string passage = "test";


                try
                {
                    //  HttpResponseMessage Res = await client.GetAsync("de4e12af7f28f599-01/verses/" + GetShortName(_anchor.Book) + "." + _anchor.Chapter + "." +verse + "?content-type=json&include-notes=false&include-titles=false&include-verse-spans=false&use-org-id=false");
                      HttpResponseMessage Res = await client.GetAsync("/"+ _anchor.Book  +" "+_anchor.Chapter + ":" +_anchor.VerseFrom+"-"+_anchor.VersrTo + "?translation=kjv");
                    //  HttpResponseMessage Res = await client.GetAsync("/Joshua9:10-11?translation=kjv");

                    if (Res.IsSuccessStatusCode)
                        {

                            string verseJson = Res.Content.ReadAsStringAsync().Result;
                            BibleAPI verses = JsonConvert.DeserializeObject<BibleAPI>(verseJson);
                        passage = verses.text;
                        return passage;
                        }
                        else
                        {
                        passage = "Failed to get verse "+ Res.StatusCode+" "+ Res.RequestMessage ;
                        return passage;

                    }
                }
                     catch
                    {
                    return passage;
                }
            }
            else
            {
                string passage = "test1";

                try
                {
                    //  HttpResponseMessage Res = await client.GetAsync("de4e12af7f28f599-01/verses/" + GetShortName(_anchor.Book) + "." + _anchor.Chapter + "." +verse + "?content-type=json&include-notes=false&include-titles=false&include-verse-spans=false&use-org-id=false");
                    HttpResponseMessage Res = await client.GetAsync(GetShortName("/"+_anchor.Book) + "" + _anchor.Chapter + ":" + _anchor.VerseFrom + "-" + _anchor.VerseFrom + "?translation=kjv");

                    if (Res.IsSuccessStatusCode)
                    {

                        string verseJson = Res.Content.ReadAsStringAsync().Result;
                        BibleAPI verses = JsonConvert.DeserializeObject<BibleAPI>(verseJson);
                        passage = verses.text;
                        return passage;
                    }
                    else
                    {
                        passage = "Failed to get verse " + Res.StatusCode + " " + Res.RequestMessage;
                        return passage;
                    }
                }
                catch
                {
                    return passage;
                }
            }


        }

        public string GetShortName(string long_name) {
            switch (long_name)
            {
                case "Genesis":
                    return "GEN";
                case "Exodus":
                    return "EXO";
                case "Leviticus":
                    return "LEV";

                case "Numbers":
                    return "NUM";

                case "Deuteronomy":
                    return "DEU";
                case "Joshua":
                    return "JOS";

                case "Judges":
                    return "JDG";


                case "Ruth":
                    return "RUT";


                case "1 Samuel":
                    return "1SA";


                case "2 Samuel":
                    return "2SA";


                case "1 Kings":
                    return "1KI";


                case "2 Kings":
                    return "2KI";



                case "1 Chronicles":
                    return "1CH";



                case "2 Chronicles":
                    return "2CH";



                case "Ezra":
                    return "EZR";



                case "Nehemiah":
                    return "NEH";



                case "Esther":
                    return "EST";



                case "Job":
                    return "JOB";



                case "Psalm":
                    return "PSA";



                case "Proverbs":
                    return "PRO";



                case "Ecclesiastes":
                    return "ECC";



                case "Song of Solomon":
                    return "SNG";



                case "Isaiah":
                    return "ISA";
                case "Jeremiah":
                    return "JER";



                case "Lamentations":
                    return "LAM";



                case "Ezekiel":
                    return "EZK";



                case "Daniel":
                    return "DAN";



                case "Hosea":
                    return "HOS";



                case "Joel":
                    return "JOL";



                case "Amos":
                    return "AMO";



                case "Obadiah":
                    return "OBA";


                case "Jonah":
                    return "JON";



                case "Micah":
                    return "MIC";



                case "Nahum":
                    return "NAM";



                case "Habakkuk":
                    return "HAB";



                case "Zephaniah":
                    return "ZEP";



                case "Haggai":
                    return "Hag";



                case "Zechariah":
                    return "HAG";



                case "Malachi":
                    return "MAL";



                case "Matthew":
                    return "MAT";



                case "Mark":
                    return "MRK";



                case "Luke":
                    return "LUK";



                case "John":
                    return "JHN";



                case "Acts":
                    return "ACT";



                case "Romans":
                    return "ROM";



                case "1 Corinthians":
                    return "1CO";



                case "2 Corinthians":
                    return "2CO";



                case "Galatians":
                    return "GAL";



                case "Ephesians":
                    return "EPH";



                case "Philippians":
                    return "Phil";



                case "Colossians":
                    return "COL";



                case "1 Thessalonians":
                    return "1TH";



                case "2 Thessalonians":
                    return "2TH";



                case "1 Timothy":
                    return "1TI";



                case "2 Timothy":
                    return "2TI";



                case "Titus":
                    return "TIT";



                case "Philemon":
                    return "PHM";



                case "Hebrews":
                    return "HEB";



                case "James":
                    return "JAS";



                case "1 Peter":
                    return "1PE";


                case "2 Peter":
                    return "2PE";


                case "1 John":

                    return "1JN";



                case "2 John":
                    return "2JN";

                case "3 John":
                    return "3JN";


                case "Jude":
                    return "JUD";

                case "Revelation":
                    return "REV";

                default:
                    return long_name;

            }

        }
        }

}