using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Urasandesu.NAnonym.Test
{
    public class NewDomainTestTarget
    {
        readonly NewDomainTestInfo testInfo;
        public NewDomainTestTarget(NewDomainTestInfo testInfo)
        {
            this.testInfo = testInfo;
        }

        public NewDomainTestInfo TestInfo
        {
            get { return testInfo; }
        }

        Assembly assembly;
        public virtual Assembly Assembly
        {
            get
            {
                if (assembly == null)
                {
                    assembly = Assembly.LoadFile(testInfo.AssemblyFileName);
                }
                return assembly;
            }
        }

        object instance;
        public virtual object Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Assembly.CreateInstance(testInfo.TypeFullName);
                }
                return instance;
            }
        }


        MethodInfo method;
        public virtual MethodInfo Method
        {
            get
            {
                if (method == null)
                {
                    var type = Instance.GetType();
                    method = type.GetMethod(testInfo.MethodName);
                }
                return method;
            }
        }
    }
}
