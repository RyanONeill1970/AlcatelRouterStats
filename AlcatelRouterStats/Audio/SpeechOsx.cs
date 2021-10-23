namespace AlcatelRouterStats.Audio
{
    using System.Diagnostics;

    public class SpeechOsx : ISpeech
    {
        public void Speek(string text)
        {
            Process.Start("say", text);
        }
    }
}
