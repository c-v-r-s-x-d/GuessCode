namespace GuessCode.Domain.Utils;

public static class ArrayUtils
{
    public static T[] AddToArray<T>(IEnumerable<T> array, T newElement)
    {
        var list = new List<T>(array) { newElement };
        return list.ToArray();
    }
}