

namespace Assets.Scripts.CloudContent
{
	public class AppContent
	{
		public Session Session { get; private set; }
		public GameSettings GameSettings { get; private set; }
		public SkillList SkillList { get; private set; }
		public ConstellationList ConstellationList { get; private set; }
        public Account Account { get; private set; }

        public AppContent()
		{
			Session = new Session();
			GameSettings = new GameSettings(Session);
            SkillList = new SkillList(GameSettings);
            ConstellationList = new ConstellationList(SkillList);
            Account = new Account(ConstellationList);
		}
	}
}
