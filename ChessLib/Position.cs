using System;
using System.Collections.Generic;

namespace ChessLib
{
    public class Position : ICloneable
    {
        public Position() : this(-1, -1)
        {

        }

        public Position(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public int PosX { get; set; }

        public int PosY { get; set; }

        public bool IsInBoard()
        {
            return PosX >= 0 && PosX < 8 && PosY >= 0 && PosY < 8;
        }

        public Position GetDirection(Position b)
        {
            var dirs = new List<Position>() { new Position(1, -1), new Position(1, 1), new Position(0, -1), new Position(-1, 0),
                                   new Position(0, 1), new Position(1, 0), new Position(-1, -1), new Position(-1, 1) };
            foreach (var dir in dirs)
                if (IsInDirection(dir, b))
                    return dir;

            return null;
        }

        public IEnumerable<Position> AllMovesFromStartToEnd(Position dir, Position end)
        {
            if (dir == null)
                yield break;

            var start = new Position(PosX, PosY);

            while (start != end)
            {
                start += dir;

                if (start == end)
                    yield break;

                if (start.IsInBoard())
                    yield return start;
                else
                    yield break;
            }

            yield break;
        }

        public bool IsInDirection(Position dir, Position end)
        {
            if (dir == null)
                return false;

            var start = new Position(PosX, PosY);

            while (start != end)
            {
                start += dir;

                if (!start.IsInBoard())
                    return false;
            }

            return true;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1?.PosX == p2?.PosX && p1?.PosY == p2?.PosY;
        }

        public static Position operator +(Position p1, Position p2)
        {
            if (p1 == null || p2 == null)
                return null;

            return new Position(p1.PosX + p2.PosX, p1.PosY + p2.PosY);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return p1?.PosX != p2?.PosX || p1?.PosY != p2?.PosY;
        }

        public override string ToString()
        {
            return "[" + PosX + ", " + PosY + "]";
        }

        public object Clone()
        {
            return new Position(PosX, PosY);
        }
    }
}
