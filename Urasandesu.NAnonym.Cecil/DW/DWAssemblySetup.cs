/* 
 * File: DWAssemblySetup.cs
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
using System.Xml.Serialization;

namespace Urasandesu.NAnonym.Cecil.DW
{
    public class DWAssemblySetup
    {
        public const string DebugSymbolExtension = ".pdb";

        string codeBase;
        string location;

        public string CodeBase
        {
            get { return codeBase; }
            set
            {
                codeBase = value;
                CodeBaseLocalPath = new Uri(codeBase).LocalPath;
                SymbolCodeBase = codeBase.WithoutExtension() + DebugSymbolExtension;
                SymbolCodeBaseLocalPath = new Uri(SymbolCodeBase).LocalPath;
            }
        }

        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                SymbolLocation = location.WithoutExtension() + DebugSymbolExtension;
            }
        }

        [XmlIgnore]
        public string SymbolCodeBase { get; private set; }

        [XmlIgnore]
        public string SymbolLocation { get; private set; }

        [XmlIgnore]
        public string CodeBaseLocalPath { get; private set; }

        [XmlIgnore]
        public string SymbolCodeBaseLocalPath { get; private set; }

        public DWAssemblySetup()
        {
        }

        public DWAssemblySetup(string codeBase, string location)
        {
            CodeBase = codeBase;
            Location = location;
        }



        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var that = default(DWAssemblySetup);
            if ((that = obj as DWAssemblySetup) == null) return false;

            return this.CodeBase == that.CodeBase && this.Location == that.Location;
        }

        public override int GetHashCode()
        {
            return CodeBase.GetHashCodeOrDefault() ^ Location.GetHashCodeOrDefault();
        }
    }
}

