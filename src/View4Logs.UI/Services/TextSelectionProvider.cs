using System.Windows.Controls;
using System.Windows.Input;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.Services
{
    public class TextSelectionProvider : ITextSelectionProvider
    {
        public string GetSelectedText()
        {
            if (Keyboard.FocusedElement is TextBox textBox)
            {                
                return string.IsNullOrEmpty(textBox.SelectedText)
                    ? textBox.Text
                    : textBox.SelectedText;
            }

            return null;
        }
    }
}
