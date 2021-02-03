using KTEngine;
using Microsoft.Xna.Framework;

namespace NJ
{
    public class Player : Actor
    {
        public const string StateNormal = "normal";
        public const string StateAir = "air";
        public const string StateLeaveRoom = "leaveRoom";
        
        public Vector2 MovementInput = Vector2.Zero;
        
        public const float Acceleration = 850f;
        public const float Friction = 800f;
        public const float Gravity = 700f;
        public const float JumpGravity = 280f;
        public const float JumpForce = 94f;
        public const float JumpTime = 0.12f;
        
        public const float MaxSpeed = 28f;
        
        public bool InNextRoom;
        
        public Animation Sprite;
        public readonly StateMachine StateMachine;
        public Movement Movement;
        
        public Player()
        {
            Add(new GraphicRect(4, 8, Color.Red)).CenterOrigin();
            MoveCollider = Add(new Hitbox(-2, -4, 4, 8));
            MoveCollider.Tag((int) Tags.Player);
            Movement = Add(new Movement(MoveCollider, (int) Tags.Player));
            
            StateMachine = new StateMachine();
            StateMachine.Add(StateNormal, NormalBegin, NormalUpdate);
            StateMachine.Add(StateAir, AirBegin, AirUpdate, AirEnd);
            StateMachine.Set(StateNormal);
            Add(StateMachine);
        }

        public override void Update()
        {
            GetInput();
            Movement.Accelerate(MovementInput.X * Acceleration, MovementInput.Y * Acceleration);
            Movement.Maxspeed(MaxSpeed);
            base.Update();
            if (Level.Instance.NextRoom != null && StateMachine.State != StateLeaveRoom && !InNextRoom)
            {
                if (Position.X > Room.Width)
                {
                    StateMachine.Set(StateLeaveRoom);
                }
            }

        }

        private void GetInput()
        {
            //var wasOnGround = m_on_ground;
            //m_on_ground = mover->on_ground();
            
            if (MovementInput.X == 0f)
                Movement.Friction(Friction, 0f);
            if (MovementInput.Y == 0f)
                Movement.Friction(0f, Friction);

            Vector2 axis = Vector2.Zero;
            if (Input.Down("left"))
                axis.X = -1f;
            if (Input.Down("right"))
                axis.X = 1f;

            if (axis.Length() > 0.0)
                axis.Normalize();
            MovementInput = axis;
        }
        
        private void NormalBegin()
        {
        }

        private void NormalUpdate()
        {
        }
        
        private void NormalEnd()
        {
        }
        
        private void AirBegin()
        {
            
        }

        private void AirUpdate()
        {
            
        }

        private void AirEnd()
        {
            
        }
    }
}