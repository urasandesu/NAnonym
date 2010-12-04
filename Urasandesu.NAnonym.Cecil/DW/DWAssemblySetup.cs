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
using System.Runtime.Serialization;

namespace Urasandesu.NAnonym.Cecil.DW
{
    [Serializable]
    public class DWAssemblySetup : ManuallyDeserializable
    {
        public const string DebugSymbolExtension = ".pdb";

        string codeBase;
        string location;

        [NonSerialized]
        string symbolCodeBase;

        [NonSerialized]
        string symbolLocation;

        [NonSerialized]
        string codeBaseLocalPath;

        [NonSerialized]
        string symbolCodeBaseLocalPath;


        public string CodeBase
        {
            get { return codeBase; }
            set
            {
                codeBase = value;
                codeBaseLocalPath = codeBase.ToLocalPath();
                symbolCodeBase = codeBase.WithoutExtension() + DebugSymbolExtension;
                symbolCodeBaseLocalPath = SymbolCodeBase.ToLocalPath();
            }
        }

        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                symbolLocation = location.WithoutExtension() + DebugSymbolExtension;
            }
        }

        [XmlIgnore]
        public string SymbolCodeBase { get { return symbolCodeBase; } }

        [XmlIgnore]
        public string SymbolLocation { get { return symbolLocation; } }

        [XmlIgnore]
        public string CodeBaseLocalPath { get { return codeBaseLocalPath; } }

        [XmlIgnore]
        public string SymbolCodeBaseLocalPath { get { return symbolCodeBaseLocalPath; } }

        public DWAssemblySetup()
            : base(true)
        {
        }

        public DWAssemblySetup(string codeBase, string location)
            : this()
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
            return CodeBase.NullableGetHashCode() ^ Location.NullableGetHashCode();
        }

        protected override void OnDeserializedManually(StreamingContext context)
        {
            CodeBase = codeBase;
            Location = location;
        }
    }
}

