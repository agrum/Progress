namespace Assets.Scripts.View
{
    public class WestText : UnityEngine.UI.Text
    {
        private string reference = null;

        protected override void Start()
        {
            base.Start();

            reference = text;
        }

        public string Reference
        {
            get
            {
                if (reference == null)
                    reference = text;

                return reference;
            }
        }

        public void Format(params string[] replacmeent)
        {
            text = Reference;
            for (int i = 0; i < replacmeent.Length; ++i)
                text = text.Replace("#" + i + "#", replacmeent[i]);
        }
    }
}
