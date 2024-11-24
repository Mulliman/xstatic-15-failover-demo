using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Web;
using XStatic.Core.Generator;
using XStatic.Core.Generator.Transformers;

namespace FifteenDemo.Umbraco.Transformers
{
    public class FormDisabledTransformer : XStatic.Core.Generator.Transformers.ITransformer
    {
        public string Transform(string input, IUmbracoContext context)
        {
            const string formDisabledHtml = "<h5>We are currently experiencing technical difficulties and some functionality is disabled.</h5>";
           
            string pattern = @"<form\b[^>]*>(.*?)<\/form>";

            return Regex.Replace(input, pattern, formDisabledHtml, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
    }

    public class FormDisabledTransformerFactory : XStatic.Core.Generator.Transformers.DefaultHtmlTransformerListFactory
    {
        public override IEnumerable<ITransformer> BuildTransformers(ISiteConfig siteConfig)
        {
            var defaults = base.BuildTransformers(siteConfig).ToList();

            return defaults.Concat([new FormDisabledTransformer()]);
        }
    }
}
