using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ES_Lib
{
    public partial class DrawHoffman : Form
    {
        public Histo root;
        public int DrawBlock = 0;
        public const int Radius = 30;
        private Graphics g;
        private Pen TransitionPen, StatePen;
        private int BlockCounter = 0;
        private int StateNumber = 1;
        private int[] GlobalHeightCounter;
        private int[] TempGlobalHeightCounter;
        private int maxwidth = 0;

        public DrawHoffman()
        {
            InitializeComponent();
        }

        private void DrawHoffman_Load(object sender, EventArgs e)
        {
            if (root != null)
            {
                int Width = CalculateWidth(root, 2);
                //Width -= 1;
                DrawBlock = panel1.Size.Width / Width;
                g = panel1.CreateGraphics();
                TransitionPen = new Pen(Color.White);
                StatePen = new Pen(Color.Red);
                
            }
            else
            {
                MessageBox.Show("There is No Root...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Draw(Histo Rroot, Histo root, int HeightCounter, int HeightBlock, int x, int y)
        {
            if (root != null && root.Key != "")
            {
                if (root.IsInitial)
                {
                    g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, panel1.Height / 2 - Radius, Radius * 2, Radius * 2);
                    g.DrawString(root.Kind, new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 20, panel1.Height / 2 - 10);
                    if (ShowValuesCB.Checked)
                        g.DrawString(root.Kind+","+root.Value, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 30, panel1.Height / 2 - 50);
                    BlockCounter++;
                    for (int i = 0; i < root.nextstates.Count; i++)
                    {
                        Histo Temp = (Histo)root.nextstates[i];
                        Draw(root, Temp, i, panel1.Size.Height / root.nextstates.Count, ((BlockCounter - 1) * DrawBlock + DrawBlock / 2) + Radius, panel1.Height / 2);
                    }
                }
                else
                {
                    g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, (HeightCounter * HeightBlock + HeightBlock / 2) - Radius, Radius * 2, Radius * 2);
                    g.DrawLine(TransitionPen, x, y, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, (HeightCounter * HeightBlock + HeightBlock / 2));

                    StateNumber++;
                    
                    g.DrawString(root.Kind, new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 20, (HeightCounter * HeightBlock + HeightBlock / 2) - 10);
                    int NewX = (((BlockCounter * DrawBlock + DrawBlock / 2) - Radius) - x) / 2 + x;
                    int OldX = NewX;
                    int NewY = y + ((HeightCounter * HeightBlock + HeightBlock / 2) - y) / 2;
                    if (y <= (HeightCounter * HeightBlock + HeightBlock / 2))
                    {
                        g.DrawString(root.Transition, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                        NewX -= 10;
                        if (NewX < x)
                        {
                            NewY += 20;
                            NewX = OldX;
                        }
                    }
                    else
                    {
                        g.DrawString(root.Transition, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, (y - (HeightCounter * HeightBlock + HeightBlock / 2)) / 2 + (HeightCounter * HeightBlock + HeightBlock / 2));
                        NewX += 10;
                    }
                    if (ShowValuesCB.Checked)
                        g.DrawString(root.Key+","+root.Value, new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 30, (HeightCounter * HeightBlock + HeightBlock / 2) - 50);
                    if (root.nextstates.Count != 0)
                    {
                        BlockCounter++;
                        //GlobalHeightCounter[BlockCounter] = 0;
                        for (int i = 0; i < root.nextstates.Count; i++)
                        {
                            Histo Temp = (Histo)root.nextstates[i];
                            int NextLevel = 0;
                            for (int j = 0; j < Rroot.nextstates.Count; j++)
                            {
                                NextLevel += ((Histo)Rroot.nextstates[j]).nextstates.Count;
                            }
                            Draw(root, Temp, TempGlobalHeightCounter[BlockCounter], panel1.Size.Height / GlobalHeightCounter[BlockCounter], ((BlockCounter - 1) * DrawBlock + DrawBlock / 2) + Radius, (HeightCounter * HeightBlock + HeightBlock / 2));
                            TempGlobalHeightCounter[BlockCounter]++;

                        }
                        BlockCounter--;
                    }
                }
            }
        }

        private int CalculateWidth(Histo root, int width)
        {
            try
            {
                int NewWidth = width;
                int Temp = 0;
                if (root != null && root.nextstates.Count != 0)
                    NewWidth++;
                for (int i = 0; i < root.nextstates.Count; i++)
                {
                    Histo Te = (Histo)root.nextstates[i];
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

        private void CalculateSegments(Histo root, int StartIndex)
        {
            if (root != null)
            {
                GlobalHeightCounter[StartIndex]++;
                for (int i = 0; i < root.nextstates.Count; i++)
                {
                    CalculateSegments((Histo)root.nextstates[i], StartIndex + 1);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
