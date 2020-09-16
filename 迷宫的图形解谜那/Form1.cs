using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace 迷宫的图形解谜那
{

    public partial class 简易迷宫 : Form
    {
        public static int  start_choice;

        Button[,] maze = new Button[15, 15];
        Button[] menu = new Button[5];
        Font btnFont = new Font("楷体",18,FontStyle.Bold);
        int[,] initBoard;
        int x, y;
        int startflag = 0;
        int num = 0;                                        //防止出现两次键盘响应事件
        int[] cmaze;                                        //保存迷宫
        int[] search_maze;
        int[] blank = new int[225];                         //清屏
        bool solve = false;
        bool creat_maze = false;

        [DllImport("sub.dll")]
        public static extern void creat(int[] maze);
        [DllImport("sub.dll")]
        public static extern void dfs_solve(int[] maze);


        public 简易迷宫()
        {
            InitializeComponent();
            cmaze = new int[225];
            search_maze = new int[225];
            this.KeyDown += Form1_KeyDown;
            //设置控件尺寸
            Size bs = new Size(40, 40);
            Size menuSize = new Size(160, 80);
            Size mainSize = new Size(1024, 768);
            //设置主窗口属性
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = mainSize;
            //  this.BackColor = ColorTranslator.FromHtml("#")
            //添加迷宫格子
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    maze[i, j] = new Button();
                    maze[i, j].Size = bs;                                               //按钮尺寸
                    maze[i, j].Text = "";                                               //文本内容设置为空
                    maze[i, j].BackColor = ColorTranslator.FromHtml("#958e96");          //颜色    
                    maze[i, j].Visible = true;                                          //可见
                    maze[i, j].Location = new Point(50 + 40 * i, 100 + 40 * j);         //位置  
                    maze[i, j].Name = (15*j+i).ToString();
                    this.Controls.Add(maze[i, j]);
                    maze[i, j].Enabled = false;        
                    maze[i, j].Click += mazeButton_Click;
                }
            }


            //添加五个菜单选项
            for (int i = 0; i < 5; i++)
            {
                menu[i] = new Button();
                menu[i].Size = menuSize;
                menu[i].BackColor = ColorTranslator.FromHtml("#929077");
                menu[i].Visible = true;
                menu[i].Location = new Point(750, 100 + 130 * i);
                this.Controls.Add(menu[i]);
                menu[i].Font = btnFont;
            }
            menu[0].Text = "开始游戏";
            menu[1].Text = "自动寻路";
            menu[2].Text = "生成迷宫";
            menu[3].Text = "自定义迷宫";
            menu[4].Text = "游戏说明";

            menu[1].Enabled = false;
            menu[2].Enabled = false;
            menu[3].Enabled = false;

            menu[0].Click += start_Click;
            menu[1].Click += solve1_Click;
            menu[2].Click += creat_Click;
            menu[3].Click += diy_click;
        }
        //x为水平方向 y为竖直方向
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (num % 2 == 0)
            {
                if (startflag == 1)
                {
                    if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)          //向下
                    {
                        if (y != 14 && maze[x, y + 1].BackColor != ColorTranslator.FromHtml("#3c371e"))
                        {
                            maze[x, y++].BackColor = ColorTranslator.FromHtml("#929077");
                            maze[x, y].BackColor = ColorTranslator.FromHtml("#6d9e0e");
                        }
                    }
                    else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)     //向左
                    {
                        if (x != 0 && maze[x - 1, y].BackColor != ColorTranslator.FromHtml("#3c371e"))
                        {
                            maze[x--, y].BackColor = ColorTranslator.FromHtml("#929077");
                            maze[x, y].BackColor = ColorTranslator.FromHtml("#6d9e0e");
                        }
                    }
                    else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Left)      //向上
                    {
                        if (y != 0 && maze[x, y - 1].BackColor != ColorTranslator.FromHtml("#3c371e"))
                        {
                            maze[x, y--].BackColor = ColorTranslator.FromHtml("#929077");
                            maze[x, y].BackColor = ColorTranslator.FromHtml("#6a9e0e");
                        }
                    }
                    else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)     //向右
                    {
                        if (x != 14 && maze[x + 1, y].BackColor != ColorTranslator.FromHtml("#3c371e"))
                        {
                            maze[x++, y].BackColor = ColorTranslator.FromHtml("#929077");
                            maze[x, y].BackColor = ColorTranslator.FromHtml("#6a9e0e");
                        }
                    }
                }
                if (x == 14 && y == 14)
                {
                    MessageBox.Show("恭喜走到终点！");
                    this.KeyPreview = false;
                    print_Maze(blank);
                    maze[0, 0].BackColor = ColorTranslator.FromHtml("#929077");
                    maze[14, 14].BackColor = ColorTranslator.FromHtml("#929077");
                }
            }
            num++;

        }

        private void mazeButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.BackColor == ColorTranslator.FromHtml("#3c371e"))                   //墙壁
            {
                search_maze[Int32.Parse(btn.Name)] = 0;
                btn.BackColor = ColorTranslator.FromHtml("#929077");                    //通路
            }
            else if (btn.BackColor == ColorTranslator.FromHtml("#929077"))              //通路
            {
                btn.BackColor = ColorTranslator.FromHtml("#3c371e");                    //墙壁
                search_maze[Int32.Parse(btn.Name)] = 1;
            }
            
        }

        //开始游戏
        public void start_Click(object sender, EventArgs e)
        {
            fixed_maze();
            solve = true;
            creat_maze = true;
        }
        
        //固定迷宫
        public void fixed_maze()
        {
            x = 0;
            y = 0;
            startflag = 1;

            this.KeyPreview = true;
       
            initBoard = new int[15, 15]{    { 0,0,0,0,0,1,0,1,0,0,0,1,1,1,0 },
                                            { 0,1,1,1,0,1,0,0,0,0,0,0,0,1,0 },
                                            { 0,1,0,0,0,0,1,1,0,1,1,1,0,1,0 },
                                            { 0,1,1,0,0,0,0,0,0,0,1,1,0,0,1},
                                            { 1,1,0,1,0,1,1,0,1,0,0,1,1,0,1},
                                            { 0,0,0,1,1,0,0,0,0,1,0,1,1,0,1},
                                            { 1,1,1,1,0,0,0,0,0,0,1,0,0,0,0},
                                            { 1,1,1,1,1,1,1,0,1,0,0,1,1,1,1},
                                            { 1,1,0,0,0,0,0,0,1,0,1,1,0,0,0},
                                            { 0,1,1,0,0,0,0,1,1,0,0,0,0,1,0},
                                            { 1,1,1,1,0,0,0,1,0,1,0,1,0,1,0},
                                            { 0,1,1,0,0,1,0,0,0,1,1,1,0,1,0},
                                            { 1,0,1,0,1,1,0,1,1,1,0,0,0,1,0},
                                            { 0,0,0,0,1,1,0,1,1,1,0,1,1,0,0},
                                            { 0,1,1,0,1,0,0,0,0,1,0,1,1,0,0}};
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (initBoard[i, j] == 1)
                        maze[i, j].BackColor = ColorTranslator.FromHtml("#3c371e");                     //墙壁颜色
                    else
                        maze[i, j].BackColor = ColorTranslator.FromHtml("#929077");                     //通路颜色
                }
                maze[0, 0].BackColor = ColorTranslator.FromHtml("#d16971");                             //迷宫解的颜色
                maze[14, 14].BackColor = ColorTranslator.FromHtml("#d16971");                           //迷宫解的颜色
            }
            menu[1].Enabled = true;
            menu[2].Enabled = true;
            menu[3].Enabled = true;

            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    cmaze[15 * i + j] = initBoard[i, j];


            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    cmaze[15 * i + j] = initBoard[i, j];
        }

        //自定义迷宫
        public void diy_click(object sender,EventArgs e)
        {
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    maze[i, j].BackColor = ColorTranslator.FromHtml("#929077");
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                {
                    cmaze[i * 15 + j] = 0;
                    maze[i, j].Enabled = true;
                }
            solve = true;
        }
        //输出迷宫解
        public void print_Maze(int[] board)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (board[i * 15 + j] == 1)
                        maze[i, j].BackColor = ColorTranslator.FromHtml("#3c371e");                     //墙壁颜色
                    else if (board[i * 15 + j] == 2)
                        maze[i, j].BackColor = ColorTranslator.FromHtml("#d16971");                     //迷宫解的颜色  
                    else
                        maze[i, j].BackColor = ColorTranslator.FromHtml("#929077");                     //通路颜色
                }
                maze[0, 0].BackColor = ColorTranslator.FromHtml("#d16971");
                maze[14, 14].BackColor = ColorTranslator.FromHtml("#d16971");
            }
        }

        //生成迷宫
        public void creat_Click(object sender, EventArgs e)
        {
            if (creat_maze)
            {
                cmaze = new int[225];                                                                       //存储迷宫的变量
                creat(cmaze);                                                                               //创建迷宫
                print_Maze(cmaze);                                                                          //输出迷宫图形
                solve = true;
            }
        }
      
        //得到通路 打印迷宫 不需得知原迷宫 迷宫除了第一个 都是重新生成的
        private void solve1_Click(object sender, EventArgs e)
        {
            if (solve)
            {
                for (int i = 0; i < 15; i++)
                    for (int j = 0; j < 15; j++)
                        search_maze[i*15+j] = cmaze[i*15+j];
                dfs_solve(search_maze);
                print_Maze(search_maze);
                solve = false;
            }
        }
    }
}
//深度优先搜索的输出路线仍然存在问题 