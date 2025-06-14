using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Draw
{
    /// <summary>
    /// Върху главната форма е поставен потребителски контрол,
    /// в който се осъществява визуализацията
    /// </summary>
    public partial class MainForm : Form
    {


        /// <summary>
        /// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();

        // Form controls for rotation
        private Button entrRotateBtn;
        private const float deltaTime = 0.004f;
        private Timer MyTimer = new Timer();
        private TextBox RotatTextBox;
        private Label TextRotat;
        private Form RotatForm;

        private Form colorForm;
        private Form borderWidthForm;
        private ColorDialog fillColorDialog;
        private ColorDialog borderColorDialog;
        private TrackBar borderWidthTrackBar;
        private Label borderWidthLabel;
        private Button applyBorderWidthBtn;
        private Button cancelBorderWidthBtn;

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        /// <summary>
        /// Изход от програмата. Затваря главната форма, а с това и програмата.
        /// </summary>
        void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
        /// </summary>
        void ViewPortPaint(object sender, PaintEventArgs e)
        {
            dialogProcessor.ReDraw(sender, e);
        }

        /// <summary>
        /// Бутон, който поставя на произволно място правоъгълник със зададените размери.
        /// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
        /// </summary>
        void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

            viewPort.Invalidate();
        }

        /// <summary>
        /// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
        /// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
        /// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
        /// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
        /// </summary>
        void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pickUpSpeedButton.Checked)
            {
                Shape selectedShape = dialogProcessor.ContainsPoint(e.Location);
                if (selectedShape != null)
                {
                    dialogProcessor.ToggleShapeSelection(selectedShape);
                }

                if (dialogProcessor.Selection.Count > 0)
                {
                    statusBar.Items[0].Text = $"Селектирани: {dialogProcessor.Selection.Count} примитив(а)";
                }
                else
                {
                    statusBar.Items[0].Text = "Няма селектирани примитиви";
                }

                dialogProcessor.IsDragging = true;
                dialogProcessor.LastLocation = e.Location;
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на преместването на мишката.
        /// Ако сме в режим на "влачене", то всички избрани елементи се транслират.
        /// </summary>
        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dialogProcessor.IsDragging && dialogProcessor.Selection.Count > 0)
            {
                statusBar.Items[0].Text = $"Последно действие: Влачене на {dialogProcessor.Selection.Count} примитив(а)";

                dialogProcessor.TranslateTo(e.Location);
                viewPort.Invalidate();
            }
        }

        /// <summary>
        /// Прихващане на отпускането на бутона на мишката.
        /// Излизаме от режим "влачене".
        /// </summary>
        void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            dialogProcessor.IsDragging = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomElipse();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            dialogProcessor.GravityUpdate(deltaTime, viewPort.Height);
            viewPort.Invalidate();
        }

        private void viewPort_Load(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (Shape shape in dialogProcessor.Selection)
                {
                    shape.FillColor = colorDialog1.Color;
                }

                if (dialogProcessor.Selection.Count > 0)
                {
                    statusBar.Items[0].Text = $"Променен цвят на {dialogProcessor.Selection.Count} примитив(а)";
                }

                viewPort.Invalidate();
            }
        }

        private void speedMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            dialogProcessor.CreateGroup();
            statusBar.Items[0].Text = "Последно действие: Групиране на примитиви";
            viewPort.Invalidate();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            dialogProcessor.UnGroupSelectedShapes();
            statusBar.Items[0].Text = "Последно действие: Разгрупиране на примитиви";
            viewPort.Invalidate();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            RotateFormMethod(sender, e);
        }

        private void RotateFormMethod(object sender, EventArgs e)
        {
            RotatForm = new Form();

            RotatForm.Text = "Enter rotate degree: ";
            entrRotateBtn = new Button();
            Button cancelRotateBtn = new Button();
            RotatTextBox = new TextBox();
            TextRotat = new Label();
            TextRotat.Text = "Degree(1-1000): ";
            entrRotateBtn.Text = "Set Rotate Radius";
            cancelRotateBtn.Text = "Cancel";
            TextRotat.Location = new Point(90, 80);
            RotatTextBox.Location = new Point(TextRotat.Left, TextRotat.Height + TextRotat.Top + 10);
            RotatForm.Controls.Add(TextRotat);
            RotatForm.Controls.Add(RotatTextBox);
            entrRotateBtn.Location = new Point(RotatTextBox.Left, RotatTextBox.Height + RotatTextBox.Top + 10);
            cancelRotateBtn.Location = new Point(entrRotateBtn.Left, entrRotateBtn.Height + entrRotateBtn.Top + 10);
            RotatForm.AcceptButton = entrRotateBtn;
            RotatForm.CancelButton = cancelRotateBtn;
            RotatForm.Controls.Add(entrRotateBtn);
            entrRotateBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            RotatForm.Controls.Add(cancelRotateBtn);
            RotatForm.StartPosition = FormStartPosition.CenterScreen;
            RotatForm.ShowDialog();

            enterRotateBtn_Click(sender, e);
        }

        private void enterRotateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (RotatTextBox.Text == "")
                {
                    RotatForm.Close();
                }
                else if ((float.Parse(RotatTextBox.Text) < 1) || (float.Parse(RotatTextBox.Text) > 1000))
                {
                    string message = "Enter appropriate value(1-1000)!";
                    string caption = "Error Detected in Input";
                    MessageBoxButtons button = MessageBoxButtons.OK;
                    DialogResult result;

                    result = MessageBox.Show(message, caption, button);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {

                    }
                }
                else
                {
                    dialogProcessor.RotateSelectedShapes(float.Parse(RotatTextBox.Text));
                    statusBar.Items[0].Text = "Последно действие: Завъртане на фигура/фигури.";
                    viewPort.Invalidate();
                }
            }
            catch
            {
                RotatForm.Close();
            }
        }

        private void gravityButton(object sender, EventArgs e)
        {
            dialogProcessor.ApplyGravityToSelectedShapes();
            MyTimer.Interval = 4;
            MyTimer.Start();
            statusBar.Items[0].Text = "Последно действие: Вкючена гравитация";
            viewPort.Invalidate();
        }

        private void stopGravityButton(object sender, EventArgs e)
        {
            dialogProcessor.StopGravityToSelectedShapes();
            MyTimer.Stop();
            statusBar.Items[0].Text = "Последно действие: Изключена гравитация";
            viewPort.Invalidate();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomLine();

            statusBar.Items[0].Text = "Последно действие: Рисуване на линия";

            viewPort.Invalidate();
        }

        private void FillColorBtn_Click(object sender, EventArgs e)
        {
            if (fillColorDialog == null)
                fillColorDialog = new ColorDialog();

            if (fillColorDialog.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.currentFillColor = fillColorDialog.Color;
                if (dialogProcessor.Selection.Count > 0)
                {
                    foreach (Shape shape in dialogProcessor.Selection)
                    {
                        shape.FillColor = fillColorDialog.Color;
                    }
                }
                viewPort.Invalidate();
            }
        }

        private void BorderColorBtn_Click(object sender, EventArgs e)
        {
            if (borderColorDialog == null)
                borderColorDialog = new ColorDialog();

            if (borderColorDialog.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.currentBorderColor = borderColorDialog.Color;
                if (dialogProcessor.Selection.Count > 0)
                {
                    foreach (Shape shape in dialogProcessor.Selection)
                    {
                        shape.StrokeColor = borderColorDialog.Color;
                    }
                }
                viewPort.Invalidate();
            }
        }


        private void BorderWidthTrackBar_ValueChanged(object sender, EventArgs e)
        {
            borderWidthLabel.Text = $"Border Width: {borderWidthTrackBar.Value}";
        }

        private void ApplyBorderWidthBtn_Click(object sender, EventArgs e)
        {
            dialogProcessor.currentWidth = borderWidthTrackBar.Value;
            if (dialogProcessor.Selection.Count > 0)
            {
                foreach (Shape shape in dialogProcessor.Selection)
                {
                    shape.Borderwidth = borderWidthTrackBar.Value;
                }
            }
            viewPort.Invalidate();
            borderWidthForm.Close();
        }

        private void CancelBorderWidthBtn_Click(object sender, EventArgs e)
        {
            borderWidthForm.Close();
        }

        private void toolStripButton9_Click_1(object sender, EventArgs e)
        {
            borderWidthForm = new Form();
            borderWidthForm.Text = "Border Width Settings";
            borderWidthForm.Size = new Size(300, 200);
            borderWidthForm.StartPosition = FormStartPosition.CenterScreen;

            borderWidthLabel = new Label();
            borderWidthLabel.Text = "Border Width: 1";
            borderWidthLabel.Location = new Point(50, 20);

            borderWidthTrackBar = new TrackBar();
            borderWidthTrackBar.Minimum = 1;
            borderWidthTrackBar.Maximum = 10;
            borderWidthTrackBar.Value = 1;
            borderWidthTrackBar.Location = new Point(50, 50);
            borderWidthTrackBar.Width = 200;
            borderWidthTrackBar.ValueChanged += BorderWidthTrackBar_ValueChanged;

            applyBorderWidthBtn = new Button();
            applyBorderWidthBtn.Text = "Apply";
            applyBorderWidthBtn.Location = new Point(50, 100);
            applyBorderWidthBtn.Click += ApplyBorderWidthBtn_Click;

            cancelBorderWidthBtn = new Button();
            cancelBorderWidthBtn.Text = "Cancel";
            cancelBorderWidthBtn.Location = new Point(150, 100);
            cancelBorderWidthBtn.Click += CancelBorderWidthBtn_Click;

            borderWidthForm.Controls.Add(borderWidthLabel);
            borderWidthForm.Controls.Add(borderWidthTrackBar);
            borderWidthForm.Controls.Add(applyBorderWidthBtn);
            borderWidthForm.Controls.Add(cancelBorderWidthBtn);
            borderWidthForm.ShowDialog();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {

            dialogProcessor.AddRandomCustomShape();

            statusBar.Items[0].Text = "Последно действие: Рисуване на custom shape";

            viewPort.Invalidate();
        }
        private void toolStripButton12_Click_1(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomCustomShape7();
            statusBar.Items[0].Text = "Последно действие: Рисуване на custom shape 7";
            viewPort.Invalidate();
        }
    }
}