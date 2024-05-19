namespace PointOfSale.Helpers
{
    public static class Helpers
    {
        public static string DateFormat(this DateTime? value)
        {
            DateTime v2 = (DateTime)value;

            return v2.ToString("dd/MM/yyyy");

        }
        public static string DatetimeFormat(this DateTime? value)
        {
            DateTime v2 = (DateTime)value;

            return v2.ToString("dddd, dd MMM h:mm tt");

        }
    }
}
