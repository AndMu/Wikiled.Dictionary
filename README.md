# Dictionary API

![Nuget](https://img.shields.io/nuget/v/Wikiled.Dictionary.svg)

.NET Standard 1.1

37 Languages Online Free Dictionary API.

Supports translation from/to English and any of below languages:

* Arabic
* Croatian
* Czech
* Dutch
* Danish
* Esperanto
* Estonian
* Finnish
* French
* German
* Greek
* Hebrew
* Italian
* JapaneseKanaRomaji
* JapaneseKanjiKana
* JapaneseKanjiRomaji
* JapaneseRomajiKanji
* Latin
* Latvian
* Lithuanian
* Norwegian
* Persian
* PersianFarsi
* Polish
* Portuguese
* Romanian
* Russian
* Serbian
* SimpleChinese
* Slovak
* Spanish
* Swedish
* Thai
* Turkish
* Ukrainian
* Yiddish


Samle code:
```C#
var request = new TranslationRequest
                             {
                                 From = Language.English,
                                 To = Language.German,
                                 Word = "Love"
                             };
var result = await instance.Translate(request, CancellationToken.None)
			
```
