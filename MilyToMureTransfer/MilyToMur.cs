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
            string[,] mily = new string[k, m];
            //Все вспомогательные целочисленные переменные
            int line, column, currentSCondition, currentQCindition, currentOutputSymbol, recurringTransition, currentSNumber;

            // На случай, если в таблице Мили S-ки будут нумероваться не с нуля
            int minSNumber = int.MaxValue;

            // Для хранения уникальных пар S/Y
            List<string> unicTransition = new List<string>();

            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(", ");
                for (column = 0; column < m; column++)
                {
                    //  Сохраняем значения в таблицу Мили
                    mily[line, column] = mas[column];
                    // Если такая пара S/Y еще не встречалась и это не "-", то сохраняем ее в списке уникальных пар
                    if ((!unicTransition.Contains(mily[line, column])) && (mily[line, column] != "-"))
                    {
                        unicTransition.Add(mily[line, column]);   
                    }                   
                    // Сохраняем минимальный номер состояния S
                    if (mily[line, column] != "-")
                    {
                        currentSNumber = Convert.ToInt32(mily[line, column].Substring(mily[line, column].IndexOf('S') + 1, 1));
                        if (minSNumber > currentSNumber)
                        {
                            minSNumber = currentSNumber;
                        }
                    }

                }
            }
            unicTransition.Sort();
            // Добавление в таблицу Мили состояний q
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m; column++)
                {
                    mily[line, column] = mily[line, column] + $" {unicTransition.IndexOf(mily[line, column])}";
                }
            }

            //Идем по списку уникальных пар S/Y
            for (currentQCindition = 0; currentQCindition < unicTransition.Count; currentQCindition++)
            {
                // Выделяем номера у S и у Y
                currentSCondition = Convert.ToInt32(unicTransition[currentQCindition].Substring(1, 1));
                currentOutputSymbol = Convert.ToInt32(unicTransition[currentQCindition].Substring(4));

                Console.Write('Y' + $"{currentOutputSymbol}");

                // Проходим по соответствующей строке таблицы Мили и выводим все состояния q
                for (column = 0; column < m; column++)
                {
                    if (mily[currentSCondition - minSNumber, column] != "-")
                    {
                        Console.Write(" q" + $"{mily[currentSCondition - minSNumber, column].Substring(6)}");
                    }
                    else
                    {
                        Console.Write(" -");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}