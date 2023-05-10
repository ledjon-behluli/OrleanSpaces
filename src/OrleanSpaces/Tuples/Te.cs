using System.Globalization;

public sealed class SpaceTupleFormatProvider : IFormatProvider, ICustomFormatter
{
    public static readonly UriEncodeFormatter Default = new UriEncodeFormatter();
    public object GetFormat(Type type)
    {
        // boilerplate
        if (typeof(ICustomFormatter) == type)
            return this;
        else
            return null;
    }

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        // if "ue" isn't specified, then try other formatters
        if ("ue" != format)
            return _FormatOther(format, arg);

        // if "ue" is specified and arg is a non-empty string then url encode
        // the value
        var s = Convert.ChangeType(arg, typeof(string)) as string;
        if (null == s) // sanity
            s = "";
        return Uri.EscapeDataString(s);
    }

    string _FormatOther(string format, object arg)
    {
        // try to format using a default formatter
        var fmt = arg as IFormattable;
        if (null != fmt)
            return fmt.ToString(format, CultureInfo.CurrentCulture);
        else if (null != arg)
            return arg.ToString();
        else
            return string.Empty;
    }
}