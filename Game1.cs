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

            ballDir = new Vector2(0.5f, 0.5f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ballPos += ballDir * (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            WallCollision(ballPos);

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

        void WallCollision(Vector2 in_ball)
        {
            if(in_ball.X <= 0 || in_ball.X >= (screenWidth - ball.Width))
            {
                if(in_ball.X <= 0)
                {
                    enemyScore++;
                }
                else
                {
                    playerScore++;
                }

                ballDir.X *= -1;
                wallHit.Play();
            }
            if (in_ball.Y <= 0 || in_ball.Y >= (screenHeight - ball.Height))
            {
                ballDir.Y *= -1;
                wallHit.Play();
            }
        }
    }
}
