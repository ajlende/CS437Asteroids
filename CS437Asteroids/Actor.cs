using BulletSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CS437
{
    abstract class Actor
    {
        internal Model _model;
        internal Texture2D _texture;

        public RigidBody Body { get; set; }

        public MotionState MotionState
        {
            get
            {
                return Body.MotionState;
            }
            set
            {
                Body.MotionState = value;
            }
        }
        public CollisionShape CollisionShape
        {
            get
            {
                return Body.CollisionShape;
            }
            set
            {
                Body.CollisionShape = value;
            }
        }

        public Vector3 Position { get { return Body.WorldTransform.Translation; } }
        public Quaternion Orientation { get { return Body.Orientation; } }
        public Vector3 Forward { get { return Vector3.Transform(Vector3.Forward, Orientation); } }
        public Vector3 Up { get { return Vector3.Transform(Vector3.Up, Orientation); } }
        public Vector3 Right { get { return Vector3.Transform(Vector3.Right, Orientation); } }

        public float Scale { get; set; }
        public float Mass { get; set; }

        public Actor(DynamicsWorld DynamicsWorld,
            Model model,
            Texture2D texture = null,
            CollisionShape shape = null,
            GamePhysics.CollisionTypes collisionType = GamePhysics.CollisionTypes.Nothing,
            GamePhysics.CollisionTypes collidesWith = GamePhysics.CollisionTypes.Nothing,
            float scale = 1f,
            float mass = 0f)
        {
            Mass = mass;
            Scale = scale;
            var collisionShape = shape ?? new BoxShape(5f, 5f, 5f);
            var motionState = new DefaultMotionState();
            var constructionInfo = new RigidBodyConstructionInfo(Mass, motionState, collisionShape);
            Body = new RigidBody(constructionInfo);
            Body.UserObject = this;
            Body.UserIndex = (int) collisionType;

            Body.Restitution = 0.5f;

            DynamicsWorld.AddRigidBody(Body, (short) collisionType, (short) collidesWith);
        }

        public virtual void Update(GameTime gameTime)
        {
            // TODO
        }

        public virtual void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateScale(Scale)
                        * Matrix.CreateFromQuaternion(Orientation)
                        * Matrix.CreateTranslation(Position);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    if (_texture != null)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = _texture;
                    }
                }
                mesh.Draw();
            }
        }

        public virtual void Dispose()
        {
            Body.Dispose();
        }
    }
}
