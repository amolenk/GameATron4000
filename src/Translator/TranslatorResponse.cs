namespace GameATron4000.Translator
{
    public class TranslatorResponse
    {
        public TranslatorDetectedLanguage DetectedLanguage { get; set; }

        public Translation[] Translations { get; set; } = new Translation[0]; 
    }

    public class TranslatorDetectedLanguage
    {
        public string Language { get; set; }

        public double Score { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }

        public string To { get; set; }

    }
}