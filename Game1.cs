using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Texture2D background, player, enemy, ball, scoreBar;

        private SoundEffect wallHit, paddleHit;

        private Vector2 playerPos, enemyPos, ballPos, ballDir, scoreBarPos;

        private int playerScore, enemyScore, playerSpeed, enemySpeed;

        private int screenWidth = 1024;
        private int screenHeight = 769;
        private int ballSpeed = 500;
        private int enemyDir = 1;
        private bool started = false;
        private float eTime;
        private Random rnd = new Random();

        private const float init_eTime = 0.15f;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();

            playerSpeed = 300;
            enemySpeed = 600;

            playerScore = 0;
            enemyScore = 0;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //LOAD IMAGES
            background = Content.Load<Texture2D>("background");
            player = Content.Load<Texture2D>("paddleOrange");
            enemy = Content.Load<Texture2D>("paddleBlue");
            ball = Content.Load<Texture2D>("ball");
            scoreBar = Content.Load<Texture2D>("scoreBar");

            //LOAD SOUND CLIPS
            wallHit = Content.Load<SoundEffect>("Chime32");
            paddleHit = Content.Load<SoundEffect>("Hit_1");

            //SETUP VECTOR2
            scoreBarPos = new Vector2((screenWidth / 2) - (scoreBar.Width / 2), 0);
            enemyPos = new Vector2((screenWidth - enemy.Width), (screenHeight / 2) - (enemy.Height / 2));
            playerPos = new Vector2(0, (screenHeight / 2) - (player.Height / 2));
            ballPos = new Vector2((screenWidth / 2) - (ball.Width / 2), (screenHeight / 2) - (ball.Height / 2));

            ballDir = new Vector2(0.5f, 0);

            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (eTime <= 0)
            {
                eTime = init_eTime;
                EnemyAI();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !started)
            {
                started = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                playerPos.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                playerPos.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (playerPos.Y <= 0)
            {
                playerPos.Y = 0;
            }

            if (playerPos.Y >= screenHeight - player.Height)
            {
                playerPos.Y = screenHeight - player.Height;
            }

            if (started)
            {
                ballPos += ballDir * (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                PlayerCollision();
                EnemyCollision();
                WallCollision();
                if (ballDir.X > 0)
                {
                    enemyPos.Y += enemySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds * enemyDir;
                }
            }

            if (enemyPos.Y <= 0)
            {
                enemyPos.Y = 0;
            }
            if (enemyPos.Y >= screenHeight - enemy.Height)
            {
                enemyPos.Y = screenHeight - enemy.Height;
            }

            eTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(player, playerPos, Color.White);
            _spriteBatch.Draw(enemy, enemyPos, Color.White);
            _spriteBatch.Draw(ball, ballPos, Color.White);
            _spriteBatch.Draw(scoreBar, scoreBarPos, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void WallCollision()
        {
            if(ballPos.X <= 0 || ballPos.X >= (screenWidth - ball.Width))
            {
                if(ballPos.X <= 0)
                {
                    enemyScore++;
                }
                else
                {
                    playerScore++;
                }

                ballPos = new Vector2((screenWidth / 2) - (ball.Width / 2), (screenHeight / 2) - (ball.Height / 2));
                started = false;
                ballDir.X *= -1;
                ballDir.Y = 0;
                wallHit.Play();
            }
            if (ballPos.Y <= 0 || ballPos.Y >= (screenHeight - ball.Height))
            {
                ballDir.Y *= -1;
                wallHit.Play();
            }
        }

        void PlayerCollision()
        {
            if (ballPos.X <= playerPos.X + player.Width && ballPos.Y + ball.Height >= playerPos.Y && ballPos.Y <= playerPos.Y + player.Height)
            {
                ballDir.X *= -1;
                if (ballDir.Y == 0)
                {
                    ballDir.Y = rnd.Next(-1, 1);
                }
            }

        }

        void EnemyCollision()
        {
            if (ballPos.X + ball.Width >= enemyPos.X && ballPos.Y + ball.Height >= enemyPos.Y && ballPos.Y <= enemyPos.Y + enemy.Height)
            {
                ballDir.X *= -1;
                if (ballDir.Y == 0)
                {
                    ballDir.Y = rnd.Next(-1, 1);
                }
            }
        }

        void EnemyAI()
        {
            if (ballDir.X > 0)
            {
                if (ballPos.Y + ball.Height <= enemyPos.Y)
                {
                    enemyDir = -1;
                }
                else if (ballPos.Y >= enemyPos.Y + enemy.Height)
                {
                    enemyDir = 1;
                }
            }
        }
    }
}
