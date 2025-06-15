using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>


    [Serializable]
    public class PracticeShape : Shape
    {
        #region Constructor

        public PracticeShape(RectangleF rect) : base(rect)
        {
        }

        public PracticeShape(RectangleShape rectangle) : base(rectangle)
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
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                return true;
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
        }

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            float centerX = Rectangle.X + Rectangle.Width / 2;
            float centerY = Rectangle.Y + Rectangle.Height / 2;
            float radiusX = Rectangle.Width / 2;
            float radiusY = Rectangle.Height / 2;

            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawEllipse(new Pen(StrokeColor, Borderwidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            float angle1 = 225 * (float)Math.PI / 180;
            PointF bottomLeft = new PointF(
                centerX + radiusX * (float)Math.Cos(angle1),
                centerY + radiusY * (float)Math.Sin(angle1)
            );

            float angle2 = 45 * (float)Math.PI / 180;
            PointF topRight = new PointF(
                centerX + radiusX * (float)Math.Cos(angle2),
                centerY + radiusY * (float)Math.Sin(angle2)
            );

            float angle3 = 315 * (float)Math.PI / 180;
            PointF bottomRight = new PointF(
                centerX + radiusX * (float)Math.Cos(angle3),
                centerY + radiusY * (float)Math.Sin(angle3)
            );

            float angle4 = 135 * (float)Math.PI / 180;
            PointF topLeft = new PointF(
                centerX + radiusX * (float)Math.Cos(angle4),
                centerY + radiusY * (float)Math.Sin(angle4)
            );

            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), bottomLeft, new PointF(centerX, centerY));
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), new PointF(centerX, centerY), topRight);
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), bottomRight, new PointF(centerX, centerY));
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), new PointF(centerX, centerY), topLeft);
        }
    }
}
