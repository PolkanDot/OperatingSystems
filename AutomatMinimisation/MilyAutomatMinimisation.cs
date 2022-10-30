﻿namespace ConsoleApplication
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

            //Все вспомогательные целочисленные переменные
            int line, column, currentSCondition, currentQCindition, currentOutputSymbol, recurringTransition, currentSNumber;
            string currentSequence = "";

            // Для хранения уникальных последовательностей состояний или выходных символов
            List<string> unicSequence = new List<string>();

            // Вспомогательные таблицы для цикла минимизации
            List<List<string>> firstWorkTable = new List<List<string>>();
            List<List<string>> secondWorkTable = new List<List<string>>();

            bool endOfMinimisation = true;

            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(" ");
                for (column = 0; column < m * 2; column++)
                {
                    if (mas[column] == "-")
                    {
                        milyStates[line, column / 2] = mas[column];
                        milyOutputSymbols[line, column / 2] = mas[column];
                        currentSequence = currentSequence + mas[column] + " ";
                    }
                    else
                    {
                        // Если это состояние
                        if (column % 2 == 0)
                        {
                            milyStates[line, column / 2] = mas[column];
                        }
                        // Если это выходной символ
                        else
                        {
                            milyOutputSymbols[line, column / 2] = mas[column];
                            currentSequence = currentSequence + mas[column] + " ";
                        }
                    }
                }
                if (!(unicSequence.Contains(currentSequence)))
                {
                    unicSequence.Add(currentSequence);
                }
                //Присваеваем текущей строке номер группы по последователбности выходных символов
                // и записываем последней ячейкой строки таблицы
                milyStates[line, m] = (unicSequence.IndexOf(currentSequence) + 1).ToString();
                currentSequence = "";
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
                    helpColumn = Convert.ToInt32(milyStates[line, column - 1]);
                    firstWorkTable[line].Add(milyStates[helpColumn, m]);
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
            Console.WriteLine();
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m + 2; column++)
                {
                    Console.Write(firstWorkTable[line][column]);
                }
                Console.WriteLine();
            }
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
                // Производим один "виток" минимизации из первой вспомогательной таблицы во вторую
                for (line = 0; line < k; line++)
                {
                    // Создаем новую "строку" таблицы
                    secondWorkTable.Add(new List<string>());
                    // Первой ячейкой записываем номер старой группы
                    secondWorkTable[line].Add((firstWorkTable[line])[m + 1]);
                    // Производим заполнение таблицы новыми состояниями
                    for (column = 1; column < m + 1; column++)
                    {
                        helpColumn = Convert.ToInt32(milyStates[line, column - 1]);
                        secondWorkTable[line].Add(milyStates[helpColumn, m]);
                        currentSequence = currentSequence + secondWorkTable[line][column - 1] + " ";
                    }
                    // Сохраняем уникальные последовательности переходов
                    if (!(unicSequence.Contains(currentSequence)))
                    {
                        unicSequence.Add(currentSequence);
                    }
                    // Последней ячейкой записываем новый номер группы для строки таблицы
                    secondWorkTable[line].Add((unicSequence.IndexOf(currentSequence) + 1).ToString());
                    currentSequence = "";
                    // Если хотя бы в одной строке номер первоначальной группы не совпал с новым,
                    // то считаем, что нужно минимизировать дальше
                    if ((secondWorkTable[line])[0] != (secondWorkTable[line])[m + 1])
                    {
                        endOfMinimisation = false;
                    }
                }
                unicSequence.Clear();
                // Сохраняем результат витка минимизации в первую таблицу
                for (line = 0; line < k; line++)
                {
                    for (column = 0; column < m + 2; column++)
                    {
                        (firstWorkTable[line])[column] = secondWorkTable[line][column];
                    }
                }
                for (line = 0; line < k; line++)
                {
                    for (column = 0; column < m + 2; column++)
                    {
                        Console.Write(secondWorkTable[line][column]);
                    }
                    Console.WriteLine();
                }
                // Очищаем вторую таблицу для следуюшего витка минимизации
                secondWorkTable.Clear();
            }
            Console.WriteLine();
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m + 2; column++)
                {
                    Console.Write(firstWorkTable[line][column]);
                }
                Console.WriteLine();
            }
            // Вывод результата
            Console.WriteLine();
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m; column++)
                {
                    currentSequence += (Convert.ToInt32(firstWorkTable[line][column + 1]) - 1).ToString() + " " + milyOutputSymbols[line, column] + " ";        
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