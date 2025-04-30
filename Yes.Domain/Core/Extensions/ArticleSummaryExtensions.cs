

namespace Yes.Domain.Core.Extensions
{
    public static class ArticleSummaryExtensions
    {

        public static string ToSummary(this string content)
        {
            // 去除 HTML 标签
            string text = RemoveHtmlTags(content);
            // 去除 Markdown 标签（简单处理，可能不全面）
            text = Regex.Replace(text, @"\!\[.*?\]\(.*?\)|\[.*?\]\(.*?\)|\*\*.*?\*\*|\*.*?\*|~~.*?~~", "");


            int endIndex = GetEndIndex(text, 50, 100);
            if (endIndex < 50)
            {
                return text;
            }
            else
            {
                return text[..(endIndex + 1)];
            }
        }


        private static string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, @"<[^>]*>", "");
        }

        private static int GetEndIndex(string text, int start, int end)
        {
            if (text.Length < start)
            {
                return -1;
            }

            end = Math.Min(end, text.Length - 1);

            for (int i = end; i >= start; i--)
            {
                if (".。!?”".Contains(text[i]))
                {
                    return i;
                }
            }
            return end;
        }
    }
}
