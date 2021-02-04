using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class WellboyClassic : Classic
    {
        // ~ wellboy ~
        // Azaliya Khamitova & Ural Khamitov

        #region "global" variables

        private const float GameSpeed = 16f;
        private const float OffsetTop = 33f;
        private const float DeadLine = 180f - 24f;

        private const string LEVEL_DATA = "1223133213321311221122113213";
        private static int currentPos = 0;

        private static toolbar tbar;
        private static player _player;

        #endregion

        #region entry point

        /// <summary>
        /// Инициализация объектов, необходимых для игры. Вызывается при нажатии Insert Coin в игровом автомате
        /// </summary>
        /// <param name="emulator">Сюда передаётся инстанс "эмулятора"</param>
        public override void Init(Emulator emulator)
        {
            base.Init(emulator);

            tbar = new toolbar
            {
                x = Engine.Instance.Screen.Width / 2f - 43,
                y = OffsetTop - 3
            };
            tbar.init(emulator);

            var emuCenter = tbar.x + tbar.hitbox.Width / 2f;

            _player = new player();
            _player.x = emuCenter - 12f / 2f;
            _player.y = OffsetTop + 73;
            _player.init(emulator);

            init_object(new brick1(), tbar.x + 10f, OffsetTop - 20f);
            init_object(new brick2(), tbar.x + tbar.hitbox.Width - 21f - 10f, OffsetTop - 70);

            init_object(new pipe(true), tbar.x + tbar.hitbox.Width - 26f - 16f, OffsetTop - 40);

            init_object(new wall(), Engine.Instance.Screen.Width / 2f - 33, 0);
            init_object(new wall(), Engine.Instance.Screen.Width / 2f + 33, 0);
        }

        #endregion

        public override void Destroy()
        {
            base.Destroy();

            if (tbar != null)
                destroy_object(tbar);
            if (_player != null)
                destroy_object(_player);
        }

        #region update

        public override void Update()
        {
            base.Update();
            _player.update();
        }

        #endregion

        #region drawing

        public override void Draw()
        {
            base.Draw();
            tbar.draw();
            _player.draw();
        }

        #endregion

        private static void createNextObject()
        {
            if (currentPos > LEVEL_DATA.Length - 1)
                currentPos = 0;
            char currentChar = LEVEL_DATA[currentPos];
            bool isRight = currentPos % 2 == 0;
            currentPos += 1;
            switch (currentChar)
            {
                case '1':
                    if (isRight)
                    {
                        // правая сторона
                        init_object(new brick1(), tbar.x + tbar.hitbox.Width - 23f - 16f, OffsetTop - 50);
                    }
                    else
                    {
                        // левая сторона
                        init_object(new brick1(), tbar.x + 16f, OffsetTop - 50);
                    }
                    break;
                case '2':
                    if (isRight)
                    {
                        // правая сторона
                        init_object(new brick2(), tbar.x + tbar.hitbox.Width - 21f - 16f, OffsetTop - 50);
                    }
                    else
                    {
                        // левая сторона
                        init_object(new brick2(), tbar.x + 16f, OffsetTop - 50);
                    }
                    break;
                case '3':
                    if (isRight)
                    {
                        // правая сторона
                        init_object(new pipe(true), tbar.x + tbar.hitbox.Width - 26f - 10f, OffsetTop - 50);
                    }
                    else
                    {
                        // левая сторона
                        init_object(new pipe(), tbar.x + 10f, OffsetTop - 50);
                    }
                    break;
                case '4':
                    break;
            }
        }

        public class toolbar : ClassicObject
        {
            public override void init(Emulator e)
            {
                base.init(e);
                spr = new Animation(GFX.Game["emulatorboy/toolbar"], 86, 10);
                spr.CenterOrigin();
                hitbox = spr.Bounds;
            }
        }

        public class wall : ClassicObject
        {
            public override void init(Emulator e)
            {
                base.init(e);
                hitbox = new Rectangle(0, 0, 1, Engine.Instance.Screen.Height);
            }
        }

        public class brick1 : ClassicObject
        {
            private float timer;

            public brick1(bool flip = false)
            {
                flipX = flip;
            }

            public override void init(Emulator e)
            {
                base.init(e);
                timer = 1.0f;

                spr = new Animation(GFX.Game["emulatorboy/back1"], 23, 15);
                spr.CenterOrigin();
                hitbox = spr.Bounds;
            }

            public override void update()
            {
                base.update();
                timer -= Engine.DeltaTime * GameSpeed;
                if (timer < 0)
                {
                    timer = 1.0f;
                    move(0, 1);
                }

                if (y > DeadLine)
                {
                    destroy_object(this);
                    createNextObject();
                }
            }
        }

        public class brick2 : ClassicObject
        {
            private float timer;

            public brick2(bool flip = false)
            {
                flipX = flip;
            }

            public override void init(Emulator e)
            {
                base.init(e);
                timer = 1.0f;

                spr = new Animation(GFX.Game["emulatorboy/back2"], 21, 18);
                spr.CenterOrigin();
                hitbox = spr.Bounds;
            }

            public override void update()
            {
                base.update();
                timer -= Engine.DeltaTime * GameSpeed;
                if (timer < 0)
                {
                    timer = 1.0f;
                    move(0, 1);
                }

                if (y > DeadLine)
                {
                    destroy_object(this);
                    createNextObject();
                }
            }
        }

        public class pipe : ClassicObject
        {
            private float timer;

            public pipe(bool flip = false)
            {
                flipX = flip;
            }

            public override void init(Emulator e)
            {
                base.init(e);
                timer = 1.0f;

                spr = new Animation(GFX.Game["emulatorboy/pipe"], 26, 9);
                spr.Add("freeze", 4f, 0, 1, 2, 1, 2, 1);
                spr.Play("freeze");
                spr.CenterOrigin();
                hitbox = spr.Bounds;
            }

            public override void update()
            {
                base.update();
                timer -= Engine.DeltaTime * GameSpeed;
                if (timer < 0)
                {
                    timer = 1.0f;
                    move(0, 1);
                }

                if (y > DeadLine)
                {
                    destroy_object(this);
                    createNextObject();
                }
            }
        }

        public class coin : ClassicObject
        {
            private float timer;

            public override void init(Emulator e)
            {
                base.init(e);
                spr = new Animation(GFX.Game["emulatorboy/coin"], 24, 6);
                hitbox = spr.Bounds;
            }

            public override void update()
            {
                base.update();
                timer -= Engine.DeltaTime * GameSpeed;
                if (timer < 0)
                {
                    timer = 1.0f;
                    move(0, 1);
                }

                if (y > DeadLine)
                {
                    destroy_object(this);
                    createNextObject();
                }
            }
        }

        public class player : ClassicObject
        {
            public int lives = 3;
            private bool invincible;
            private float timer;

            public override void init(Emulator e)
            {
                base.init(e);
                spr = new Animation(GFX.Game["emulatorboy/boy"], 13, 25);
                spr.Add("fly", 5f, 0, 1, 2, 3, 1, 1, 2, 3);
                spr.Play("fly");
                spr.CenterOrigin();
                hitbox = spr.Bounds;
            }

            public override void update()
            {
                base.update();
                //Console.WriteLine(x);
                timer -= Engine.DeltaTime;
                if (timer > 0)
                {

                }
                if (check<pipe>(1, 0) && !invincible)
                {
                    invincible = true;
                    timer = 2f;
                    lives -= 1;
                    //Console.WriteLine(lives);
                }
                if (Input.Down("left"))
                {
                    if (!check<wall>(-1, 0))
                        move(-1, 0);
                    flipX = false;
                }
                else if (Input.Down("right"))
                {
                    if (!check<wall>(1, 0))
                        move(1, 0);
                    flipX = true;
                }
            }
        }
    }
}