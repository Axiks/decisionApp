using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTreeApp
{
    class VirtualTable
    {
        //Row -> Col
        //private string valueCashe;
        public List<List<string>> dataList;
        public int countRow;
        Dictionary<int, string> atrybutLevel = new Dictionary<int, string>(5);

        //public List<int> atrybutLevel; //*
        //public List<string> levelCashe; //*

        public VirtualTable(int countRow)
        {

            //atrybutLevel = new List<int>();
            //levelCashe = new List<string>();
            dataList = new List<List<string>>();
            this.countRow = countRow;
        }

        public void createLevel(int atrybut)
        {
            atrybutLevel.Add(atrybut);
        }

        public void addValue(string value)
        {
            levelCashe.Add(value);
            //valueCashe = value;
            //dataCol[atrybutLevel[atrybutLevel.Count-1]] = value;
        }

        public void addAnswer(string answer)
        {
            List<string> dataCol = new List<string>(countRow);
            for (int i = 0; i < countRow; i++)
            {
                dataCol.Add(null);
            }

            for (int i = 0; i < atrybutLevel.Count; i++)
            {
                dataCol.Insert(atrybutLevel[i], levelCashe[i]);
            }

            //dataCol.Insert(atrybutLevel.Count - 1, valueCashe);
            dataCol.Insert(countRow - 1, answer);
            dataList.Add(dataCol);
            //dataCol[countRow - 1] = answer;
        }

        public void exitLevel()
        {
            if (atrybutLevel.Count > 1)
            {
                atrybutLevel.RemoveAt(atrybutLevel.Count-1);
                levelCashe.RemoveAt(levelCashe.Count - 1);
            }
        }

        public void casheAdd()
        {

        }

        public void casheGet()
        {

        }

        public void casheDelete()
        {

        }
    }
}
