using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape5 : Shape
    {
        #region Constructor

        public CustomShape5(RectangleF rect) : base(rect)
        {
        }

        public CustomShape5(CustomShape5 shape) : base(shape)
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

                // Check vertical line
                if (IsPointNearLine(point,
                    new PointF(centerX, Rectangle.Y),
                    new PointF(centerX, Rectangle.Y + Rectangle.Height)))
                {
                    return true;
                }

                // Check horizontal line
                if (IsPointNearLine(point,
                    new PointF(Rectangle.X, centerY),
                    new PointF(Rectangle.X + Rectangle.Width, centerY)))
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

            // Draw vertical line
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(centerX, Rectangle.Y),
                new PointF(centerX, Rectangle.Y + Rectangle.Height));

            // Draw horizontal line
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(Rectangle.X, centerY),
                new PointF(Rectangle.X + Rectangle.Width, centerY));

            grfx.Transform = originalMatrix;
        }
    }
}