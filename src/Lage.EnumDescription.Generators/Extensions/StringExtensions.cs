namespace Lage.EnumDescription.Generators.Extensions;

public static class StringHelper
{
    //public static string Indent(int level) => new(' ', level * 4);

    extension(string)
    {
        public static string Indent(int indent) => new(' ', indent * 4);
    }
}
