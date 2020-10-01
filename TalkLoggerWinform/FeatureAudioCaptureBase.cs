// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

namespace TalkLoggerWinform
{
    public abstract class FeatureAudioCaptureBase : CoreFeatureBase
    {
        public virtual string DisplayName { get => GetType().Name; }
    }
}
