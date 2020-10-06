// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;
using static Tono.GuiWinForm.PartsCollectionBase;

namespace TalkLoggerWinform
{
    public class FeatureTextLogList : CoreFeatureBase
    {
        private FormTextLogList _form = null;

        public override void OnInitInstance()
        {
            base.OnInitInstance();
        }

        public override bool Enabled
        {
            get
            {
                return _form == null;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public override void Start(NamedId who)
        {
            base.Start(who);

            if (_form == null)
            {
                _form = new FormTextLogList();
                _form.FormClosed += (s, e) =>
                {
                    _form.Dispose();
                    _form = null;
                };
                foreach (var fc in AudioFeatures())
                {
                    _form.DisplayNames[fc.ID] = fc.DisplayName;
                }
                _form.Show();
            }
            else
            {
                _form.Visible = true;
                _form.WindowState = FormWindowState.Normal;
            }
            var talks = GetUnits();
            if (talks != null)
            {
                _form.SetTextData(talks.OrderBy(a => a.TimeGenerated));
            }
            else
            {
                LOG.WriteMesLine("FeatureTextLogList", "ConfrictError");
            }
        }

        /// <summary>
        /// Collect parts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FormTextLogList.Talk> GetUnits()
        {
            foreach (var p in Parts.GetLayerParts(LayerTalkBar))
            {
                if (p is PartsTalkBar talk)
                {
                    yield return new FormTextLogList.Talk
                    {
                        ID = Id.From(talk.Rect.LT.Y),
                        TimeGenerated = talk.TimeTalkStarted,
                        Color = talk.BarColor,
                        Text = talk.Text,
                    };
                }
            }
        }

        public IEnumerable<FeatureAudioCaptureBase> AudioFeatures()
        {
            foreach (var obj in GetRoot().GetChildFeatureInstance())
            {
                if (obj is FeatureAudioCaptureBase fc)
                {
                    yield return fc;
                }
            }
        }
    }
}
