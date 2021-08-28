using System.Collections.Generic;

namespace Assets.Scripts.View
{
    public class WestText : UnityEngine.UI.Text
    {
        private WestString reference = null;

        protected override void Start()
        {
            base.Start();

            if (reference == null)
                reference = text;
        }

        public WestString Reference
        {
            get
            {
                if (reference == null)
                    reference = text;

                return reference;
            }
        }

        public void Format(params string[] replacements_)
        {
            text = Reference.Format(replacements_);
        }

        public void FormatPair(string key_, string value_)
        {
            text = Reference.FormatPair(key_, value_);
        }

        public void Format(ICollection<KeyValuePair<string, string>> replacements_)
        {
            text = Reference.Format(replacements_);
        }

        public void Format(params KeyValuePair<string, string>[] replacements_)
        {
            text = Reference.Format(replacements_);
        }
    }
}
