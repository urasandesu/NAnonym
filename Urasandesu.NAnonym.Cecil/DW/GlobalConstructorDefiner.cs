/* 
 * File: GlobalConstructorDefiner.cs
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
using Urasandesu.NAnonym.DW;
using SR = System.Reflection;
using UNI = Urasandesu.NAnonym.ILTools;

namespace Urasandesu.NAnonym.Cecil.DW
{
    class GlobalConstructorDefiner : ConstructorWeaveDefiner
    {
        public GlobalConstructorDefiner(ConstructorWeaver parent)
            : base(parent)
        {
        }

        public override void Create()
        {
            cachedConstructor = Parent.DeclaringTypeGenerator.AddField(
                    GlobalClass.CacheFieldPrefix + "Constructor", typeof(Action), SR::FieldAttributes.Private | SR::FieldAttributes.Static);

            int fieldForDeclaringTypeIndex = 0;
            foreach (var injectionField in Parent.FieldSet)
            {
                var field = TypeSavable.GetFieldInfo(injectionField.FieldReference);
                if (!Parent.FieldsForDeclaringType.ContainsKey(field.DeclaringType))
                {
                    var fieldForDeclaringType = Parent.DeclaringTypeGenerator.AddField(
                            GlobalClass.CacheFieldPrefix + "FieldForDeclaringType" + fieldForDeclaringTypeIndex++,
                            field.DeclaringType, SR::FieldAttributes.Private);

                    Parent.FieldsForDeclaringType.Add(field.DeclaringType, fieldForDeclaringType);
                    InitializedDeclaringTypeConstructor.Add(field.DeclaringType, false);
                }
            }
        }

        UNI::IFieldGenerator cachedConstructor;
        public override UNI::IFieldGenerator CachedConstructor
        {
            get { return cachedConstructor; }
        }

        public override UNI::IConstructorGenerator NewConstructor
        {
            get { throw new NotImplementedException(); }
        }
    }
}

