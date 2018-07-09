using System.Collections.Generic;
using SimpleJSON;

namespace Assets.Scripts.View
{
    public class WestString
    {
        private string reference = null;
        private static string colorPrefix = "<color=#";
        private static string colorSuffix = "</color>";

        public WestString(string reference_)
        {
            reference = reference_;
        }

        public static implicit operator string(WestString westString_)
        {
            return westString_.reference.Clone() as string;
        }

        public static implicit operator WestString(string reference_)
        {
            return new WestString(reference_);
        }

        public static implicit operator WestString(JSONNode d)
        {
            return (d == null) ? null : d.Value;
        }

        public string Color { get; set; } = null;

        public string[] Split(char separator)
        {
            return reference.Split(separator);
        }

        public string Format(params string[] replacements_)
        {
            string text = reference;
            for (int i = 0; i < replacements_.Length; ++i)
                text = Replace(text, i.ToString(), replacements_[i]);
            return text;
        }

        public string FormatPair(string key_, string value_)
        {
            return Replace(reference, key_, value_);
        }

        public string Format(ICollection<KeyValuePair<string, string>> replacements_)
        {
            string text = reference;
            foreach (var replacement in replacements_)
                text = Replace(text, replacement.Key, replacement.Value);
            return text;
        }

        public string Format(params KeyValuePair<string, string>[] replacements_)
        {
            string text = reference;
            foreach (var replacement in replacements_)
                text = Replace(text, replacement.Key, replacement.Value);
            return text;
        }

        private string Replace(string text_, string pattern_, string replacement_)
        {
            if (text_.Contains("#" + pattern_ + "#"))
            {
                if (Color != null)
                    replacement_ = colorPrefix + Color + ">" + replacement_ + colorSuffix;
                return text_.Replace("#" + pattern_ + "#", replacement_);
            }
            else if (text_.Contains("%" + pattern_ + "%"))
            {
                try
                {
                    double number = System.Convert.ToDouble(replacement_);
                    if (Color != null)
                        replacement_ = colorPrefix + Color + ">" + number.ToString("P1", System.Globalization.CultureInfo.InvariantCulture) + colorSuffix + "%";
                    else
                        replacement_ = number.ToString("P1", System.Globalization.CultureInfo.InvariantCulture) + "%";
                    return text_.Replace("%" + pattern_ + "%", replacement_);
                }
                catch (System.FormatException)
                {

                }
            }
            return text_;
        }
    }
}
