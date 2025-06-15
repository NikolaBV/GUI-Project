using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape3 : Shape
    {
        #region Constructor

        public CustomShape3(RectangleF rect) : base(rect)
        {
        }

        public CustomShape3(CustomShape3 shape) : base(shape)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                // Check if point is inside the ellipse
                float a = Rectangle.Width / 2;
                float b = Rectangle.Height / 2;
                float centerX = Rectangle.X + a;
                float centerY = Rectangle.Y + b;

                bool isInEllipse = (Math.Pow((point.X - centerX) / a, 2) + Math.Pow((point.Y - centerY) / b, 2)) <= 1;

                if (isInEllipse)
                {
                    return true;
                }

                // Check if point is near the diagonal line from bottom-left to top-right
                if (IsPointNearLine(point,
                    new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height),
                    new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y)))
                {
                    return true;
                }
            }
            return false;
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

            // Draw ellipse
            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawEllipse(new Pen(StrokeColor, Borderwidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            // Draw diagonal line from bottom-left to top-right
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height),
                new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y));

            grfx.Transform = originalMatrix;
        }
    }
}