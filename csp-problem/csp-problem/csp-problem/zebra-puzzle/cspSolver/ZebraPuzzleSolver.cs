using System;
using System.Collections.Generic;
using System.Linq;
using csp_problem.csp;
using csp_problem.csp.constraints;
using csp_problem.csp.cspSolver;
using csp_problem.cspSolver.constraints;
using csp_problem.models;

namespace csp_problem
{
    public class ZebraPuzzleSolver
    {
        private readonly ISolver<string, int> _solver;
        private readonly List<string> _cigaretteVars = GetStaticPropertyNames(typeof(Cigarette));
        private readonly List<string> _colorVars = GetStaticPropertyNames(typeof(Color));
        private readonly List<string> _drinksVars = GetStaticPropertyNames(typeof(Drink));
        private readonly List<string> _nationalityVars = GetStaticPropertyNames(typeof(Nationality));
        private readonly List<string> _petVars = GetStaticPropertyNames(typeof(Pet));

        public ZebraPuzzleSolver(ISolver<string, int> solver)
        {
            _solver = solver;
        }

        public IDictionary<string, int> Solve()
        {
            var csp = GetCsp();
            var assignment = new Assignment<string, int>(csp);
            var resultAssignment = _solver.Solve(csp, assignment);
            if (resultAssignment == null)
            {
                throw new Exception($"Couldn't find solution, time of executing: {_solver.ExecutionTimeInMs} ms.");
            }

            var variableValues = resultAssignment.GetAssignedValueForAll();
            return variableValues;
        }

        private Csp<string, int> GetCsp()
        {
            var variables = GetVariables();
            var domains = new List<int> {1, 2, 3, 4, 5};
            var variableDomains = new Dictionary<string, ICollection<int>>();
            foreach (var variable in variables)
            {
                variableDomains[variable] = domains;
            }

            var constraints = new List<IConstraint<string, int>>
            {
                new AllDifferentConstraint<string, int>(_cigaretteVars),
                new AllDifferentConstraint<string, int>(_colorVars),
                new AllDifferentConstraint<string, int>(_drinksVars),
                new AllDifferentConstraint<string, int>(_nationalityVars),
                new AllDifferentConstraint<string, int>(_petVars),
                new ValueEqualToGiven(Nationality.Norwegian, 1),
                new TheSameValueAssigned(Nationality.English, Color.Red),
                new ToTheLeftOf(Color.Green, Color.White),
                new TheSameValueAssigned(Nationality.Dane, Drink.Tea),
                new NextToThe(Cigarette.Light, Pet.Cat),
                new TheSameValueAssigned(Color.Yellow, Cigarette.Cigar),
                new TheSameValueAssigned(Nationality.German, Cigarette.Bong),
                new ValueEqualToGiven(Drink.Milk, 3),
                new NextToThe(Cigarette.Light, Drink.Water),
                new TheSameValueAssigned(Cigarette.Unfiltered, Pet.Bird),
                new TheSameValueAssigned(Nationality.Sweden, Pet.Dog),
                new NextToThe(Pet.Horse, Color.Yellow),
                new TheSameValueAssigned(Cigarette.Menthol, Drink.Beer),
                new TheSameValueAssigned(Drink.Coffee, Color.Green),
            };
            var csp = new Csp<string, int>(variableDomains, constraints);
            return csp;
        }

        private IEnumerable<string> GetVariables()
        {
            var variables = _cigaretteVars
                .Concat(_colorVars)
                .Concat(_drinksVars)
                .Concat(_nationalityVars)
                .Concat(_petVars);
            return variables;
        }

        private static List<string> GetStaticPropertyNames(Type type)
        {
            var propertyNames = type
                .GetProperties()
                .Select(p => p.GetValue(null, null)?.ToString())
                .ToList();
            return propertyNames;
        }

        public long SearchTimeInMs()
        {
            return _solver.ExecutionTimeInMs;
        }
    }
}