namespace Lage.Generators.Extensions
{
    public static class StringHelper
    {
        public static string Indent(int level) => new(' ', level * 4);
    }
}
