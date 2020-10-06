// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class DataHot : DataHotBase
    {
        public bool IsPlaying = false;
        public const int LayoutPixelPerSecond = 20;
        public DateTime FirstSpeech { get; set; }
        public PartsTimeline TimelineParts { get; set; }
        public SettingModel Setting { get; set; } = new SettingModel();
        public Queue<SpeechEvent> SpeechEventQueue { get; } = new Queue<SpeechEvent>();
        public string SelectedText { get; set; }
        private readonly Dictionary<string/*audioName*/, WaveFormat> WaveFormats = new Dictionary<string, WaveFormat>();
        private readonly Dictionary<string/*dicName*/, Dictionary<string/*audioName*/, Queue<(byte[] Buffer, int Length)>>> WavQueue = new Dictionary<string, Dictionary<string, Queue<(byte[] Buffer, int Length)>>>();
        public void AddWaveQueue(string dicName)
        {
            if (WavQueue.ContainsKey(dicName) == false)
            {
                WavQueue[dicName] = new Dictionary<string, Queue<(byte[] Buffer, int Length)>>();
            }
        }
        public void AddWavToAllQueue(string audioName, byte[] buf0, int len)
        {

            var buf = new byte[len];
            Array.Copy(buf0 ?? new byte[] { }, buf, len);

            foreach (var dic in WavQueue.Values)
            {
                var queue = dic.GetValueOrDefault(audioName, true, a => new Queue<(byte[] Buffer, int Length)>());
                if( buf?.Length > 0)
                {
                    lock (queue)
                    {
                        queue.Enqueue((buf, len));
                    }
                }
            }
        }

        public void SetWavFormat(string audioName, WaveFormat fmt)
        {
            if(WaveFormats.ContainsKey(audioName) == false)
            {
                WaveFormats[audioName] = fmt;
            }
        }

        public WaveFormat GetWavFormat(string audioName)
        {
            return WaveFormats[audioName];
        }

        public Dictionary<string/*audioName*/, Queue<(byte[] Buffer, int Length)>> GetWavDictionary(string dicName)
        {
            return WavQueue[dicName];
        }

        /// <summary>
        /// RowID (You always have to manually sort this list by OrderNo)
        /// </summary>
        public List<(int RowID, int OrderNo, int LayoutHeight, bool IsVisible)> RowIDs = new List<(int RowID, int OrderNo, int LayoutHeight, bool IsVisible)>();
        public void AddRowID(int rowID, int orderNo, int layoutHeight, bool isVisible = true)
        {
            int i;
            for (i = RowIDs.Count - 1; i >= 0; i--)
            {
                if (RowIDs[i].RowID == rowID)
                {
                    RowIDs[i] = (rowID, orderNo, layoutHeight, isVisible);
                    break;
                }
            }
            if (i < 0)
            {
                RowIDs.Add((rowID, orderNo, layoutHeight, isVisible));
            }
            RowIDs.Sort((a, b) => a.OrderNo - b.OrderNo);
        }
    }
}
