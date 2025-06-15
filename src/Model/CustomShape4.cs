using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape4 : Shape
    {
        #region Constructor

        public CustomShape4(RectangleF rect) : base(rect)
        {
        }

        public CustomShape4(CustomShape4 shape) : base(shape)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                // Check if point is inside the rectangle
                if (point.X >= Rectangle.X && point.X <= Rectangle.X + Rectangle.Width &&
                    point.Y >= Rectangle.Y && point.Y <= Rectangle.Y + Rectangle.Height)
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

            grfx.FillRectangle(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawRectangle(new Pen(StrokeColor, Borderwidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

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
