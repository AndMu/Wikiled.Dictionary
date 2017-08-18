using System;
using System.Collections.Generic;
using System.Text;

namespace Wikiled.Dictionary.Data
{
    public class TranslationResult
    {
        public TranslationRequest Request { get; set; }

        public Word[] Similar { get; set; }

        public Word[] Translations { get; set; }
    }
}
