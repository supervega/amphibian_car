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
    public partial class DrawDFA : Form
    {
        public State root;
        public int DrawBlock = 0;
        public int Radius = 35;
        private Graphics g;
        private Pen TransitionPen,StatePen;
        private int BlockCounter = 0;
        private int StateNumber = 1;
        private int[] GlobalHeightCounter;
        private int[] TempGlobalHeightCounter;
        private int maxwidth = 0;
     
        public DrawDFA()
        {
            InitializeComponent();
        }

        private void DrawDFA_Load(object sender, EventArgs e)
        {
            if (root != null)
            {
                int Width = CalculateWidth(root, 2);
                //Width -= 1;
                DrawBlock = panel1.Size.Width / Width;
                g = panel1.CreateGraphics();
                TransitionPen = new Pen(Color.White);
                StatePen = new Pen(Color.Red);
                StatePen.Width=4;
         
            }
            else
            {
                MessageBox.Show("There is No Root...","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void Draw(State Rroot,State root,int HeightCounter,int HeightBlock,int x,int y,ArrayList TransitionsChars,ArrayList LoopChars)
        {
            if (root != null)
            {                
                if (root.IS_Initial)
                {
                    g.DrawLine(TransitionPen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius - 20, panel1.Height / 2, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, panel1.Height / 2);
                    g.DrawLine(TransitionPen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius - 10, panel1.Height / 2 - 10, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, panel1.Height / 2);
                    g.DrawLine(TransitionPen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius - 10, panel1.Height / 2 + 10, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, panel1.Height / 2);
                    g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, panel1.Height / 2 - Radius, Radius * 2, Radius * 2);
                    g.DrawString(root.Name, new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 20, panel1.Height / 2 - 10);
                    if (root.IS_Accepted)
                    {
                        g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius + 10, panel1.Height / 2 - Radius + 10, Radius * 2 - 20, Radius * 2 - 20);
                    }
                    if (root.IS_Loop)
                    {
                        int LoopX = (BlockCounter * DrawBlock + DrawBlock / 2);
                        int LoopY = panel1.Height / 2 - Radius;
                        g.DrawBezier(TransitionPen, new Point(LoopX, LoopY), new Point(LoopX-25, LoopY-25), new Point(LoopX+25, LoopY-25), new Point(LoopX, LoopY));
                        TransitionPen.Width = 3;
                        g.DrawLine(TransitionPen, (BlockCounter * DrawBlock + DrawBlock / 2) , panel1.Height / 2 -Radius, (BlockCounter * DrawBlock + DrawBlock / 2) + 3, panel1.Height / 2 -Radius -10);
                        g.DrawLine(TransitionPen, (BlockCounter * DrawBlock + DrawBlock / 2), panel1.Height / 2 - Radius, (BlockCounter * DrawBlock + DrawBlock / 2) + 10, panel1.Height / 2 - Radius - 5);
                        TransitionPen.Width = 1;

                        int NewX = (BlockCounter * DrawBlock + DrawBlock / 2)+20;
                        int OldX = NewX;
                        int NewY = panel1.Height / 2 - Radius-35;
                        if (LoopChars.Count <= 3)
                        {
                            for (int i = 0; i < LoopChars.Count; i++)
                            {
                                if (y <= (HeightCounter * HeightBlock + HeightBlock / 2))
                                {
                                    g.DrawString(LoopChars[LoopChars.Count - i - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                                    NewX -= 10;
                                    if (NewX < x)
                                    {
                                        NewY += 20;
                                        NewX = OldX;
                                    }
                                }
                                else
                                {
                                    g.DrawString(LoopChars[LoopChars.Count - i - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, (y - (HeightCounter * HeightBlock + HeightBlock / 2)) / 2 + (HeightCounter * HeightBlock + HeightBlock / 2));
                                    NewX += 10;
                                }
                            }
                        }
                        else
                        {
                            g.DrawString(LoopChars[0].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                            NewX += 10;
                            g.DrawString("-", new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                            NewX += 10;
                            g.DrawString(LoopChars[LoopChars.Count - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                        }
                    }
                    BlockCounter++;
                    for (int i = 0; i < root.nextstates.Count; i++)
                    {
                        State Temp = ((State.Transition)root.nextstates[i]).state;
                        Draw(root,Temp, i, panel1.Size.Height / root.nextstates.Count, ((BlockCounter - 1) * DrawBlock + DrawBlock / 2) + Radius, panel1.Height / 2, ((State.Transition)root.nextstates[i]).TransitionChars,((State.Transition)root.nextstates[i]).state.LoopNextStates);
                    }
                }
                else
                {
                    g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, (HeightCounter * HeightBlock + HeightBlock / 2) - Radius, Radius * 2, Radius * 2);
                    g.DrawLine(TransitionPen, x, y, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius, (HeightCounter * HeightBlock + HeightBlock / 2));
                    int NewX = (((BlockCounter * DrawBlock + DrawBlock / 2) - Radius) - x) / 2 + x;
                    int OldX=NewX;
                    int NewY=y + ((HeightCounter * HeightBlock + HeightBlock / 2) - y) / 2;
                    if (TransitionsChars.Count <= 3)
                    {
                        for (int i = 0; i < TransitionsChars.Count; i++)
                        {
                            if (y <= (HeightCounter * HeightBlock + HeightBlock / 2))
                            {
                                g.DrawString(TransitionsChars[TransitionsChars.Count - i - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                                NewX -= 10;
                                if (NewX < x)
                                {
                                    NewY += 20;
                                    NewX = OldX;
                                }
                            }
                            else
                            {
                                g.DrawString(TransitionsChars[TransitionsChars.Count - i - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, (y - (HeightCounter * HeightBlock + HeightBlock / 2)) / 2 + (HeightCounter * HeightBlock + HeightBlock / 2));
                                NewX += 10;
                            }
                        }
                    }
                    else
                    {
                        g.DrawString(TransitionsChars[0].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                        NewX += 10;
                        g.DrawString("-", new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                        NewX += 10;
                        g.DrawString(TransitionsChars[TransitionsChars.Count - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                    }
                    root.Name = "q" + StateNumber;
                    StateNumber++;
                    g.DrawString(root.Name, new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold), StatePen.Brush, (BlockCounter * DrawBlock + DrawBlock / 2) - 20, (HeightCounter * HeightBlock + HeightBlock / 2) - 10);
                    if (root.IS_Accepted)
                    {
                        g.DrawEllipse(StatePen, (BlockCounter * DrawBlock + DrawBlock / 2) - Radius + 10, (HeightCounter * HeightBlock + HeightBlock / 2) - Radius + 10, Radius * 2-20, Radius * 2-20);
                    }
                    if (root.IS_Loop)
                    {
                        int LoopX = (BlockCounter * DrawBlock + DrawBlock / 2);
                        int LoopY = panel1.Height / 2 - Radius;
                        g.DrawBezier(TransitionPen, new Point(LoopX, LoopY), new Point(LoopX - 25, LoopY - 25), new Point(LoopX + 25, LoopY - 25), new Point(LoopX, LoopY));
                        TransitionPen.Width = 3;
                        g.DrawLine(TransitionPen, (BlockCounter * DrawBlock + DrawBlock / 2), panel1.Height / 2 - Radius, (BlockCounter * DrawBlock + DrawBlock / 2) + 3, panel1.Height / 2 - Radius - 10);
                        g.DrawLine(TransitionPen, (BlockCounter * DrawBlock + DrawBlock / 2), panel1.Height / 2 - Radius, (BlockCounter * DrawBlock + DrawBlock / 2) + 10, panel1.Height / 2 - Radius - 5);
                        TransitionPen.Width = 1;

                        NewX = (BlockCounter * DrawBlock + DrawBlock / 2) + 20;
                        OldX = NewX;
                        NewY = panel1.Height / 2 - Radius - 35;
                        if (LoopChars.Count <= 3)
                        {
                            for (int i = 0; i < LoopChars.Count; i++)
                            {
                                if (y <= (HeightCounter * HeightBlock + HeightBlock / 2))
                                {
                                    g.DrawString(LoopChars[LoopChars.Count - i - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                                    NewX -= 10;
                                    if (NewX < x)
                                    {
                                        NewY += 20;
                                        NewX = OldX;
                                    }
                                }
                                else
                                {
                                    g.DrawString(LoopChars[LoopChars.Count - i - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, (y - (HeightCounter * HeightBlock + HeightBlock / 2)) / 2 + (HeightCounter * HeightBlock + HeightBlock / 2));
                                    NewX += 10;
                                }
                            }
                        }
                        else
                        {
                            g.DrawString(LoopChars[0].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                            NewX += 10;
                            g.DrawString("-", new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                            NewX += 10;
                            g.DrawString(LoopChars[TransitionsChars.Count - 1].ToString(), new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), StatePen.Brush, NewX, NewY);
                        }
                    }
                    if (root.nextstates.Count != 0)
                    {
                        BlockCounter++;
                        //GlobalHeightCounter[BlockCounter] = 0;
                        for (int i = 0; i < root.nextstates.Count; i++)
                        {
                            State Temp = ((State.Transition)root.nextstates[i]).state;
                            int NextLevel = 0;
                            for (int j = 0; j < Rroot.nextstates.Count; j++)
                            {
                                NextLevel += ((State.Transition)Rroot.nextstates[j]).state.nextstates.Count;
                            }                            
                            Draw(root, Temp, TempGlobalHeightCounter[BlockCounter], panel1.Size.Height / GlobalHeightCounter[BlockCounter], ((BlockCounter - 1) * DrawBlock + DrawBlock / 2) + Radius, (HeightCounter * HeightBlock + HeightBlock / 2), ((State.Transition)root.nextstates[i]).TransitionChars,((State.Transition)root.nextstates[i]).state.LoopNextStates);
                            TempGlobalHeightCounter[BlockCounter]++;
                          
                        }
                        BlockCounter--;
                    }
                    else
                    {
                        ;
                    }
                }
            }
        }
        
        private int CalculateWidth(State root,int width)
        {
            int NewWidth = width;
            int Temp=0;
            if (root!=null && root.nextstates.Count != 0)
                NewWidth++;
            for (int i = 0; i < root.nextstates.Count; i++)
            {
                State Te = ((State.Transition)root.nextstates[i]).state;
                Temp = CalculateWidth(Te,NewWidth);                
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

        private void CalculateSegments(State root,int StartIndex)
        {
            if (root != null)
            {
                GlobalHeightCounter[StartIndex]++;
                for (int i = 0; i < root.nextstates.Count; i++)
                {
                    CalculateSegments(((State.Transition)root.nextstates[i]).state, StartIndex+1);                   
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (root != null)
            {
                Radius = (int)NodeSizeNUD.Value;
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
                Draw(null, root, 0, 0, 0, 0, null, root.LoopNextStates);
            }
            else
            {
                MessageBox.Show("There is No Root...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void NodeSizeNUD_ValueChanged(object sender, EventArgs e)
        {
            if (root != null)
            {
                Radius = (int)NodeSizeNUD.Value;
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
                Draw(null, root, 0, 0, 0, 0, null, root.LoopNextStates);
            }
            else
            {
                MessageBox.Show("There is No Root...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
