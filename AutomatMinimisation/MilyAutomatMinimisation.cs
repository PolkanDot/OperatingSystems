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

            //Все вспомогательные целочисленные переменные
            int line, column, currentSCondition, currentQCindition, currentOutputSymbol, recurringTransition, currentSNumber;
            string currentSequence = "";

            // На случай, если в таблице Мили S-ки будут нумероваться не с нуля
            int minSNumber = int.MaxValue;

            // Для хранения уникальных последовательностей состояний или выходных символов
            List<string> unicSequence = new List<string>();

            List<List<string>> firstWorkTable = new List<List<string>>();
            List<List<string>> secondWorkTable = new List<List<string>>();

            bool endOfMinimisation = false;

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
                        if (column % 2 == 0)
                        {
                            milyStates[line, column / 2] = mas[column];
                            currentSNumber = Convert.ToInt32(milyStates[line, column / 2]);
                            if (minSNumber > currentSNumber)
                            {
                                minSNumber = currentSNumber;
                            }
                        }
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
                milyStates[line, m] = (unicSequence.IndexOf(currentSequence) + 1).ToString();
                currentSequence = "";
            }
            unicSequence.Clear();
            int helpColumn;
            for (line = 0; line < k; line++)
            {
                firstWorkTable.Add(new List<string>());
                firstWorkTable[line].Add(milyStates[line, m]);
                for (column = 1; column < m + 1; column++)
                {
                    helpColumn = Convert.ToInt32(milyStates[line, column - 1]);
                    firstWorkTable[line].Add(milyStates[helpColumn, m]);
                    currentSequence = currentSequence + firstWorkTable[line][column - 1] + " ";
                }
                if (!(unicSequence.Contains(currentSequence)))
                {
                    unicSequence.Add(currentSequence);
                }
                firstWorkTable[line].Add((unicSequence.IndexOf(currentSequence) + 1).ToString());
                currentSequence = "";
            }
            unicSequence.Clear();

            while (!endOfMinimisation)
            {
                endOfMinimisation = true;
                for (line = 0; line < k; line++)
                {
                    milyStates[line, m] = (firstWorkTable[line])[m + 1];
                }
                for (line = 0; line < k; line++)
                {
                    secondWorkTable.Add(new List<string>());
                    secondWorkTable[line].Add((firstWorkTable[line])[m + 1]);
                    for (column = 1; column < m + 1; column++)
                    {
                        helpColumn = Convert.ToInt32(milyStates[line, column - 1]);
                        secondWorkTable[line].Add(milyStates[helpColumn, m]);
                        currentSequence = currentSequence + secondWorkTable[line][column - 1] + " ";
                    }
                    if (!(unicSequence.Contains(currentSequence)))
                    {
                        unicSequence.Add(currentSequence);
                    }
                    secondWorkTable[line].Add((unicSequence.IndexOf(currentSequence) + 1).ToString());
                    currentSequence = "";
                    if ((secondWorkTable[line])[0] != (secondWorkTable[line])[m + 1])
                    {
                        endOfMinimisation = false;
                    }
                }
                unicSequence.Clear();
                for (line = 0; line < k; line++)
                {
                    for (column = 0; column < m + 2; column++)
                    {
                        (firstWorkTable[line])[column] = secondWorkTable[line][column];
                    }
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
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m; column++)
                {
                    currentSequence += (Convert.ToInt32(secondWorkTable[line][column + 1]) - 1).ToString() + " " + milyOutputSymbols[line, column] + " ";        
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