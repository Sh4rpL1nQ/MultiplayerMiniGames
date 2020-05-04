using System;
using System.Collections.Generic;
using System.Text;

namespace LudoLib
{
    public class Board
    {
        public List<Field> Fields { get; set; }

        public Board()
        {
            Fields = new List<Field>();
        }

        public void MakeBoard()
        {
            Fields = new List<Field>()
            {
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(1, Color.White, FieldType.Normal), new Field(2, Color.White, FieldType.Gate), new Field(3, Color.Green, FieldType.Entrance), new Field(-1), new Field(-1), new Field(-1), new Field(-1),
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(40, Color.White, FieldType.Normal), new Field(1, Color.Green, FieldType.Goal), new Field(4, Color.White, FieldType.Normal), new Field(-1), new Field(-1), new Field(-1), new Field(-1),
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(39, Color.White, FieldType.Normal), new Field(2, Color.Green, FieldType.Goal), new Field(5, Color.White, FieldType.Normal), new Field(-1), new Field(-1), new Field(-1), new Field(-1),
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(38, Color.White, FieldType.Normal), new Field(3, Color.Green, FieldType.Goal), new Field(6, Color.White, FieldType.Normal), new Field(-1), new Field(-1), new Field(-1), new Field(-1),
                new Field(33, Color.Yellow, FieldType.Entrance), new Field(34, Color.White, FieldType.Normal), new Field(35, Color.White, FieldType.Normal), new Field(36, Color.White, FieldType.Normal), new Field(37, Color.White, RuleSet.CanDiagonal ? FieldType.Diagonal : FieldType.Normal), new Field(4, Color.Green, FieldType.Goal), new Field(7, Color.White, RuleSet.CanDiagonal ? FieldType.Diagonal : FieldType.Normal), new Field(8, Color.White, FieldType.Normal), new Field(9, Color.White, FieldType.Normal), new Field(10, Color.White, FieldType.Normal), new Field(11, Color.White, FieldType.Normal),
                new Field(32, Color.White, FieldType.Gate), new Field(1, Color.Yellow, FieldType.Goal), new Field(2, Color.Yellow, FieldType.Goal), new Field(3, Color.Yellow, FieldType.Goal), new Field(4, Color.Yellow, FieldType.Goal), new Field(-1), new Field(1, Color.Red, FieldType.Goal), new Field(2, Color.Red, FieldType.Goal), new Field(3, Color.Red, FieldType.Goal), new Field(4, Color.Red, FieldType.Goal), new Field(12, Color.White, FieldType.Gate),
                new Field(31, Color.Yellow, FieldType.Normal), new Field(30, Color.White, FieldType.Normal), new Field(29, Color.White, FieldType.Normal), new Field(28, Color.White, FieldType.Normal), new Field(27, Color.White, RuleSet.CanDiagonal ? FieldType.Diagonal : FieldType.Normal), new Field(4, Color.Blue, FieldType.Goal), new Field(17, Color.White, RuleSet.CanDiagonal ? FieldType.Diagonal : FieldType.Normal), new Field(16, Color.White, FieldType.Normal), new Field(15, Color.White, FieldType.Normal), new Field(14, Color.White, FieldType.Normal), new Field(13, Color.Red, FieldType.Entrance),
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(26, Color.White, FieldType.Normal), new Field(3, Color.White, FieldType.Gate), new Field(18, Color.White, FieldType.Normal), new Field(-1), new Field(-1), new Field(-1), new Field(-1),
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(25, Color.White, FieldType.Normal), new Field(2, Color.Green, FieldType.Goal), new Field(19, Color.White, FieldType.Normal), new Field(-1), new Field(-1), new Field(-1), new Field(-1),
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(24, Color.White, FieldType.Normal), new Field(1, Color.Green, FieldType.Goal), new Field(20, Color.White, FieldType.Normal), new Field(-1), new Field(-1), new Field(-1), new Field(-1),
                new Field(-1), new Field(-1), new Field(-1), new Field(-1), new Field(23, Color.Blue, FieldType.Entrance), new Field(22, Color.Green, FieldType.Gate), new Field(21, Color.White, FieldType.Normal), new Field(-1), new Field(-1), new Field(-1), new Field(-1)
            };
        }
    }
}
