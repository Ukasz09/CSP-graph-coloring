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
            return !(from variable in Variables
                let isAssigned = inAssignment.IsAssigned(variable)
                where isAssigned
                let assignedValue = inAssignment.GetAssignedValue(variable)
                let neighbours = VariablesNeighbours[variable]
                from neighbour in neighbours
                let neighbourIsAssigned = inAssignment.IsAssigned(neighbour)
                where neighbourIsAssigned
                let neighbourValue = inAssignment.GetAssignedValue(neighbour)
                where neighbourValue.Equals(assignedValue)
                select assignedValue).Any();
        }
    }
}