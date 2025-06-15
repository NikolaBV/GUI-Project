using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape8 : Shape
    {
        #region Constructor

        public CustomShape8(RectangleF rect) : base(rect)
        {
        }

        public CustomShape8(CustomShape8 shape) : base(shape)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                float centerX = Rectangle.X + Rectangle.Width / 2;
                float centerY = Rectangle.Y + Rectangle.Height / 2;

                PointF p1 = new PointF(centerX, Rectangle.Y);
                PointF p2 = new PointF(Rectangle.X + Rectangle.Width, centerY);
                PointF p3 = new PointF(centerX, Rectangle.Y + Rectangle.Height);
                PointF p4 = new PointF(Rectangle.X, centerY);

                // Check if point is inside the diamond
                if (IsPointInPolygon(point, new PointF[] { p1, p2, p3, p4 }))
                {
                    return true;
                }

                // Check diagonal lines
                if (IsPointNearLine(point, p1, p3) || IsPointNearLine(point, p2, p4))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPointInPolygon(PointF point, PointF[] polygon)
        {
            bool inside = false;
            int j = polygon.Length - 1;

            for (int i = 0; i < polygon.Length; i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
                j = i;
            }
            return inside;
        }

        private bool IsPointNearLine(PointF point, PointF lineStart, PointF lineEnd)
        {
            float tolerance = Borderwidth;
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

            // Define diamond points
            PointF p1 = new PointF(centerX, Rectangle.Y);
            PointF p2 = new PointF(Rectangle.X + Rectangle.Width, centerY);
            PointF p3 = new PointF(centerX, Rectangle.Y + Rectangle.Height);
            PointF p4 = new PointF(Rectangle.X, centerY);

            PointF[] diamond = { p1, p2, p3, p4 };

            grfx.FillPolygon(new SolidBrush(FillColor), diamond);
            grfx.DrawPolygon(new Pen(StrokeColor, Borderwidth), diamond);

            // Draw cross lines
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), p1, p3);
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), p2, p4);

            grfx.Transform = originalMatrix;
        }
    }
}