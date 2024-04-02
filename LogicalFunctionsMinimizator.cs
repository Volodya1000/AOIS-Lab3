using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class LogicalFunctionsMinimizator
    {
        public List<List<bool?>> SKNF ;
        public List<List<bool?>> SDNF ;
        public List<char> UniqueLetters = new List<char>();

        public string Skleivanie;
        public string CalculationMethodRez;
        public string CalculationTableMethodRez;
        public string KarnoMapMethodRez;
        public string KarnoTable;
        public string ImplicantTable;

        public bool IsPerfect = false;
        public LogicalFunctionsMinimizator(Composite expr)
        {
           if(IsSKNF(expr))
           {
                IsPerfect = true;
                Skleivanie = GenerateString(CalculationMethod(SKNF),false);
                CalculationMethodRez = GenerateString(DeleteImlications(CalculationMethod(SKNF)), false);
                CalculationTableMethodRez = GenerateString(CalculationTableMethod(SKNF), false);
                KarnoMapMethodRez = GenerateString(KarnoMapMethod(SKNF), false);
            }
           else if(IsSDNF(expr))
           {
                IsPerfect = true;
                Skleivanie = GenerateString(CalculationMethod(SDNF),true);
                CalculationMethodRez = GenerateString(DeleteImlications(CalculationMethod(SDNF)), true);
                CalculationTableMethodRez = GenerateString(CalculationTableMethod(SDNF), true);
                KarnoMapMethodRez = GenerateString(KarnoMapMethod(SDNF), true);
            }

        }


        public List<List<bool?>> CalculationMethod(List<List<bool?>> expr)
        {
            List<List<bool?>> result = new List<List<bool?>>(expr); 
            List<List<bool?>> Copy;
            bool NewChanges = true;
            for (int i = result[0].Count ; i >= 2 && NewChanges; i--)
            {
                NewChanges = false;
                Copy = new List<List<bool?>>(result); 
                for (int j = 0; j < Copy.Count; j++)
                {
                    for (int k = j+1; k < Copy.Count; k++)
                    {
                        int index = FindDifferentElement(Copy[j], Copy[k]);
                        if (index != -1)
                        {
                            result.Remove(Copy[j]); 
                            result.Remove(Copy[k]); 
                            List<bool?> newItem = new List<bool?>(Copy[j]); 
                            newItem[index] = null;
                            if(!result.Any(list => list.SequenceEqual(newItem)))
                                result.Add(newItem); 
                            NewChanges = true;
                        }
                    }
                }
            }
            return result;
        }

        public List<List<bool?>> DeleteImlications(List<List<bool?>> expr)
        {
            if (expr == null)
                return null;
            List<List<bool?>> result = new List<List<bool?>>(expr);
            List<List<bool?>> CicleList;
            Dictionary<char, bool> LettersValues = new Dictionary<char, bool>();

            bool WasDeletionOfImpicant = true;
            while (WasDeletionOfImpicant)
            {
                CicleList = result;
                WasDeletionOfImpicant = false;
                foreach (List<bool?> elem in CicleList)
                {
                    List<List<bool?>> TestExpr = new List<List<bool?>>(expr);
                    TestExpr.Remove(elem);
                    LettersValues = GetLetterValues(elem);
                    Composite TestComposite = ConvertListListBoolInComposite(TestExpr);
                    IComponent ExpressionValue = TestComposite.EvaluateWithPartialValues(LettersValues);
                    if (ExpressionValue is Leaf l)
                        if (SDNF != null)
                        {
                            if (l._value == '1')
                            {
                                result.Remove(elem);
                                WasDeletionOfImpicant = true;
                                break;
                            }
                        }
                        else
                        {
                            if (l._value == '0')
                            {
                                result.Remove(elem);
                                WasDeletionOfImpicant = true;
                                break;
                            }
                        }
                }
            }
            return result;
        }

        public Dictionary<char, bool> GetLetterValues(List<bool?> elem)
        {
            Dictionary<char, bool> LettersValues = new Dictionary<char, bool>();
            Composite composite;
            int VarCount = 0;
            if (SDNF != null)
            {
                composite = new AndComposite();
                for (int i = 0; i < elem.Count(); i++)
                    if (elem[i] != null)
                    {
                        if (elem[i] == true)
                        {
                            composite.Add(new Leaf(UniqueLetters[i]));
                            VarCount++;
                        }
                        else
                        {
                            NotComposite notComposite = new NotComposite();
                            notComposite.Add(new Leaf(UniqueLetters[i]));
                            composite.Add(notComposite);
                            VarCount++;
                        }
                        LettersValues.Add(UniqueLetters[i], false);
                    }
            }

            else
            {
                composite = new OrComposite();
                for (int i = 0; i < elem.Count(); i++)
                    if (elem[i] != null)
                    {
                        if (elem[i] == true)
                        {
                            composite.Add(new Leaf(UniqueLetters[i]));
                            VarCount++;
                        }
                        else
                        {
                            NotComposite notComposite = new NotComposite();
                            notComposite.Add(new Leaf(UniqueLetters[i]));
                            composite.Add(notComposite);
                            VarCount++;
                        }
                        LettersValues.Add(UniqueLetters[i], false);
                    }
            }
            int rowsCount = (int)Math.Pow(2, VarCount);
            for (int i = 0; i < rowsCount; i++)
            {
                int j = 0;
                List<bool> binaryList= BinaryRepresentation(i, VarCount);
                foreach (var kvp in LettersValues)
                {
                    LettersValues[kvp.Key] = binaryList[j];
                    j++;
                }
                IComponent ExpressionValue=composite.EvaluateWithPartialValues(LettersValues);
                if (SDNF != null)
                {
                    if (((Leaf)ExpressionValue)._value == '1')
                    {
                        return LettersValues;
                    }
                }
                else
                {
                    if (((Leaf)ExpressionValue)._value == '0')
                    {
                        return LettersValues;
                    }
                }
            }

            return LettersValues;
        }

        private static List<bool> BinaryRepresentation(int n, int len)
        {
            List<bool> binaryList = new List<bool>();

            for (int i = len - 1; i >= 0; i--)
            {
                int bit = (n >> i) & 1;
                binaryList.Add(bit == 1);
            }

            return binaryList;
        }


        public Composite ConvertListListBoolInComposite(List<List<bool?>> expr)
        {
            Composite result;
            if (SDNF != null)
            {
                result = new OrComposite();
                foreach (List<bool?> elem in expr)
                {
                    if (elem.Count(item => item != null) == 1)
                    {
                        for (int i = 0; i < elem.Count(); i++)
                            if (elem[i] != null)
                            {
                                if (elem[i] == true)
                                    result.Add(new Leaf(UniqueLetters[i]));
                                else
                                {
                                    NotComposite notComposite = new NotComposite();
                                    notComposite.Add(new Leaf(UniqueLetters[i]));
                                    result.Add(notComposite);
                                }
                                break;
                            }
                    }
                    else
                    {
                        AndComposite andComposite = new AndComposite();
                        for (int i = 0; i < elem.Count(); i++)
                            if (elem[i] != null)
                                if (elem[i] == true)
                                    andComposite.Add(new Leaf(UniqueLetters[i]));
                                else
                                {
                                    NotComposite notComposite = new NotComposite();
                                    notComposite.Add(new Leaf(UniqueLetters[i]));
                                    andComposite.Add(notComposite);
                                }
                        result.Add(andComposite);
                    }
                }
            }

            else
            {
                result = new AndComposite();
                foreach (List<bool?> elem in expr)
                {
                    if (elem.Count(item => item != null) == 1)
                    {
                        for (int i = 0; i < elem.Count(); i++)
                            if (elem[i] != null)
                            {
                                if (elem[i] == true)
                                    result.Add(new Leaf(UniqueLetters[i]));
                                else
                                {
                                    NotComposite notComposite = new NotComposite();
                                    notComposite.Add(new Leaf(UniqueLetters[i]));
                                    result.Add(notComposite);
                                }
                                break;
                            }
                    }
                    else
                    {
                        OrComposite orComposite = new OrComposite();
                        for (int i = 0; i < elem.Count(); i++)
                            if (elem[i] != null)
                                if (elem[i] == true)
                                    orComposite.Add(new Leaf(UniqueLetters[i]));
                                else
                                {
                                    NotComposite notComposite = new NotComposite();
                                    notComposite.Add(new Leaf(UniqueLetters[i]));
                                    orComposite.Add(notComposite);
                                }
                        result.Add(orComposite);
                    }
                }
            }
            return result;
        }

        public List<List<bool?>> CalculationTableMethod(List<List<bool?>> expr)
        {
            List<List<bool?>> result = new List<List<bool?>>(expr);
            TableCalculatedMethod t = new(expr, CalculationMethod(expr),UniqueLetters,SDNF!=null);
            ImplicantTable = t.PrintTable();
            return t.DeleteImplicants();
        }

        public List<List<bool?>> KarnoMapMethod(List<List<bool?>> expr)
        {
            if (UniqueLetters.Count > 3)
                return null;
            List<List<bool?>> result = new List<List<bool?>>(expr);
            if(UniqueLetters.Count == 2)
            {
                bool f = SDNF != null;
                Square sqare = new(f);
                foreach (List<bool?> d in expr)
                {
                    sqare[d[0] ?? false, d[1] ?? false] = f;
                }
                KarnoTable = sqare.PrintSquare();
                return DeleteImlications(sqare.Minimiz());
            }
            else if (UniqueLetters.Count == 3)
            {
                bool f = SDNF != null;
                Cube cube = new(f);
                foreach( List<bool?> d in expr)
                {
                    cube[d[0] ?? false, d[1] ?? false, d[2] ?? false]= f;
                }
                KarnoTable = cube.PrintCube();
                return DeleteImlications(cube.Minimiz());
            }

            
            return result;
        }

        private int FindDifferentElement(List<bool?> list1, List<bool?> list2)
        {
            if (list1.Count != list2.Count)
                throw new ArgumentException("Списки должны иметь одинаковый размер");
            int differentIndex = -1;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                    if (list1[i] == null)
                        return -1;
                    else if (differentIndex == -1)
                        differentIndex = i;
                    else
                        return  -1;
            }
            return differentIndex;
        }

        public string GenerateString(List<List<bool?>> expr, bool SDNF) 
        {
            if (expr == null|| expr.Count==0)
                if (SDNF)
                    return "1";
                else return "0";
            List<string> results = new List<string>();
            char s1, s2;
            if (SDNF)
            { s1 = '&'; s2 = '|'; }
            else
            { s1 = '|'; s2 = '&'; }
            foreach (var innerList in expr)
            {
                List<string> result = new List<string>();
                for (int i = 0; i < innerList.Count; i++)
                {
                    if (innerList[i] == null)
                        continue;

                    if (innerList[i] == false)
                        result.Add("!" + UniqueLetters[i]);
                    else
                        result.Add(UniqueLetters[i].ToString());
                }

                string expression = string.Join(s1, result);
                if (result.Count != 1)
                    expression = "(" + expression + ")";

                results.Add(expression);
            }

            return string.Join(s2, results);
        }



        public bool IsSKNF(Composite expression)
        {
            if (!(expression is AndComposite))
                return false;

            List<char> uniqueLetters = null; // Список символов первого OrComposite

            List<List<bool>> binaryNumbers = new List<List<bool>>();

            foreach (var child in ((AndComposite)expression)._children)
            {
                if (!(child is OrComposite))
                    return false;

                var orComposite = (OrComposite)child;

                if (uniqueLetters == null) // Если это первый OrComposite
                {
                    uniqueLetters = new List<char>(); // Создаем список символов
                    if (!IsValideComposite(orComposite, uniqueLetters, binaryNumbers))
                        return false;
                }
                else // Если это не первый OrComposite
                {
                    // Создаем локальный список символов
                    List<char> localUniqueLetters = new List<char>(uniqueLetters);
                    if (!IsValideComposite(orComposite, localUniqueLetters, binaryNumbers))
                        return false;

                    // Проверяем совпадение локального списка символов с uniqueLetters
                    if (!localUniqueLetters.SequenceEqual(uniqueLetters))
                        return false;
                }

                // Получение двоичного представления OrComposite
                List<bool> binaryRepresentation = GetBinaryRepresentation(orComposite);

                // Проверка уникальности двоичного представления
                if (binaryNumbers.Any(number => number.SequenceEqual(binaryRepresentation)))
                    return false; // Если не удалось добавить двоичное число в список, это означает дубликат и возвращаем false
                else
                    binaryNumbers.Add(binaryRepresentation);
            }
            SKNF = binaryNumbers.ConvertAll(sublist => sublist.ConvertAll(b => (bool?)b));
            UniqueLetters = uniqueLetters;
            return true;
        }

        public bool IsSDNF(Composite expression)
        {
            if (!(expression is OrComposite))
                return false;

            List<char> uniqueLetters = null; // Список символов первого OrComposite

            List<List<bool>> binaryNumbers = new List<List<bool>>();

            foreach (var child in ((OrComposite)expression)._children)
            {
                if (!(child is AndComposite))
                    return false;

                if (uniqueLetters == null) // Если это первый OrComposite
                {
                    uniqueLetters = new List<char>(); // Создаем список символов
                    if (!IsValideComposite((Composite)child, uniqueLetters, binaryNumbers))
                        return false;
                }
                else // Если это не первый OrComposite
                {
                    // Создаем локальный список символов
                    List<char> localUniqueLetters = new List<char>(uniqueLetters);
                    if (!IsValideComposite((Composite)child, localUniqueLetters, binaryNumbers))
                        return false;

                    // Проверяем совпадение локального списка символов с uniqueLetters
                    if (!localUniqueLetters.SequenceEqual(uniqueLetters))
                        return false;
                }

                // Получение двоичного представления OrComposite
                List<bool> binaryRepresentation = GetBinaryRepresentation((Composite)child);

                // Проверка уникальности двоичного представления
                if (binaryNumbers.Any(number => number.SequenceEqual(binaryRepresentation)))
                    return false; // Если не удалось добавить двоичное число в список, это означает дубликат и возвращаем false
                else
                    binaryNumbers.Add(binaryRepresentation);
            }
            SDNF = binaryNumbers.ConvertAll(sublist => sublist.ConvertAll(b => (bool?)b));
            UniqueLetters = uniqueLetters;
            return true;
        }

        private static bool IsValideComposite(Composite orComposite, List<char> uniqueLetters, List<List<bool>> binaryNumbers)
        {
            uniqueLetters.Clear();
            // Проверка уникальности множества букв
            foreach (var child in orComposite._children)
            {
                if (child is Leaf)
                {
                    var letter = ((Leaf)child)._value;
                    if (uniqueLetters.Contains(letter))
                        return false; // Повторяющаяся буква внутри одного OrComposite
                    uniqueLetters.Add(letter);
                }
                else if (child is NotComposite)
                {
                    // Проверка на содержимое NotComposite
                    if (((NotComposite)child)._children.Count != 1 || !(((NotComposite)child)._children[0] is Leaf))
                        return false;

                    var letter = ((Leaf)((NotComposite)child)._children[0])._value;
                    if (uniqueLetters.Contains(letter))
                        return false; // Повторяющаяся буква внутри одного OrComposite
                    uniqueLetters.Add(letter);
                }
                else
                {
                    // Если внутри OrComposite не Leaf и не NotComposite, то это недопустимое состояние
                    return false;
                }
            }

            return true;
        }


      
       

        // Метод для получения двоичного представления OrComposite
        private static List<bool> GetBinaryRepresentation(Composite Composite)
        {
            List<bool> binaryRepresentation = new List<bool>();

            foreach (var child in Composite._children)
            {
                if (child is Leaf)
                    binaryRepresentation.Add(true);
                else if (child is NotComposite)
                    binaryRepresentation.Add(false);
            }

            return binaryRepresentation;
        }

      
    }


}

