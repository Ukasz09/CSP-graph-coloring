using System.Collections.Generic;
using System.Linq;
using csp_problem.csp;

namespace csp_problem
{
    public class NoNeighboursWithTheSameColor : IConstraint<string, string>
    {
        public readonly IDictionary<string, ICollection<string>> VariablesNeighbours;

        public ICollection<string> Variables => VariablesNeighbours.Keys;

        public NoNeighboursWithTheSameColor(IDictionary<string, ICollection<string>> variablesNeighbours)
        {
            VariablesNeighbours = variablesNeighbours;
        }

        public bool Affects(string variable)
        {
            return Variables.Contains(variable);
        }

        public bool IsSatisfied(string value, IAssignment<string, string> inAssignment)
        {
            foreach (var variable in Variables)
            {
                var isAssigned = inAssignment.IsAssigned(variable);
                if (isAssigned)
                {
                    // var assignedValue = inAssignment.GetAssignedValue(variable);
                    var neighbours = VariablesNeighbours[variable];
                    foreach (var neighbour in neighbours)
                    {
                        var neighbourIsAssigned = inAssignment.IsAssigned(neighbour);
                        if (neighbourIsAssigned)
                        {
                            var neighbourValue = inAssignment.GetAssignedValue(neighbour);
                            if (neighbourValue.Equals(value))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}