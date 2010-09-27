using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Urasandesu.NAnonym.Cecil.DI
{
    public class DIAssemblySetup
    {
        string codeBase;
        string location;

        public string CodeBase
        {
            get { return codeBase; }
            set
            {
                codeBase = value;
                CodeBaseLocalPath = new Uri(codeBase).LocalPath;
                SymbolCodeBase = codeBase.WithoutExtension() + ".pdb";
                SymbolCodeBaseLocalPath = new Uri(SymbolCodeBase).LocalPath;
            }
        }

        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                SymbolLocation = location.WithoutExtension() + ".pdb";
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

        public DIAssemblySetup()
        {
        }

        public DIAssemblySetup(string codeBase, string location)
        {
            CodeBase = codeBase;
            Location = location;
        }



        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var that = default(DIAssemblySetup);
            if ((that = obj as DIAssemblySetup) == null) return false;

            return this.CodeBase == that.CodeBase && this.Location == that.Location;
        }

        public override int GetHashCode()
        {
            return CodeBase.GetHashCodeOrDefault() ^ Location.GetHashCodeOrDefault();
        }
    }
}
