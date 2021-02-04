using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class EggEnemy : Enemy
    {
        private readonly Animation sprite;
        private readonly StateMachine state;
        private readonly float walkspeed;
        
        private int direction = 1;
        private float timer;

        public EggEnemy()
        {
            Health = 6;
            Points = 1;
            
            // sprite
            sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            sprite.OnFinish += animation =>
            {
                // Если закончилась анимация пробуждения, то начинаем ходить
                if (animation.Equals("wake_up"))
                {
                    state.Set("walk");
                }
            };

            // collider
            MoveCollider = Add(new Hitbox(-8, -8, 16, 32));
            MoveCollider.Tag((int) Tags.Enemy);
            
            // states
            state = Add(new StateMachine());
            state.Add("sleep", SleepBegin);
            state.Add("wake_up", BeginWakeUp);
            state.Add("walk", BeginWalk, UpdateWalk);
            state.Add("turn", BeginTurn, UpdateTurn);
            state.Add("fall", BeginFall, UpdateFall, EndFall);
            state.Add("dead");
            
            state.Set("sleep");
            walkspeed = Utils.Range(40, 60);
            DeadTimeoutRate = 6f;
        }

        public override void Update()
        {
            if ((Player.Instance.Position - Position).Length() < 90f && state.State == "sleep")
            {
                state.Set("wake_up");
            }
            if (MoveCollider.Check(0, 1, (int) Tags.Acid))
                Hurt(Health);
            base.Update();
        }

        private void SleepBegin()
        {
            sprite.Play("sleep");
        }

        private void BeginWakeUp()
        {
            sprite.Play("wake_up");
        }

        private void EndWakeUp()
        {
            state.Set("walk");
        }

        private void BeginWalk()
        {
            direction = -direction;
            sprite.Scale.X = -direction;  
            sprite.Play("walk");
        }

        private void UpdateWalk()
        {
            Speed.X = direction * walkspeed;
            Move();
            if (!MoveCollider.Check(direction * 16, 1, (int) Tags.Solid))
                state.Set("turn");
            if (!MoveCollider.Check(0, 1, (int) Tags.Solid))
                state.Set("fall");
            if ((X > (double) (Room.SceneX + 16) || direction >= 0) &&
                (X < (double) (Room.SceneX + Room.SceneWidth - 16) || direction <= 0))
                return;
            state.Set("turn");
        }

        private void BeginTurn()
        {
            timer = 1f;
            sprite.Play("stand");
        }

        private void UpdateTurn()
        {
            timer -= Engine.DeltaTime;
            if (timer <= 0.0)
                state.Set("walk");
            if (MoveCollider.Check(0, 1, (int) Tags.Solid))
                return;
            state.Set("fall");
        }

        private void BeginFall()
        {
            Speed.X = 0.0f;
            Speed.Y = 0.0f;
        }

        private void UpdateFall()
        {
            Fall(600f);
            Move();
        }

        private void EndFall()
        {
            
        }

        public override void DyingBegin()
        {
            MoveCollider.Collidable = false;
            state.Set("dead");
        }

        public override void DyingUpdate(float timer)
        {
            DieMutateGrahpic(sprite, timer);
        }

        public override void Knockback(Entity from)
        {
            Push.X = ForceDirection(@from.X) * 120;
            Speed.X = 0.0f;
            state.Set("turn");
        }

        public override void HitSolid(Hit axis, ref Vector2 velocity, Collider collision)
        {
            if (axis == Hit.Horizontal)
                state.Set("turn");
            else if (axis == Hit.Vertical && state.State == "fall")
                state.Set("walk");
            base.HitSolid(axis, ref velocity, collision);
        }
    }
}