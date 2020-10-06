using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureSaveAuto : CoreFeatureBase, CoreFeatureBase.ICloseCallback, ITokenListener
    {
        private static string DicName;
        private readonly Dictionary<string/*audioName*/, WaveFileWriter> Writers = new Dictionary<string, WaveFileWriter>();

        public NamedId TokenTriggerID
        {
            get
            {
                return TokenWavDataQueued;
            }
        }

        public override void OnInitInstance()
        {
            base.OnInitInstance();
            DicName = $"{GetType().Name}${ID.Value}";
            Hot.AddWaveQueue(DicName);

            Timer.AddTrigger(1000, OnPorling);
        }
        bool isPrePlaying = false;

        private void OnPorling()
        {
            if(isPrePlaying && Hot.IsPlaying == false)
            {
                isPrePlaying = false;
                RecStop();
            }
            Timer.AddTrigger(1000, OnPorling);
        }

        public override void Start(NamedId who)
        {
            base.Start(who);

            if (Hot.IsPlaying && TokenWavDataQueued.Equals(who))
            {
                SaveWave();
            }
        }

        private void RecStop()
        {
            var files = new List<string>();
            foreach (var nw in Writers)
            {
                var name = nw.Value.Filename;
                files.Add(name);
                nw.Value.Close();
                nw.Value.Dispose();
            }
            Writers.Clear();
            foreach (var path in files)
            {
                CopyWavToMp3(path);
            }
        }

        private void CopyWavToMp3(string wavFileName)
        {
            Task.Run(() =>
            {
                try
                {
                    var reader = new MediaFoundationReader(wavFileName);
                    var mp3 = MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_MP3, reader.WaveFormat, 64000);
                    var mp3FileName = Path.Combine(Path.GetDirectoryName(wavFileName), $"{Path.GetFileNameWithoutExtension(wavFileName)}.mp3");

                    using (var converter = new MediaFoundationEncoder(mp3))
                    {
                        converter.Encode(mp3FileName, reader);
                    }
                    File.Delete(wavFileName);
                    LOG.WriteLine(LLV.INF, $"Saved as {Path.GetFileName(mp3FileName)} at {Path.GetDirectoryName(mp3FileName)}");
                }
                catch (Exception ex)
                {
                    LOG.WriteLine(LLV.WAR, $"Saved as {wavFileName}");
                }
            });
        }

        private void SaveWave()
        {
            var dic = Hot.GetWavDictionary(DicName);

            foreach (var /*<audioName,queue>*/ kv in dic)
            {
                var items = new List<(byte[] Buffer, int Length, DateTime TimeGenerated)>();
                var queue = kv.Value;
                lock (queue)
                {
                    while (queue.Count > 0)
                    {
                        items.Add(queue.Dequeue());
                    }
                }
                if (items.Count > 0)
                {
                    var i0 = items[0];
                    if (Writers.TryGetValue(kv.Key, out var writer) == false)
                    {
                        writer = Writers.GetValueOrDefault(kv.Key, true, audioName => makeWriter(audioName, i0.TimeGenerated));
                        isPrePlaying = true;
                    }
                    foreach (var item in items)
                    {
                        writer.Write(item.Buffer, 0, item.Length);
                    }
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

        private WaveFileWriter makeWriter(string audioName, DateTime timeGenerated)
        {
            var fmt = Hot.GetWavFormat(audioName);
            var writer = new WaveFileWriter(
                Path.Combine(@"C:\Users\ManabuTonosaki\Documents", $"TalkLog.{timeGenerated:yyyyMMddHHmmss}.{audioName}.wav"),
                fmt);
            return writer;
        }
    }
}
