namespace csp_problem.models
{
    public class House
    {
        public int Number { get; }
        public Color Color { get; }
        public Cigarette Cigarette { get; }
        public Drink Drink { get; }
        public Pet Pet { get; }
        public Nationality Nationality { get; }

        public House(int number, Color color, Cigarette cigarette, Drink drink, Pet pet, Nationality nationality)
        {
            Number = number;
            Color = color;
            Cigarette = cigarette;
            Drink = drink;
            Pet = pet;
            Nationality = nationality;
        }

        public House()
        {
        }
    }
}