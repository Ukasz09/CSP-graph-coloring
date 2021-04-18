using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace csp_problem.csp
{
    public class Assignment<V, D> : IAssignment<V, D>
    {
        private readonly IDictionary<V, D> _variableValues;
        private readonly Csp<V, D> _csp;

        public Assignment(Csp<V, D> csp)
        {
            _csp = csp;
            _variableValues = new Dictionary<V, D>();
        }

        public bool AllVariablesAssigned()
        {
            return _csp.Variables.All(IsAssigned);
        }

        public bool IsAssigned(V variable)
        {
            return _variableValues.ContainsKey(variable);
        }

        public void UnassignVariable(V variable)
        {
            _variableValues.Remove(variable);
        }

        public void AssignVariable(V variable, D value)
        {
            _variableValues[variable] = value;
        }

        public D GetAssignedValue(V variable)
        {
            return _variableValues[variable];
        }

        public bool IsConsistent(V variable)
        {
            return _csp.VariableConstraints[variable].All(constraint => constraint.IsSatisfied(this));
        }

        public bool WillBeConsistent(V variable, D value)
        {
            AssignVariable(variable, value);
            var isConsistent = IsConsistent(variable);
            UnassignVariable(variable);
            return isConsistent;
        }

        public IDictionary<V, D> GetAssignedValueForAll()
        {
            return _variableValues;
        }

        public List<V> GetConnectedVariables(V variable)
        {
            var constraints = _csp.VariableConstraints[variable];
            var connectedVariables = constraints
                .SelectMany(c => c.Variables).ToList()
                .FindAll(v => !IsAssigned(v));
            return connectedVariables;
        }
    }
}