
using System;
using System.Reflection;
namespace Urasandesu.NAnonym.ILTools
{
    public interface IILOperator
    {
        object Source { get; }
        ILocalGenerator AddLocal(string name, Type localType);
        ILocalGenerator AddLocal(Type localType);
        ILocalGenerator AddLocal(Type localType, bool pinned);
        ILabelGenerator AddLabel();
        void Emit(OpCode opcode);
        void Emit(OpCode opcode, byte arg);
        void Emit(OpCode opcode, ConstructorInfo con);
        void Emit(OpCode opcode, double arg);
        void Emit(OpCode opcode, FieldInfo field);
        void Emit(OpCode opcode, float arg);
        void Emit(OpCode opcode, int arg);
        void Emit(OpCode opcode, ILabelDeclaration label);
        void Emit(OpCode opcode, ILabelDeclaration[] labels);
        void Emit(OpCode opcode, ILocalDeclaration local);
        void Emit(OpCode opcode, long arg);
        void Emit(OpCode opcode, MethodInfo meth);
        void Emit(OpCode opcode, sbyte arg);
        void Emit(OpCode opcode, short arg);
        void Emit(OpCode opcode, string str);
        void Emit(OpCode opcode, Type cls);
        void Emit(OpCode opcode, IConstructorDeclaration constructorDecl);
        void Emit(OpCode opcode, IMethodDeclaration methodDecl);
        void Emit(OpCode opcode, IParameterDeclaration parameterDecl);
        void Emit(OpCode opcode, IFieldDeclaration fieldDecl);
        void Emit(OpCode opcode, IPortableScopeItem scopeItem);
        void SetLabel(ILabelDeclaration loc);
    }

}