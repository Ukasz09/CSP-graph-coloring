using System.Collections.Generic;
using csp_problem.csp;

namespace csp_problem
{
    public class NoNeighboursWithTheSameColor : IConstraint<string, string>
    {
        public ICollection<string> Variables { get; }
        private readonly string _variable;

        public NoNeighboursWithTheSameColor(string variable, ICollection<string> neighbours)
        {
            _variable = variable;
            Variables = neighbours;
        }

        public bool Affects(string variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(IAssignment<string, string> inAssignment)
        {
            // foreach (var variable in Variables)
            // {
            var isAssigned = inAssignment.IsAssigned(_variable);
            if (isAssigned)
            {
                var assignedValue = inAssignment.GetAssignedValue(_variable);
                foreach (var neighbour in Variables)
                {
                    var neighbourIsAssigned = inAssignment.IsAssigned(neighbour);
                    if (neighbourIsAssigned)
                    {
                        var neighbourValue = inAssignment.GetAssignedValue(neighbour);
                        if (neighbourValue.Equals(assignedValue))
                        {
                            return false;
                        }
                    }
                }
            }
            // }

            return true;
        }
    }
}