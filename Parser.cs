using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lab3
{
    public class Parser
    {
        public static Composite Parse(string line)
        {
            int cursor = 0;
            line = Regex.Replace(line, @"\s+", "");//удаление пробелов
            if (!(Char.IsLetter(line[line.Length - 1]) || line[line.Length - 1] == ')'))
                throw new InvalidOperationException($"Недопустимый символ '{line.Length - 1}' в конце строки Ожидался операнд или закрывающая скобка .");
            int openParenthesesCount = 0;
            foreach (char c in line)
            {
                if (c == '(')
                    openParenthesesCount++;
                else if (c == ')')
                {
                    openParenthesesCount--;
                    if (openParenthesesCount < 0)
                        throw new InvalidOperationException("Неправильное количество закрывающих скобок.");
                }
            }
            if (openParenthesesCount != 0)
                throw new InvalidOperationException("Неправильное количество открывающих и закрывающих скобок.");
            return Parse(line, ref cursor);
        }


        private static Composite Parse(string line, ref int cursor)
        {
            Composite? ret = null;
            int startcur = cursor;
            char NextS = '.';
            while (cursor < line.Length)
            {
                char s = line[cursor];
                if (cursor <= line.Length - 2)
                    NextS = line[cursor + 1];
                if (s == ')') return ret; // конец подскобочного выражения 
                else if (s == '(')        // начало подскобочного выражения
                {
                    cursor++;
                    if (ret == null)
                        ret = Parse(line, ref cursor);
                    else
                        ret.Add(Parse(line, ref cursor));// встраиваем подскобочное
                }
                else if (s == '|')
                {
                    if (!(NextS == '(' || NextS == '!' || Char.IsLetter(NextS)))
                        throw new InvalidOperationException($"Недопустимый символ '{NextS}' Ожидался операнд, открывающая скобка или отрицание.");
                    else if (ret is AndComposite) // продолжение строки с другим оператором
                    {
                        OrComposite NewRet = new OrComposite();
                        NewRet.Add(ret);
                        ret = NewRet;
                    }
                    else if (ret is NotComposite)
                    {
                        if (Char.IsLetter(NextS) && cursor <= line.Length - 3 && line[cursor + 2] == ')')
                        {
                            cursor++;
                            OrComposite NewRet = new OrComposite();
                            NewRet.Add(ret);
                            Leaf l = new Leaf(line[cursor]);
                            NewRet.Add(l);
                            ret = NewRet;
                        }
                        else
                        {
                            var NewParse = Parse(line, ref cursor);
                            if (NewParse is OrComposite)
                            {
                                OrComposite NewRet = new OrComposite();
                                NewRet.Add(ret);
                                foreach (var i in NewParse._children)
                                    NewRet.Add(i);
                                ret = NewRet;
                                return ret;

                            }
                            else
                            {
                                OrComposite NewRet = new OrComposite();
                                NewRet.Add(ret);
                                NewRet.Add(NewParse);
                                ret = NewRet;
                                return ret;
                            }
                        }
                    }
                }
                else if (s == '&')
                {
                    if (!(NextS == '(' || NextS == '!' || Char.IsLetter(NextS)))
                        throw new InvalidOperationException($"Недопустимый символ '{NextS}' Ожидался операнд, открывающая скобка или отрицание.");
                    else if (ret is OrComposite)
                    {
                        AndComposite NewRet = new AndComposite();
                        NewRet.Add(ret);
                        ret = NewRet;
                    }
                    else if (ret is NotComposite)
                    {
                        if (Char.IsLetter(NextS) && cursor <= line.Length - 3 && line[cursor + 2] == ')')
                        {
                            cursor++;
                            AndComposite NewRet = new AndComposite();
                            NewRet.Add(ret);
                            Leaf l = new Leaf(line[cursor]);
                            NewRet.Add(l);
                            ret = NewRet;
                        }
                        else
                        {
                            var NewParse = Parse(line, ref cursor);
                            if (NewParse is AndComposite)
                            {
                                AndComposite NewRet = new AndComposite();
                                NewRet.Add(ret);
                                foreach (var i in NewParse._children)
                                    NewRet.Add(i);
                                return NewRet;
                            }
                            else
                            {
                                AndComposite NewRet = new AndComposite();
                                NewRet.Add(ret);
                                NewRet.Add(NewParse);
                                return NewRet;
                            }
                        }
                    }
                }
                else if (s == '!')
                {
                    cursor++;
                    if (ret == null) //самое начало строки и первый символ !
                    {
                        ret = new NotComposite();
                    }
                    if (NextS == '(')
                    {
                        cursor--;// встраиваем подскобочное 
                    }
                    else if (!(ret is NotComposite) && Char.IsLetter(line[cursor]))
                    {
                        NotComposite Not = new NotComposite();
                        Not.Add(new Leaf(line[cursor]));
                        ret.Add(Not);
                    }
                    else if (Char.IsLetter(NextS))
                        ret.Add(new Leaf(NextS));
                    else
                        throw new InvalidOperationException($"Недопустимый символ '{line[cursor]}' после отрицания.");
                }
                else
                {
                    if (ret == null) // это самый левый операнд типа буква
                    {
                        if (NextS == '&')
                            ret = new AndComposite();
                        else if (NextS == '|')
                            ret = new OrComposite();
                        else throw new InvalidOperationException($"Недопустимый символ '{NextS}' после операнда.");
                        ret.Add(new Leaf(s));
                    }
                    // левый операнд
                    else if (cursor < line.Length - 1 &&
                            NextS == '&' && ret is OrComposite)//следующее |
                    {
                        if (startcur != 0 && line[startcur - 1] == '&')
                        {
                            ret.Add(new Leaf(s));
                            return ret;
                        }
                        else
                            ret.Add(Parse(line, ref cursor));
                    }
                    else if (cursor < line.Length - 1 &&
                            line[cursor + 1] == '|' && ret is AndComposite)//следующее &
                    {
                        if (startcur != 0 && line[startcur - 1] == '|')
                        {
                            ret.Add(new Leaf(s));
                            return ret;
                        }
                        else
                        {
                            ret.Add(new Leaf(s));
                            cursor++;
                            if (cursor==line.Length-2||line[cursor+2]==')')
                            {
                                cursor++;
                                OrComposite NewRet = new OrComposite();
                                NewRet.Add(ret);
                                NewRet.Add(new Leaf(line[cursor]));
                                return NewRet;
                            }
                            else
                            {
                                var NewParse = Parse(line, ref cursor);
                                OrComposite NewRet = new OrComposite();
                                NewRet.Add(ret);
                                NewRet.Add(NewParse);
                                ret = NewRet;
                            }
                           
                        }
                    }
                    else
                        ret.Add(new Leaf(s));             // правый операнд той же операцмм
                }
                cursor++;
            }
            return ret;
        }


        public static string ConvertToRPN(IComponent root)
        {
            StringBuilder result = new StringBuilder();
            Stack<IComponent> stack = new Stack<IComponent>();

            DepthFirstTraversal(root, stack);

            while (stack.Count > 0)
            {
                IComponent component = stack.Pop();
                if (component is Leaf)
                {
                    result.Append(((Leaf)component).GetValue());
                }
                else if (component is Composite)
                {
                    if (component is NotComposite)
                    {
                        result.Append("!");
                    }
                    else if (component is AndComposite)
                    {
                        result.Append("&");
                    }
                    else if (component is OrComposite)
                    {
                        result.Append("|");
                    }
                }
            }

            return result.ToString().Trim();
        }

        private static void DepthFirstTraversal(IComponent node, Stack<IComponent> stack)
        {
            if (node is Leaf)
            {
                stack.Push(node);
            }
            else if (node is Composite)
            {
                stack.Push(node);
                Composite composite = (Composite)node;
                for(int i= composite._children.Count-1; i>=0;i-- )
                {
                    DepthFirstTraversal(composite._children[i], stack);
                }
            }
        }

    }

}

