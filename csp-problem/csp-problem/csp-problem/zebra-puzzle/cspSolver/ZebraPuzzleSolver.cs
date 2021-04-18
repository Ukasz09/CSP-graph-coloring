using System;
using System.Collections.Generic;
using System.Linq;
using csp_problem.csp;
using csp_problem.csp.constraints;
using csp_problem.csp.cspSolver;
using csp_problem.cspSolver.constraints;
using csp_problem.models;
using NLog;

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
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public long SearchTimeInMs => _solver.ExecutionTimeInMs;
        public int VisitedNodesQty => _solver.VisitedNodesQty;

        public ZebraPuzzleSolver(ISolver<string, int> solver)
        {
            _solver = solver;
        }

        public IDictionary<string, int> Solve()
        {
            var csp = GetCsp();
            Ac3<string, int>.ReduceDomains(csp);
            var assignment = new Assignment<string, int>(csp);
            var resultAssignment = _solver.Solve(csp, assignment);
            if (resultAssignment == null)
            {
                throw new Exception(
                    $"Couldn't find solution, time of executing: {_solver.ExecutionTimeInMs} ms. Visited nodes: {_solver.VisitedNodesQty}");
            }

            return resultAssignment;
        }

        public IEnumerable<IDictionary<string, int>> SolveAllSolutions()
        {
            var csp = GetCsp();
            Ac3<string, int>.ReduceDomains(csp);
            LogAc3Results(csp);

            var assignment = new Assignment<string, int>(csp);
            var listOfVariableValues = _solver.SolveAll(csp, assignment);
            if (listOfVariableValues.Count == 0)
            {
                throw new Exception(
                    $"Couldn't find any solution, time of executing: {_solver.ExecutionTimeInMs} ms, visited nodes: {_solver.VisitedNodesQty}"
                );
            }

            return listOfVariableValues;
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

            const int milkHouse = 3;
            const int norwegianHouse = 1;
            var constraints = new List<IConstraint<string, int>>
            {
                new AllDifferentConstraint<string, int>(_cigaretteVars),
                new AllDifferentConstraint<string, int>(_colorVars),
                new AllDifferentConstraint<string, int>(_drinksVars),
                new AllDifferentConstraint<string, int>(_nationalityVars),
                new AllDifferentConstraint<string, int>(_petVars),
                
                new ValueEqualToGiven(Nationality.Norwegian, norwegianHouse),
                new TheSameValueAssigned(Nationality.English, Color.Red),
                new ToTheLeftOf(Color.Green, Color.White),
                new TheSameValueAssigned(Nationality.Dane, Drink.Tea),
                new NextToThe(Cigarette.Light, Pet.Cat),
                new TheSameValueAssigned(Color.Yellow, Cigarette.Cigar),
                new TheSameValueAssigned(Nationality.German, Cigarette.Bong),
                new ValueEqualToGiven(Drink.Milk, milkHouse),
                new NextToThe(Cigarette.Light, Drink.Water),
                new TheSameValueAssigned(Cigarette.Unfiltered, Pet.Bird),
                new TheSameValueAssigned(Nationality.Sweden, Pet.Dog),
                new NextToThe(Pet.Horse, Color.Yellow),
                new TheSameValueAssigned(Cigarette.Menthol, Drink.Beer),
                new TheSameValueAssigned(Drink.Coffee, Color.Green),

                // Extra constraints for more efficient reducing in AC3 

                // Each nationality other than norwegian cannot be assigned to house {norwegianHouse}
                new OtherThenGivenValue(Nationality.Dane, norwegianHouse),
                new OtherThenGivenValue(Nationality.English, norwegianHouse),
                new OtherThenGivenValue(Nationality.German, norwegianHouse),
                new OtherThenGivenValue(Nationality.Sweden, norwegianHouse),
                
                // Each drink other than milk cannot be assigned to house {milkHouse}
                new OtherThenGivenValue(Drink.Beer, milkHouse),
                new OtherThenGivenValue(Drink.Coffee, milkHouse),
                new OtherThenGivenValue(Drink.Tea, milkHouse),
                new OtherThenGivenValue(Drink.Water, milkHouse),
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

        private static void LogAc3Results(Csp<string, int> csp)
        {
            _logger.Debug("-----------------------------------------");
            _logger.Debug("--------------- After AC3 ---------------");
            _logger.Debug("-----------------------------------------");
            var maxVarTextLength = csp.Variables.Max(v => v.Length);

            foreach (var variable in csp.Domains.Keys)
            {
                _logger.Debug($"{variable.PadRight(maxVarTextLength)} : [{string.Join(",", csp.Domains[variable])}]");
            }
        }
    }
}