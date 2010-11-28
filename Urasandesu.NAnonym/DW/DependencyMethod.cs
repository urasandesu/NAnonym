using System;
using System.Reflection;


namespace Urasandesu.NAnonym.DW
{
    public abstract class DependencyMethod
    {
        readonly DependencyClass @class;
        readonly MethodInfo source;

        public DependencyMethod(DependencyClass @class, MethodInfo source)
        {
            this.@class = @class;
            this.source = source;
        }

        public DependencyClass IsImplementedBy(Delegate @delegate)
        {
            @class.MethodSet.Add(new WeaveMethodInfo(SetupModes.Implement, source, @delegate.Method, @delegate.GetType()));
            return @class;
        }

        public DependencyClass IsOverridedBy(Delegate @delegate)
        {
            @class.MethodSet.Add(new WeaveMethodInfo(SetupModes.Override, source, @delegate.Method, @delegate.GetType()));
            return @class;
        }
    }
}
