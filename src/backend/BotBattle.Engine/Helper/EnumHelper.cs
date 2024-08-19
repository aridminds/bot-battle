namespace BotBattle.Engine.Helper;

public static class EnumHelper
{
    private static readonly Random _random = new Random();

    public static T GetRandomEnumValue<T>(params T[] exclude) where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        List<T> filteredValues = values.Cast<T>().Except(exclude).ToList();

        if (filteredValues.Count == 0)
            throw new ArgumentException("No valid enum values available after exclusion.");

        int randomIndex = _random.Next(filteredValues.Count);
        return filteredValues[randomIndex];
    }
}