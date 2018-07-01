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

    }
}
