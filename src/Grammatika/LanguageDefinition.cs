using System;

namespace Grammatika
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LanguageDefinition
    {
        public LanguageDefinition()
        {
            _rules = new List<RuleDefinition>();
        }

        private List<RuleDefinition> _rules;
        public string Name { get; set; }

        public List<RuleDefinition> Rules
        {
            get { return _rules; }
        }
    }

    public class RuleDefinition
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}