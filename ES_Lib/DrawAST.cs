using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ES_Lib
{
    public partial class DrawAST : Form
    {
        public Node root;
        public int DrawBlock = 0;
        public const int Radius = 30;
        private Graphics g;
        private Pen TransitionPen, StatePen;
        private int BlockCounter = 0;
        private int StateNumber = 1;
        private int[] GlobalHeightCounter;
        private int[] TempGlobalHeightCounter;
        private int maxwidth = 0;


        public DrawAST()
        {
            InitializeComponent();
        }

        
        private void DrawAST_Load(object sender, EventArgs e)
        {
            if (root != null)
            {
                int Width = CalculateWidth(root, 2);
                //Width -= 1;
                DrawBlock = panel1.Size.Width / Width;
                g = panel1.CreateGraphics();
                TransitionPen = new Pen(Color.White);
                StatePen = new Pen(Color.Red);
                //Project.SetDrawingWindow(panel1.Handle);
                //Project.Draw();
                timer1.Start();
            }
            else
            {
                MessageBox.Show("There is No Root...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DrawOnExternalPanel(Panel P)
        {
            if (root != null && !P.IsDisposed)
            {
                int Width = CalculateWidth(root, 2);
                DrawBlock = P.Size.Width / Width;
                Width--;
                GlobalHeightCounter = new int[25];
                TempGlobalHeightCounter = new int[25];
                CalculateSegments(root, 0);
                //Width -= 1;
                StateNumber = 1;
                BlockCounter = 0;
                g = P.CreateGraphics();
                TransitionPen = new Pen(Color.White);
                StatePen = new Pen(Color.Red);
                Draw(null, root, 0, 0, 0, 0);
            }
            else
            {
                MessageBox.Show("There is No Root...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Draw(Node Rroot, Node root, int HeightCounter, int HeightBlock, int x, int y)
        {
            if (root != null && root.NodeKind != "")
            {
                if (root.isInitial)
                {
                    g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, g.VisibleClipBounds.Height / 2 - Radius, Radius * 2, Radius * 2);
                    string Data = "";
                    if (root.NodeKind.Length > 4)
                    {
                        Data = "  " + root.NodeKind.Substring(0, 4);
                        if (!ShortCutslbl.Text.Contains(Data))
                            ShortCutslbl.Text += Data + " : " + root.NodeKind + " ,";
                    }
                    else
                        Data = root.NodeKind;
                    g.DrawString(Data, new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 20,(int) g.VisibleClipBounds.Height / 2 - 10);
                    if (ShowValuesCB.Checked)
                        g.DrawString(root.Value, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 30,(int) g.VisibleClipBounds.Height / 2 - 50);
                    BlockCounter++;
                    for (int i = 0; i < root.nextstates.Count; i++)
                    {
                        Node Temp = (Node)root.nextstates[i];
                        Draw(root, Temp, i, (int) g.VisibleClipBounds.Height / root.nextstates.Count, ((BlockCounter - 1) * DrawBlock + DrawBlock / 2) + Radius, (int)g.VisibleClipBounds.Height / 2);
                    }
                }
                else
                {
                    g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, (HeightCounter * HeightBlock + HeightBlock / 2) - Radius, Radius * 2, Radius * 2);
                    g.DrawLine(TransitionPen, x, y, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, (HeightCounter * HeightBlock + HeightBlock / 2));

                    StateNumber++;
                    string Data = "";
                    if (root.NodeKind.Length > 4)
                    {
                        Data = "  " + root.NodeKind.Substring(0, 4);
                        if (!ShortCutslbl.Text.Contains(Data))
                            ShortCutslbl.Text += Data + " : " + root.NodeKind + " ,";
                    }
                    else
                        Data = root.NodeKind;
                    g.DrawString(Data, new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 20, (HeightCounter * HeightBlock + HeightBlock / 2) - 10);
                    if (ShowValuesCB.Checked)
                        g.DrawString(root.Value, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 30, (HeightCounter * HeightBlock + HeightBlock / 2) - 50);
                    if (root.nextstates.Count != 0)
                    {
                        BlockCounter++;
                        //GlobalHeightCounter[BlockCounter] = 0;
                        for (int i = 0; i < root.nextstates.Count; i++)
                        {
                            Node Temp = (Node)root.nextstates[i];
                            int NextLevel = 0;
                            for (int j = 0; j < Rroot.nextstates.Count; j++)
                            {
                                NextLevel += ((Node)Rroot.nextstates[j]).nextstates.Count;
                            }
                            Draw(root, Temp, TempGlobalHeightCounter[BlockCounter], (int)g.VisibleClipBounds.Height / GlobalHeightCounter[BlockCounter], ((BlockCounter - 1) * DrawBlock + DrawBlock / 2) + Radius, (HeightCounter * HeightBlock + HeightBlock / 2));
                            TempGlobalHeightCounter[BlockCounter]++;

                        }
                        BlockCounter--;
                    }
                }
            }
        }

        private int CalculateWidth(Node root, int width)
        {
            try
            {
                int NewWidth = width;
                int Temp = 0;
                if (root != null && root.nextstates.Count != 0)
                    NewWidth++;
                for (int i = 0; i < root.nextstates.Count; i++)
                {
                    Node Te = (Node)root.nextstates[i];
                    Temp = CalculateWidth(Te, NewWidth);
                }
                if (Temp > NewWidth && Temp > maxwidth)
                {
                    maxwidth = Temp;
                    return Temp;
                }
                else
                {
                    if (NewWidth > maxwidth)
                    {
                        maxwidth = NewWidth;
                        return NewWidth;
                    }
                    else
                        return maxwidth;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void CalculateSegments(Node root, int StartIndex)
        {
            if (root != null)
            {
                GlobalHeightCounter[StartIndex]++;
                for (int i = 0; i < root.nextstates.Count; i++)
                {
                    CalculateSegments((Node)root.nextstates[i], StartIndex + 1);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (root != null)
            {
                g.Clear(Color.Black);
                int Width = CalculateWidth(root, 2);
                Width--;
                GlobalHeightCounter = new int[25];
                TempGlobalHeightCounter = new int[25];
                CalculateSegments(root, 0);
                //Width -= 1;
                StateNumber = 1;
                BlockCounter = 0;
                DrawBlock = panel1.Size.Width / Width;
                g = panel1.CreateGraphics();
                TransitionPen = new Pen(Color.White);
                StatePen = new Pen(Color.Red);
                Draw(null, root, 0, 0, 0, 0);
            }
            else
            {
                MessageBox.Show("There is No Root...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void DrawAST_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Project.DisposeRendering();
        }

        private void DrawAST_KeyUp(object sender, KeyEventArgs e)
        {
            ShortCutslbl.Text = Project.GInterface.Lookatz.ToString()+" ";
            ShortCutslbl.Text += Project.GInterface.Lookatx.ToString()+" ";
            ShortCutslbl.Text += Project.GInterface.Lookaty.ToString();
            if (e.KeyCode == Keys.D)
                Project.GInterface.Lookatz -= 0.5;
            if(e.KeyCode==Keys.A)
                Project.GInterface.Lookatz += 0.5;
            if (e.KeyCode == Keys.S)
                Project.GInterface.Lookatx -= 0.5;
            if (e.KeyCode == Keys.W)
                Project.GInterface.Lookatx += 0.5;
            if (e.KeyCode == Keys.Up)
                Project.GInterface.TransX += 0.5;
            if (e.KeyCode == Keys.Down)
                Project.GInterface.TransX -= 0.5;
            if (e.KeyCode == Keys.Z)
                Project.GInterface.TransZ += 0.5;
            if (e.KeyCode == Keys.X)
                Project.GInterface.TransZ -= 0.5;
        }

        private void DrawAST_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void ShowValuesCB_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
