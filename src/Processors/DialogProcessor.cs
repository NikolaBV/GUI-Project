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
            CustomShape1 customShape = new CustomShape1(new Rectangle(x, y, 120, 80));
            customShape.FillColor = currentFillColor;
            customShape.StrokeColor = currentBorderColor;
            customShape.Borderwidth = currentWidth;
            ShapeList.Add(customShape);
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
        public void AddRandomCustomShape0()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape0 customShape0 = new CustomShape0(new Rectangle(x, y, 100, 100));
            customShape0.FillColor = currentFillColor;
            customShape0.StrokeColor = currentBorderColor;
            customShape0.Borderwidth = currentWidth;
            ShapeList.Add(customShape0);
        }


        /// <summary>
        /// Добавя примитив - CustomShape2 на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomCustomShape2()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape2 customShape2 = new CustomShape2(new Rectangle(x, y, 100, 100));
            customShape2.FillColor = currentFillColor;
            customShape2.StrokeColor = currentBorderColor;
            customShape2.Borderwidth = currentWidth;
            ShapeList.Add(customShape2);
        }

        /// <summary>
        /// Добавя примитив - CustomShape3 на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomCustomShape3()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape3 customShape3 = new CustomShape3(new Rectangle(x, y, 100, 100));
            customShape3.FillColor = currentFillColor;
            customShape3.StrokeColor = currentBorderColor;
            customShape3.Borderwidth = currentWidth;
            ShapeList.Add(customShape3);
        }
        public void AddRandomCustomShape4()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape4 customShape4 = new CustomShape4(new Rectangle(x, y, 120, 80));
            customShape4.FillColor = currentFillColor;
            customShape4.StrokeColor = currentBorderColor;
            customShape4.Borderwidth = currentWidth;
            ShapeList.Add(customShape4);
        }

        public void AddRandomCustomShape5()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape5 customShape5 = new CustomShape5(new Rectangle(x, y, 100, 100));
            customShape5.FillColor = currentFillColor;
            customShape5.StrokeColor = currentBorderColor;
            customShape5.Borderwidth = currentWidth;
            ShapeList.Add(customShape5);
        }

        public void AddRandomCustomShape6()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape6 customShape6 = new CustomShape6(new Rectangle(x, y, 120, 80));
            customShape6.FillColor = currentFillColor;
            customShape6.StrokeColor = currentBorderColor;
            customShape6.Borderwidth = currentWidth;
            ShapeList.Add(customShape6);
        }

        public void AddRandomCustomShape8()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape8 customShape8 = new CustomShape8(new Rectangle(x, y, 100, 100));
            customShape8.FillColor = currentFillColor;
            customShape8.StrokeColor = currentBorderColor;
            customShape8.Borderwidth = currentWidth;
            ShapeList.Add(customShape8);
        }

        public void AddRandomCustomShape9()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            CustomShape9 customShape9 = new CustomShape9(new Rectangle(x, y, 100, 100));
            customShape9.FillColor = currentFillColor;
            customShape9.StrokeColor = currentBorderColor;
            customShape9.Borderwidth = currentWidth;
            ShapeList.Add(customShape9);
        }

        public void AddPracticeShape() {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);
            PracticeShape customShape9 = new PracticeShape(new Rectangle(x, y, 100, 100));
            customShape9.FillColor = currentFillColor;
            customShape9.StrokeColor = currentBorderColor;
            customShape9.Borderwidth = currentWidth;
            ShapeList.Add(customShape9);

        }

    }
}