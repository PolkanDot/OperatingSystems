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

            // Вспомогательная таблица для хранения eclose каждого начального состояния
            List<List<string>> ecloses = new List<List<string>>();

            // Заполнение первичной таблицы входными данными и присваивание первых групп
            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(" ");
                for (column = 0; column < m + 1; column++)
                {
                    initialStates[line, column] = mas[column];
                }         
            }

            

            for (column = 0; column < k; column++)
            {
                List<string> workList = new List<string>();
                workList.Add(column.ToString());
                SearchEclose(ref initialStates, ref workList, column, m);
                ecloses.Add(workList);
            }
            Console.WriteLine();
            for (line = 0; line < ecloses.Count; line++)
            {
                for (column = 0; column < ecloses[line].Count; column++)
                {
                    Console.Write(ecloses[line][column] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}