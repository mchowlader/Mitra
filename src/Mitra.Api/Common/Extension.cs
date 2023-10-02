namespace Mitra.Api.Common;

public static class Extension
{
    public static bool IsNotNullOrEmpty(this string s)
    {
        if (!String.IsNullOrEmpty(s)) return true;
        else return false;
    }
    public static string ToCommaSeparatedString(this List<string> strList)
    {
        var str = String.Empty;
        if (strList.Count > 0)
        {
            foreach (var _str in strList)
            {
                var index = strList.FindIndex(x => x == _str);
                if (index < (strList.Count - 1))
                {
                    str = str + _str + ",";
                }
                else if (index == (strList.Count - 1))
                {
                    str = str + _str;
                }
            }
        }
        return str;
    }
    public static int ToInt32(this object o)
    {
        try
        {
            return Convert.ToInt32(o);
        }
        catch (Exception ex)
        {
            throw;
        }
            
    }
}