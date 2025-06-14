using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    public class GroupShape : Shape
    {
        #region Constructor

        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        #region Properties
        private List<Shape> _groupedShapes = new List<Shape>();
        public List<Shape> GroupedShapes
        {
            get { return _groupedShapes; }
            set { _groupedShapes = value; }
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
            foreach (Shape shape in _groupedShapes)
            {
                if (shape.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            foreach (Shape shape in _groupedShapes)
            {
                shape.DrawSelf(grfx);
            }
        }

        public void MoveGroupedShape(float deltaX, float deltaY)
        {
            foreach (Shape shape in _groupedShapes)
            {
                shape.Location = new PointF(
                    shape.Location.X + deltaX,
                    shape.Location.Y + deltaY
                );
            }
        }

        public void GroupResizeWidth(float width)
        {
            foreach (Shape shape in _groupedShapes)
            {
                shape.Width = width;
            }
        }

        public void GroupResizeHeight(float height)
        {
            foreach (Shape shape in _groupedShapes)
            {
                shape.Height = height;
            }
        }

        public override void ChangeGroupRotate(float angle)
        {
            foreach (Shape shape in _groupedShapes)
            {
                shape.ShapeAngle = angle;
            }
        }
    }
}
