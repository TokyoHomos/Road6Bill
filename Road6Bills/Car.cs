using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road6Bills
{
    public class Car
    {
        private int carNO;
        private int carName;
        private int carYear;

        //--------------------------------------------------
        public Car()
        {

        }
        public Car(int carNO)
        {
            this.carNO = carNO;
        }
        public Car(int carNo, int carName, int carYear)
        {
            this.carNO = carNo;
            this.carName = carName;
            this.carYear = carYear;
        }
        //--------------------------------------------------
        public int getCarNO()
        {
            return carNO;
        }
        public void setCarNO(int carNO)
        {
            this.carNO = carNO;
        }
        //--------------------------------------------------
        public override string ToString()
        {
            string a = $"{carNO}"; // Convert carNO to string (assuming carNO is numeric)

            // Format the string with hyphens
            if (a.Length == 7)
            {
                a = a.Insert(2, "-");
                a = a.Insert(6, "-"); // Note: Adjusted index to account for the first hyphen
            }
            else if (a.Length == 8)
            {
                a = a.Insert(3, "-");
                a = a.Insert(6, "-"); // Adjusted index for the first hyphen
            }

            return a; // Return the formatted string
        }
    }
}
