﻿using System;
using System.Windows.Forms;

/**********************************************************************************
* Mark off what items are complete (e.g. x, done, checkmark, etc), and put a P if 
* partially complete. If 'P' include how to test what is working for partial credit 
* below the checklist line.
*
* Total available points:  100
* ✓______	25	Tutorial completed (if not, what was the last section completed)
* ✓______	10	My Favorite Color
* ✓______	5	Horizontal Gradient Image
* ✓______	5	Vertical Gradient Image
* ✓______	10	Diagonal Gradient Image
* ✓______	5	Horizontal Line
* ✓______	5	Vertical Wider Line
* ✓______	10	Diagonal Line
* ✓______	10	Monochrome Image Filter
* ✓______	15	Median Filter
* ______	Total (please add the points and include the total here)
* 
* The grade you compute is the starting point for course staff, who reserve the 
* right to change the grade if they disagree with your assessment and to deduct 
* points for other issues they may encounter, such as errors in the submission 
* process, naming issues, etc.
*
**********************************************************************************/

namespace ImageProcess
{
    public partial class MainForm : Form
    {
        enum ModelType { None, Generate, Process}; //mode for menu enabling

        Image model;
        ImageEditor editor;


        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true; //stop flicker

            SetMenuOptionEnable(ModelType.None);
        }

        //main Paint function
        protected override void OnPaint(PaintEventArgs e)
        {
            if (model == null)
                return;

            model.OnPaint(e);
        }

        //redraw on resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }


        //
        //forward mouse calls
        //

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (model == null)
                return;

            base.OnMouseDown(e);
            editor.MousePress(model, e.X, e.Y);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (model == null)
                return;

            base.OnMouseUp(e);
            editor.MouseRelease();
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (model == null)
                return;

