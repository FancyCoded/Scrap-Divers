public static class MathComparer
{
    public static bool Compare(float first, float second, MathCompareType mathCompareType)
    {
        if(mathCompareType == MathCompareType.Equal)
            if(first == second) return true;
        if(mathCompareType == MathCompareType.More)
            if(first > second) return true;
        if (mathCompareType == MathCompareType.Less)
            if (first < second) return true;
        if(mathCompareType == MathCompareType.EqualLess)
            if(first <= second) return true;
        if(mathCompareType == MathCompareType.EqualMore) 
            if(first >= second) return true;

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