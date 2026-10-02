using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using WMPLib;

namespace SpaceShooter
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootgMedia;
        WindowsMediaPlayer explosion;


        PictureBox[] stars;
        int backgroundSpeed;
        int playerSpeed;
        Random rnd;

        int score;
        int level;
        int difficulty;
        bool pause;
        bool GameIsOver;
        bool pauseCooldown;



        PictureBox[] munitions;
        int MunitionSpeed;

        PictureBox[] enemies;
        int enemiSpeed;

        PictureBox[] enemiesMunition;
        int enemiesMunitionSpeed;


        public Form1()
        {
            InitializeComponent();

            KeyPreview = true;



            LeftMoveTimer.Tick += LeftMoveTimer_Tick;
            RightMoveTimer.Tick += RightMoveTimer_Tick;
            UpMoveTimer.Tick += UpMoveTimer_Tick;
            DownMoveTimer.Tick += DownMoveTimer_Tick;
            MoveMunitionTimer.Tick += MoveMunitionTimer_Tick;


            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pause = false;
            GameIsOver = false;
            score = 0;
            level = 1;
            difficulty = 9;


            rnd = new Random();

            backgroundSpeed = 20;
            playerSpeed = 4;
            MunitionSpeed = 20;
            enemiSpeed = 5;
            enemiesMunitionSpeed = 5;

            munitions = new PictureBox[3];

            //Load images

            Image munition = Image.FromFile(@"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\asserts\munition.png");

            Image enemi1 = Image.FromFile(@"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\asserts\E1.png");
            Image enemi2 = Image.FromFile(@"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\asserts\E2.png");
            Image enemi3 = Image.FromFile(@"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\asserts\E3.png");
            Image boss1 = Image.FromFile(@"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\asserts\Boss1.png");
            Image boss2 = Image.FromFile(@"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\asserts\Boss2.png");

            enemies = new PictureBox[10];

            //Initialize enemies

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }


            enemies[0].Image = boss1;
            enemies[1].Image = enemi1;
            enemies[2].Image = enemi2;
            enemies[3].Image = enemi3;
            enemies[4].Image = enemi1;
            enemies[5].Image = enemi3;
            enemies[6].Image = enemi2;
            enemies[7].Image = enemi3;
            enemies[8].Image = enemi2;
            enemies[9].Image = boss2;



            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                munitions[i].Visible = false;
                this.Controls.Add(munitions[i]);
            }

            //Create a new instance of WindowsMediaPlayer
            gameMedia = new WindowsMediaPlayer();
            shootgMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //Load all songs
            gameMedia.URL = @"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\songs\GameSong.mp3";
            shootgMedia.URL = @"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\songs\shoot.mp3";
            explosion.URL = @"C:\Users\darkn\OneDrive\Escritorio\Proyectos\0. 2D Game\1. SpaceShooter\component\songs\boom.mp3";

            //Setup song settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootgMedia.settings.volume = 1;
            explosion.settings.volume = 6;

            //Enemies Munition
            enemiesMunition = new PictureBox[10];
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i] = new PictureBox();
                enemiesMunition[i].Size = new Size(2, 25);
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10);
                enemiesMunition[i].Location = new Point(0, -50);
                this.Controls.Add(enemiesMunition[i]);

            }

            gameMedia.controls.play();

            stars = new PictureBox[15];

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(20, 580), rnd.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.White;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }
                this.Controls.Add(stars[i]);
            }

            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            EnemiesMunitionTimer.Start();

            scorelbl.Text = "Score:  0";
            levellbl.Text = "Level:  1";

        }

        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                stars[i].Top += backgroundSpeed;
                if (stars[i].Top >= this.Height)
                    stars[i].Top = -stars[i].Height;
            }

            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundSpeed - 2;
                if (stars[i].Top >= this.Height)
                    stars[i].Top = -stars[i].Height;
            }
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
                Player.Left -= playerSpeed;
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
                Player.Left += playerSpeed;
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
                Player.Top += playerSpeed;
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
                Player.Top -= playerSpeed;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            this.Text = e.KeyCode.ToString();
            e.Handled = true;

            if (!pause && !GameIsOver)

            {

                if (e.KeyCode == Keys.Right) RightMoveTimer.Start();
                if (e.KeyCode == Keys.Left) LeftMoveTimer.Start();
                if (e.KeyCode == Keys.Down) DownMoveTimer.Start();
                if (e.KeyCode == Keys.Up) UpMoveTimer.Start();

                if (e.KeyCode == Keys.Space)

                {
                    if (!GameIsOver)

                    {
                        if (!munitions[0].Visible && !munitions[1].Visible && !munitions[2].Visible)
                        {
                            munitions[0].Location = new Point(Player.Left + Player.Width / 2 - 4, Player.Top);
                            munitions[1].Location = new Point(Player.Left + Player.Width / 2 - 4, Player.Top - 30);
                            munitions[2].Location = new Point(Player.Left + Player.Width / 2 - 4, Player.Top - 60);

                            munitions[0].Visible = true;
                            munitions[1].Visible = true;
                            munitions[2].Visible = true;

                            shootgMedia.controls.play();
                            MoveMunitionTimer.Start();
                        }
                    }

                }
            }

            if (e.KeyCode == Keys.P)
            {
                if (!GameIsOver)
                {
                    if (pause)
                    {
                        StartTimers();

                        if (munitions[0].Visible || munitions[1].Visible || munitions[2].Visible)

                        {
                            MoveMunitionTimer.Start();
                        }

                        label1.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label1.Location = new Point(this.Width / 2 - 120, 150);
                        label1.Text = "PAUSED";
                        label1.Visible = true;
                        gameMedia.controls.pause();
                        StopTimers();
                        pause = true;
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right) RightMoveTimer.Stop();
            if (e.KeyCode == Keys.Left) LeftMoveTimer.Stop();
            if (e.KeyCode == Keys.Down) DownMoveTimer.Stop();
            if (e.KeyCode == Keys.Up) UpMoveTimer.Stop();
        }

        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            bool hayActiva = false;

            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Visible)
                {
                    munitions[i].Top -= MunitionSpeed;
                    munitions[i].Left = Player.Left + Player.Width / 2 - 4;

                    if (munitions[i].Top < 0)
                        munitions[i].Visible = false;
                    else
                        hayActiva = true;
                }
            }

            if (!hayActiva)
                MoveMunitionTimer.Stop();
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiSpeed);
            Collision();
        }

        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }

            }

        }

        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (munitions[0].Visible && munitions[0].Bounds.IntersectsWith(enemies[i].Bounds)
                    || munitions[1].Visible && munitions[1].Bounds.IntersectsWith(enemies[i].Bounds)
                    || munitions[2].Visible && munitions[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.controls.play();

                    score += 1;
                    scorelbl.Text = "Score: " + (score < 10 ? "0" + score.ToString() : score.ToString());


                    if (score % 15 == 0)
                    {
                        level += 1;
                        levellbl.Text = "Level: " + (level < 10 ? "0" + level.ToString() : level.ToString());
                    }


                    if (enemiSpeed < 10 && enemiesMunitionSpeed < 10 && difficulty >= 0)
                    {
                        difficulty--;
                        enemiSpeed++;
                        enemiesMunitionSpeed++;
                    }

                    if (level == 10)
                    {
                        GameOver("You Win");
                    }


                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }
        private void GameOver(string str)
        {
            GameIsOver = true;
            label1.Text = str;
            label1.Location = new Point(120, 120);
            label1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;


            gameMedia.controls.stop();
            StopTimers();
        }

        //Stop Timers
        private void StopTimers()
        {
            MoveBgTimer.Stop();
            MoveMunitionTimer.Stop();
            MoveEnemiesTimer.Stop();
            EnemiesMunitionTimer.Stop();
        }

        //Start Timers
        private void StartTimers()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            EnemiesMunitionTimer.Start();

        }

        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemiesMunition.Length - difficulty); i++)
            {
                if (enemiesMunition[i].Top >= 0 && enemiesMunition[i].Top < this.Height)
                {
                    enemiesMunition[i].Visible = true;
                    enemiesMunition[i].Top += enemiesMunitionSpeed;
                }
                else
                {
                    enemiesMunition[i].Visible = false;

                    int intentos = 0;
                    int x;
                    do
                    {
                        x = rnd.Next(0, 10);
                        intentos++;
                    } while ((enemies[x].Top < 0 || enemies[x].Top > this.Height) && intentos < 10);

                    if (enemies[x].Top >= 0 && enemies[x].Top <= this.Height)
                    {
                        enemiesMunition[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                    }
                    else
                    {
                        enemiesMunition[i].Location = new Point(0, -100);
                    }
                }
            }

            collisionWithEnemiesMunition();
        }

        private void collisionWithEnemiesMunition()
        {
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                if (enemiesMunition[i].Visible && enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMunition[i].Visible = false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    explosion.settings.volume = 6;
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ocultar botones y label
            button1.Visible = false;
            button2.Visible = false;
            label1.Visible = false;

            // Mostrar jugador
            Player.Visible = true;
            Player.Location = new System.Drawing.Point(267, 409);

            // Reiniciar variables
            pause = false;
            GameIsOver = false;
            score = 0;
            level = 1;
            enemiSpeed = 5;           
            enemiesMunitionSpeed = 5;
            difficulty = 9;


            //Reiniciar labels
            scorelbl.Text = "Score: 0";     
            levellbl.Text = "Level: 1";     


            // Reiniciar enemigos
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Visible = false;
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }

            // Reiniciar municiones del jugador
            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i].Visible = false; 
            }

            // Reiniciar municiones enemigas
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].Location = new Point(0, -200);
            }



            // Reiniciar música y timers

            explosion.settings.volume = 6;
            gameMedia.controls.stop();
            gameMedia.controls.play();
            StartTimers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
