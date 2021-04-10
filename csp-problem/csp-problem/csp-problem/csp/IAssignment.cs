using System.Collections.Generic;

namespace csp_problem.csp
{
    /**
     * Configuration of variables and selected values from domain
     * V - type of Variable
     * D - type of Domain values
     */
    public interface IAssignment<V, D>
    {
        bool AllVariablesAssigned();
        bool VariableIsAssigned(V variable);
        void UnassignVariable(V variable);
        void AssignVariable(V variable, D value);
        D GetAssignedValue(V variable);
        ISet<D> GetDomain(V variable);

        /**
         * Determine if given value satisfy all constraints for given variable
         */
        bool IsConsistent(V variable, D value);
    }
}