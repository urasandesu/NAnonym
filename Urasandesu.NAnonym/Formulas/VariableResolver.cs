using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Urasandesu.NAnonym.Formulas
{
    public class VariableResolver : FormulaAdapter
    {
        public VariableResolver(IFormulaVisitor visitor)
            : base(visitor)
        {
        }

        public override void Visit(AssignFormula formula)
        {
            if (formula.Left != null)
            {
                if (formula.Left.NodeType == NodeType.Variable)
                {
                    var variable = (VariableFormula)formula.Left;
                    if (variable.Resolved == null)
                    {
                        // そもそも VariableFormula に現在の Block 記憶させちゃえば良くね？
                        var currentBlock = GetCurrentBlock(variable);
                        var local = GetDefinedLocals(currentBlock).FirstOrDefault(_ => _.LocalName == variable.VariableName);
                        if (local == null)
                        {
                            local = new LocalFormula(variable.VariableName, variable.TypeDeclaration);
                            currentBlock.Locals.Add(local);
                        }
                        variable.Resolved = local;
                    }
                }
            }
            base.Visit(formula);
        }

        public override void Visit(VariableFormula formula)
        {
            if (formula.Resolved == null)
            {
                var currentBlock = GetCurrentBlock(formula);
                var local = GetDefinedLocals(currentBlock).FirstOrDefault(_ => _.LocalName == formula.VariableName);
                if (local == null)
                {
                    throw new InvalidOperationException(string.Format("The variable \"{0}\" has not defined.", formula.VariableName));
                }
                formula.Resolved = local;
            }
            base.Visit(formula);
        }

        BlockFormula GetCurrentBlock(Formula formula)
        {
            var block = default(BlockFormula);

            if (formula == null || formula.Referrer == null) return block;

            if ((block = formula.Referrer as BlockFormula) != null)
            {
                return block;
            }
            else
            {
                return GetCurrentBlock(formula.Referrer);
            }
        }

        IEnumerable<LocalFormula> GetDefinedLocals(BlockFormula block)
        {
            if (block == null) yield break;

            foreach (var local in block.Locals)
            {
                yield return local;
            }

            foreach (var local in GetDefinedLocals(block.ParentBlock))
            {
                yield return local;
            }
        }
    }
}
