// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System.Collections.Generic;

namespace TalkLoggerWinform
{
    public static class AzureSpeechToText
    {
        public class LangCode
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }

            public override bool Equals(object obj)
            {
                if (obj is LangCode tar)
                {
                    return tar.Code.Equals(Code);
                }
                else
                {
                    return false;
                }
            }
            public override int GetHashCode()
            {
                return Code.GetHashCode();
            }
        }

        public static List<LangCode> Languages = new List<LangCode>() {
            new LangCode{ Code = "ar-AE",  Name = "Arabic (United Arab Emirates)", },
            new LangCode{ Code = "ar-BH",  Name = "Arabic (Bahrain), modern standard", },
            new LangCode{ Code = "ar-EG",  Name = "Arabic (Egypt)", },
            new LangCode{ Code = "ar-IQ",  Name = "Arabic (Iraq)", },
            new LangCode{ Code = "ar-JO",  Name = "Arabic (Jordan)", },
            new LangCode{ Code = "ar-KW",  Name = "Arabic (Kuwait)", },
            new LangCode{ Code = "ar-LB",  Name = "Arabic (Lebanon)", },
            new LangCode{ Code = "ar-OM",  Name = "Arabic (Oman)", },
            new LangCode{ Code = "ar-QA",  Name = "Arabic (Qatar)", },
            new LangCode{ Code = "ar-SA",  Name = "Arabic (Saudi Arabia)", },
            new LangCode{ Code = "ar-SY",  Name = "Arabic (Syria)", },
            new LangCode{ Code = "bg-BG",  Name = "Bulgarian (Bulgaria)", },
            new LangCode{ Code = "ca-ES",  Name = "Catalan (Spain)", },
            new LangCode{ Code = "cs-CZ",  Name = "Czech (Czech Republic)", },
            new LangCode{ Code = "da-DK",  Name = "Danish (Denmark)", },
            new LangCode{ Code = "de-DE",  Name = "German (Germany)", },
            new LangCode{ Code = "el-GR",  Name = "Greek (Greece)", },
            new LangCode{ Code = "en-AU",  Name = "English (Australia)", },
            new LangCode{ Code = "en-CA",  Name = "English (Canada)", },
            new LangCode{ Code = "en-GB",  Name = "English (United Kingdom)", },
            new LangCode{ Code = "en-HK",  Name = "English (Hong Kong)", },
            new LangCode{ Code = "en-IE",  Name = "English (Ireland)", },
            new LangCode{ Code = "en-IN",  Name = "English (India)", },
            new LangCode{ Code = "en-NZ",  Name = "English (New Zealand)", },
            new LangCode{ Code = "en-PH",  Name = "English (Philippines)", },
            new LangCode{ Code = "en-SG",  Name = "English (Singapore)", },
            new LangCode{ Code = "en-US",  Name = "English (United States)", },
            new LangCode{ Code = "en-ZA",  Name = "English (South Africa)", },
            new LangCode{ Code = "es-AR",  Name = "Spanish (Argentina)", },
            new LangCode{ Code = "es-BO",  Name = "Spanish (Bolivia)", },
            new LangCode{ Code = "es-CL",  Name = "Spanish (Chile)", },
            new LangCode{ Code = "es-CO",  Name = "Spanish (Colombia)", },
            new LangCode{ Code = "es-CR",  Name = "Spanish (Costa Rica)", },
            new LangCode{ Code = "es-CU",  Name = "Spanish (Cuba)", },
            new LangCode{ Code = "es-DO",  Name = "Spanish (Dominican Republic)", },
            new LangCode{ Code = "es-EC",  Name = "Spanish (Ecuador)", },
            new LangCode{ Code = "es-ES",  Name = "Spanish (Spain)", },
            new LangCode{ Code = "es-GT",  Name = "Spanish (Guatemala)", },
            new LangCode{ Code = "es-HN",  Name = "Spanish (Honduras)", },
            new LangCode{ Code = "es-MX",  Name = "Spanish (Mexico)", },
            new LangCode{ Code = "es-NI",  Name = "Spanish (Nicaragua)", },
            new LangCode{ Code = "es-PA",  Name = "Spanish (Panama)", },
            new LangCode{ Code = "es-PE",  Name = "Spanish (Peru)", },
            new LangCode{ Code = "es-PR",  Name = "Spanish (Puerto Rico)", },
            new LangCode{ Code = "es-PY",  Name = "Spanish (Paraguay)", },
            new LangCode{ Code = "es-SV",  Name = "Spanish (El Salvador)", },
            new LangCode{ Code = "es-US",  Name = "Spanish (USA)", },
            new LangCode{ Code = "es-UY",  Name = "Spanish (Uruguay)", },
            new LangCode{ Code = "es-VE",  Name = "Spanish (Venezuela)", },
            new LangCode{ Code = "et-EE",  Name = "Estonian(Estonia)", },
            new LangCode{ Code = "fi-FI",  Name = "Finnish (Finland)", },
            new LangCode{ Code = "fr-CA",  Name = "French (Canada)", },
            new LangCode{ Code = "fr-FR",  Name = "French (France)", },
            new LangCode{ Code = "ga-IE",  Name = "Irish(Ireland)", },
            new LangCode{ Code = "gu-IN",  Name = "Gujarati (Indian)", },
            new LangCode{ Code = "hi-IN",  Name = "Hindi (India)", },
            new LangCode{ Code = "hr-HR",  Name = "Croatian (Croatia)", },
            new LangCode{ Code = "hu-HU",  Name = "Hungarian (Hungary)", },
            new LangCode{ Code = "it-IT",  Name = "Italian (Italy)", },
            new LangCode{ Code = "ja-JP",  Name = "Japanese (Japan)", },
            new LangCode{ Code = "ko-KR",  Name = "Korean (Korea)", },
            new LangCode{ Code = "lt-LT",  Name = "Lithuanian (Lithuania)", },
            new LangCode{ Code = "lv-LV",  Name = "Latvian (Latvia)", },
            new LangCode{ Code = "mr-IN",  Name = "Marathi (India)", },
            new LangCode{ Code = "mt-MT",  Name = "Maltese(Malta)", },
            new LangCode{ Code = "nb-NO",  Name = "Norwegian (Bokmål) (Norway)", },
            new LangCode{ Code = "nl-NL",  Name = "Dutch (Netherlands)", },
            new LangCode{ Code = "pl-PL",  Name = "Polish (Poland)", },
            new LangCode{ Code = "pt-BR",  Name = "Portuguese (Brazil)", },
            new LangCode{ Code = "pt-PT",  Name = "Portuguese (Portugal)", },
            new LangCode{ Code = "ro-RO",  Name = "Romanian (Romania)", },
            new LangCode{ Code = "ru-RU",  Name = "Russian (Russia)", },
            new LangCode{ Code = "sk-SK",  Name = "Slovak (Slovakia)", },
            new LangCode{ Code = "sl-SI",  Name = "Slovenian (Slovenia)", },
            new LangCode{ Code = "sv-SE",  Name = "Swedish (Sweden)", },
            new LangCode{ Code = "ta-IN",  Name = "Tamil (India)", },
            new LangCode{ Code = "te-IN",  Name = "Telugu (India)", },
            new LangCode{ Code = "th-TH",  Name = "Thai (Thailand)", },
            new LangCode{ Code = "tr-TR",  Name = "Turkish (Turkey)", },
            new LangCode{ Code = "zh-CN",  Name = "Chinese (Mandarin, Simplified)", },
            new LangCode{ Code = "zh-HK",  Name = "Chinese (Cantonese, Traditional)", },
            new LangCode{ Code = "zh-TW",  Name = "Chinese (Taiwanese Mandarin)", },

        };

        static AzureSpeechToText()
        {
            Languages.Sort((a, b) => string.Compare(a.Name, b.Name));
        }
    }
}