            base.OnMouseMove(e);
            editor.MouseMove(model, e.X, e.Y);
            Invalidate();
        }


        //
        //forward menu calls-------------------------------------------------------------------------
        //
        private void newMenu_Click(object sender, EventArgs e)
        {
            editor = new ImageGenerate();
            model = new Image(menuStrip1.Height);
            ImageGenerate.FillBlack(model);
            SetMenuOptionEnable(ModelType.Generate);
            Invalidate();
        }

        private void drawMenu_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.Draw);

            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;
         
            Invalidate();
        }

        private void copyMenu_Click(object sender, EventArgs e)
        {
            model.Reset();

            Invalidate();
        }

        private void negativeMenu_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.None);
            ImageProcess.OnFilterNegative(model);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;

            Invalidate();
        }

        private void thresholdMenu_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.Threshold);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;
        }

        private void warpMenu_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.Warp);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;
        }

        private void warpBilinearMenu_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.WarpNearest);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;
        }

        private void fillWhiteMenu_Click(object sender, EventArgs e)
        {
            ImageGenerate.FillWhite(model);
        }

        private void openTestMenu_Click(object sender, EventArgs e)
        {
            model = new Image(menuStrip1.Height);
            model.OnOpenDocument(null);
            editor = new ImageProcess();
            SetMenuOptionEnable(ModelType.Process);
            Invalidate();
        }

        private void openMenu_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                model = new Image(menuStrip1.Height);
                model.OnOpenDocument(openFileDialog1.FileName);
                editor = new ImageProcess();
                SetMenuOptionEnable(ModelType.Process);
                Invalidate();
            }
        }

        private void closeMenu_Click(object sender, EventArgs e)
        {
            model.Close();
            model = null;
            SetMenuOptionEnable(ModelType.None);
            Invalidate();
        }

        private void exitMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveMenu_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                model.OnSaveDocument(saveFileDialog1.FileName, saveFileDialog1.FilterIndex);
            }
        }

        private void SetMenuOptionEnable(ModelType mode)
        {
            bool on = true;
            switch(mode)
            {
                case ModelType.None:
                    on = false;
                    fillWhiteMenu.Enabled = on;
                    fillLavenderToolStripMenuItem.Enabled = on;
                    fillGreenToolStripMenuItem.Enabled = on;
                    horizontalGradientToolStripMenuItem.Enabled = on;
                    verticalBlueGradientToolStripMenuItem.Enabled = on;
                    diagonalGradientToolStripMenuItem.Enabled = on;
                    verticalLineToolStripMenuItem.Enabled = on;
                    horizontalLineToolStripMenuItem.Enabled = on;
                    diagonalLineToolStripMenuItem.Enabled = on;
                    copyMenu.Enabled = on;
                    negativeMenu.Enabled = on;
                    thresholdMenu.Enabled = on;
                    dimToolStripMenuItem.Enabled = on;
                    tintToolStripMenuItem.Enabled = on;
                    lowpassFilterToolStripMenuItem.Enabled = on;
                    monochromeToolStripMenuItem.Enabled = on;
                    medianFilterToolStripMenuItem.Enabled = on;
                    warpMenu.Enabled = on;
                    warpBilinearMenu.Enabled = on;
                    drawMenu.Enabled = on;
                    drawMenu.Checked = false;
                    break;
                case ModelType.Generate:
                    drawMenu.Enabled = true;

                    fillWhiteMenu.Enabled = on;
                    fillLavenderToolStripMenuItem.Enabled = on;
                    fillGreenToolStripMenuItem.Enabled = on;
                    horizontalGradientToolStripMenuItem.Enabled = on;
                    verticalBlueGradientToolStripMenuItem.Enabled = on;
                    diagonalGradientToolStripMenuItem.Enabled = on;
                    verticalLineToolStripMenuItem.Enabled = on;
                    horizontalLineToolStripMenuItem.Enabled = on;
                    diagonalLineToolStripMenuItem.Enabled = on;
                    copyMenu.Enabled = !on;
                    negativeMenu.Enabled = !on;
                    thresholdMenu.Enabled = !on;
                    dimToolStripMenuItem.Enabled = !on;
                    tintToolStripMenuItem.Enabled = !on;
                    lowpassFilterToolStripMenuItem.Enabled = !on;
                    monochromeToolStripMenuItem.Enabled = !on;
                    medianFilterToolStripMenuItem.Enabled = !on;
                    warpMenu.Enabled = !on;
                    warpBilinearMenu.Enabled = !on;
                    
                    break;
                case ModelType.Process:
                    drawMenu.Enabled = true;

                    on = !on;
                    fillWhiteMenu.Enabled = on;
                    fillGreenToolStripMenuItem.Enabled = on;
                    fillLavenderToolStripMenuItem.Enabled = on;
                    horizontalGradientToolStripMenuItem.Enabled = on;
                    verticalBlueGradientToolStripMenuItem.Enabled = on;
                    diagonalGradientToolStripMenuItem.Enabled = on;
                    verticalLineToolStripMenuItem.Enabled = on;
                    horizontalLineToolStripMenuItem.Enabled = on;
                    diagonalLineToolStripMenuItem.Enabled = on;
                    copyMenu.Enabled = !on;
                    negativeMenu.Enabled = !on;
                    thresholdMenu.Enabled = !on;
                    dimToolStripMenuItem.Enabled = !on;
                    tintToolStripMenuItem.Enabled = !on;
                    lowpassFilterToolStripMenuItem.Enabled = !on;
                    monochromeToolStripMenuItem.Enabled = !on;
                    medianFilterToolStripMenuItem.Enabled = !on;
                    warpMenu.Enabled = !on;
                    warpBilinearMenu.Enabled = !on;
                    break;
            }    
        }

        private void fillGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.FillGreen(model);
        }

        private void dimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.None);
            ImageProcess.OnFilterDim(model);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;

            Invalidate();
        }

        private void tintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.None);
            ImageProcess.OnFilterTint(model);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;

            Invalidate();
        }

        private void lowpassFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.None);
            ImageProcess.OnFilterLowpass(model);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;

            Invalidate();
        }

        private void fillLavenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.FillLavender(model);
        }

        private void horizontalGradientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.HorizontalGradient(model);
        }

        private void verticalBlueGradientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.VerticalBlueGradient(model);
        }

        private void diagonalGradientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.DiagonalGradient(model);
        }

        private void horizontalLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.HorizontalLine(model);
        }

        private void verticalLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.VerticalLine(model);
        }

        private void diagonalLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGenerate.DiagonalLine(model);
        }

        private void medianFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.None);
            ImageProcess.OnMedianFilter(model);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;

            Invalidate();
        }

        private void monochromeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SetMode(ImageEditor.MODE.None);
            ImageProcess.OnFilterMonochrome(model);
            drawMenu.Checked = editor.MouseMode == ImageEditor.MODE.Draw;

            Invalidate();
        }
    }
}
