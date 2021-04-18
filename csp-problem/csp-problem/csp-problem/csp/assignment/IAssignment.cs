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
        bool IsAssigned(V variable);
        void UnassignVariable(V variable);
        void AssignVariable(V variable, D value);
        D GetAssignedValue(V variable);
        IDictionary<V, D> GetAssignedValueForAll();

        /**
         * Determine if given value satisfy all constraints for given variable
         */
        bool IsConsistent(V variable);

        bool WillBeConsistent(V variable, D value);

        List<V> GetConnectedVariables(V variable);
    }
}