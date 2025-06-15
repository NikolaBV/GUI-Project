using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape6 : Shape
    {
        #region Constructor

        public CustomShape6(RectangleF rect) : base(rect)
        {
        }

        public CustomShape6(CustomShape6 shape) : base(shape)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                // Define the diamond points based on the rectangle
                float centerX = Rectangle.X + Rectangle.Width / 2;
                float centerY = Rectangle.Y + Rectangle.Height / 2;

                PointF p1 = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height);
                PointF p2 = new PointF(Rectangle.X + Rectangle.Width * 0.2f, Rectangle.Y);
                PointF p3 = new PointF(Rectangle.X + Rectangle.Width * 0.8f, Rectangle.Y);
                PointF p4 = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);
                PointF center = new PointF(centerX, centerY);

                // Check if point is inside any of the triangles
                if (IsPointInTriangle(point, p1, p2, center) ||
                    IsPointInTriangle(point, p4, p3, center) ||
                    IsPointInTriangle(point, center, p2, p3))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPointInTriangle(PointF point, PointF p1, PointF p2, PointF p3)
        {
            float denominator = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            if (Math.Abs(denominator) < 0.001f) return false;

            float a = ((p2.Y - p3.Y) * (point.X - p3.X) + (p3.X - p2.X) * (point.Y - p3.Y)) / denominator;
            float b = ((p3.Y - p1.Y) * (point.X - p3.X) + (p1.X - p3.X) * (point.Y - p3.Y)) / denominator;
            float c = 1 - a - b;

            return a >= 0 && b >= 0 && c >= 0;
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

            // Define points
            PointF p1 = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height);
            PointF p2 = new PointF(Rectangle.X + Rectangle.Width * 0.2f, Rectangle.Y);
            PointF p3 = new PointF(Rectangle.X + Rectangle.Width * 0.8f, Rectangle.Y);
            PointF p4 = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height);
            PointF center = new PointF(centerX, centerY);

            // Draw triangles
            PointF[] triangle1 = { p1, p2, center };
            PointF[] triangle2 = { p4, p3, center };
            PointF[] triangle3 = { center, p2, p3 };

            grfx.FillPolygon(new SolidBrush(FillColor), triangle1);
            grfx.FillPolygon(new SolidBrush(FillColor), triangle2);
            grfx.FillPolygon(new SolidBrush(FillColor), triangle3);

            grfx.DrawPolygon(new Pen(StrokeColor, Borderwidth), triangle1);
            grfx.DrawPolygon(new Pen(StrokeColor, Borderwidth), triangle2);
            grfx.DrawPolygon(new Pen(StrokeColor, Borderwidth), triangle3);

            grfx.Transform = originalMatrix;
        }
    }
}
