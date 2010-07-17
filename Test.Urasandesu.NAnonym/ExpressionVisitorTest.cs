using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Urasandesu.NAnonym;
using Urasandesu.NAnonym.Linq;
using System.Xml.Linq;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace Test.Urasandesu.NAnonym
{
    [TestFixture]
    public class ExpressionVisitorTest
    {
        [Test]
        public void Hoge2()
        {
            Expression<Func<int>> test = () => 0;
            Console.WriteLine(test.ToString());
            Expression<Func<Item>> lambda = () => new Item() { Key = 1, Value = "ほげ" };
            var expression = ((MemberAssignment)((ReadOnlyCollection<MemberBinding>)
                (((MemberInitExpression)(((LambdaExpression)(lambda)).Body)).Bindings))[0]).Expression;
            var value = Expression.Lambda(expression).Compile().DynamicInvoke();
            var expressionVisitor = new ExpressionVisitor(Expression.Lambda(lambda));
            Console.WriteLine(expressionVisitor.GenerateSource());
        }

        class Item
        {
            public int Key { get; set; }
            public string Value { get; set; }
        }
    }
}
