using System.Linq;

namespace ConsoleApplication
{
    class MainProgram
    {
        public static void SearchEclose(ref string[,] states, ref List<string> eclose, int currentState, int indexE)
        {
            string[] currentTransitions = states[currentState, indexE].Split(',');
            if (currentTransitions[0] != "-")
            {
                for (int column = 0; column < currentTransitions.Length; column++)
                {
                    if (!eclose.Contains(currentTransitions[column]))
                    {
                        eclose.Add(currentTransitions[column]);
                        int newCurrentState = Convert.ToInt32(currentTransitions[column]);
                        SearchEclose(ref states, ref eclose, newCurrentState, indexE);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            // Подкотовка к чтению таблицы
            string[] mas = Console.ReadLine().Split();
            int k = Convert.ToInt32(mas[0]);
            int m = Convert.ToInt32(mas[1]);
            string[,] initialStates = new string[k, m + 1];

            //Все вспомогательные переменные
            int line, column = 0;

            // Вспомогательный список для хранения eclose каждого начального состояния
            List<List<string>> ecloses = new List<List<string>>();

            // Вспомогательный список для определения завершения детерминирования
            List<List<string>> remainingStates = new List<List<string>>();

            // Таблица результата детерминирования
            List<List<string>> resultAutomat = new List<List<string>>();

            // Заполнение первичной таблицы входными данными
            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(" ");
                for (column = 0; column < m + 1; column++)
                {
                    initialStates[line, column] = mas[column];
                }         
            }
            // Поиск eclose-ов для каждого состояния
            for (column = 0; column < k; column++)
            {
                List<string> workList = new List<string>();
                workList.Add(column.ToString());
                SearchEclose(ref initialStates, ref workList, column, m);
                ecloses.Add(workList);
            }

            List<string> startList = new List<string>();
            startList.Add("0");
            remainingStates.Add(startList);
            startList = new List<string>();
            startList.Add("1");
            startList.Add("2");
            remainingStates.Add(startList);
            startList = new List<string>();
            startList.Add("1");
            startList.Add("2");
            startList.Add("3");
            remainingStates.Add(startList);
            startList = new List<string>();
            startList.Add("4");
            remainingStates.Add(startList);
            List<string> currentEcloseList = new List<string>();
            string currentEcloseString;
            string[] allSymbols;
            HashSet<string> uniqueSymbolsSet = new HashSet<string>();
            Console.WriteLine();

            while (remainingStates.Count > 0)
            {
                currentEcloseList = remainingStates[0];
                currentEcloseString = "";
                for (column = 0; column < currentEcloseList.Count; column++)
                {
                    currentEcloseString += "," + (string.Join(",", ecloses[Convert.ToInt32(currentEcloseList[column])]));
                }
                allSymbols = currentEcloseString.Split(",");
                uniqueSymbolsSet = new HashSet<string>(allSymbols);
                currentEcloseList = uniqueSymbolsSet.ToList();
                currentEcloseList.RemoveAt(0);
                remainingStates.RemoveAt(0);
                for (column = 0; column < currentEcloseList.Count; column++)
                {
                    Console.Write(currentEcloseList[column] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}