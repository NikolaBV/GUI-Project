using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape0 : Shape
    {
        #region Constructor

        public CustomShape0(RectangleF rect) : base(rect)
        {
        }

        public CustomShape0(CustomShape0 shape) : base(shape)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                // Get triangle points
                var p1 = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y); // Top point
                var p2 = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height); // Bottom left
                var p3 = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height); // Bottom right
                var center = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height * 0.6f);

                // Check if point is inside any of the three triangles or near the bottom line
                return IsPointInTriangle(point, p1, p2, center) ||
                       IsPointInTriangle(point, p3, p1, center) ||
                       IsPointNearLine(point, p2, p3);
            }
            return false;
        }

        private bool IsPointInTriangle(PointF p, PointF a, PointF b, PointF c)
        {
            float denom = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            if (Math.Abs(denom) < 0.001f) return false;

            float alpha = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / denom;
            float beta = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / denom;
            float gamma = 1 - alpha - beta;

            return alpha >= 0 && beta >= 0 && gamma >= 0;
        }

        private bool IsPointNearLine(PointF point, PointF lineStart, PointF lineEnd)
        {
            float tolerance = Math.Max(Borderwidth, 3);
            float lineLength = (float)Math.Sqrt(
                Math.Pow(lineEnd.X - lineStart.X, 2) +
                Math.Pow(lineEnd.Y - lineStart.Y, 2));

            if (lineLength == 0) return false;

            float distance = Math.Abs(
                (lineEnd.Y - lineStart.Y) * point.X -
                (lineEnd.X - lineStart.X) * point.Y +
                lineEnd.X * lineStart.Y -
                lineEnd.Y * lineStart.X) / lineLength;

            return distance <= tolerance;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            Matrix originalMatrix = grfx.Transform;

            Matrix rotationMatrix = new Matrix();

            float centerX = Rectangle.X + Rectangle.Width / 2;
            float centerY = Rectangle.Y + Rectangle.Height / 2;

            rotationMatrix.RotateAt(ShapeAngle, new PointF(centerX, centerY));

            grfx.Transform = rotationMatrix;

            // Define triangle points
            var p1 = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y); // Top point
            var p2 = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height); // Bottom left
            var p3 = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height); // Bottom right
            var center = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height * 0.6f);

            // Draw the triangles
            PointF[] leftTriangle = { p1, p2, center };
            PointF[] rightTriangle = { p3, p1, center };

            grfx.FillPolygon(new SolidBrush(FillColor), leftTriangle);
            grfx.FillPolygon(new SolidBrush(FillColor), rightTriangle);

            grfx.DrawPolygon(new Pen(StrokeColor, Borderwidth), leftTriangle);
            grfx.DrawPolygon(new Pen(StrokeColor, Borderwidth), rightTriangle);

            // Draw bottom line
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), p2, p3);

            grfx.Transform = originalMatrix;
        }
    }
}