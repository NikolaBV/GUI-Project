using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>


    [Serializable]
    public class LineShape : Shape
    {
        #region Constructor

        public LineShape(RectangleF rect) : base(rect)
        {
        }

        public LineShape(LineShape rectangle) : base(rectangle)
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
        public override bool Contains(PointF MouseAt)
        {
            if (base.Contains(MouseAt))
            {

                float paramOneM = Width / 2;
                float paramTwoM = Width / 2;
                float x1 = Location.X + paramOneM;
                float y1 = Location.Y + paramTwoM;
                bool isItOnTheLine = Math.Pow((MouseAt.X - x1) / paramOneM, 2) + Math.Pow((MouseAt.Y - y1) / paramTwoM, 2) - Borderwidth <= 0;


                return isItOnTheLine;
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
            Pen blackPen = new Pen((FillColor), Borderwidth);

            PointF P1 = new PointF(Rectangle.X, Rectangle.Y);
            PointF P2 = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Width);

            grfx.DrawLine(blackPen, P1, P2);
            grfx.ResetTransform();
        }
    }
}
