using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_Day5_HydrothermalVenture
{
    internal class Line
    {
        private readonly Point _start;
        private readonly Point _end;

        public Line(string lineDefinition)
        {
            var parts = lineDefinition.Split(' ');
            _start = GetPointFromPart(parts[0]);
            _end = GetPointFromPart(parts[2]);
        }

        public override string ToString()
        {
            return $"{_start} => {_end}";
        }

        public IEnumerable<Point> GetPoints()
        {
            Point p = _start;

            while (p != _end)
            {
                yield return p;
                p.Offset(LineDirection);
            }

            yield return _end;
        }

        private Point GetPointFromPart(string part)
        {
            var coords = part.Split(',');
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);

            return new Point(x, y);
        }

        private int Direction(int a, int b) => a == b ? 0 : a < b ? 1 : -1;

        private Point LineDirection => new Point(Direction(_start.X, _end.X), Direction(_start.Y, _end.Y));  
    }
}
