using Parser.Semantic;
using System;
using Parser.Syntax;

namespace GMData.Mod
{
    public class ModElementLoader
    {
        public static T Load<T>(string fileName, string fileContent)
        {
            try
            {
                var syntaxItem = SyntaxItem.RootParse(fileContent);
                return SemanticParser.DoParser<T>(syntaxItem);
            }
            catch (Exception e)
            {
                throw new Exception($"Parse error in script:{fileName}", e);
            }

        }
    }
}
