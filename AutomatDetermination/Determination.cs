namespace ConsoleApplication
{
    class MainProgram
    {
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
        }
    }
}