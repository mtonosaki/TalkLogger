// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

namespace TalkLoggerWinform
{
    /// <summary>
    /// Setting POCO
    /// </summary>
    public class SettingModel
    {
        public string SubscriptionKey { get; set; }
        public string ServiceRegion { get; set; }
        public string Device1ID { get; set; }
        public string Device1LanguageCode { get; set; }
        public string Device2ID { get; set; }
        public string Device2LanguageCode { get; set; }
        public string RecordingFilesPath { get; set; }
    }
}
