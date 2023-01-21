public static class MathComparer
{
    public static bool Compare(float first, float second, MathCompareType mathCompareType)
    {
        if(mathCompareType == MathCompareType.Equal)
            return first == second;
        if(mathCompareType == MathCompareType.More)
            return first > second;
        if (mathCompareType == MathCompareType.Less)
            return first < second;
        if(mathCompareType == MathCompareType.EqualLess)
            return first <= second;
        if(mathCompareType == MathCompareType.EqualMore) 
            return first >= second;

        return false;
    }
}

public enum MathCompareType
{
    Equal,
    More,
    Less,
    EqualMore,
    EqualLess
}