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
            string[,] milyStates = new string[k, m + 1];
            string[,] milyOutputSymbols = new string[k, m];

            //Все вспомогательные переменные
            int line, column, dashCount = 0;
            string currentSequence = "";

            // Для хранения уникальных последовательностей состояний или выходных символов
            List<string> unicSequence = new List<string>();

            // Вспомогательная таблица для цикла минимизации
            List<List<string>> firstWorkTable = new List<List<string>>();

            bool endOfMinimisation = true;

            // Заполнение первичной таблицы входными данными и присваивание первых групп
            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(" ");
                for (column = 0; column < m * 2; column++)
                {
                    if (mas[column - dashCount] != "-")
                    {
                        // Если это состояние
                        if (column % 2 == 0)
                        {
                            milyStates[line, column / 2] = mas[column - dashCount];
                        }
                        // Если это выходной символ
                        else
                        {
                            milyOutputSymbols[line, column / 2] = mas[column - dashCount];
                            currentSequence = currentSequence + mas[column - dashCount] + " ";
                        }
                    }
                    else
                    {
                        milyStates[line, column / 2] = mas[column - dashCount];
                        milyOutputSymbols[line, column / 2] = mas[column - dashCount];
                        currentSequence = currentSequence + mas[column - dashCount] + " ";
                        // Штука для обхода проблемы несоответствия ширирны массива mas и milyStates/OutputSymbols
                        // при осутствии перехода, тк при вводе "-" мы всместо 2 символов получаем 1
                        dashCount++;
                        column++;
                    }
                }
                if (!(unicSequence.Contains(currentSequence)))
                {
                    unicSequence.Add(currentSequence);
                }
                //Присваеваем текущей строке номер группы по последователбности выходных символов
                // и записываем последней ячейкой строки таблицы
                milyStates[line, m] = (unicSequence.IndexOf(currentSequence) + 1).ToString();
                // Обнуляем вспомогательные переменные для работы с новой строкой
                currentSequence = "";
                dashCount = 0;
            }

            unicSequence.Clear();
            int helpColumn;

            // Проводим первый этап минимизации
            for (line = 0; line < k; line++)
            {
                // Создаем новую "строку" таблицы
                firstWorkTable.Add(new List<string>());
                // Первой ячейкой записываем номер старой группы
                firstWorkTable[line].Add(milyStates[line, m]);
                // Производим заполнение таблицы новыми состояниями
                for (column = 1; column < m + 1; column++)
                {
                    if (milyStates[line, column - 1] != "-")
                    {
                        helpColumn = Convert.ToInt32(milyStates[line, column - 1]);
                        firstWorkTable[line].Add(milyStates[helpColumn, m]);
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
                    milyStates[line, m] = (firstWorkTable[line])[m + 1];
                }
                // Очищаем вспомогательную таблицу для этого витка минимизации
                firstWorkTable.Clear();
                // Производим один "виток" минимизации из основной таблицы во вспомогательную
                for (line = 0; line < k; line++)
                {
                    // Создаем новую "строку" таблицы
                    firstWorkTable.Add(new List<string>());
                    // Первой ячейкой записываем номер старой группы
                    firstWorkTable[line].Add(milyStates[line, m]);
                    // Производим заполнение таблицы новыми состояниями
                    for (column = 1; column < m + 1; column++)
                    {
                        if (milyStates[line, column - 1] != "-")
                        {
                            helpColumn = Convert.ToInt32(milyStates[line, column - 1]);
                            firstWorkTable[line].Add(milyStates[helpColumn, m]);
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
                for (column = 0; column < m; column++)
                {
                    currentSequence += firstWorkTable[line][column + 1] + " " + milyOutputSymbols[line, column] + " ";                          
                }
                if (!(unicSequence.Contains(currentSequence)))
                {
                    unicSequence.Add(currentSequence);
                    Console.WriteLine(currentSequence.TrimEnd());
                }
                currentSequence = "";
            }
        }
    }
}