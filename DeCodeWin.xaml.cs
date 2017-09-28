using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DasApp.Log4Net;
using DasApp.Models;
using DasApp.Socket;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;
namespace DasApp
{
    /// <summary>
    /// DeCodeWin.xaml 的交互逻辑
    /// </summary>
    public partial class DeCodeWin : Window
    {
        public string CodeStr { get; set; }
        public DeCodeWin(string codePrm)
        {
            CodeStr = codePrm;
            InitializeComponent();
            try
            {
                var temp = DataPacketCodec.Decode(CodeStr.TrimEnd('#'), YXJK_JKD.CryptKey);
                Editor.Document = CreateFormattedDocument(temp, ".json");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Source, ex);
            }
        }

        private RadDocument CreateFormattedDocument(string text, string fileFormat)
        {
            RadDocument document = new RadDocument();
            document.LayoutMode = DocumentLayoutMode.Flow;
            document.SectionDefaultPageMargin = new Padding(25);

            Section section = new Section();
            document.Sections.Add(section);

            Tokenizer tokenizer = new Tokenizer();
            List<Token> tokens = tokenizer.TokenizeCode(text, fileFormat);

            Paragraph currentParagraph = new Paragraph();
            currentParagraph.SpacingAfter = 0;
            section.Blocks.Add(currentParagraph);
            foreach (Token token in tokens)
            {
                string[] lines = Regex.Split(token.Value, DocumentEnvironment.NewLine);

                bool createParagraph = false;
                foreach (string line in lines)
                {
                    if (createParagraph)
                    {
                        currentParagraph = new Paragraph();
                        currentParagraph.SpacingAfter = 0;
                        section.Blocks.Add(currentParagraph);
                    }
                    createParagraph = true;

                    if (!string.IsNullOrEmpty(line))
                    {
                        Span span = token.GetSpanStyle();
                        span.Text = line;
                        currentParagraph.Inlines.Add(span);
                    }
                }
            }

            return document;
        }
    }
}
