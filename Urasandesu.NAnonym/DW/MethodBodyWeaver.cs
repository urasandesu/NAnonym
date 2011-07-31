/* 
 * File: MethodBodyWeaver.cs
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
using Urasandesu.NAnonym.ILTools;
using BuilderType = Urasandesu.NAnonym.DW.MethodBodyWeaveBuilderType;

namespace Urasandesu.NAnonym.DW
{
    abstract class MethodBodyWeaver : BodyWeaver
    {
        public new MethodWeaveBuilder ParentBuilder { get { return (MethodWeaveBuilder)base.ParentBuilder; } }
        public MethodBodyWeaver(ReflectiveMethodDesigner2 gen, MethodWeaveBuilder parentBuilder)
            : base(gen, parentBuilder)
        {
        }

        public override void Apply()
        {
            var bodyDefiner = GetMethodBodyDefiner(this);
            bodyDefiner.Create();

            var bodyBuilder = GetMethodBodyBuilder(bodyDefiner);
            bodyBuilder.Construct();
        }

        protected abstract MethodBodyWeaveDefiner GetMethodBodyDefiner(MethodBodyWeaver parentBody);
        protected virtual MethodBodyWeaveBuilder GetMethodBodyBuilder(MethodBodyWeaveDefiner parentBodyDefiner)
        {
            var weaveMethod = parentBodyDefiner.ParentBody.ParentBuilder.ParentDefiner.WeaveMethod;
            var destinationType = weaveMethod.DestinationType;
            switch (destinationType)
            {
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.Before | BuilderType.WithAliasList:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.Before | BuilderType.WithAliasSet:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.Before | BuilderType.WithAlias:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.Before | BuilderType.WithPrev:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.Before:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.After | BuilderType.WithAliasList:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.After | BuilderType.WithAliasSet:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.After | BuilderType.WithAlias:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.After | BuilderType.WithPrev:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.After:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.WithAliasList:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.WithAliasSet:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.WithAlias:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Instance | BuilderType.WithPrev:
                    return new AnonymInstanceBodyBuilderWithPrev(parentBodyDefiner);
                case BuilderType.Anonym | BuilderType.Instance:
                    return new AnonymInstanceBodyBuilder(parentBodyDefiner);
                case BuilderType.Anonym | BuilderType.Static | BuilderType.Before | BuilderType.WithAliasList:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.Before | BuilderType.WithAliasSet:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.Before | BuilderType.WithAlias:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.Before | BuilderType.WithPrev:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.Before:
                    return new AnonymStaticBeforeBodyBuilder(parentBodyDefiner);
                case BuilderType.Anonym | BuilderType.Static | BuilderType.After | BuilderType.WithAliasList:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.After | BuilderType.WithAliasSet:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.After | BuilderType.WithAlias:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.After | BuilderType.WithPrev:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.After:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.WithAliasList:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.WithAliasSet:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.WithAlias:
                    throw new NotImplementedException();
                case BuilderType.Anonym | BuilderType.Static | BuilderType.WithPrev:
                    return new AnonymStaticBodyBuilderWithPrev(parentBodyDefiner);
                case BuilderType.Anonym | BuilderType.Static:
                    return new AnonymStaticBodyBuilder(parentBodyDefiner);
                default:
                    throw new NotSupportedException();
            }
            //if ((destinationType & BuilderType.Anonym) == BuilderType.Anonym && 
            //    (destinationType & BuilderType.Instance) == BuilderType.Instance &&
            //    (destinationType )
            //{
            //    return new AnonymInstanceBodyBuilderWithAliasList(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymInstanceWithAliasSet) == BuilderType.AnonymInstanceWithAliasSet)
            //{
            //    return new AnonymInstanceBodyBuilderWithAliasSet(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymInstanceWithAlias) == BuilderType.AnonymInstanceWithAlias)
            //{
            //    return new AnonymInstanceBodyBuilderWithAlias(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymInstanceWithPrev) == BuilderType.AnonymInstanceWithPrev)
            //{
            //    return new AnonymInstanceBodyBuilderWithPrev(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymInstance) == BuilderType.AnonymInstance)
            //{
            //    return new AnonymInstanceBodyBuilder(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymStaticWithAliasList) == BuilderType.AnonymStaticWithAliasList)
            //{
            //    return new AnonymStaticBodyBuilderWithAliasList(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymStaticWithAliasSet) == BuilderType.AnonymStaticWithAliasSet)
            //{
            //    return new AnonymStaticBodyBuilderWithAliasSet(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymStaticWithAlias) == BuilderType.AnonymStaticWithAlias)
            //{
            //    return new AnonymStaticBodyBuilderWithAlias(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymStaticWithPrev) == BuilderType.AnonymStaticWithPrev)
            //{
            //    return new AnonymStaticBodyBuilderWithPrev(parentBodyDefiner);
            //}
            //else if ((destinationType & BuilderType.AnonymStatic) == BuilderType.AnonymStatic)
            //{
            //    return new AnonymStaticBodyBuilder(parentBodyDefiner);
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}

            // Before/After も、ここに追加されるということは Local 側でも使える場面があるんじゃないの？
            // →あるにはある（Override 時）。単に基本処理を呼んで、パフォーマンス計測したい時とか。
            // →そこまで必須ではない。…が、ここにあってはいけない積極的な理由にはならない。
            //   →それよりクラス名が長すぎる(笑)
            //   →Anonymous はライブラリと同じく、短縮形の Anonym に。
            //   →Weave は特化版部分まで来ると必要なさそう。省略する。
            //   →Constructor もしくは Method が特化版の名前を見てすぐわかるなら省略する。
        }
    }
}

