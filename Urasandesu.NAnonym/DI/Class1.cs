



































//using System;
//using Urasandesu.NAnonym.DI;

//namespace Test.Urasandesu.NAnonym
//{
//    public class Class1
//    {
//        static void Main()
//        {
//            var localClass = new LocalClass<IHoge>();
//            // Setup で中身を編集。
//            localClass.Setup(the =>
//            {
//                // メソッドの設定。中身を定義し、Override で同じ I/F を持つメソッドをオーバーライド。
//                the.Method(() =>
//                {
//                    if (DateTime.Now < new DateTime(2010, 1, 1))
//                    {
//                        Console.WriteLine("こんにちは！世界！");
//                    }
//                    else
//                    {
//                        Console.WriteLine("Hello, World!!");
//                    }
//                })
//                .Override(_ => _.Output);

//                the.Method(() =>
//                {
//                    return "Hello, Local Class !!";
//                })
//                .Override(_ => _.Print);

//                the.Method((string content) =>
//                {
//                    return "Hello, " + content + " World !!";
//                })
//                .Override(_ => _.Print);



//                // プロパティの設定。中身を定義し、Override で同じ I/F を持つプロパティをオーバーライド。
//                int this_value = 0;
//                the.Property(() =>
//                {
//                    return this_value;
//                })
//                .Override(_ => () => _.Value);

//                the.Property((int value) =>
//                {
//                    this_value = value * 2;
//                })
//                .Override(_ => value => _.Value = value);
//            });

//            // Load でアセンブリ生成。キャッシュ。
//            localClass.Load();

//            // New でインスタンス化。
//            var hoge = localClass.New();

//            // 実行
//            hoge.Value = 10;
//            Console.WriteLine(hoge.Value);
//            Console.WriteLine(hoge.Print());
//            Console.WriteLine(hoge.Print("Local Class"));
//            /*
//             * 20
//             * Hello, Local Class !!
//             * Hello, Local Class World !!
//             */
//        }
//    }

//    // 対象のインターフェース
//    interface IHoge
//    {
//        int Value { get; set; }
//        void Output();
//        string Print();
//        string Print(string content);
//    }
//}
















//using System;
//using System.Reflection.Emit;
//using Mono.Cecil;
//using Urasandesu.NAnonym.CREUtilities;
//using MC = Mono.Cecil;
//using SR = System.Reflection;

//namespace Urasandesu.NAnonym.DI
//{
//    class Class1
//    {
//        static void Main()
//        {
//            // ToTypeDef は System.Type から Mono.Cecil.TypeDefinition に変換する拡張メソッド。
//            var mainDef = typeof(Class1).ToTypeDef();

//            var testDef = new MethodDefinition("Test", 
//                MC::MethodAttributes.Private | MC::MethodAttributes.Static, mainDef.Module.Import(typeof(void)));
//            mainDef.Methods.Add(testDef);

//            // 式木で直接 Emit。
//            var egen = new ExpressiveILProcessor(testDef);
//            egen.Emit(_ => Console.WriteLine("aiueo"));

//            // .NET 3.5 までだと変数の宣言や代入式無理。
//            // → ExpressiveILProcessor 自身のメソッドを定義。
//            int a = 0;
//            egen.Emit(_ => _.Addloc(() => a, default(int)));
//            egen.Emit(_ => _.Stloc(() => a, 100));

//            // ローカル変数に入れた上ににアクセスする場合。
//            var cachedAnonymousMethod = default(DynamicMethod);
//            var gen = default(ILGenerator);
//            var label27 = default(Label);
//            egen.Emit(_ => _.Addloc(() => cachedAnonymousMethod, 
//                new DynamicMethod("cachedAnonymousMethod", typeof(string), new Type[] { typeof(string) }, true)));
//            egen.Emit(_ => _.Addloc(() => gen, _.Ldloc(() => cachedAnonymousMethod).GetILGenerator()));
//            egen.Emit(_ => _.Addloc(() => label27, _.Ldloc(() => gen).DefineLabel()));
//            egen.Emit(_ => _.Ldloc(() => gen).Emit(SR::Emit.OpCodes.Brtrue_S, _.Ldloc(() => label27)));

//            // 通常の Emit 処理混合。
//            egen.Emit(_ => _.Direct.Emit(MC::Cil.OpCodes.Ldc_I4_S, (sbyte)100));
//            egen.Emit(_ => _.Direct.Emit(MC::Cil.OpCodes.Stloc, _.Locals(() => a)));
//        }
//    }
//}
