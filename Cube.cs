using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Lab3
{
    class Cube
    {
        private Dictionary<(bool, bool, bool), bool> cube;
        bool SDNF;
        public Cube(bool sdnf)
        {
            SDNF=sdnf;
            cube = new Dictionary<(bool, bool, bool), bool>();
        }
        public bool this[bool x, bool y, bool z]
        {
            get
            {
                if (cube.ContainsKey((x, y, z)))
                    return SDNF;
                else
                    return !SDNF;
            }
            set { cube[(x, y, z)] = value; }
        }

        private int FindDifferentElement(List<bool> list1, List<bool> list2)
        {
            int differentIndex = -1;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                     if (differentIndex == -1)
                        differentIndex = i;
                    else
                        return -1;
            }
            return differentIndex;
        }

        public List<List<bool?>> Minimiz()
        {
            if (cube.Count == 8)
                return null;
            List<List<bool?>> rez = new List<List<bool?>>();
            int pos = -1;
            for (int i = 0; i <= 1; i++)
            {
                if (this[i == 1, false, false] == SDNF &&
                   this[i == 1, false, true] == SDNF &&
                   this[i == 1, true, false] == SDNF &&
                   this[i == 1, true, true] == SDNF)
                {
                    List<bool?> NewList = new List<bool?> { i == 1, null, null };
                    rez.Add(NewList);
                    pos = 0;
                }
            }
            for (int i = 0; i <= 1; i++)
            {
                if (this[false, i == 1, false] == SDNF &&
                   this[false, i == 1, true] == SDNF &&
                   this[true, i == 1, false] == SDNF &&
                   this[true, i == 1, true] == SDNF)
                {
                    List<bool?> NewList = new List<bool?> { null, i == 1, null };
                    rez.Add(NewList);
                    pos = 1;
                }
            }
            for (int i = 0; i <= 1; i++)
            {
                if (this[false, false, i == 1] == SDNF&&
                   this[false, true, i == 1] == SDNF &&
                   this[true, false, i == 1] == SDNF &&
                   this[true, true, i == 1] == SDNF)
                {
                    List<bool?> NewList = new List<bool?> { null, null, i==1 };
                    rez.Add(NewList);
                    pos = 2;
                }
            }
            if(rez.Count<=1)
            {
                List<List<bool?>> UsedDrops = new List<List<bool?>>();
                List<List<bool>> edges = new List<List<bool>>();
                foreach (var d1 in cube)
                {
                    foreach (var d2 in cube)
                    {
                        List<bool> boolList1 = new List<bool> { d1.Key.Item1, d1.Key.Item2, d1.Key.Item3 };
                        List<bool> boolList2 = new List<bool> { d2.Key.Item1, d2.Key.Item2, d2.Key.Item3 };
                        int n = FindDifferentElement(boolList1, boolList2);
                        if (n!=-1 && d1.Value == SDNF && d2.Value == SDNF && (n==pos||pos==-1))
                        {
                            List<bool?> NewItem = new List<bool?> { d1.Key.Item1, d1.Key.Item2, d1.Key.Item3 };
                            NewItem[n] = null;
                            if (!rez.Any(l => Enumerable.SequenceEqual(l, NewItem)))
                                rez.Add(NewItem);
                        }
                    }
                }
            }
            return rez;
        }


        public string PrintCube()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\t00\t01\t11\t10");

            bool[][] data = {
            new bool[] { false, true },
            new bool[] { false, false, true, true },
            new bool[] { false, true, true,false }};

            for (int i = 0; i < data[0].Length; i++)
            {
                sb.Append(i + ":\t");
                for (int j = 0; j < data[1].Length; j++)
                {
                     sb.Append(this[i == 1, data[1][j], data[2][j]] ? "1\t" : "0\t");
                    
                }
                sb.AppendLine();
            }
           
            return sb.ToString();
        }
    }
}
