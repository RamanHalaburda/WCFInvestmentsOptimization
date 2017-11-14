using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ELW.Library.Math.Tools;
using ELW.Library.Math;

namespace InvestmentsWCF
{
    public class FunctionParser
    {
        public double getValue(double _X1, double _X2, string _function)
        {
            ELW.Library.Math.Expressions.PreparedExpression preparedExpression = ToolsHelper.Parser.Parse(_function);
            ELW.Library.Math.Expressions.CompiledExpression compiledExpression = ToolsHelper.Compiler.Compile(preparedExpression);
            List<VariableValue> variables = new List<VariableValue>();
            variables.Add(new VariableValue(_X1, "X1"));
            variables.Add(new VariableValue(_X2, "X2"));
            return ToolsHelper.Calculator.Calculate(compiledExpression, variables);
        }
    }
}