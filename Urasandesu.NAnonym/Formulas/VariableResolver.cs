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

        public override void Visit(VariableFormula formula)
        {
            base.Visit(formula);
            if (formula.Resolved == null)
            {
                if (formula.Referrer.NodeType == NodeType.Assign)
                {
                    var local = GetDefinedLocals(formula.Block).FirstOrDefault(_ => _.LocalName == formula.VariableName);
                    if (local == null)
                    {
                        local = new LocalFormula(formula.VariableName, formula.TypeDeclaration);
                        formula.Block.Locals.Add(local);
                    }
                    formula.Resolved = local;
                }
                else
                {
                    // Fully resolving.
                    var local = GetDefinedLocals(formula.Block).FirstOrDefault(_ => _.LocalName == formula.VariableName);
                    if (local == null)
                    {
                        throw new InvalidOperationException(string.Format("The variable \"{0}\" has not defined.", formula.VariableName));
                    }
                    formula.Resolved = local;
                }
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
