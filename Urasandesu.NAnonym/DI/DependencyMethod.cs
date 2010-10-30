using System;
using System.Reflection;


namespace Urasandesu.NAnonym.DI
{
    public abstract class DependencyMethod
    {
        readonly DependencyClass @class;
        readonly MethodInfo oldMethod;

        public DependencyMethod(DependencyClass @class, MethodInfo oldMethod)
        {
            this.@class = @class;
            this.oldMethod = oldMethod;
        }

        public DependencyClass IsImplementedBy(Delegate @delegate)
        {
            @class.TargetMethodInfoSet.Add(new TargetMethodInfo(SetupModes.Implement, oldMethod, @delegate.Method, @delegate.GetType()));
            return @class;
        }
    }
}
