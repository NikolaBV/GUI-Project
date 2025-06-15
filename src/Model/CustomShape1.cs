using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    [Serializable]
    public class CustomShape1 : Shape
    {
        #region Constructor
        
        public CustomShape1(RectangleF rect) : base(rect)
        {
        }
        
        public CustomShape1(CustomShape1 shape) : base(shape)
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

                // Check if point is near the diagonal lines
                float centerX = Rectangle.X + Rectangle.Width / 2;
                float centerY = Rectangle.Y + Rectangle.Height / 2;

                // Check first diagonal line (left middle to center)
                if (IsPointNearLine(point, 
                    new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height / 2),
                    new PointF(centerX, centerY)))
                {
                    return true;
                }

                // Check second diagonal line (right top to center)
                if (IsPointNearLine(point,
                    new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y),
                    new PointF(centerX, centerY)))
                {
                    return true;
                }

                // Check third diagonal line (center to bottom right)
                if (IsPointNearLine(point,
                    new PointF(centerX, centerY),
                    new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height)))
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
            
            //Matrix originalMatrix = grfx.Transform;
            
            //Matrix rotationMatrix = new Matrix();
            
            float centerX = Rectangle.X + Rectangle.Width / 2;
            float centerY = Rectangle.Y + Rectangle.Height / 2;
            
            //rotationMatrix.RotateAt(ShapeAngle, new PointF(centerX, centerY));
            
            //grfx.Transform = rotationMatrix;
            
            grfx.FillRectangle(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawRectangle(new Pen(StrokeColor, Borderwidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height / 2),
                new PointF(centerX, centerY));
            
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y),
                new PointF(centerX, centerY));
            
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth),
                new PointF(centerX, centerY),
                new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height));
            
            //grfx.Transform = originalMatrix;
        }
    }
} 