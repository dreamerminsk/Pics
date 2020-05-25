using System;
using System.Text.RegularExpressions;

namespace Rater.Utils
{
    public struct MonthYear : IComparable<MonthYear>
    {
        //private const string MonthYearFormat = "^((?<mes>MM).*(?<ano>YYYY))|((?<ano>YYYY).*(?<mes>MM))|((?<mes>MM).*(?<ano>YY))|((?<ano>YY).*(?<mes>MM))$";

        public int Month { get; private set; }

        public int Year { get; private set; }

        public MonthYear(DateTime day)
        {
            Month = day.Month;
            Year = day.Year;
        }

        public MonthYear(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month));
            this.Month = month;
            this.Year = year;
        }

        public DateTime GetDate(int day)
        {
            return new DateTime(this.Year, this.Month, day);
        }

        public DateTime GetFirstDay()
        {
            return this.GetDate(1);
        }

        //public DateTime GetLastDay()
        //{
        //return this.GetDate(1).LastDayOfMonth();
        //}

        public MonthYear AddMonths(int months)
        {
            return new MonthYear(GetDate(1).AddMonths(months));
        }

        public MonthYear Next()
        {
            return this.AddMonths(1);
        }

        public MonthYear Previous()
        {
            return this.AddMonths(-1);
        }

        public int CompareTo(MonthYear other)
        {
            return this.GetHashCode() - other.GetHashCode();
        }

        public override string ToString()
        {
            return this.GetDate(1).ToString("yyyy-MM");
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is MonthYear monthYear && this == monthYear;
        }

        public override int GetHashCode()
        {
            return this.Year * 12 + this.Month;
        }

        public static MonthYear Parse(string strValue)
        {
            string[] strArray = strValue.Split('/');
            if (strArray.Length != 2)
                throw new FormatException();
            return new MonthYear(int.Parse(strArray[0]), int.Parse(strArray[1]));
        }

        public static MonthYear Parse(string strValue, string format)
        {
            Match match = Regex.Match(format, "^((?<mes>MM).*(?<ano>YYYY))|((?<ano>YYYY).*(?<mes>MM))|((?<mes>MM).*(?<ano>YY))|((?<ano>YY).*(?<mes>MM))$", RegexOptions.IgnoreCase);
            if (match == null || !match.Success)
                throw new FormatException();
            return new MonthYear(int.Parse(MonthYear.PartialParse(strValue, format, match.Groups["mes"].Value)), int.Parse(MonthYear.PartialParse(strValue, format, match.Groups["ano"].Value)));
        }

        private static string PartialParse(string strValue, string format, string part)
        {
            return strValue.Substring(format.IndexOf(part), part.Length);
        }

        public static bool operator ==(MonthYear a, MonthYear b)
        {
            return a.GetHashCode() == b.GetHashCode();
        }

        public static bool operator !=(MonthYear a, MonthYear b)
        {
            return a.GetHashCode() != b.GetHashCode();
        }

        public static bool operator <(MonthYear a, MonthYear b)
        {
            return a.GetHashCode() < b.GetHashCode();
        }

        public static bool operator >(MonthYear a, MonthYear b)
        {
            return a.GetHashCode() > b.GetHashCode();
        }

        public static bool operator <=(MonthYear a, MonthYear b)
        {
            return a.GetHashCode() <= b.GetHashCode();
        }

        public static bool operator >=(MonthYear a, MonthYear b)
        {
            return a.GetHashCode() >= b.GetHashCode();
        }

        public static MonthYear operator +(MonthYear a, int b)
        {
            return a.AddMonths(b);
        }

        public static MonthYear operator -(MonthYear a, int b)
        {
            return a.AddMonths(-b);
        }

        public static int operator -(MonthYear a, MonthYear b)
        {
            return a.GetHashCode() - b.GetHashCode();
        }

    }
}
