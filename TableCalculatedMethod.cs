using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab3
{
    public class TableCalculatedMethod
    {
        bool[,] table;

        List<List<bool?>> Expr;
        List<List<bool?>> Implicats;
        List<char> Letters;
        bool SDNF;

        public TableCalculatedMethod(List<List<bool?>> expr, List<List<bool?>> implicats, List<char> letters, bool sdnf)
        {
            Expr = expr;
            Implicats = implicats;
            Letters = letters;
            table = new bool[expr.Count, implicats.Count];
            for (int i = 0; i < Expr.Count; i++)
            {
                for (int j = 0; j < Implicats.Count; j++)
                {
                    if (AddX(Expr[i], Implicats[j]))
                        table[i, j] = true;
                    else
                        table[i, j] = false;
                }
            }
        }

        public static bool AddX(List<bool?> Expr, List<bool?> Imp)
        {
            for (int i = 0; i < Expr.Count; i++)
                if (Expr[i] == null && Imp[i] != null)
                    return false;
                else if (Expr[i] != Imp[i]&& Imp[i] != null)
                    return false;
            return true;
        }

        public List<List<bool?>> DeleteImplicants()
        {
            List<List<bool?>> result = new List<List<bool?>>(Implicats);
            bool WasDeletionOfImpicant = true;
            List<int> DeletedRows = new List<int>();
            while (WasDeletionOfImpicant)
            {
                WasDeletionOfImpicant = false;
                for (int i = 0; i < Implicats.Count &&!WasDeletionOfImpicant; i++)
                {
                    if (DeletedRows.Contains(i))
                        continue;
                    bool[] TableWithoutIImplicant = new bool[Expr.Count];
                    for (int j = 0; j < Implicats.Count && !WasDeletionOfImpicant; j++)
                    {
                        if(j!=i)
                            for (int k = 0; k < Expr.Count; k++)
                            {
                                if (TableWithoutIImplicant[k] == false && table[k, j])
                                    TableWithoutIImplicant[k] = true;
                            }

                        if(TableWithoutIImplicant.All(x => x))
                        {
                            DeletedRows.Add(i);
                            WasDeletionOfImpicant = true;
                        }
                    }
                }
            }
            // Удаление элементов из result на основе значений в DeletedRows
            result = result.Where((row, index) => !DeletedRows.Contains(index)).ToList();
            return result;
        }
        
        public string PrintTable()
        {
            StringBuilder sb = new StringBuilder();

            
            List<string> Imp = new List<string>();
            for (int i = 0; i < Implicats.Count; i++)
            {
                string item = PrintTerm(Implicats[i]);
                Imp.Add(item);
            }
            int maxImpLen = Imp.Max(s => s.Length)+1;
            sb.Append(' ', maxImpLen);
            List<string> Exp = new List<string>();
            for (int i = 0; i<Expr.Count; i++)
            {
                string item = PrintTerm(Expr[i]);
                Exp.Add(item);
                sb.Append(item+"  ");
            }
            sb.Append("\n");
            for (int i = 0; i <=Implicats.Count-1; i++)
            {
                sb.Append(Imp[i]+" ");
                int prob = maxImpLen -Imp[i].Count();
                for (int j = 0; j <= Expr.Count-1; j++)
                {
                    if (table[j, i])
                    {
                        sb.Append('X', Exp[j].Length);
                        sb.Append("  ");
                    }
                    else
                        sb.Append(' ', Exp[j].Length + 2);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public string PrintTerm(List<bool?> term)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < term.Count; i++)
            {
                if (term[i] == null)
                    continue;

                if (term[i] == false)
                    result.Add("!" + Letters[i]);
                else
                    result.Add(Letters[i].ToString());
            }
            if(SDNF)
                return  string.Join('&', result);
            else
                return string.Join('|', result);

        }
    }
}
