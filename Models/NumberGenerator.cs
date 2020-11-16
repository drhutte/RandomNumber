using System.ComponentModel.DataAnnotations;

namespace RandomNumber.Models
{
    //  Base class abstract - all but one property to be overridden by derived classes 
    //  Properties required either to meet project requirements or to allow autogeneration of html elements via html helpers
    public abstract class NumberGenerator
    {
        public int Random { get; set; }
        public abstract int Guess { get; set; }
        public abstract int Level { get; set; }
        public abstract string LevelDescription { get; set; }
        public abstract int Range { get; set; }
        public abstract int Guesses { get; set; }
    }
    //  Derived classes with preset properties and varying data annotation for validation
    //  This design allows us to extend/amend the levels of difficulty in the game without making 'any other changes to the solution'
    //  Fulfills Liskovs' substitution principle - allowing substitution of base class with derived classes
    public class NumberGeneratorEasy : NumberGenerator
    {
        [Required]
        [Range(1, 10, ErrorMessage = "Your guess must be between 1 to 10")]
        public override int Guess { get; set; }
        public override int Level { get { return 1; } set { } }
        public override string LevelDescription { get { return "Easy"; } set { } }
        public override int Range { get { return 10; } set { } }
        public override int Guesses { get; set; } = 6;
    }
    public class NumberGeneratorMedium : NumberGenerator
    {
        [Required]
        [Range(1, 100, ErrorMessage = "Your guess must be between 1 to 100")]
        public override int Guess { get; set; }
        public override int Level { get { return 2; } set { } }
        public override string LevelDescription { get { return "Medium"; } set { } }
        public override int Range { get { return 100; } set { } }
        public override int Guesses { get; set; } = 5;
    }
    public class NumberGeneratorHard : NumberGenerator
    {
        [Required]
        [Range(1, 1000, ErrorMessage = "Your guess must be between 1 to 1000")]
        public override int Guess { get; set; }
        public override int Level { get { return 3; } set { } }
        public override string LevelDescription { get { return "Hard"; } set { } }
        public override int Range { get { return 1000; } set { } }
        public override int Guesses { get; set; } = 4;
    }
}
