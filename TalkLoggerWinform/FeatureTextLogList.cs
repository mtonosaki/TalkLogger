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
            List<FormTextLogList.Talk> talks = null;
            for (var i = 5; i > 0; i--)
            {
                try
                {
                    talks = GetUnits().ToList();
                    break;
                }
                catch
                {
                    Application.DoEvents(); // When pars collection thread confrict
                }
            }
            if (talks != null)
            {
                _form.SetTextData(talks);
            }
            else
            {
                LOG.WriteMesLine("FeatureTextLogList", "ConfrictError");
            } 
        }

        public IEnumerable<FormTextLogList.Talk> GetUnits()
        {
            foreach (PartsEntry pe in Parts)
            {
                if (pe.Parts is PartsTalkBar talk)
                {
                    yield return new FormTextLogList.Talk
                    {
                        ID = Id.From(talk.Rect.LT.Y),
                        TimeGenerated = Hot.FirstSpeech + TimeSpan.FromSeconds(talk.Rect.LT.X),
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
