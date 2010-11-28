/* 
 * File: ConstructorWeaver.cs
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
using Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.DW
{
    abstract class ConstructorWeaver : Weaver
    {
        public ITypeGenerator DeclaringTypeGenerator { get; private set; }
        public Dictionary<Type, IFieldGenerator> FieldsForDeclaringType { get; private set; }
        public HashSet<WeaveFieldInfo> FieldSet { get; private set; }

        public ConstructorWeaver(
            ITypeGenerator declaringTypeGenerator,
            HashSet<WeaveFieldInfo> fieldSet)
        {
            DeclaringTypeGenerator = declaringTypeGenerator;
            FieldsForDeclaringType = new Dictionary<Type, IFieldGenerator>();
            FieldSet = fieldSet;
        }

        public Type DeclaringType { get { return DeclaringTypeGenerator.Source; } }

        public string GetFieldNameForDeclaringType(Type declaringType)
        {
            return FieldsForDeclaringType[declaringType].Name;
        }

        public override void Apply()
        {
            if (0 < FieldSet.Count)
            {
                var definer = GetConstructorDefiner(this);
                definer.Create();

                var builder = GetConstructorBuilder(definer);
                builder.Construct();
            }
        }

        protected abstract ConstructorWeaveDefiner GetConstructorDefiner(ConstructorWeaver parent);
        protected abstract ConstructorWeaveBuilder GetConstructorBuilder(ConstructorWeaveDefiner parentDefiner);
    }
}

