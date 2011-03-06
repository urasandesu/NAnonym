/* 
 * File: SRTypeDeclarationImpl.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Urasandesu.NAnonym.Mixins.System.Reflection;

namespace Urasandesu.NAnonym.ILTools.Impl.System.Reflection
{
    class SRTypeDeclarationImpl : SRMemberDeclarationImpl, ITypeDeclaration
    {
        readonly Type type;

        readonly ITypeDeclaration baseTypeDecl;
        IModuleDeclaration moduleDecl;
        ReadOnlyCollection<IFieldDeclaration> fields;
        ReadOnlyCollection<ITypeDeclaration> interfaces;

        public SRTypeDeclarationImpl(Type type)
            : base(type)
        {
            this.type = type;
            baseTypeDecl = type == typeof(object) || type.BaseType == null ? null : new SRTypeDeclarationImpl(type.BaseType);
            var moduleBuilder = ExportModule(type);
            if (moduleBuilder != null)
            {
                moduleDecl = new SRModuleGeneratorImple(moduleBuilder);
            }
        }

        static ModuleBuilder ExportModule(Type type)
        {
            if (!(type is TypeBuilder)) return null;

            var typeBuilderType = typeof(TypeBuilder);
            var m_moduleField = typeBuilderType.GetField("m_module", BindingFlags.NonPublic | BindingFlags.Instance);
            return m_moduleField == null ? default(ModuleBuilder) : (ModuleBuilder)m_moduleField.GetValue(type);
        }
        

        public string FullName
        {
            get { return type.FullName; }
        }

        public string AssemblyQualifiedName { get { return type.AssemblyQualifiedName; } }

        public ITypeDeclaration BaseType
        {
            get { return baseTypeDecl; }
        }

        public IModuleDeclaration Module { get { return moduleDecl; } }

        public IConstructorDeclaration GetConstructor(Type[] types)
        {
            return new SRConstructorDeclarationImpl(type.GetConstructor(types));
        }




        public IFieldDeclaration[] GetFields(global::System.Reflection.BindingFlags attr)
        {
            throw new NotImplementedException();
        }

        public virtual IFieldDeclaration GetField(string name, BindingFlags bindingAttr)
        {
            return new SRFieldDeclarationImpl(type.GetField(name, bindingAttr));
        }





        public ReadOnlyCollection<IFieldDeclaration> Fields
        {
            get 
            {
                if (fields == null)
                {
                    var listFields = new List<IFieldDeclaration>();
                    if (!(type is TypeBuilder))
                    {
                        var bindingAttr = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
                        listFields.AddRange(type.GetFields(bindingAttr).Select(field => field.ToFieldDecl(this)));
                    }
                    fields = new ReadOnlyCollection<IFieldDeclaration>(listFields);
                }

                return fields; 
            }
        }






        public ReadOnlyCollection<IConstructorDeclaration> Constructors
        {
            get { throw new NotImplementedException(); }
        }






        public ReadOnlyCollection<IMethodDeclaration> Methods
        {
            get { throw new NotImplementedException(); }
        }






        public new Type Source
        {
            get { return (Type)base.Source; }
        }




        public bool IsValueType
        {
            get { return Source.IsValueType; }
        }

        public override string ToString()
        {
            return type.ToString();
        }


        public bool IsAssignableFrom(ITypeDeclaration that)
        {
            var srthat = default(SRTypeDeclarationImpl);
            if ((srthat = that as SRTypeDeclarationImpl) != null)
            {
                return Source.IsAssignableFrom(srthat.Source);
            }
            else
            {
                if (FullName == that.FullName)
                {
                    return true;
                }
                else if (IsAssignableFrom(that.BaseType))
                {
                    return true;
                }
                else
                {
                    return that.Interfaces.Any(_ => FullName == _.FullName);
                }
            }
        }

        public ReadOnlyCollection<ITypeDeclaration> Interfaces
        {
            get 
            {
                if (interfaces == null)
                {
                    var _interfaces = Source.GetInterfaces().Select(_ => (ITypeDeclaration)new SRTypeDeclarationImpl(_)).ToList();
                    interfaces = new ReadOnlyCollection<ITypeDeclaration>(_interfaces);
                }
                return interfaces;
            }
        }


        public ITypeDeclaration MakeArrayType()
        {
            return new SRTypeDeclarationImpl(Source.MakeArrayType());
        }


        public ITypeDeclaration GetElementType()
        {
            return new SRTypeDeclarationImpl(Source.GetElementType());
        }
    }
}

