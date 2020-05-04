using System;
using System.Collections.Generic;
using System.Text;

namespace LudoLib
{
    public static class RuleSet
    {
        public static int MinDiceValue { get; set; }

        public static int MaxDiceValue { get; set; }

        public static bool CanCapture { get; set; }

        public static bool CanDiagonal { get; set; }
    }
}
