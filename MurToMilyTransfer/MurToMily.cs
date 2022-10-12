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
            string[,] mur = new string[k, m + 1];
            //Все вспомогательные целочисленные переменные
            int line, column, currentSNumber;

            // Для хранения y под соответствующими индексами q
            List<int> qAndY = new List<int>();

            // Читаем входную таблицу Мура
            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(" ");
                // Сохранем выхлдные символы под индексом соответствующего им состояния
                qAndY.Add(Convert.ToInt32(mas[0].Substring(1)));

                for (column = 1; column < m + 1; column++)
                {
                    // Сохраняем только цифу от q<цифра>
                    if (mas[column] != "-")
                    {
                        mur[line, column] = mas[column].Substring(1);
                    }    
                    // Если это "-" то просто сохраняем его 
                    else
                    {
                        mur[line, column] = mas[column];
                    }
                }
            }
            Console.WriteLine();

            // Проходим по таблице Мура, состояния q выводятся как S, с соответствующим им Y из списка qAndY
            for (line = 0; line < k; line++)
            {
                for (column = 1; column < m + 1; column++)
                {
                    if (mur[line, column] != "-")
                    {
                        currentSNumber = Convert.ToInt32(mur[line, column]);
                        Console.Write("S" + $"{mur[line, column]} Y" + $"{qAndY[currentSNumber]} ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                    
                }
                Console.WriteLine();
            }
        }
    }
}
