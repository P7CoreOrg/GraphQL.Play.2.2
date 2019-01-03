using System;

namespace P7Core.Burner
{
    public class Dog
    {
        public Dog()
        {

        }

        public Dog(string name)
        {
            Name = name;
        }

        private string _dog;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_dog))
                {
                    _dog = "Heidi";
                }

                return _dog;
            }
            set => _dog = value;
        }
    }

}
