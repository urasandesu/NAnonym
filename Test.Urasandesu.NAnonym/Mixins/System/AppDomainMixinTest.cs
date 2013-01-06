using System;
using System.Collections;
using NUnit.Framework;
using Urasandesu.NAnonym.Mixins.System;

namespace Test.Urasandesu.NAnonym.Mixins.System
{
    [TestFixture]
    public class AppDomainMixinTest
    {
        [Test]
        public void RunAtIsolatedDomainTest_ShouldInitStaticMember()
        {
            // Arrange
            var bag = new CrossDomainBag();
            bag["Actual"] = 0;

            
            // Act
            AppDomain.CurrentDomain.RunAtIsolatedDomain(bag_ => 
            {
                bag_["Actual"] = AppDomain.CurrentDomain.GetHashCode();
            }, bag);

            
            // Assert
            Assert.AreNotEqual(AppDomain.CurrentDomain.GetHashCode(), bag["Actual"]);
        }


        
        class CrossDomainBag : MarshalByRefObject
        {
            Hashtable m_hashtable = new Hashtable();

            public object this[object key]
            {
                get 
                {
                    if (!m_hashtable.ContainsKey(key))
                        m_hashtable.Add(key, null);
                    return m_hashtable[key]; 
                }
                set 
                {
                    if (!m_hashtable.ContainsKey(key))
                        m_hashtable.Add(key, null);
                    m_hashtable[key] = value; 
                }
            }
        }
    }
}
