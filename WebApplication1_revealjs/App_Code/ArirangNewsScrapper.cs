using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApplication1_revealjs.App_Code
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ArirangNewsScrapper
    {
        public ArirangNewsScrapper()
        {

        }

        const bool DEBUG = false;

        public static ArirangNewsPost GetTopIndex()
        {
            WebClient wc = new WebClient();
            byte[] docBytes = wc.DownloadData("http://www.arirang.co.kr/News/");
            string doc = Encoding.UTF8.GetString(docBytes);

            HtmlToText htt = new HtmlToText();

            // get other links
            var seqTitle = new Dictionary<string, string>();
            var anchors = htt.GetAllAnchors(doc).Where<HtmlNode>(n => n.OuterHtml.Contains("News_View.asp")
                && !n.OuterHtml.Contains("&amp")
                && !n.OuterHtml.Contains("<img"));
            foreach (HtmlNode node in anchors)
            {
                string pattern = @"=(\d+)"">([^<]+)";
                MatchCollection matches = Regex.Matches(node.OuterHtml, pattern);
                foreach (Match match in matches)
                {
                    if (!seqTitle.ContainsKey(match.Groups[1].Value))
                        seqTitle.Add(match.Groups[1].Value, match.Groups[2].Value);
                }
            }

            return new ArirangNewsPost
            {
                Seq = null,
                Title = null,
                UpdateAt = null,
                Body = null,
                SeqTitle = seqTitle
            };
        }


        public static ArirangNewsPost GetNextNewsFrom(string nseq)
        {

            bool retval = true;
            Trace.WriteLine(string.Format("nseq : {0}", nseq));
            WebClient wc = new WebClient();

            //string url = "http://ko.wikipedia.org/wiki/%EC%9D%B8%ED%84%B0%EB%84%B7";
            //string url = "http://www.wowkorea.jp";

            string url = string.Format("http://www.arirang.co.kr/News/News_View.asp?nseq={0}", nseq);
            byte[] docBytes = null;
            try
            {
                docBytes = wc.DownloadData(url);
            }
            catch (WebException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Status.ToString());
            }

            // get encoding from wc;
            string encodeType = wc.ResponseHeaders["Content-Type"];
            string charsetKey = "charset";
            int pos = encodeType.IndexOf(charsetKey);

            Encoding currentEncoding = Encoding.Default;
            if (pos != -1)
            {
                pos = encodeType.IndexOf("=", pos + charsetKey.Length);
                if (pos != -1)
                {
                    string charset = encodeType.Substring(pos + 1);
                    if (charset.Contains("utf-8"))
                        currentEncoding = Encoding.UTF8;
                }
            }

            string doc = currentEncoding.GetString(docBytes);

            HtmlToText htt = new HtmlToText();
            string innerText = htt.ConvertHtmlToText(doc);

            if (innerText.IndexOf("lastest news") < 0)
                throw new ArgumentException("\"lastest news\" is not found");

            string strTarget = GetStringBetween(innerText, "googleplusprint", "Reporter");
            if (DEBUG)
                Trace.WriteLine(innerText);

            // get title
            string strTitle = GetStringBetween(strTarget, "", "KST").Trim();
            string strTime = GetStringBetween(strTitle, "Updated: ", null);
            strTitle = strTitle.Substring(0, strTitle.IndexOf("Updated: "));

            if (DEBUG)
            {
                Trace.Write("Title : " + strTitle);
                Trace.Write("Time: " + strTime);
            }
            // get body
            strTarget = GetStringBetween(strTarget, "KST", null);
            string strBody = GetStringBetween(strTarget, "\n", null).Trim();
            if (DEBUG)
            {
                Trace.WriteLine("Body : " + strBody);
            }

            // get other links
            var seqTitle = new Dictionary<string, string>();
            var anchors = htt.GetAllAnchors(doc).Where<HtmlNode>(n => n.OuterHtml.Contains("News_View.asp")
                && !n.OuterHtml.Contains("&amp")
                && !n.OuterHtml.Contains("<img"));
            foreach (HtmlNode node in anchors)
            {
                string pattern = @"=(\d+)"">([^<]+)";
                MatchCollection matches = Regex.Matches(node.OuterHtml, pattern);
                foreach (Match match in matches)
                {
                    if (!seqTitle.ContainsKey(match.Groups[1].Value))
                        seqTitle.Add(match.Groups[1].Value, match.Groups[2].Value);
                }
            }

            return new ArirangNewsPost
            {
                Seq = nseq,
                Title = strTitle.Trim(),
                UpdateAt = strTime.Trim(),
                Body = strBody.Trim(),
                SeqTitle = seqTitle
            };

            /*
            // add title and time to c:\crowdoc\dochome\data\news\index.txt
            string newsIndexPath = @"c:\crowdoc\dochome\data\news\index.txt";
            string newsDetailPath = string.Format(@"c:\crowdoc\dochome\data\news\{0}.txt", nseq);
            string txt = System.IO.File.ReadAllText(newsIndexPath);
            string aLine = string.Format("{0} {1} \t\t\t--->edit {2}.txt\n", strTime, strTitle, nseq);
            System.IO.File.WriteAllText(newsIndexPath, aLine + txt);
            System.IO.File.WriteAllText(newsDetailPath, strBody);
            */

            // return retval;
        }


        public static string GetStringBetween(string strSource, string strStart, string strEnd)
        {
            string strRet = string.Empty;

            int iStart = 0;
            if (string.IsNullOrEmpty(strStart))
            {
                iStart = 0;
            }
            else
            {
                iStart = strSource.IndexOf(strStart);
            }
            if (iStart < 0)
                return strRet;

            int iEnd = 0;
            if (string.IsNullOrEmpty(strEnd))
            {
                iEnd = strSource.Length;
            }
            else
            {
                iEnd = strSource.IndexOf(strEnd, iStart);
            }

            if (iStart >= 0 & iEnd >= 0)
            {
                strRet = strSource.Substring(iStart + strStart.Length, iEnd - (iStart + strStart.Length));
            }

            return strRet;
        }

        public static IEnumerable<string> TextToSentences(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Enumerable.Empty<string>();

            text = text.Replace("(Korean,  ,  )\n", String.Empty);
            return text.Split((new char[] { '\n' }), StringSplitOptions.RemoveEmptyEntries);
        }
    }


    public class ArirangNewsPost
    {
        public string Seq { get; set; }
        public string Title { get; set; }
        public string UpdateAt { get; set; }
        public string Body { get; set; }
        public IDictionary<string, string> SeqTitle { get; set; }

    }
}
