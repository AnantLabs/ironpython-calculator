using System.Windows.Media;


namespace Calculator.Controls.SyntaxHighlight
{
    /// <summary>
    /// Highlight rule
    /// </summary>
    public class SyntaxRuleItem
    {
        /// <summary>
        /// Category of highlight rule
        /// </summary>
        public CodeTokenType RuleType
        {
            get;
            set;
        }

        /// <summary>
        /// Foreground brush
        /// </summary>
        public Brush Foreground
        {
            get;
            set;
        }
    }
}
