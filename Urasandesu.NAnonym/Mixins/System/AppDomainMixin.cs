using System;
using System.Runtime.Serialization;
using System.Security.Policy;

namespace Urasandesu.NAnonym.Mixins.System
{
    public static class AppDomainMixin
    {
        public static void RunAtIsolatedDomain(this AppDomain source, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain(source.Evidence, source.SetupInformation, action);
        }

        public static void RunAtIsolatedDomain(this AppDomain source,
                                               Evidence securityInfo, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain(securityInfo, source.SetupInformation, action);
        }

        public static void RunAtIsolatedDomain(this AppDomain source,
                                               AppDomainSetup info, Action action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain(source.Evidence, info, action);
        }

        public static void RunAtIsolatedDomain(Evidence securityInfo,
                                               AppDomainSetup info, Action action)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action);
        }

        public static void RunAtIsolatedDomain<T>(this AppDomain source,
                                                  Action<T> action, T arg)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T>(source.Evidence, source.SetupInformation, action, arg);
        }

        public static void RunAtIsolatedDomain<T>(this AppDomain source,
                                        Evidence securityInfo, Action<T> action, T arg)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T>(securityInfo, source.SetupInformation, action, arg);
        }

        public static void RunAtIsolatedDomain<T>(this AppDomain source,
                                            AppDomainSetup info, Action<T> action, T arg)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T>(source.Evidence, info, action, arg);
        }

        public static void RunAtIsolatedDomain<T>(Evidence securityInfo,
                                            AppDomainSetup info, Action<T> action, T arg)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action, arg);
        }

        public static void RunAtIsolatedDomain<T1, T2>(this AppDomain source,
                                                  Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2>(source.Evidence, source.SetupInformation, action, arg1, arg2);
        }

        public static void RunAtIsolatedDomain<T1, T2>(this AppDomain source,
                                        Evidence securityInfo, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2>(securityInfo, source.SetupInformation, action, arg1, arg2);
        }

        public static void RunAtIsolatedDomain<T1, T2>(this AppDomain source,
                                            AppDomainSetup info, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            RunAtIsolatedDomain<T1, T2>(source.Evidence, info, action, arg1, arg2);
        }

        public static void RunAtIsolatedDomain<T1, T2>(Evidence securityInfo,
                                            AppDomainSetup info, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            RunAtIsolatedDomain(securityInfo, info, (Delegate)action, arg1, arg2);
        }

        static void RunAtIsolatedDomain(Evidence securityInfo,
                            AppDomainSetup info, Delegate action, params object[] args)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (!action.Method.IsStatic)
                throw new ArgumentException(
                          "The parameter must be designated a static method.", "action");


            var domain = default(AppDomain);
            try
            {
                domain = AppDomain.CreateDomain("Domain " + action.Method.ToString(),
                                               securityInfo, info);
                var type = typeof(MarshalByRefRunner);
                var runner = (MarshalByRefRunner)domain.CreateInstanceAndUnwrap(
                                                  type.Assembly.FullName, type.FullName);
                runner.Action = action;
                runner.Run(args);
            }
            catch (SerializationException e)
            {
                throw new ArgumentException("The parameter must be domain crossable. " +
                          "Please confirm that the type inherits MarshalByRefObject, " +
                          "or it is applied SerializableAttribute.", e);
            }
            finally
            {
                try
                {
                    if (domain != null)
                        AppDomain.Unload(domain);
                }
                catch { }
            }
        }

        class MarshalByRefRunner : MarshalByRefObject
        {
            public Delegate Action { get; set; }
            public void Run(params object[] args)
            {
                if (Action != null)
                    Action.DynamicInvoke(args);
            }
        }
    }
}
