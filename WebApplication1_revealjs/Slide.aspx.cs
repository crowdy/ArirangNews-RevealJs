using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1_revealjs.App_Code;

namespace WebApplication1_revealjs
{
    public partial class Slide : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ArirangNewsPost post = null;

            string seq = Request["seq"];
            if (string.IsNullOrEmpty(seq))
                post = ArirangNewsScrapper.GetTopIndex();
            else
                post = ArirangNewsScrapper.GetNextNewsFrom(seq);

            // articles
            var sb = new System.Text.StringBuilder();

            if (! String.IsNullOrEmpty(post.Body))
                foreach (var sentence in ArirangNewsScrapper.TextToSentences(post.Body))
                    sb.Append("<section><h2></h2><p>").Append(sentence).AppendLine("</p></section>\n");

            // index
            sb.AppendLine("<section><p>news index</p>");

            // last indexes
            foreach (var s in post.SeqTitle.Keys.OrderByDescending(a => a))
            {
                sb.Append("<section><p>")
                    .Append(string.Format("<a href=\"/slide?seq={0}\">{1}</a>", s, post.SeqTitle[s]))
                    .AppendLine("<br/></p></section>\n");
            }

            //
            sb.AppendLine("</section>");

            ltContent.Text = sb.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SlideSectionContext
    {
        public static IEnumerable<Section> GetSections()
        {
            return Enumerable.Empty<Section>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Section
    {
    }
}