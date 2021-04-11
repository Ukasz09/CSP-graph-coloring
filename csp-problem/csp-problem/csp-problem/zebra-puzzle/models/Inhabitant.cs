namespace csp_problem.models
{
    public class Inhabitant
    {
        public Cigarette Cigarette { get; }
        public Drink Drink { get; }
        public Nationality Nationality { get; }
        public Pet Pet { get; }

        public Inhabitant(Cigarette cigarette, Drink drink, Nationality nationality, Pet pet)
        {
            Cigarette = cigarette;
            Drink = drink;
            Nationality = nationality;
            Pet = pet;
        }

        public Inhabitant()
        {
        }
    }
}