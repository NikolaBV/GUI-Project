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

            grfx.FillRectangle(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawRectangle(Pens.Black, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);


            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height), new PointF(centerX, centerY));

            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), new PointF(centerX, centerY), new PointF(centerX + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2));
            grfx.DrawLine(new Pen(StrokeColor, Borderwidth), new PointF(centerX, centerY), new PointF(centerX, Rectangle.Y));


        }
    }
}
