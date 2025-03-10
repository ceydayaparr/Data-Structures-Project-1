using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp4
{
    class Cities
    {
        string[] cities;
        int[][] distances;
        ArrayList dictionary;
        Dictionary<string, Dictionary<int, List<string>>> cityDistances; 
        Dictionary<string, Dictionary<string, int>> cityToCityDistances; 

        public Cities(string[] cities_, int[][] distances_, ArrayList dictionary_, Dictionary<string, Dictionary<int, List<string>>> cityDistances_, Dictionary<string, Dictionary<string, int>> cityToCityDistances_)
        {
            cities = cities_;
            distances = distances_;
            dictionary = dictionary_;
            cityDistances = cityDistances_;
            cityToCityDistances = cityToCityDistances_;
        }

        public void ProcessMatrix(string[][] matrix)
        {
            for (int i = 2; i < 83; i++)
            {
                cities[i - 2] = matrix[i][1];
                distances[i - 2] = new int[81];
                for (int j = 2; j < 83; j++)
                {
                    int num = 0;
                    if (!string.IsNullOrEmpty(matrix[i][j]))
                    {
                        num = int.Parse(matrix[i][j]);
                    }

                    distances[i - 2][j - 2] = num;
                    dictionary.Add(new object[] { matrix[i][1], matrix[1][j], num });
                    if (!cityDistances.ContainsKey(matrix[i][1]))
                    {
                        cityDistances[matrix[i][1]] = new Dictionary<int, List<string>>();
                        cityToCityDistances[matrix[i][1]] = new Dictionary<string, int>();
                    }
                    if (!cityDistances[matrix[i][1]].ContainsKey(num))
                    {
                        cityDistances[matrix[i][1]][num] = new List<string>();
                    }
                    if (!cityToCityDistances[matrix[i][1]].ContainsKey(matrix[1][j]))
                    {
                        cityToCityDistances[matrix[i][1]][matrix[1][j]] = num;
                    }
                    cityDistances[matrix[i][1]][num].Add(matrix[1][j]);
                }
            }
        }

        public List<string> GetCoveredCities()
        {
            List<string> coveredCities = new List<string>();
            Console.Write("Enter city name: ");
            string cityName = Console.ReadLine();
            Console.Write("Enter distance range: ");
            int distanceRange = int.Parse(Console.ReadLine());
            if (distanceRange > 0 && cityDistances.ContainsKey(cityName))
            {
                foreach (int distance in cityDistances[cityName].Keys)
                {
                    if (distance < distanceRange)
                    {
                        foreach (string city in cityDistances[cityName][distance])
                        {
                            coveredCities.Add(city);
                        }
                    }
                }
            }
            return coveredCities;
        }

        public void GetNearestAndFarthestCities()
        {
            int nearest = Int32.MaxValue;
            string[] nearestCities = new string[2];
            int farthest = Int32.MinValue;
            string[] farthestCities = new string[2];
            foreach (object[] s in dictionary)
            {
                int distance = (int)s[2];
                if (distance == 0)
                {
                    continue;
                }
                if (distance > farthest)
                {
                    farthest = distance;
                    farthestCities = new string[] { (string)s[0], (string)s[1] };
                }
                if (distance < nearest)
                {
                    nearest = distance;
                    nearestCities = new string[] { (string)s[0], (string)s[1] };
                }
            }
            Console.WriteLine("Nearest cities: " + nearestCities[0] + ", " + nearestCities[1] + ". Distance between these two cities: " + nearest);
            Console.WriteLine("Farthest cities: " + farthestCities[0] + ", " + farthestCities[1] + ". Distance between these two cities: " + farthest);
        }

        public ArrayList MostVisitedCities(string startCity, int maxDistance)
        {
            List<ArrayList> nextCities = new List<ArrayList>();
            nextCities.Add(new ArrayList { startCity, 0, new List<string>(), new HashSet<string> { startCity } });

            ArrayList answer = new ArrayList { int.MaxValue, new List<string>(), 0 };

            while (nextCities.Count > 0)
            {
                ArrayList currentCity = nextCities[0];
                nextCities.RemoveAt(0);

                string city = (string)currentCity[0];
                int distance = (int)currentCity[1];
                List<string> path = (List<string>)currentCity[2];
                HashSet<string> visitedSet = (HashSet<string>)currentCity[3];

                if (path.Count > ((List<string>)answer[1]).Count)
                {
                    answer = new ArrayList { distance, new List<string>(path), path.Count };
                }

                foreach (string targetCity in cities)
                {
                    HashSet<string> visited = new HashSet<string>(visitedSet);

                    if (visited.Contains(targetCity))
                    {
                        continue;
                    }

                    int newDistance = distance + cityToCityDistances[city][targetCity];

                    if (newDistance > maxDistance)
                    {
                        continue;
                    }

                    visited.Add(targetCity);
                    List<string> newPath = new List<string>(path) { targetCity };

                    nextCities.Add(new ArrayList { targetCity, newDistance, newPath, visited });
                }
            }

            return new ArrayList { answer[0], new List<string>(((List<string>)answer[1])), answer[2] };
        }

        public void RandomCityDistances()
        {
            Random random = new Random();
            int[] randomCities = new int[5];
            string randomCityNamesStr = "";
            for (int i = 0; i < 5; i++)
            {
                randomCities[i] = random.Next(0, 81);
                randomCityNamesStr += cities[randomCities[i]] + ", ";
            }
            int[][] randomCityDistances = new int[5][];
            for (int i = 0; i < 5; i++)
            {
                randomCityDistances[i] = new int[5];
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    randomCityDistances[i][j] = distances[randomCities[i]][randomCities[j]];
                }
            }

            Console.WriteLine("Random Cities: " + randomCityNamesStr);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(randomCityDistances[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:/Users/bekir/Desktop/ilmesafe.txt";
            string[] lines = File.ReadAllLines(filePath); 
            string[][] matrix = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] lineValues = lines[i].Split('\t'); 
                matrix[i] = lineValues; 
            }
            string[] cities = new string[81];
            int[][] distances = new int[81][];
            ArrayList dictionary = new ArrayList();
            Dictionary<string, Dictionary<int, List<string>>> cityDistances = new Dictionary<string, Dictionary<int, List<string>>>();
            Dictionary<string, Dictionary<string, int>> cityToCityDistances = new Dictionary<string, Dictionary<string, int>>();
            Cities allCities = new Cities(cities, distances, dictionary, cityDistances, cityToCityDistances);

            allCities.ProcessMatrix(matrix);

            List<string> coveredCities = allCities.GetCoveredCities();
            Console.WriteLine("Total number of cities: " + coveredCities.Count);
            Console.Write("Covered cities: ");
            foreach (string city in coveredCities)
            {
                Console.Write($"{city}, ");
            }
            Console.WriteLine("\n");

            allCities.GetNearestAndFarthestCities();

            Console.Write("\nEnter a city name: ");
            string startingCity = Console.ReadLine();
            Console.Write("Enter the maximum distance you can travel: ");
            int maxDistance = int.Parse(Console.ReadLine());
            ArrayList mostVisitedCitiesResult = allCities.MostVisitedCities(startingCity, maxDistance);

            Console.WriteLine($"Most visited cities: {string.Join(" , ", (List<string>)mostVisitedCitiesResult[1])}");
            Console.WriteLine($"Total distance: {(int)mostVisitedCitiesResult[0]} km");
            Console.WriteLine($"Total number of cities: {(int)mostVisitedCitiesResult[2]}");

            Console.WriteLine("\nRandom 5 city distances matrix:");
            allCities.RandomCityDistances();

            Console.WriteLine();
            Console.WriteLine("Jagged array:");
            Console.WriteLine();

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            StreamReader dosya = new StreamReader(desktop + "\\iller.txt");//iller.txt dosyası masaüstünde kalsın
            string[] iller = {"",
        "Adana", "Adıyaman", "Afyonkarahisar", "Ağrı", "Amasya",
        "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir",
        "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur",
        "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli",
        "Diyarbakır", "Düzce", "Edirne", "Elazığ", "Erzincan",
        "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane",
        "Hakkari", "Hatay", "Iğdır", "Isparta", "İstanbul",
        "İzmir", "Kahramanmaraş", "Karabük", "Karaman", "Kars",
        "Kastamonu", "Kayseri", "Kırıkkale", "Kırklareli", "Kırşehir",
        "Kilis", "Kocaeli", "Konya", "Kütahya", "Malatya",
        "Manisa", "Mardin", "Mersin", "Muğla", "Muş",
        "Nevşehir", "Niğde", "Ordu", "Osmaniye", "Rize",
        "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas",
        "Şanlıurfa", "Şırnak", "Tekirdağ", "Tokat", "Trabzon",
        "Tunceli", "Uşak", "Van", "Yalova", "Yozgat", "Zonguldak",""};

            // Jagged Array oluşturmak için örnek
            int[][] jaggedArray = null;

            // İlk boyutu belirleme
            jaggedArray = new int[81][];


            for (int i = 0; i < iller.Length - 2; i++)
            {
                jaggedArray[i] = new int[i + 1];
                string satir = dosya.ReadLine();
                string[] inputs = satir.Split('	'); //DOSYADAKI VERİLERİ AYIRMAK İÇİN
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    jaggedArray[i][j] = Convert.ToInt32(inputs[j]);
                }
            }
            // Jagged Array'i yazdırma
            for (int i = 0; i < jaggedArray.Length; i++)
            {

                Console.Write(iller[i + 1] + " ");
                for (int j = 0; j < jaggedArray[i].Length - 1; j++)
                {
                    Console.Write(jaggedArray[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
    
}
