using System;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.Services
{
    public class WebSearchService : IWebSearchService
    {
        public void OpenWebSearch(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            try
            {
                System.Diagnostics.Process.Start($"https://www.google.com/search?q=\"{Uri.EscapeDataString(text)}\"");
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
