
public class EnumComparer<TEnum> : System.Collections.Generic.IEqualityComparer<TEnum> where TEnum : struct
{
    int ToInt(TEnum t)
    {
        return System.Convert.ToInt32(t);
    }
    public bool Equals(TEnum x, TEnum y)
    {
        return (ToInt(x) == ToInt(y));
    }

    public int GetHashCode(TEnum obj)
    {
        return ToInt(obj);
    }
}
