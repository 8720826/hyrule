namespace Yes.Domain.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToSafeTime(this DateTime dateTime)
        {
            var minDateTime = DateTime.Parse("2000-1-1");
            if (dateTime.Subtract(minDateTime).TotalMinutes < 0)
            {
                return minDateTime;
            }
            return dateTime;
        }
    }
}
