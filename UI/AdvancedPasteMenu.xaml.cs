using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HtmlAgilityPack;
using SuperMemoAssistant.Extensions;
using SuperMemoAssistant.Interop.SuperMemo.Content.Controls;
using SuperMemoAssistant.Services;
using mshtml;

namespace SuperMemoAssistant.Plugins.EditorPlusPlus.UI
{
  /// <summary>
  /// Interaction logic for AdvancedPasteMenu.xaml
  /// </summary>
  public partial class AdvancedPasteMenu
  {

    public AdvancedPasteMenu()
    {
      InitializeComponent();
      
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Enter:
          OkBtn_Click();
          break;
        case Key.Escape:
          Close();
          break;
      }
    }

    private void ExecuteSelected()
    {
      if (PlainTextPaste.IsChecked == true)
      {
        string clipboardText = GetClipboardText();
        if (!string.IsNullOrEmpty(clipboardText))
        {
          string cleanText = FilterHtmlTags(clipboardText);
          if (!string.IsNullOrEmpty(cleanText))
          {
            PasteText(cleanText);
          }
        }
        
      }
      Close();
    }

    private string GetClipboardText()
    {
      if (Clipboard.ContainsText())
      {
        return Clipboard.GetText();
      }

      return null;
    }

    private string FilterHtmlTags(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return text;
      }
      var doc = new HtmlDocument();
      doc.LoadHtml(text);
      return doc.DocumentNode.InnerText;
    }

    private void PasteText(string text)
    {
        IControlHtml ctrlHtml = Svc.SM.UI.ElementWdw.ControlGroup.FocusedControl.AsHtml();

        if (ctrlHtml != null)
        {
          var htmlDoc = ctrlHtml.GetDocument();
          var htmlSelObj = htmlDoc?.selection;

          if (htmlSelObj?.createRange() is IHTMLTxtRange textSel)
            textSel.text = text;
        }
    }

    private void OkBtn_Click(object sender, RoutedEventArgs e)
    {
      ExecuteSelected();
    }
  }
}
