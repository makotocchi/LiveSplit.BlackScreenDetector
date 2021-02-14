using LiveSplit.Mgs3LoadRemover;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;

[assembly: ComponentFactory(typeof(LoadRemoverFactory))]

namespace LiveSplit.Mgs3LoadRemover
{
    public class LoadRemoverFactory : IComponentFactory
    {
        public string ComponentName => "MGS3 HD Load Remover";
        public string Description => "Automatically detects MGS3 HD load screens and removes them from GameTime.";
        public ComponentCategory Category => ComponentCategory.Control;
        public IComponent Create(LiveSplitState state) => new LoadRemoverComponent(state);
        public string UpdateName => ComponentName;
        public string UpdateURL => "";
        public string XMLURL => "";
        public Version Version => Version.Parse("1.0");
    }
}