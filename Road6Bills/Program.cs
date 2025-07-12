using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Road6Bills
{
    internal class Program
    {
        // I Use ChatGPT To learn how do folders and how to save or create file in this folder (Method Use in: SearchClientInformaionForDates \ SearchClientInformaionForName \ SaveEntryDatesByVehicleNO \ checkIFDateDuplicate)
        static Date[] datelist = new Date[62];
        static Date date = new Date();
        static string clientName;

        static int counter = 1;
        static int countDateListBP = 0;
        static int c; // counter in EntryGate Method
        static int r = 0; // to count how mucd date in datelistBU
        static Date[] datelistBackUP = new Date[datelist.Length]; 
        static double[] feelist = new double[datelist.Length];
        static bool check = true;
        static double tax = 0;
        static int total = 0;

        static string folderPath; 
        static string filePath;

        static void BillGate() // Display billing information for a car, including name, car number, entry dates, and fees.
        {
            tax = 0;
            total = 0;
            Console.WriteLine("Road 6 Bills");
            Console.WriteLine("Need Some information to issue the invoice");
            Console.WriteLine("------------------------------------------");

            Console.Write("Car NO: ");
            int clientCarNO = int.Parse(Console.ReadLine());
            Car car = new Car(clientCarNO);
            Console.WriteLine("------------------------------------------");

            SearchClientInformaionForDates(clientCarNO); 
            SearchClientInformaionForName(clientCarNO);
            checkIFDateDuplicate(clientCarNO);

            Console.Clear();
            Console.WriteLine("Road 6 Bill");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Name: {clientName}");
            Console.WriteLine($"Car NO: {car.ToString()}");
            Console.WriteLine($"Entry Times: {r}");

            for( int i = 0; i < r; i++)
            {
                Console.Write($"Day {i+1}:  {datelistBackUP[i].ToString()}");
                if (feelist[i] == 1)
                {
                    Console.WriteLine($"/  Fee: 15.0");
                    tax += 15 * 0.18;
                    total += 15;
                }
                else
                {
                    Console.WriteLine($"/  Fee: 30.0");
                    tax += 30 * 0.18;
                    total += 30;
                }
            }
            Console.WriteLine("------------------------------------------");

            Console.WriteLine($"Total Tax Fee: {tax}");
            Console.WriteLine($"Total Payment: {total}");

            Console.WriteLine();
            Console.WriteLine();

            SaveBillINFolder(clientCarNO);
            Console.WriteLine($"You'r Bill Saved IN (bin/Debug/CustomersBills/{clientCarNO})");
            Console.WriteLine();

        }
        static void EntryGate() // Collect and save a car's owner name and their entry dates through the toll gate.  // Handles multiple entries on the same day.
        {
            string name;
            int carNo;
            string d;
            string m;
            string y;
            c = 0;

            Console.Write("Name: ");
            name = Console.ReadLine();
            Console.Write("Car NO: ");
            carNo = int.Parse(Console.ReadLine());
            Console.WriteLine("-------------------------");
            Console.WriteLine("Insert Entry Dates:");


            while (check)
            {
                Console.WriteLine($"Day {counter}");
                Console.Write("DD: ");
                d = Console.ReadLine();
                Console.Write("MM: ");
                m = Console.ReadLine();
                Console.Write("YY: ");
                y = Console.ReadLine();

                date.SetDay(d);
                date.SetMonth(m);
                date.SetYear(y);
                datelist[c] = new Date(d,m,y);

                Console.WriteLine("IF Do You Want to more Entry in Same Day Choose (y/n)");
                string dup = Console.ReadLine();

                if(dup == "y" ||  dup == "Y") // to check if need to save same date in same day
                {
                    datelist[c + 1] = new Date(d, m, y);
                    Console.WriteLine("Save Entry");

                    //if date duplicate c plus 2 (because when inset more time same date in same day need put it After last date in array)
                    c += 2;
                }
                else
                {
                    //if date not duplicate c plus 1
                    c++;
                }

                Console.WriteLine("Do You Watn TO Continue Insert Dates? (y/n)");
                dup = Console.ReadLine();
                if (dup == "n" || dup == "N")
                {
                    check = false;
                }
                else
                {
                    counter++;
                    continue;
                }
            }
            SaveDataVehicles(name, carNo);
            SaveEntryDatesByVehicleNO(carNo);
        }
        static void SearchClientInformaionForDates(int carNo) // Read and load all entry dates for a specific car from its saved file into memory.
        {
            folderPath = @"./CarEntryDates";

            filePath = Path.Combine(folderPath, $"{carNo}.txt");
            int i = 0;
            using (StreamReader reader = new StreamReader(filePath, true))
            {
            
                 while (check)
                 {
                     if (!string.IsNullOrEmpty(filePath))
                     {
                         string a = reader.ReadLine();
                         string[] parts = a.Split('-');
                         string day = parts[0];
                         string month = parts[1];
                         string year = parts[2];
                 
                         datelistBackUP[i] = new Date(day, month, year);
                         r++;
                         countDateListBP++;
                     }
                     i++;
                     if (reader.EndOfStream)
                     {
                         check = false ;
                     }
                 }
            }
        }
        static void SearchClientInformaionForName(int carNo) // Search and load the client's name associated with the given car number.
        {
            check = true;
            using(StreamReader reader = new StreamReader("DataVehicles.txt", true))
            {
                while (check)
                {
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        string line = reader.ReadLine();
                        string[] parts = line.Split(',');
                        string name = parts[0];
                        int cn = int.Parse(parts[1]);

                        if (carNo == cn)
                        {
                            clientName = name;
                            check = false;
                        }
                    }
                }

            }
        }
        static void SaveDataVehicles(string name, int carNO) // Save the vehicle owner's name and car number to a general vehicles data file.
        {
            using (StreamWriter sw = new StreamWriter("DataVehicles.txt", true))
            {
                sw.WriteLine($"{name},{carNO}");
                sw.Close();
            }
        }
        static void SaveEntryDatesByVehicleNO(int carNo) // Save all entered dates for a given car into a separate text file under a folder.
        {
            string folderPath = @"./CarEntryDates";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, $"{carNo}.txt");

            using(StreamWriter sw = new StreamWriter(filePath, true))
            {
                for (int i = 0; i < c; i++)
                {
                    string a;
                    a = datelist[i].ToString();
                    sw.WriteLine(a);
                }
                sw.Close();
            }
        }
        static void checkIFDateDuplicate(int carNo)// Check the loaded dates list and mark entries as duplicates if they occur on the same day.  // Assigns a lower fee (15.0) to repeated entries on the same day.
        {
            
            check = true;
            int i = 0;
            string line;

            folderPath = @"./CarEntryDates";
            filePath = Path.Combine(folderPath, $"{carNo}.txt");


            while (check)
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (datelistBackUP[i] == null || datelistBackUP[i + 1] == null) // To Check IF Field is null or Fill
                    {
                        check = false;
                        break;
                    }


                    if (datelistBackUP[i].ToString() == datelistBackUP[i + 1].ToString())
                    {
                        feelist[i + 1] = 1; // to put one in seconde date 1 (1: us mean 15.0 shekel)
                    }
                    else
                    {
                        if (feelist[i + 1] == 1) // to check if seconde field is one if true put in third field 0
                        {
                            feelist[i + 2] = 0;
                        }
                        feelist[i + 1] = 0;

                    }
                    i++;
                }
            }

            

        }
        static void SaveBillINFolder(int carNo)
        {
            string folderPath = @"./CustomersBills";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, $"{carNo}.txt");

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                SearchClientInformaionForDates(carNo);
                SearchClientInformaionForName(carNo);
                checkIFDateDuplicate(carNo);

                sw.WriteLine("Road 6 Bill");
                sw.WriteLine("------------------------------------------");
                sw.WriteLine($"Name: {clientName}");
                sw.WriteLine($"Car NO: {carNo.ToString()}");
                sw.WriteLine($"Entry Times: {r}");

                for (int i = 0; i < r; i++)
                {
                    sw.Write($"Day {i + 1}:  {datelistBackUP[i].ToString()}");
                    if (feelist[i] == 1)
                    {
                        sw.WriteLine($"/  Fee: 15.0");
                        tax += 15 * 0.18;
                        total += 15;
                    }
                    else
                    {
                        sw.WriteLine($"/  Fee: 30.0");
                        tax += 30 * 0.18;
                        total += 30;
                    }
                }
                sw.WriteLine("------------------------------------------");

                sw.WriteLine($"Total Tax Fee: {tax}");
                sw.WriteLine($"Total Payment: {total}");
            }
        }
        static void InterFace()
        {
            Console.WriteLine("Welcome To Road 6 Bills");
            Console.WriteLine("1) Entry Gate");
            Console.WriteLine("2) Bill Gate");
            Console.Write("Choose Service: ");

            string choice = Console.ReadLine();


            if (choice == "1")
            {
                Console.Clear();
                EntryGate();
            }
            if (choice == "2")
            {
                Console.Clear();
                BillGate();
            }
            Console.WriteLine();
            Console.Write("Doyou Want Back TO Interface Page (y/n): ");
            choice = Console.ReadLine();

            if (choice == "y" || choice == "Y")
            {
                Console.Clear();
                InterFace();
            }
            else if (choice == "n" || choice == "N")
            {
                Environment.Exit(0);
            }
        }
        static void Main(string[] args)
        {
            InterFace();

            Console.ReadKey();
        }
    }
}
