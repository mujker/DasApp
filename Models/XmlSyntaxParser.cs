using System.Collections.ObjectModel;
using System.Windows.Media;

namespace DasApp.Models
{
	internal class XmlSyntaxParser
	{
		private Collection<LanguageSyntaxStructure> languageSyntax;

		public Collection<LanguageSyntaxStructure> LanguageSyntax
		{
			get
			{
				if (this.languageSyntax == null)
				{
					this.languageSyntax = new Collection<LanguageSyntaxStructure>();

					string attributes = @"\G(?<attribute>[a-zA-Z][a-zA-Z0-9.:*_]*\s*(?==))";
					languageSyntax.Add(new LanguageSyntaxStructure(attributes, "attribute", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF004E"))));

					string elements = @"\G(?<element>(?<=(<)|(</))[a-zA-Z][a-zA-Z0-9.:*_]*\s*)";
					languageSyntax.Add(new LanguageSyntaxStructure(elements, "element", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"))));

					string comments = @"\G(?<comment><!--\s*[\s\S]*\s*-->\s*)";
					languageSyntax.Add(new LanguageSyntaxStructure(comments, "comment", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#537D01"))));

					string tags = @"\G(?<tag>(</|<|/>|>)\s*)";
					languageSyntax.Add(new LanguageSyntaxStructure(tags, "tag", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0160E5"))));

					string strings = "\\G(?<string>=\\s*\"[_=#{}a-zA-Z0-9.:;\\s-/,*]*\\s*\"\\s*)";
					languageSyntax.Add(new LanguageSyntaxStructure(strings, "string", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0160E5"))));

					string content = @"\G(?<content>[^<]+\s*)";
					languageSyntax.Add(new LanguageSyntaxStructure(content, "content", new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0160E5"))));
				}

				return languageSyntax;
			}
		}

	}
}
