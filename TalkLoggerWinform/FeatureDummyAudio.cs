using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureDummyAudio : CoreFeatureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();

            var fid = GetRoot().FindChildFeatures(typeof(FeatureAudioLoopback)).FirstOrDefault().ID;

            Timer.AddTrigger(1200, () =>
            {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Start,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(3.5),
                    SessionID = "DUMMY-001",
                });
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognizing,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(3.5),
                    SessionID = "DUMMY-001",
                    Text = "こんに",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
            Timer.AddTrigger(2500, () =>
            {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognizing,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(1.0),
                    SessionID = "DUMMY-001",
                    Text = "こんには。ただいま",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
            Timer.AddTrigger(5000, () =>
            {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognizing,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(0.3),
                    SessionID = "DUMMY-001",
                    Text = "こんには。只今マイクのテストをしております",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
            Timer.AddTrigger(5300, () =>
            {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognized,
                    TimeGenerated = DateTime.Now,
                    SessionID = "DUMMY-001",
                    Text = @"こんには。ただいまマイクのテストをしております。ウィキ（Wiki）とは、不特定多数のユーザーが共同してウェブブラウザから直接コンテンツを編集するウェブサイトである。一般的なウィキにおいては、コンテンツはマークアップ言語によって記述されるか、リッチテキストエディタによって編集される。ウィキはウィキソフトウェア（ウィキエンジンとも呼ばれる）上で動作する。ウィキソフトウェアはコンテンツ管理システムの一種であるが、サイト所有者や特定のユーザーによってコンテンツが作られるわけではないという点において、ブログなど他のコンテンツ管理システムとは異なる。またウィキには固まったサイト構造というものはなく、サイトユーザーのニーズに沿って様々にサイト構造を作り上げることが可能であり、そうした点でも他のシステムとは異なっている。ウィキウィキはハワイ語で「速い」を意味する形容詞の wikiwiki から来ており、ウィキのページの作成更新の迅速なことを表し、ウォード・カニンガムがホノルル国際空港内を走る 'Wiki Wiki Shuttle' からとって 'WikiWikiWeb' と命名したことに始まる。",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
        }
    }
}
