using System;
using System.Collections.Generic;
using Tono.Gui;
using Tono.Gui.Uwp;
using Windows.System;
using Windows.UI.Xaml;

namespace TalkLoggerUwp
{
    public class FeatureAutoScroll : FeatureCommonBase, IKeyListener
    {
        public double FPS { get; set; } = 0.5;
        public IEnumerable<KeyListenSetting> KeyListenSettings => _keys;
        private static readonly KeyListenSetting[] _keys = new KeyListenSetting[]
        {
            new KeyListenSetting // [0] You can Start/Stop with [F] key
			{
                Name = "Clock Start/Stop",
                KeyStates = new[]
                {
                    (VirtualKey.F, KeyListenSetting.States.Down),
                },
            },
        };
        private FrameworkElement buttonStop;
        private FrameworkElement buttonStart;
        private DispatcherTimer _timer = null;

        public override void OnInitialInstance()
        {
            buttonStart = ControlUtil.FindControl(View, "ClockStartCaption");
            buttonStop = ControlUtil.FindControl(View, "ClockStopCaption");
            Pane.Target = Pane.Main;

            Token.AddNew(new EventTokenButton   // Auto Start
            {
                Name = "ClockStart",
            });
        }

        [EventCatch(Name = "ClockStart")]
        public void ClockStart(EventTokenButton token)
        {
            buttonStop.Opacity = 0.2;
            buttonStart.Opacity = 0.8;
            _timer?.Stop();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000 / FPS),  // 4[fps] is enough for Speech update
            };
            _timer.Tick += OnTick;
            _timer.Start();
        }

        private void OnTick(object sender, object args)
        {
            if (_timer != null)
            {
                _timer.Stop();
                UpdateSimulationTime(EventTokenDispatchTimerWrapper.From(sender as DispatcherTimer, this, "onTick"), this, DateTime.Now);
                _timer.Start();
            }
        }

        public static void UpdateSimulationTime(EventToken token, FeatureCommonBase from, DateTime newtime, string remarks = null)
        {
            var pre = from.Hot.Now;
            from.Hot.Now = newtime;
            from.Token.Link(token, new EventClockUpdatedTokenTrigger
            {
                TokenID = TOKENS.ClockUpdated,
                Pre = pre,
                Now = newtime,
                Sender = from,
                Remarks = remarks ?? "ClockUpdated",
            });
            from.Redraw();
        }


        [EventCatch(Name = "ClockStop")]
        public void ClockStop(EventTokenButton token)
        {
            buttonStop.Opacity = 0.8;
            buttonStart.Opacity = 0.2;
            _timer?.Stop();
            _timer = null;
        }

        public void OnKey(KeyEventToken kt)
        {
            if (_timer != null)
            {
                ClockStop(null);
            }
            else
            {
                ClockStart(null);
            }
        }
    }
}
