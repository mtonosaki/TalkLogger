using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureSaveMp3Auto : CoreFeatureBase, CoreFeatureBase.ICloseCallback, IMultiTokenListener
    {
        private static string DicName;
        private readonly Dictionary<string/*audioName*/, WaveFileWriter> Writers = new Dictionary<string, WaveFileWriter>();
        private static readonly NamedId[] _tokens = new NamedId[] { TokenWavDataQueued, FeaturePlayButton.TokenStart, FeaturePlayButton.TokenStop };
        public NamedId[] MultiTokenTriggerID
        {
            get
            {
                return _tokens;
            }
        }

        public override void OnInitInstance()
        {
            base.OnInitInstance();
            DicName = $"{GetType().Name}${ID.Value}";
            Hot.AddWaveQueue(DicName);
        }

        public override void Start(NamedId who)
        {
            base.Start(who);

            if (TokenWavDataQueued.Equals(who))
            {
                SaveWave();
            }
            if (FeaturePlayButton.TokenStart.Equals(who))
            {
                RecStart();
            }
            if (FeaturePlayButton.TokenStop.Equals(who))
            {
                RecStop();
            }
        }

        private void RecStart()
        {
            Writers.Clear();
        }
        private void RecStop()
        {
            foreach (var nw in Writers)
            {
                var name = nw.Value.Filename;
                nw.Value.Close();
                nw.Value.Dispose();
                LOG.WriteLine(LLV.INF, $"Saved voice '{name}'.");
            }
            Writers.Clear();
        }

        private void SaveWave()
        {
            var dic = Hot.GetWavDictionary(DicName);

            foreach (var /*<audioName,queue>*/ kv in dic)
            {
                var items = new List<(byte[] Buffer, int Length)>();
                var writer = Writers.GetValueOrDefault(kv.Key, true, makeWriter);
                var queue = kv.Value;
                lock (queue)
                {
                    while (queue.Count > 0)
                    {
                        items.Add(queue.Dequeue());
                    }
                }
                foreach (var item in items)
                {
                    writer.Write(item.Buffer, 0, item.Length);
                }
            }
        }

        public void OnClosing()
        {
            foreach (var writer in Writers.Values)
            {
                writer.Dispose();
            }
        }

        private WaveFileWriter makeWriter(string audioName)
        {
            var fmt = Hot.GetWavFormat(audioName);
            var writer = new WaveFileWriter(
                Path.Combine(@"C:\Users\ManabuTonosaki\Documents", $"TalkLog.{audioName}.{DateTime.Now.ToString("yyyyMMddHHmmss")}.wav"),
                fmt);
            return writer;
        }
    }
}
