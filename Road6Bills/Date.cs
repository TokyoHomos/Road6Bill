using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road6Bills
{
    public class Date
    {
        private string day;
        private string year;
        private string month;

        //--------------------------------------------------
        public Date()
        {

        }
        public Date(string day, string month, string year)
        {
            this.day = day;
            this.year = year;
            this.month = month;
        }
        //--------------------------------------------------
        public override string ToString()
        {
            return $"{this.day}-{this.month}-{this.year}";
        }
        //--------------------------------------------------
        public string GetDay()
        {
            return day;
        }
        public string GetYear()
        {
            return year;
        }
        public string GetMonth()
        {
            return month;
        }
        //--------------------------------------------------
        public void SetDay(string day)
        {
            this.day = day;
        }
        public void SetYear(string year)
        {
            this.year = year;
        }
        public void SetMonth(string month)
        {
            this.month=month;
        }
        //--------------------------------------------------
        //public bool IsEqual(Date date)
        //{
        //    return this.day == day && this.month == month && this.year == year;
        //}
    }
}
