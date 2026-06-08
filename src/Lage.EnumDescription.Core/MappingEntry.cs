using System.Collections.Generic;

namespace Lage.EnumDescription.Core
{
    public class MappingEntry<T>
    {

        public T Value { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        //private MappingEntry<T>{}

        public static MappingEntry<T> NewStruct(T value, string name, string description)
        {
            return new MappingEntry<T>()
            {
                Value = value,
                Name = name,
                Description = description,
            };
        }
    }

    internal struct NewStruct<T>
    {
        public T value;
        public string name;
        public string description;

        public NewStruct(T value, string name, string description)
        {
            this.value = value;
            this.name = name;
            this.description = description;
        }

        public override bool Equals(object obj)
        {
            return obj is NewStruct<T> other &&
                   EqualityComparer<T>.Default.Equals(value, other.value) &&
                   name == other.name &&
                   description == other.description;
        }

        public override int GetHashCode()
        {
            int hashCode = 1859025065;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(value);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
            return hashCode;
        }

        public void Deconstruct(out T value, out string name, out string description)
        {
            value = this.value;
            name = this.name;
            description = this.description;
        }

        public static implicit operator (T value, string name, string description)(NewStruct<T> value)
        {
            return (value.value, value.name, value.description);
        }

        public static implicit operator NewStruct<T>((T value, string name, string description) value)
        {
            return new NewStruct<T>(value.value, value.name, value.description);
        }
    }
}
