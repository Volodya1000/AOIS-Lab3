using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class Square
    {
        private Dictionary<(bool, bool), bool> square;

        bool SDNF;
        public Square(bool sdnf)
        {
            SDNF = sdnf;
            square = new Dictionary<(bool, bool), bool>();
        }

        public bool this[bool x, bool y]
        {
            get
            {
                if (square.ContainsKey((x, y)))
                    return SDNF;
                else
                    return !SDNF;
            }
            set { square[(x, y)] = value; }
        }

        public List<List<bool?>> Minimiz()
        {
            if (square.Count == 4)
                return null;
            List<List<bool?>> rez = new List<List<bool?>>();
            if (this[false,false]==SDNF && this[false, true] == SDNF)
            {
                rez.Add(new List<bool?> { false, null});
            }
            if (this[true, false] == SDNF && this[false, false] == SDNF)
            {
                rez.Add(new List<bool?> { null, false});
            }
            if (this[true, true] == SDNF && this[false, true] == SDNF)
            {
                rez.Add(new List<bool?> { true, null });
            }
            if (this[true, true] == SDNF && this[true, false] == SDNF)
            {
                rez.Add(new List<bool?> { null, true });
            }
            return rez;
        }

        public string PrintSquare()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t0\t1");
            for (int i = 0; i <=1; i++)
            {
                sb.Append(i + ":\t");
                for (int j = 0; j <=1; j++)
                {
                    sb.Append(this[i == 1, j==1] ? "1\t" : "0\t");

                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
