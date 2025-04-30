namespace Yes.Domain.Core.Models
{

    public static class UniqueShortIdGenerator
    {
        private static char[] sc;
        private static DateTime startTime;
        private static long prve = 0;
        private static readonly object _lock = new object();

        static UniqueShortIdGenerator()
        {
            if (sc == null)
            {
                string scString = "0123456789abcdefghijklmnopqrstuvwxyz";
                sc = scString.ToCharArray();
                startTime = new DateTime(2024, 10, 1, 0, 0, 0, 0);
            }
        }

        public static string CreateUniqueShortString()
        {
            lock (_lock)
            {
                TimeSpan ts = DateTime.UtcNow - startTime;
                long temp = Convert.ToInt64(ts.TotalMilliseconds * 10);
                if (temp > prve)
                {
                    prve = temp;
                    return ToShortString(temp);
                }
                else
                {
                    prve++;
                    return ToShortString(prve);
                }
            }
        }

        private static string ToShortString(long num)
        {
            string str = "";
            while (num >= sc.Length)
            {
                str = sc[num % sc.Length] + str;
                num = num / sc.Length;
            }
            return sc[num] + str;
        }
    }
}
