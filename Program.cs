using static System.IO.Directory;
using static System.IO.Path;

namespace EV2_Breyne_Ward;
class Program
{
    public static string[] parkeerplaatsen = new string[10];
    public static Random gen = new Random();
    public static string parkingVrij = "[0]";
    public static string parkingBezet = "[X]";
    public static string dirName = "EV2_Breyne_Ward";
    public static string fileName = "Ward's_logfiles.txt";
    public static string directoryPath = Path.Combine(GetTempPath(), dirName);
    public static string filePath = Path.Combine(directoryPath, fileName);
    public static DateTime startTijd;
    public static DateTime eindTijd;
    public static TimeSpan duur;
    public static double duurInMS;
    static void Main(string[] args)
    {
        //TEST 
        // for (int i = 0; i < 100; i++)
        // {
        //     int result = generateRandomParkeerplaats();
        //     Console.WriteLine(result);
        // }

        // initialize parkeerplaatsen
        SetStatusParkeerplaatsen(parkingVrij);
        PrintParkeerplaatsen();
        // Infinite loop, that breaks when parking is full
        startTijd = DateTime.Now;
        while (true)
        {

            int generatedNumber = GenerateRandomNumber();
            if (generatedNumber == 1)
            {
                Console.WriteLine("Binnenrijden called");
                AutoRijdtBinnen();

            }
            else if (generatedNumber == 2)
            {
                Console.WriteLine("Buitenrijden called");
                AutoRijdtBuiten();
            }
        }

    }

    public static void SetStatusParkeerplaatsen(string statusIn)
    {
        for (int i = 0; i < parkeerplaatsen.Length; i++)
        {
            parkeerplaatsen[i] = statusIn;
        }

    }

    public static void PrintParkeerplaatsen()
    {
        for (int i = 0; i < parkeerplaatsen.Length; i++)
        {
            Console.Write(parkeerplaatsen[i]);
        }
        Console.WriteLine();
        Console.WriteLine("------------------------------");
    }

    public static int GenerateRandomNumber()
    {
        int randomNumber = gen.Next(-2, 0);
        int absValueOfRandNumber = Math.Abs(randomNumber);
        //TEST
        // Console.WriteLine("Random number: " + randomNumber);
        // Console.WriteLine("Absolute value: " + absValueOfRandNumber);
        return absValueOfRandNumber;

    }

    public static void AutoRijdtBinnen()
    {
        bool volzet = ParkingVolzet();
        int parkeerplaats;
        if (!volzet)
        {
            do
            {
                parkeerplaats = generateRandomParkeerplaats();
            }
            while (parkeerplaatsen[parkeerplaats] == parkingBezet);

            // Als lege plaats gevonden is zet de parkeerplaats op bezet.
            parkeerplaatsen[parkeerplaats] = parkingBezet;

            // Maak scherm leeg
            //Console.Clear();
            // Print nieuw parkeerplaats status af
            PrintParkeerplaatsen();

        }
        else
        {
            Console.WriteLine("Parking is vol");
            // Maak logging
            eindTijd = DateTime.Now;
            duur = eindTijd - startTijd;
            duurInMS = duur.TotalMilliseconds;
            bool fileExists = false;

            do
            {
                fileExists = FileExists();
            } while (!fileExists);
            
            // Terminate application
            Environment.Exit(0);
        }

    }

    public static void AutoRijdtBuiten()
    {
        bool geparkeerdeAutoAanwezig = GeparkeerdeAutoAanwezig();
        int parkeerplaats;
        if (geparkeerdeAutoAanwezig)
        {
            do
            {
                parkeerplaats = generateRandomParkeerplaats();
            }
            while (parkeerplaatsen[parkeerplaats] == parkingVrij);

            // Als volle plaats gevonden is zet de parkeerplaats op leeg.
            parkeerplaatsen[parkeerplaats] = parkingVrij;

            // Maak scherm leeg
            //Console.Clear();
            // Print nieuw parkeerplaats status af
            PrintParkeerplaatsen();

        }
        else
        {
            // TEST
             Console.WriteLine("Geen geparkeerde wagens");

        }
    }


    public static bool ParkingVolzet()
    {
        bool resultParkingVolzet = true;
        for (int i = 0; i < parkeerplaatsen.Length; i++)
        {
            if (parkeerplaatsen[i] == parkingVrij)
            {
                resultParkingVolzet = false;
            }
        }


        return resultParkingVolzet;
    }

    public static int generateRandomParkeerplaats()
    {
        int randomNumber;
        return randomNumber = gen.Next(0, 10);
    }

    public static bool GeparkeerdeAutoAanwezig()
    {
        bool geparkeerdeAutoAanwezig = false;
        for (int i = 0; i < parkeerplaatsen.Length; i++)
        {
            if (parkeerplaatsen[i] == parkingBezet)
            {
                geparkeerdeAutoAanwezig = true;
            }
        }


        return geparkeerdeAutoAanwezig;
    }

    
    
    public static bool FileExists()
    {
        bool fileExists = false;
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Log file not found.");
            CreateLogFile();
            fileExists = false;
        }
        else
        {
            Console.WriteLine("File found.");
            // Write to file
            using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine($"De parking ging open om " + startTijd + ". Het duurde " + duurInMS.ToString() + " ms voor ze vol stond.");
                }
            fileExists = true;
        }
        return fileExists;
    }

    public static void CreateLogFile()
    {

        Console.WriteLine($"Logfile: will be created and will be available in {filePath}");
        try
        {
            Directory.CreateDirectory(directoryPath);

        }
        catch (IOException ex)
        {

            Console.WriteLine(ex.Message);
        }

        if (!File.Exists(filePath))
        {
            try
            {
                using(File.Create(filePath));
            }
            catch (IOException ex)
            {
                //Uncomment for debugging (no sensitive info to user) 
                Console.WriteLine(ex.Message);
            }

        }
        else
        {
            // Handling situation if the file already exists
            Console.WriteLine("File already exists.Nothing to do here");
        }

    }

}

