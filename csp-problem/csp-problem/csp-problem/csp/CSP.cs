using System.Collections.Generic;

namespace csp_problem.csp
{
    public class Csp<V, D>
    {
        private List<IConstraint<V, D>> _constraints;
        private IDictionary<V, ICollection<D>> _domains;

        public Csp(IDictionary<V, ICollection<D>> domains, List<IConstraint<V, D>> constraints)
        {
            _domains = domains;
            _constraints = constraints;
        }
    }
}