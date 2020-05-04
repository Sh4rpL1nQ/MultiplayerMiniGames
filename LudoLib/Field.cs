using System;
using System.Collections.Generic;
using System.Text;

namespace LudoLib
{
    public class Field
    {
        public Color Color { get; set; }

        public int Number { get; set; }

        public FieldType Type { get; set; }

        public Field(int number, Color color, FieldType type)
        {
            Number = number;
            Color = color;
            Type = type;
        }

        public Field(int number) : this(number, Color.None, FieldType.None) { }
    }
}
