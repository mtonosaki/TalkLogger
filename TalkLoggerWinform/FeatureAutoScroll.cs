using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureAutoScroll : CoreFeatureBase
    {
        private DataSharingManager.Boolean _isAutoScroll;

        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("Resource");
            _isAutoScroll = (DataSharingManager.Boolean)Share.Get("_isAutoScroll", typeof(DataSharingManager.Boolean));
        }
        public override bool Enabled {
            get => _isAutoScroll.value;
            set => _isAutoScroll.value = value;
        }
    }
}
