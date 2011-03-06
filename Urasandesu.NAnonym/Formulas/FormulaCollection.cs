/* 
 * File: FormulaCollection.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2010 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using Urasandesu.NAnonym;
using System.Collections.Specialized;
using Urasandesu.NAnonym.Linq;

namespace Urasandesu.NAnonym.Formulas
{
    public class FormulaCollection<TFormula> :
        Formula, IList<TFormula>, ICollection<TFormula>, IEnumerable<TFormula>, INotifyCollectionChanged where TFormula : Formula
    {
        IList<TFormula> list;

        public FormulaCollection()
        {
            Initialize(new Collection<TFormula>());
        }

        public FormulaCollection(params TFormula[] formulas)
        {
            Initialize(new Collection<TFormula>(formulas));
        }

        void Initialize(IList<TFormula> list)
        {
            this.list = list;
        }

        public int IndexOf(TFormula item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, TFormula item)
        {
            list.Insert(index, item);
            SetReferrerWithoutNotification(item, this);
            Subscribe(item);
            OnCountPropertyChanged();
            OnItemPropertyChanged();
            ReceiveCollectionAdded(item, index);
        }

        public void RemoveAt(int index)
        {
            var removingItem = list[index];
            list.RemoveAt(index);
            SetReferrerWithoutNotification(removingItem, null);
            Unsubscribe(removingItem);
            OnCountPropertyChanged();
            OnItemPropertyChanged();
            ReceiveCollectionRemoved(removingItem, index);
        }

        public TFormula this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                var replacingItem = list[index];
                list[index] = value;
                SetReferrerWithoutNotification(replacingItem, null);
                Unsubscribe(replacingItem);
                SetReferrerWithoutNotification(value, Referrer);
                Subscribe(value);
                OnItemPropertyChanged();
                ReceiveCollectionReplaced(value, replacingItem, index);
            }
        }

        public void Add(TFormula item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            foreach (var removingItem in list)
            {
                SetReferrerWithoutNotification(removingItem, null);
                Unsubscribe(removingItem);
            }
            list.Clear();
            OnCountPropertyChanged();
            OnItemPropertyChanged();
            ReceiveCollectionReset();
        }

        public bool Contains(TFormula item)
        {
            return list.Contains(item);
        }

        public void CopyTo(TFormula[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        public bool Remove(TFormula item)
        {
            var success = list.Remove(item);
            if (success)
            {
                SetReferrerWithoutNotification(item, null);
                Unsubscribe(item);
                OnCountPropertyChanged();
                OnItemPropertyChanged();
                ReceiveCollectionRemoved(item);
            }
            return success;
        }

        public IEnumerator<TFormula> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public const string NameOfCount = "Count";
        protected void OnCountPropertyChanged()
        {
            ReceivePropertyChanged(NameOfCount);
        }

        public const string NameOfItem = "Item[]";
        protected void OnItemPropertyChanged()
        {
            ReceivePropertyChanged(NameOfItem);
        }

        public override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CollectionChangedEventManager))
            {
                return ReceiveCollectionChangedWithReentrantGuard(sender, (NotifyCollectionChangedEventArgs)e);
            }
            else
            {
                return base.ReceiveWeakEvent(managerType, sender, e);
            }
        }

        bool duringCollectionChanged;
        bool ReceiveCollectionChangedWithReentrantGuard(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (duringCollectionChanged) return true;
            try
            {
                duringCollectionChanged = true;
                return ReceiveCollectionChangedCore(sender, e);
            }
            finally
            {
                duringCollectionChanged = false;
            }
        }

        protected bool ReceiveCollectionReset()
        {
            return ReceiveCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected bool ReceiveCollectionAdded(TFormula item, int index)
        {
            return ReceiveCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected bool ReceiveCollectionRemoved(TFormula item)
        {
            return ReceiveCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        protected bool ReceiveCollectionRemoved(TFormula item, int index)
        {
            return ReceiveCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        protected bool ReceiveCollectionReplaced(TFormula newItem, TFormula oldItem, int index)
        {
            return ReceiveCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        protected bool ReceiveCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            return ReceiveCollectionChangedCore(this, e);
        }

        protected virtual bool ReceiveCollectionChangedCore(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged == null) return true;
            CollectionChanged(sender, e);
            return true;
        }

        protected override void PinCore()
        {
            Initialize(new ReadOnlyCollection<TFormula>(list));
            base.PinCore();
        }

        public override void AppendWithBracketTo(StringBuilder sb)
        {
            sb.Append("[");
            var oneOrMore = false;
            foreach (var formula in list)
            {
                if (!oneOrMore)
                {
                    oneOrMore = true;
                    formula.AppendWithBracketTo(sb);
                }
                else
                {
                    sb.Append(", ");
                    formula.AppendWithBracketTo(sb);
                }
            }
            sb.Append("]");
        }

        public override Formula Accept(IFormulaVisitor visitor)
        {
            return list.Select(_ => _.Accept(visitor)).LastOrDefault();
        }
    }
}

