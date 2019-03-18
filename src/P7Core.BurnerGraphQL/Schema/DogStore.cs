using P7Core.Burner;

namespace P7Core.BurnerGraphQL.Schema
{
    public class DogStore : IDogStore
    {
        public DogStore()
        {
            Dog = new Dog();
        }

        public string Name
        {
            get => Dog.Name;
            set { Dog.Name = value; }
        }

        private Dog Dog { get; }
    }
}