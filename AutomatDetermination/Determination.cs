using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            int line, column, currentState, count = 0;
            string newTransition = "";
            List<string> newState= new List<string>();
            List<List<string>> resultAutomatLine = new List<List<string>>();

            //Словарь, содержащий обозначения вновьобразовавшихся состояний
            var newStates = new Dictionary<string, string>();

            // Вспомогательный список для хранения eclose каждого начального состояния
            List<List<string>> ecloses = new List<List<string>>();

            // Вспомогательный список для определения завершения детерминирования
            List<List<string>> remainingStates = new List<List<string>>();

            // Таблица результата детерминирования
            List<List<List<string>>> resultAutomat = new List<List<List<string>>>();

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

            // Вспомогательные переменные и списки для детерминизации
            List<string> startList = new List<string>();
            startList.Add("0");
            remainingStates.Add(startList);
            List<string> currentEcloseList = new List<string>();
            string currentEcloseString;
            string workString;
            string[] allSymbols;
            HashSet<string> uniqueSymbolsSet = new HashSet<string>();
            int stringCounter = 0, stateCounter = 0;
            workString = string.Join(",", remainingStates[0]);
            newStates.Add(workString, stateCounter.ToString());
            stateCounter++;

            // Пока в ячейках переходов результирующей таблицы есть состояния, для которых мы не нашли их переходы мы продолжаем формировать таблицу
            while (remainingStates.Count > stringCounter)
            {
                // Берем первое в списке состояиние и будем заполнять его переходы
                currentEcloseList = remainingStates[stringCounter];
                currentEcloseString = "";
                // Из списка eclos-ов формируем связку пустых переходов для текущего состояния
                for (column = 0; column < currentEcloseList.Count; column++)
                {
                    currentEcloseString += "," + (string.Join(",", ecloses[Convert.ToInt32(currentEcloseList[column])]));
                }
                // Убираем дублирование состояний
                allSymbols = currentEcloseString.Split(",");
                uniqueSymbolsSet = new HashSet<string>(allSymbols);
                currentEcloseList = uniqueSymbolsSet.ToList();
                currentEcloseList.RemoveAt(0);
                // Для каждого входного символа формируем состояние перехода
                for (currentState = 0; currentState < m; currentState++)
                {
                    newState = new List<string>();
                    for (column = 0; column < currentEcloseList.Count; column++)
                    {
                        newTransition = currentEcloseList[column];
                        mas = initialStates[Convert.ToInt32(newTransition), currentState].Split(",");
                        for (line = 0; line < mas.Length; line++)
                        {
                            if (!newState.Contains(mas[line]) && (mas[line] != "-"))
                            {
                                newState.Add(mas[line]);
                            }
                        }
                    }
                    // Записываем полученное состояние в соответствующую ячейку результирующей таблицы
                    resultAutomatLine.Add(newState);

                    if (newState.Count == 0)
                    {
                        newState.Add("-");
                        
                    }
                    else
                    {
                        // Если полученное состояние еще не встречалось, то добавляем его в очередь для последующего заполнения таблицы 
                        //if (!remainingStates.Contains(newState))
                        if (!remainingStates.Any(o => o.SequenceEqual(newState)))
                        {
                            remainingStates.Add(newState);
                            // Добавляем в словарь новое состояние всесте с его новым обозначением для последующего вывода
                            workString = string.Join(",", newState);
                            newStates.Add(workString, stateCounter.ToString());
                            stateCounter++;
                        }
                    }                                  
                }
                // Добавляем в итоговою таблицу все переходы для обработанного состояния
                resultAutomat.Add(resultAutomatLine);
                resultAutomatLine= new List<List<string>>();
                stringCounter++;
                if (remainingStates.Count <= stringCounter)
                {
                    bool what;
                    Console.WriteLine();
                    for (line = 0; line < resultAutomat.Count; line++)
                    {
                        for (column = 0; column < resultAutomat[line].Count; column++)
                        {
                            workString = string.Join(",", resultAutomat[line][column]);
                            what = newStates.TryGetValue(workString, out newTransition);
                            if (what)
                            {
                                Console.Write(newTransition + " ");
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
    }
}