namespace AlcatelRouterStats.Audio
{
    using System.Diagnostics.CodeAnalysis;
    using System.Speech.Synthesis;

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Only registered with dependency injection when running Windows")]
    internal class SpeechWindows : ISpeech
    {
        private readonly SpeechSynthesizer speechSynthesizer;

        public SpeechWindows(SpeechSynthesizer speechSynthesizer)
        {
            this.speechSynthesizer = speechSynthesizer;
            this.speechSynthesizer.SetOutputToDefaultAudioDevice();
        }

        public void Speek(string text)
        {
            speechSynthesizer.Speak(text);
        }
    }
}
