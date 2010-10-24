
namespace Urasandesu.NAnonym.DI
{
    abstract class DependencyMethodInjection
    {
        int targetMethodInfoSetIndex = 0;
        protected int IncreaseSequence()
        {
            return targetMethodInfoSetIndex++;
        }
        protected abstract string UniqueCacheMethodFieldName();
        public abstract void Apply(TargetMethodInfo targetMethodInfo);
    }
}
