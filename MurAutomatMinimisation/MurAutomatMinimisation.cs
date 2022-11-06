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
            string[,] murStates = new string[k, m + 1];
            string[,] murOutputSymbols = new string[k, m];
            string[] startOutputSymbolsSequence = new string[k + 1];

            //Все вспомогательные целочисленные переменные
            int line, column, currentStateNumber;
            int minStateNumber = int.MaxValue;
            string currentSequence = "";
            int helpColumn;
            bool endOfMinimisation = true
                ;
            // Для хранения уникальных последовательностей состояний или выходных символов
            List<string> unicSequence = new List<string>();

            // Вспомогательная таблица для цикла минимизации
            List<List<string>> firstWorkTable = new List<List<string>>();
            
            // Запоминаем последовательность выходных символов и заполняем таблицу переходов
            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(" ");
                for (column = 0; column < m + 1; column++)
                {
                    
                    if (column != 0)
                    {
                        murStates[line, column - 1] = mas[column];
                        if (mas[column] != "-")
                        {                    
                            currentStateNumber = Convert.ToInt32(murStates[line, column - 1]);
                            if (minStateNumber > currentStateNumber)
                            {
                                minStateNumber = currentStateNumber;
                            }
                        }
                    }
                    else
                    {
                        startOutputSymbolsSequence[line] = mas[column];
                    }
                }
            }
            // Добавляем виртуальное состояние
            startOutputSymbolsSequence[k] = "-";

            // Заполняем таблицу выходных символов
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m; column++)
                {
                    if (murStates[line, column] != "-")
                    {
                        murOutputSymbols[line, column] = startOutputSymbolsSequence[Convert.ToInt32(murStates[line, column]) - minStateNumber];
                    }
                    else
                    {
                        murOutputSymbols[line, column] = startOutputSymbolsSequence[k];
                    }
                    
                }
            }
            // Выдиляем первичные группы
            for (line = 0; line < k; line++)
            {
                if (!(unicSequence.Contains(startOutputSymbolsSequence[line])))
                {
                    unicSequence.Add(startOutputSymbolsSequence[line]);
                }
                //Присваеваем текущей строке номер группы по последователбности выходных символов
                // и записываем последней ячейкой строки таблицы
                murStates[line, m] = (unicSequence.IndexOf(startOutputSymbolsSequence[line]) + 1).ToString();
                currentSequence = "";
            }
            unicSequence.Clear();
            
            // Проводим первый этап минимизации
            for (line = 0; line < k; line++)
            {
                // Создаем новую "строку" таблицы
                firstWorkTable.Add(new List<string>());
                // Первой ячейкой записываем номер старой группы
                firstWorkTable[line].Add(murStates[line, m]);
                // Производим заполнение таблицы новыми состояниями
                for (column = 1; column < m + 1; column++)
                {
                    if (murStates[line, column - 1] != "-")
                    {
                        helpColumn = Convert.ToInt32(murStates[line, column - 1]);
                        firstWorkTable[line].Add(murStates[helpColumn - minStateNumber, m]);
                    }
                    else
                    {
                        firstWorkTable[line].Add("-");
                    }
                    currentSequence = currentSequence + firstWorkTable[line][column - 1] + " ";
                }
                // Сохраняем уникальные последовательности переходов
                if (!(unicSequence.Contains(currentSequence)))
                {
                    unicSequence.Add(currentSequence);
                }
                // Последней ячейкой записываем новый номер группы для строки таблицы
                firstWorkTable[line].Add((unicSequence.IndexOf(currentSequence) + 1).ToString());
                currentSequence = "";
                // Если хотя бы в одной строке номер первоначальной группы не совпал с новым,
                // то считаем, что нужно минимизировать дальше
                if ((firstWorkTable[line])[0] != (firstWorkTable[line])[m + 1])
                {
                    endOfMinimisation = false;
                }
            }

            unicSequence.Clear();

            // Минимизируем, пока все старые группы не совпадут с новыми
            while (!endOfMinimisation)
            {
                endOfMinimisation = true;
                // Записываем в таблицу с изначальными состояниями текущие группы
                // для формирования переходов в этих группах
                for (line = 0; line < k; line++)
                {
                    murStates[line, m] = (firstWorkTable[line])[m + 1];
                }
                // Очищаем вспомогательную таблицу для этого витка минимизации
                firstWorkTable.Clear();
                // Производим один "виток" минимизации из основной таблицы во вспомогательную
                for (line = 0; line < k; line++)
                {
                    // Создаем новую "строку" таблицы
                    firstWorkTable.Add(new List<string>());
                    // Первой ячейкой записываем номер старой группы
                    firstWorkTable[line].Add(murStates[line, m]);
                    // Производим заполнение таблицы новыми состояниями
                    for (column = 1; column < m + 1; column++)
                    {
                        if (murStates[line, column - 1] != "-")
                        {
                            helpColumn = Convert.ToInt32(murStates[line, column - 1]);
                            firstWorkTable[line].Add(murStates[helpColumn - minStateNumber, m]);
                        }
                        else
                        {
                            firstWorkTable[line].Add("-");
                        }
                        currentSequence = currentSequence + firstWorkTable[line][column - 1] + " ";
                    }
                    // Сохраняем уникальные последовательности переходов
                    if (!(unicSequence.Contains(currentSequence)))
                    {
                        unicSequence.Add(currentSequence);
                    }
                    // Последней ячейкой записываем новый номер группы для строки таблицы
                    firstWorkTable[line].Add((unicSequence.IndexOf(currentSequence) + 1).ToString());
                    currentSequence = "";
                    // Если хотя бы в одной строке номер первоначальной группы не совпал с новым,
                    // то считаем, что нужно минимизировать дальше
                    if ((firstWorkTable[line])[0] != (firstWorkTable[line])[m + 1])
                    {
                        endOfMinimisation = false;
                    }
                }
                unicSequence.Clear();
            }

            // Вывод результата
            Console.WriteLine();
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m + 1; column++)
                {
                    currentSequence += firstWorkTable[line][column] + " ";
                }
                if (!(unicSequence.Contains(currentSequence)))
                {
                    unicSequence.Add(currentSequence);
                    currentSequence = "";
                    for (column = 0; column < m; column++)
                    {
                        currentSequence += firstWorkTable[line][column + 1] + " ";
                    }
                    Console.WriteLine(startOutputSymbolsSequence[line] + " " + (currentSequence.TrimEnd()));
                }
                currentSequence = "";
            }
        }
    }
}
