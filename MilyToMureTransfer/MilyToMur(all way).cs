namespace ConsoleApplication
{
    // Для обработки чисел с точкой и с запятой, пользоваться классом из видео и делать две проверки, меняя разделитель в классе
    // Отрицательные числа
    // Максимальные значения
    class MainProgram
    {

        static void Main(string[] args)
        {
            string[] mas = Console.ReadLine().Split();
            int k = Convert.ToInt32(mas[0]);
            int m = Convert.ToInt32(mas[1]);
            string[,] mily = new string[k, m];
            int line, column, currentSCondition, currentQCindition, currentOutputSymbol, recurringTransition, currentSNumber;
            int minSNumber = int.MaxValue;
            List<string> unicTransition = new List<string>();
            for (line = 0; line < k; line++)
            {
                mas = Console.ReadLine().Split(", ");                
                for (column = 0; column < m; column++)
                {
                    if ((!unicTransition.Contains(mas[column])) && (mas[column] != "-"))
                    {
                        unicTransition.Add(mas[column]);
                        mily[line, column] = mas[column];
                        mily[line, column] = mas[column] + $" {unicTransition.Count - 1}";
                    }
                    else if (mas[column] != "-")
                    {
                        recurringTransition = unicTransition.IndexOf(mas[column]);
                        mily[line, column] = mas[column] + $" {recurringTransition}";
                    }
                    else
                    {
                        mily[line, column] = mas[column];
                    }
                    if (mas[column] != "-")
                    {
                        currentSNumber = Convert.ToInt32(mily[line, column].Substring(mily[line, column].IndexOf('S') + 1, 1));
                        if (minSNumber > currentSNumber)
                        {
                            minSNumber = currentSNumber;
                        }
                    }
                    
                }
            }
            for (line = 0; line < k; line++)
            {
                for (column = 0; column < m; column++)
                {
                    Console.Write(mily[line, column]);
                }
                Console.WriteLine();
            }

            for (currentQCindition = 0; currentQCindition < unicTransition.Count; currentQCindition++)
            {
                currentSCondition = Convert.ToInt32(unicTransition[currentQCindition].Substring(1, 1));
                currentOutputSymbol = Convert.ToInt32(unicTransition[currentQCindition].Substring(4));
                Console.Write('Y' + $"{currentOutputSymbol}");
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