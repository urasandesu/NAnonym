/* 
 * File: LocalField.cs
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
using System.Linq.Expressions;
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{

    public abstract class LocalField : DependencyField
    {
        public LocalField(DependencyClass @class, LambdaExpression fieldReference, Type fieldType)
            : base(@class, fieldReference, fieldType)
        {
        }
    }

    public class LocalField<T> : LocalField
    {
        public LocalField(LocalClass @class, Expression<Func<T>> fieldReference)
            : base(@class, fieldReference, typeof(T))
        {
        }

        public void As(Expression<Func<Expressible, T>> initializer)
        {
            base.As(initializer);
        }
    }

    public class LocalFieldInt : LocalField
    {
        public LocalFieldInt(LocalClass @class, Expression<Func<int>> fieldReference)
            : base(@class, fieldReference, typeof(int))
        {
        }

        public void As(int arg)
        {
            Expression<Func<Expressible, int>> initializer = _ => _.X(arg);
            base.As(initializer);
        }
    }
}
