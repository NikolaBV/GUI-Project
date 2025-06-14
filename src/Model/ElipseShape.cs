using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>

    [Serializable]
    public class ElipseShape : Shape
    {
        #region Constructor

        public ElipseShape(RectangleF rect) : base(rect)
        {
        }

        public ElipseShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
            {
                float a = Width / 2;
                float b = Height / 2;
                float x1 = Location.X + a;
                float y1 = Location.Y + b;
                bool isItOn = (Math.Pow((point.X - x1) / a, 2) + Math.Pow((point.Y - y1) / b, 2) - 1) <= 0;
                return isItOn;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
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
            grfx.DrawEllipse(new Pen(StrokeColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            grfx.Transform = originalMatrix;
        }
    }
}
