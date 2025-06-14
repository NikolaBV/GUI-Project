using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Draw
{
    /// <summary>
    /// Класът, който ще бъде използван при управляване на диалога.
    /// </summary>
    public class DialogProcessor : DisplayProcessor
    {
        #region Constructor
        public DialogProcessor()
        {
        }
        #endregion

        #region Properties

        public Color currentFillColor = Color.White;
        public Color currentBorderColor = Color.Black;
        public float currentWidth = 1;
        /// <summary>
        /// Избран елемент.
        /// </summary>
        private List<Shape> selection = new List<Shape>();
        private const float GRAVITY = 9.81f;
        public List<Shape> Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        /// <summary>
        /// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
        /// </summary>
        private bool isDragging;
        public bool IsDragging
        {
            get { return isDragging; }
            set { isDragging = value; }
        }

        /// <summary>
        /// Последна позиция на мишката при "влачене".
        /// Използва се за определяне на вектора на транслация.
        /// </summary>
        private PointF lastLocation;
        public PointF LastLocation
        {
            get { return lastLocation; }
            set { lastLocation = value; }
        }


        #endregion

        /// <summary>
        /// Добавя примитив - правоъгълник на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomRectangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
            rect.FillColor = currentFillColor;
            ShapeList.Add(rect);
        }

        /// <summary>
        /// Добавя примитив - правоъгълник на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomLine()
        {
            Random rnd = new Random();
            int x1 = rnd.Next(100, 1000);
            int y1 = rnd.Next(100, 600);
            LineShape line = new LineShape(new Rectangle(x1, y1, 300, 300));

            line.Borderwidth = currentWidth;
            line.FillColor = currentFillColor;
            ShapeList.Add(line);
        }

        /// <summary>
        /// Добавя примитив - елипса на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomElipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            ElipseShape elipse = new ElipseShape(new Rectangle(x, y, 100, 200));
            elipse.FillColor = currentFillColor;
            elipse.StrokeColor = Color.Green;
            ShapeList.Add(elipse);
        }

        /// <summary>
        /// Добавя примитив - custom shape на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomCustomShape()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape customShape = new CustomShape(new Rectangle(x, y, 120, 80));
            customShape.FillColor = currentFillColor;
            customShape.StrokeColor = currentBorderColor;
            customShape.Borderwidth = currentWidth;
            ShapeList.Add(customShape);
        }

        /// <summary>
        /// Добавя примитив - custom shape7 на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomCustomShape7()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape7 customShape7 = new CustomShape7(new Rectangle(x, y, 100, 100));
            customShape7.FillColor = currentFillColor;
            customShape7.StrokeColor = currentBorderColor;
            customShape7.Borderwidth = currentWidth;
            ShapeList.Add(customShape7);
        }

        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на який принадлежи дадената точка.</returns>
        public Shape ContainsPoint(PointF point)
        {
            for (int i = ShapeList.Count - 1; i >= 0; i--)
            {
                if (ShapeList[i].Contains(point))
                {
                    return ShapeList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Транслация на избраните елементи на вектор определен от разликата между текущата и последната позиция
        /// </summary>
        /// <param name="currentLocation">Текуща позиция на мишката</param>
        public void TranslateTo(PointF currentLocation)
        {
            if (Selection == null || Selection.Count == 0)
                return;

            float deltaX = currentLocation.X - lastLocation.X;
            float deltaY = currentLocation.Y - lastLocation.Y;

            foreach (Shape shape in Selection)
            {
                shape.Location = new PointF(
                    shape.Location.X + deltaX,
                    shape.Location.Y + deltaY
                );
            }

            lastLocation = currentLocation;
        }

        public void SelectShape(Shape shape)
        {
            if (!Selection.Contains(shape))
            {
                Selection.Add(shape);
                shape.FillColor = Color.Red;
            }
        }

        public void DeselectShape(Shape shape)
        {
            if (Selection.Contains(shape))
            {
                Selection.Remove(shape);
                shape.FillColor = Color.White;
            }
        }

        public void ToggleShapeSelection(Shape shape)
        {
            if (Selection.Contains(shape))
            {
                DeselectShape(shape);
            }
            else
            {
                SelectShape(shape);
            }
        }

        public void CreateGroup()
        {
            if (Selection.Count < 2)
                return;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (Shape shape in Selection)
            {
                minX = Math.Min(minX, shape.Location.X);
                minY = Math.Min(minY, shape.Location.Y);
                maxX = Math.Max(maxX, shape.Location.X + shape.Width);
                maxY = Math.Max(maxY, shape.Location.Y + shape.Height);
            }

            RectangleF groupBounds = new RectangleF(minX, minY, maxX - minX, maxY - minY);
            GroupShape group = new GroupShape(groupBounds);

            group.GroupedShapes = new List<Shape>(Selection);

            foreach (Shape shape in Selection)
            {
                ShapeList.Remove(shape);
            }

            Selection.Clear();
            ShapeList.Add(group);
            Selection.Add(group);
        }

        public void UnGroupSelectedShapes()
        {
            List<Shape> shapesToUngroup = new List<Shape>();

            foreach (Shape shape in Selection.ToList())
            {
                if (shape is GroupShape groupShape)
                {
                    shapesToUngroup.AddRange(groupShape.GroupedShapes);
                    ShapeList.Remove(groupShape);
                    Selection.Remove(groupShape);
                }
            }

            foreach (Shape shape in shapesToUngroup)
            {
                ShapeList.Add(shape);
                Selection.Add(shape);
            }
        }


        public void RotateSelectedShapes(float angle)
        {
            if (Selection.Count != 0)
            {
                foreach (var shape in Selection)
                {
                    var type = shape.GetType().Name.ToString();
                    if (type.Equals("GroupShape"))
                    {
                        shape.ChangeGroupRotate(angle);
                    }
                    else
                    {
                        shape.ShapeAngle = angle;
                    }
                }
            }
        }
        public void ApplyGravityToSelectedShapes()
        {
            if (Selection.Count != 0)
            {
                foreach (var shape in Selection)
                {
                    shape.IsAffectedByGravity = true;
                }
            }

        }

        public void StopGravityToSelectedShapes()
        {
            if (Selection.Count != 0)
            {
                foreach (var shape in Selection)
                {
                    shape.IsAffectedByGravity = false;
                    shape.VerticalVelocity = 0;
                }
            }
        }

        /// <summary>
        /// Switches the current fill color to the next color in a predefined sequence
        /// </summary>
        public void SwitchFillColor()
        {
            if (currentFillColor == Color.Black)
                currentFillColor = Color.Red;
            else if (currentFillColor == Color.Red)
                currentFillColor = Color.Blue;
            else if (currentFillColor == Color.Blue)
                currentFillColor = Color.Green;
            else
                currentFillColor = Color.Black;
        }

        /// <summary>
        /// Switches the current border color to the next color in a predefined sequence
        /// </summary>
        public void SwitchBorderColor()
        {
            if (currentBorderColor == Color.Black)
                currentBorderColor = Color.Red;
            else if (currentBorderColor == Color.Red)
                currentBorderColor = Color.Blue;
            else if (currentBorderColor == Color.Blue)
                currentBorderColor = Color.Green;
            else
                currentBorderColor = Color.Black;
        }

        /// <summary>
        /// Switches the current border width between different predefined values
        /// </summary>
        public void SwitchBorderWidth()
        {
            if (currentWidth == 1)
                currentWidth = 2;
            else if (currentWidth == 2)
                currentWidth = 3;
            else if (currentWidth == 3)
                currentWidth = 5;
            else
                currentWidth = 1;
        }

        public void GravityUpdate(float deltaTime, float viewPortHeight)
        {
            if (Selection.Count != 0)
            {
                foreach (var shape in Selection)
                {
                    if (shape.IsAffectedByGravity)
                    {
                        //umnojavam gravitaciqta po 2 na 9ta stepen zashtoto inache figurite padat mnogo bavno
                        shape.VerticalVelocity += (GRAVITY * (float)Math.Pow(2, 12) * deltaTime);
                        shape.Location = new PointF(
                            shape.Location.X,
                            shape.Location.Y + (shape.VerticalVelocity * deltaTime)
                        );


                        if (shape.Location.Y + shape.Height >= viewPortHeight)
                        {
                            shape.VerticalVelocity = 0;
                            shape.IsAffectedByGravity = false;
                        }
                    }
                }
            }
        }
    }
}