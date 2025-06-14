using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape7 : Shape
    {
        public CustomShape7(RectangleF rect) : base(rect) { }
        public CustomShape7(CustomShape7 shape) : base(shape) { }

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

            // Calculate midpoints
            PointF center = new PointF(centerX, centerY);
            PointF left = new PointF(Rectangle.X, centerY);
            PointF right = new PointF(Rectangle.X + Rectangle.Width, centerY);
            PointF top = new PointF(centerX, Rectangle.Y);
            PointF bottom = new PointF(centerX, Rectangle.Y + Rectangle.Height);

            Pen linePen = new Pen(StrokeColor, Borderwidth);
            grfx.DrawLine(linePen, center, left);
            grfx.DrawLine(linePen, center, right);
            grfx.DrawLine(linePen, center, top);
            grfx.DrawLine(linePen, center, bottom);

            grfx.Transform = originalMatrix;
        }
    }
} 