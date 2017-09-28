using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace DasApp.Models
{
	internal class Tokenizer
	{
		private static Regex codeSyntax;
		private List<Token> tokens = new List<Token>();
		private Collection<LanguageSyntaxStructure> languageStructure;


		public List<Token> TokenizeCode(string code, string extension)
		{
			this.tokens.Clear();

			XmlSyntaxParser xmlSyntax = new XmlSyntaxParser();

			this.languageStructure = xmlSyntax.LanguageSyntax;

			codeSyntax = new Regex(this.GenerateLanguageSyntaxRegularExpression(),
				RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

			MatchCollection matches = codeSyntax.Matches(code);

			if (matches.Count != 0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					Token t = this.Tokenize(matches[i]);
					this.tokens.Add(t);
				}
			}

			return this.tokens;
		}

		private string GenerateLanguageSyntaxRegularExpression()
		{
			StringBuilder regEx = new StringBuilder();
			regEx.Append(@"\s*");
			if (this.languageStructure.Count > 0)
			{
				for (int i = 0; i < this.languageStructure.Count - 1; i++)
				{
					regEx.AppendFormat("{0}|", this.languageStructure[i].RegexString);
				}
				regEx.AppendFormat("{0}", this.languageStructure[this.languageStructure.Count - 1].RegexString);
			}
			regEx.Append(@"\s*");
			return regEx.ToString();
		}

		private Token Tokenize(Match match)
		{
			for (int i = 0; i < this.languageStructure.Count; i++)
			{
				if (match.Groups[this.languageStructure[i].Description].Success)
				{
					var value = match.Groups[this.languageStructure[i].Description].Value;
					if (value.Contains("CustomProperty") || value.Contains("CustomValue"))
					{
						return new Token(match.Groups[this.languageStructure[i].Description].Value, this.languageStructure[i].Color, true, false);
					}
					return new Token(value, this.languageStructure[i].Color);
				}
			}
			return new Token(null, null);
		}
	}
}
