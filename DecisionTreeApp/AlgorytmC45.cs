using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DecisionTreeApp
{
    class AlgorytmC45
    {
        public string[,] data;
        public List<List<string>> dataList;

        VirtualTable virtualTable;

        public AlgorytmC45()
        {
            string path = @"adult.data";// @"car.data";
            //string path = @"gielda.txt";// @"car.data";
            //string path = @"breast-cancer.data";// @"car.data";
            List<string> rows = File.ReadAllLines(path).ToList(); //Читання файлу
            List<List<string>> properSet = ProcessData(rows); //Список поділений через кому
            //CreateTree(properSet); //Створення дерева на підставі інфи
            dataList = new List<List<string>>();
            List<int> cols = new List<int>() { 0, 3, 9, 12, 14 };

            properSet = TakeTheCol(properSet, cols);
            virtualTable = new VirtualTable(properSet[0].Count);
            data = new string[properSet.Count, properSet[0].Count];

            CreateTree(properSet); //Створення дерева на підставі інфи

            Console.WriteLine("Load Func");

        }

        private static List<List<string>> TakeTheCol(List<List<string>> properSet, List<int> cols)
        {
            List<List<string>> properNewSet = new List<List<string>>();
            for (int i = 0; i < properSet.Count; i++)
            {
                List<string> oneSet = new List<string>();
                for (int j = 0; j < cols.Count; j++)
                {
                    var q = properSet[i][cols[j]];
                    oneSet.Add(q);
                }
                properNewSet.Add(oneSet);
            }
            return properNewSet;
        }

        public void CreateTree(List<List<string>> properSet, int addition = 0)
        {
            List<string> decisionValues = CreateDecisionValues(properSet); //Створення списку рішень на підставі ліста
            List<Dictionary<string, int>> countOfValues = PrepareDictionary(properSet); //Список з кількістю унікальних значень у кожній колонці
            List<double> entropies = CountEntropies(countOfValues, properSet);//Вираховує ентропію
            List<double> informationForColumn = InformationFunction(countOfValues, properSet, decisionValues);//Кількість інформації
            List<double> gains = CheckGains(countOfValues, informationForColumn, entropies);
            List<double> gainRatio = CountGainRatio(gains, entropies);
            (double, int) biggestGainRatioIndex = SelectBiggestGainRatio(gainRatio);
            int attributeIndex = biggestGainRatioIndex.Item2;
            double attributeValue = biggestGainRatioIndex.Item1;

            tableMatrix(attributeValue, attributeIndex, properSet, addition);

            //Render tree
            //if (attributeValue != 0)
            //{
            //    Console.WriteLine("Atrybut: " + (attributeIndex + 1));
            //    IOrderedEnumerable<string> values = ValuesInColumn(properSet, attributeIndex).OrderBy(x => x);
            //    foreach (string value in values)
            //    {
            //        //Значення
            //        Console.Write(new string(' ', addition) + value + " -> ");
            //        List<List<string>> newDecisionSet = new List<List<string>>();
            //        foreach (List<string> row in properSet)
            //        {
            //            if (row[attributeIndex] == value)
            //                newDecisionSet.Add(row);
            //        }
            //        CreateTree(newDecisionSet, addition + 4);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("D: " + properSet.LastOrDefault().LastOrDefault());
            //}
        }

        public void tableMatrix(double attributeValue, int attributeIndex, List<List<string>> properSet, int addition = 0)
        {
            //string[] title = { "age", "workclass", "fnlwgt", "education", "education-num", "marital-status", "occupation", "relationship", "race", "sex", "capital-gain", "capital-loss", "hours-per-week", "native-country", " salary" };

            if (attributeValue != 0)
            {
                Console.WriteLine("Atrybut: " + (attributeIndex + 1));
                virtualTable.createLevel(attributeIndex);
                IOrderedEnumerable<string> values = ValuesInColumn(properSet, attributeIndex).OrderBy(x => x);
                foreach (string value in values)
                {
                    Console.Write(new string(' ', addition) + value + " -> ");
                    virtualTable.addValue(value);
                    List<List<string>> newDecisionSet = new List<List<string>>();
                    foreach (List<string> row in properSet)
                    {
                        if (row[attributeIndex] == value)
                            newDecisionSet.Add(row);
                    }
                    CreateTree(newDecisionSet, addition + 4);
                }
            }
            else
            {
                //virtualTable.addValue(value, properSet.LastOrDefault().LastOrDefault());
                virtualTable.addAnswer(properSet.LastOrDefault().LastOrDefault());
                Console.WriteLine("D: " + properSet.LastOrDefault().LastOrDefault());
            }
        }

        //Розділяє значення
        private static List<List<string>> ProcessData(List<string> rows)
        {
            List<List<string>> properSet = new List<List<string>>();
            for (int i = 0; i < rows.Count; i++)
                properSet.Add(rows[i].Split(',').ToList());
            return properSet;
        }

        private static List<Dictionary<string, int>> PrepareDictionary(List<List<string>> properSet)
        {
            List<Dictionary<string, int>> countOfValues = new List<Dictionary<string, int>>();
            for (int i = 0; i < properSet.FirstOrDefault().Count; i++)
                countOfValues.Add(CheckProperColumn(i, properSet));
            return countOfValues;
        }

        //Рахує скільки разів зустрічається параметр
        private static Dictionary<string, int> CheckProperColumn(int col, List<List<string>> properSet)
        {
            Dictionary<string, int> valuesForColumn = new Dictionary<string, int>();
            for (int i = 0; i < properSet.Count; i++)
            {
                if (valuesForColumn.ContainsKey(properSet[i][col]))
                    valuesForColumn[properSet[i][col]]++;
                else
                    valuesForColumn[properSet[i][col]] = 1;
            }
            return valuesForColumn;
        }

        //Видобуває усі можливі варіанти відповіді
        private static List<string> CreateDecisionValues(List<List<string>> properSet)
        {
            List<string> decisionValues = new List<string>();
            foreach (List<string> row in properSet)
                if (!decisionValues.Contains(row.LastOrDefault()))
                    decisionValues.Add(row.LastOrDefault());
            return decisionValues;
        }

        private static List<double> CountEntropies(List<Dictionary<string, int>> countOfValues, List<List<string>> properSet)
        {
            List<double> entropies = new List<double>();
            for (int i = 0; i < countOfValues.Count; i++)
                entropies.Add(CountEntropy(countOfValues[i], properSet.Count));
            return entropies;
        }

        //Вираховуємо ентропію
        private static double CountEntropy(Dictionary<string, int> dictionary, int sum)
        {
            List<double> elements = new List<double>();
            foreach (int element in dictionary.Values)
            {
                double todo = (double)element / sum;
                if (element == 0)
                    elements.Add(0);
                else
                    //elements.Add(todo * Math.Log2(todo));
                    //log( n ) / log( 2 )
                    elements.Add(todo * (Math.Log(todo) / Math.Log(2)));
            }
            return -elements.Sum();
        }

        //Кількість вмісту інформаціцї??
        private static List<double> InformationFunction(List<Dictionary<string, int>> countOfValues, List<List<string>> properSet, List<string> decisionValues)
        {
            List<double> informationForColumn = new List<double>();
            for (int i = 0; i < countOfValues.Count - 1; i++)
            {
                Dictionary<string, Dictionary<string, int>> attributeToValue = AttributeToValue(i, properSet);
                List<double> listOfValues = new List<double>();
                foreach (KeyValuePair<string, int> x in countOfValues[i])
                {
                    int sum = attributeToValue[x.Key].Values.Sum();
                    double entropy = CountEntropy(GetDictionaryForEntropy(attributeToValue, x.Key, decisionValues), sum);
                    listOfValues.Add((double)x.Value / properSet.Count * entropy);
                }
                informationForColumn.Add(listOfValues.Sum());
            }
            return informationForColumn;
        }

        private static Dictionary<string, Dictionary<string, int>> AttributeToValue(int col, List<List<string>> properSet)
        {
            Dictionary<string, Dictionary<string, int>> attributeToValue = new Dictionary<string, Dictionary<string, int>>();
            for (int i = 0; i < properSet.Count; i++)
            {
                string key = properSet[i][col];
                string value = properSet[i][properSet.FirstOrDefault().Count - 1];
                if (attributeToValue.ContainsKey(key))
                {
                    if (attributeToValue[key].ContainsKey(value))
                        attributeToValue[key][value]++;
                    else
                        attributeToValue[key][value] = 1;
                }
                else
                {
                    attributeToValue[key] = new Dictionary<string, int> { { value, 1 } };
                }
            }
            return attributeToValue;
        }

        private static Dictionary<string, int> GetDictionaryForEntropy(Dictionary<string, Dictionary<string, int>> attributeToValue, string key, List<string> decisionValues)
        {
            Dictionary<string, int> toReturn = new Dictionary<string, int>();
            for (int i = 0; i < decisionValues.Count; i++)
            {
                attributeToValue[key].TryGetValue(decisionValues[i], out int value);
                toReturn.Add(decisionValues[i], value);
            }
            return toReturn;
        }

        private static List<double> CheckGains(List<Dictionary<string, int>> countOfValues, List<double> informationForColumn, List<double> entropies)
        {
            List<double> gains = new List<double>();
            for (int i = 0; i < countOfValues.Count - 1; i++)
                gains.Add(entropies.LastOrDefault() - informationForColumn[i]);
            return gains;
        }

        private static List<double> CountGainRatio(List<double> gains, List<double> entropies)
        {
            List<double> gainRatio = new List<double>();
            for (int i = 0; i < gains.Count; i++)
            {
                if (entropies[i] != 0)
                    gainRatio.Add(gains[i] / entropies[i]);
                else
                    gainRatio.Add(0);
            }
            return gainRatio;
        }

        private static (double, int) SelectBiggestGainRatio(List<double> gainRatio)
        {
            double biggestGainRatio = 0;
            int biggestGainRatioIndex = 0;
            for (int i = 0; i < gainRatio.Count; i++)
            {
                if (gainRatio[i] >= biggestGainRatio)
                {
                    biggestGainRatio = gainRatio[i];
                    biggestGainRatioIndex = i;
                }
            }
            return (biggestGainRatio, biggestGainRatioIndex);
        }

        //Витягує унікальні значення з певної колонки
        private static List<string> ValuesInColumn(List<List<string>> properSet, int attributeIndex)
        {
            List<string> values = new List<string>();
            for (int i = 0; i < properSet.Count; i++)
            {
                values.Add(properSet[i][attributeIndex]);
            }
            return values.Distinct().ToList(); //Видаляє дублікати
        }
    }
}
