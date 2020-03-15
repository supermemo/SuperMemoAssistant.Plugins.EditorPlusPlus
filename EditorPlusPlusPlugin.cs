#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   2019/04/22 17:20
// Modified On:  2019/04/22 20:52
// Modified By:  Alexis

#endregion


using System.Windows.Input;
using SuperMemoAssistant.Services;
using SuperMemoAssistant.Services.IO.Keyboard;
using SuperMemoAssistant.Services.Sentry;
using SuperMemoAssistant.Sys.IO.Devices;
using System.Windows;
using SuperMemoAssistant.Extensions;
using SuperMemoAssistant.Plugins.EditorPlusPlus.UI;
using HtmlAgilityPack;
using SuperMemoAssistant.Extensions;
using mshtml;


namespace SuperMemoAssistant.Plugins.EditorPlusPlus
{
  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  public class EditorPlusPlus : SentrySMAPluginBase<EditorPlusPlus>
  {
    #region Constructors

    public EditorPlusPlus() { }

    #endregion




    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "EditorPlusPlus";

    public override bool HasSettings => false;

    #endregion


    #region Methods Impl

    /// <inheritdoc />
    protected override void PluginInit()
    {
     Svc.HotKeyManager.RegisterGlobal(
       "AdvancedPasteWindow",
       "Open AdvancedPasteWindow",
       HotKeyScope.SM,
       new HotKey(Key.B, KeyModifiers.CtrlShift),
       OpenAdvancedPasteMenu
      );      

      Svc.HotKeyManager.RegisterGlobal(
        "RemoveClozeHighlight",
        "Remove cloze highlight",
        HotKeyScope.SM,
        new HotKey(Key.R, KeyModifiers.CtrlAlt),
        RemoveClozeHighlight
      );
    }

    public void OpenAdvancedPasteMenu()
    {

      Application.Current.Dispatcher.Invoke(
        () =>
        {
          var wdw = new AdvancedPasteMenu();
          wdw.ShowAndActivate();
        }
      );
    }

    public void RemoveClozeHighlight()
    {
      IControlHtml ctrlHtml = Svc.SM.UI.ElementWdw.ControlGroup.FocusedControl.AsHtml();

      if (ctrlHtml != null && !string.IsNullOrEmpty(ctrlHtml.Text))
      {
        string html = ctrlHtml.Text;

        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var spanNodes = doc.DocumentNode.SelectNodes("//span[contains(@class, 'clozed')]");
        if (spanNodes != null)
        {
            spanNodes.ForEach(n => n.Attributes.Remove("class"));
            ctrlHtml.Text = doc.DocumentNode.OuterHtml;
        }
      }
    }

    /// <inheritdoc />
    public override void ShowSettings()
    {
    }

    #endregion
  }
}
