using LiveSplit.Model;
using LiveSplit.PokemonRedBlue;
using LiveSplit.UI.Components;
using System;

[assembly: ComponentFactory(typeof(BlackScreenDetectorFactory))]

namespace LiveSplit.PokemonRedBlue
{
    public class BlackScreenDetectorFactory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Black Screen Detector"; }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Control; }
        }

        public string Description
        {
            get { return "Automatically detects black screens and removes them from GameTime."; }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new BlackScreenDetectorComponent(state);
        }

        public string UpdateName
        {
            get { return ComponentName; }
        }
		public string UpdateURL => "https://raw.githubusercontent.com/thomasneff/LiveSplit.BlackScreenDetector/master/";
		public string XMLURL => UpdateURL + "update.LiveSplit.BlackScreenDetector.xml";
		

        public Version Version
        {
            get { return Version.Parse("1.1"); }
        }
    }
}
