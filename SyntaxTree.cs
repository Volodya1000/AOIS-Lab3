using System;
using System.Collections.Generic;

namespace Lab3
{
    // Общий интерфейс компонентов
    public interface IComponent
    {
        IComponent EvaluateWithPartialValues(Dictionary<char, bool> partialValues);
    }

    public class Composite : IComponent
    {
        public List<IComponent> _children = new List<IComponent>();

        public void Add(IComponent component)
        {
            _children.Add(component);
        }

        public void Remove(IComponent component)
        {
            _children.Remove(component);
        }

        public virtual IComponent EvaluateWithPartialValues(Dictionary<char, bool> partialValues)
        {
            throw new NotImplementedException("EvaluateWithPartialValues method should be implemented in derived classes.");
        }
    }

    public class AndComposite : Composite
    {
        public override IComponent EvaluateWithPartialValues(Dictionary<char, bool> partialValues)
        {
            AndComposite result = new AndComposite();
            foreach (var child in _children)
            {
                IComponent evaluatedChild = child.EvaluateWithPartialValues(partialValues);
                if (evaluatedChild is Leaf leaf)
                {
                    if (leaf._value == '0')
                    {
                        return new Leaf('0');
                    }
                    else if (leaf._value == '1')
                    {
                        // Пропускаем '1', так как '1' && x = x
                        continue;
                    }
                }
                result.Add(evaluatedChild);
            }
            foreach (var child in _children)
            {
                foreach (var child1 in _children)
                {
                    if (child is Leaf l1 &&
                   child1 is NotComposite n &&
                    n._children.Count == 1 &&
                    n._children[0] is Leaf l2 &&
                    l1._value == l2._value)
                        return new Leaf('0');
                    else if (child1 is Leaf l3 &&
                   child is NotComposite n1 &&
                    n1._children.Count == 1 &&
                    n1._children[0] is Leaf l4 &&
                    l3._value == l4._value)
                        return new Leaf('0');
                }
            }
            if (result._children.Count == 1)
            {
                return result._children[0];
            }
            if (result._children.Count == 0 )
            {
                return new Leaf('1');
            }

            return result;
        }
    }

    public class OrComposite : Composite
    {
        public override IComponent EvaluateWithPartialValues(Dictionary<char, bool> partialValues)
        {
            OrComposite result = new OrComposite();
            foreach (var child in _children)
            {
                IComponent evaluatedChild = child.EvaluateWithPartialValues(partialValues);
                if (evaluatedChild is Leaf leaf)
                {
                    if (leaf._value == '1')
                    {
                        return new Leaf('1');
                    }
                    else if (leaf._value == '0')
                    {
                        // Пропускаем '0', так как '0' || x = x
                        continue;
                    }
                }
                result.Add(evaluatedChild);
            }
            bool has_change = false;
            while (has_change)
            {
                bool br = false;
                int i = -1, j = -1;
                for (int ii = 0; ii < _children.Count && !has_change; ii++)
                {
                    if (has_change == true)
                        break;
                    var child = _children[ii];
                    for (int jj = 0; j < _children.Count; j++)
                    {
                        var child1 = _children[jj];
                        if (child is Leaf l1 &&
                       child1 is NotComposite n &&
                        n._children.Count == 1 &&
                        n._children[0] is Leaf l2 &&
                        l1._value == l2._value)
                        {
                            i = ii;
                            j = jj;
                            has_change = true;
                            _children.Remove(child);
                            _children.Remove(child1);
                            break;
                        }
                        else if (child1 is Leaf l3 &&
                       child is NotComposite n1 &&
                        n1._children.Count == 1 &&
                        n1._children[0] is Leaf l4 &&
                        l3._value == l4._value)
                        {
                            i = ii;
                            j = jj;
                            has_change = true;
                            break;
                        }
                    }
                }
            }
            if (result._children.Count == 2 )
            {
                if (result._children[0] is Leaf l1 &&
                    result._children[1] is NotComposite n &&
                    n._children.Count==1 && 
                    n._children[0] is Leaf l2 &&
                    l1._value==l2._value)
                       return new Leaf('1');
                else if (result._children[1] is Leaf l3 &&
                    result._children[0] is NotComposite n3 &&
                    n3._children.Count == 1 &&
                    n3._children[0] is Leaf l4 &&
                    l3._value == l4._value)
                    return new Leaf('1');
            }

            // Если результат содержит только один элемент, который является листом, вернем этот лист
            if (result._children.Count == 1)
            {
                return result._children[0];
            }
            if (result._children.Count == 0)
            {
                return new Leaf('0');
            }

            return result;
        }
    }

    public class NotComposite : Composite
    {
        public override IComponent EvaluateWithPartialValues(Dictionary<char, bool> partialValues)
        {
            if (_children.Count != 1)
            {
                throw new InvalidOperationException("Not operation requires exactly one operand.");
            }

            IComponent evaluatedChild = _children[0].EvaluateWithPartialValues(partialValues);
            if (evaluatedChild is Leaf leaf)
            {
                if (leaf._value == '0')
                {
                    return new Leaf('1');
                }
                else if (leaf._value == '1')
                {
                    return new Leaf('0');
                }
            }
            return new NotComposite { _children = { evaluatedChild } };
        }
    }

    public class Leaf : IComponent
    {
        public char _value;

        public Leaf(char value)
        {
            _value = value;
        }

        public IComponent EvaluateWithPartialValues(Dictionary<char, bool> partialValues)
        {
            if (partialValues.ContainsKey(_value))
            {
                return partialValues[_value] ? new Leaf('1') : new Leaf('0');
            }
            return this;
        }

        public char GetValue()
        {
            return _value;
        }
    }
}