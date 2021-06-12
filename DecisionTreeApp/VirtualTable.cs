using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionTreeApp
{
    class VirtualTable
    {
        private List<List<string>> dataList; //Вигляд таблиці
        private Dictionary<int, string> atrybutLevel; //Стек атрибутів
        private string[] dataColCashe;

        public VirtualTable(int countRow)
        {
            dataList = new List<List<string>>();
            atrybutLevel = new Dictionary<int, string>();
            dataColCashe = new string[countRow];
        }

        public void createLevel(int atrybutIndex)
        {
            atrybutLevel.Add(atrybutIndex, "");
        }

        public void addValue(string value)
        {
            dataColCashe[atrybutLevel.Last().Key] = value;
        }

        public void addAnswer(string answer)
        {
            dataColCashe[dataColCashe.Length - 1] = answer; // Добавлення відповіді

            // 
            for (int i = 1; i < atrybutLevel.Count; i++)
            {
                string lastElement = "pusto";
                //
                //string lastElement = dataList.Last()[atrybutLevel.ElementAt(i-1).Key];
                if (dataColCashe[atrybutLevel.ElementAt(i - 1).Key] == null) {
                    lastElement = dataList.Last()[atrybutLevel.ElementAt(i - 1).Key];
                }
                else
                {
                    lastElement = dataColCashe[atrybutLevel.ElementAt(i - 1).Key];
                }
                dataColCashe[atrybutLevel.ElementAt(i-1).Key] = lastElement;
            }

            List<string> tempList = new List<string>();
            for (int i = 0; i < dataColCashe.Length; i++)
            {
                tempList.Add(dataColCashe[i]);
            }

            dataList.Add(tempList);
            dataColCashe = new string[dataColCashe.Length];
        }

        public void exitLevel()
        {
            if (atrybutLevel.Count > 1)
            {

                atrybutLevel.Remove(atrybutLevel.Keys.Last());
                dataColCashe = new string[dataColCashe.Length];
            }
        }
    }
}
