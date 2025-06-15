using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape9 : Shape
    {
        #region Constructor

        public CustomShape9(RectangleF rect) : base(rect)
        {
        }

        public CustomShape9(CustomShape9 shape) : base(shape)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                float centerX = Rectangle.X + Rectangle.Width / 2;
                float centerY = Rectangle.Y + Rectangle.Height / 2;
                float radiusX = Rectangle.Width / 2;
                float radiusY = Rectangle.Height / 2;

                // Check if point is inside the ellipse
                float dx = (point.X - centerX) / radiusX;
                float dy = (point.Y - centerY) / radiusY;
                if (dx * dx + dy * dy <= 1)
                {
                    return true;
                }

                // Check diagonal lines
                if (IsPointNearLine(point,
                    new PointF(Rectangle.X, Rectangle.Y),
                    new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height)))
                {
                    return true;
                }

                if (IsPointNearLine(point,
                    new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y),
                    new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height)))
                {
                    return true;
                }
            }
            return false;
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

            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawEllipse(new Pen(StrokeColor, Borderwidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            // Draw diagonal cross
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(Rectangle.X, Rectangle.Y),
                new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height));

            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y),
                new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height));

            grfx.Transform = originalMatrix;
        }
    }
}