using KTEngine;
using Microsoft.Xna.Framework;

namespace Chip
{
    public class EnemyWalker : Enemy
    {
        private readonly Animation sprite;
        private readonly StateMachine state;
        private readonly float walkspeed;
        private int direction = 1;
        private float timer;

        public EnemyWalker()
        {
            Health = 8;
            Points = 1;
            
            // sprite
            sprite = Add(XML.LoadSprite<Animation>(GFX.Game, GFX.Sprites, GetType().Name));
            
            // collider
            MoveCollider = Add(new Hitbox(-13, -16, 27, 32));
            MoveCollider.Tag((int) Tags.Enemy);
            
            // states
            state = Add(new StateMachine());
            state.Add("walk", BeginWalk, UpdateWalk);
            state.Add("turn", BeginTurn, UpdateTurn);
            state.Add("fall", BeginFall, UpdateFall, EndFall);
            state.Add("dead");
            
            state.Set("walk");
            walkspeed = Utils.Range(28, 32);
            DeadTimeoutRate = 6f;
        }

        public void Define(Vector2 position)
        {
            Position = position;
            MoveCollider.Reset(-13, -16, 27, 32);
            sprite.Visible = true;
        }

        public override void Update()
        {
            if (MoveCollider.Check(0, 1, (int) Tags.Acid))
                Hurt(Health);
            base.Update();
        }

        private void BeginWalk()
        {
            direction = -direction;
            sprite.Scale.X = direction;
            sprite.Play("walk");
        }

        private void UpdateWalk()
        {
            Speed.X = direction * walkspeed;
            Move();
            if (!MoveCollider.Check(direction * MoveCollider.Width, 1, (int) Tags.Solid))
                state.Set("turn");
            if (!MoveCollider.Check(0, 1, (int) Tags.Solid))
                state.Set("fall");
            if ((X > (double) (Room.SceneX + MoveCollider.Width) || direction >= 0) &&
                (X < (double) (Room.SceneX + Room.SceneWidth - MoveCollider.Width) || direction <= 0))
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

        public static void Born(Room room, Vector2 position, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                EnemyWalker entity = new EnemyWalker();
                entity.Room = room;
                entity.Define(position);
                Engine.Scene.Add(entity, "default");
            }
        }
    }
}