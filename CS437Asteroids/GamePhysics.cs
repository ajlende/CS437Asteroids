using BulletSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CS437
{
    class GamePhysics
    {
        public enum CollisionTypes : short
        {
            Nothing = 0,
            Ship = 2,
            Torpedo = 4,
            Asteroid = 8,
            Powerup = 16,
            Everything = Ship | Torpedo | Asteroid | Powerup
        }

        private BroadphaseInterface Broadphase { get; set; }
        private CollisionConfiguration Configuration { get; set; }
        private CollisionDispatcher Dispatcher { get; set; }
        private ConstraintSolver Solver { get; set; }

        public DynamicsWorld DynamicsWorld { get; set; }

        public GamePhysics()
        {
            // Build the broadphase
            Broadphase = new DbvtBroadphase();

            // Set up the collision configuration and dispatcher
            Configuration = new DefaultCollisionConfiguration();
            Dispatcher = new CollisionDispatcher(Configuration);

            // The actual physics solver
            Solver = new SequentialImpulseConstraintSolver();

            // The world
            DynamicsWorld = new DiscreteDynamicsWorld(Dispatcher, Broadphase, Solver, Configuration);
            DynamicsWorld.Gravity = Vector3.Zero;
        }

        public void Update(float elapsedTime)
        {
            DynamicsWorld.StepSimulation(elapsedTime);
            //int numManifolds = DynamicsWorld.Dispatcher.NumManifolds;
            //for (int i = 0; i < numManifolds; i++)
            //{
            //    PersistentManifold contactManifold = DynamicsWorld.Dispatcher.GetManifoldByIndexInternal(i);
            //    CollisionObject obA = contactManifold.Body0 as CollisionObject;
            //    CollisionObject obB = contactManifold.Body1 as CollisionObject;

            //    int numContacts = contactManifold.NumContacts;
            //    for (int j = 0; j < numContacts; j++)
            //    {
            //        ManifoldPoint pt = contactManifold.GetContactPoint(j);
            //        if (pt.Distance < 0.0f)
            //        {
            //            Vector3 ptA = pt.PositionWorldOnA;
            //            Vector3 ptB = pt.PositionWorldOnB;
            //            Vector3 normalOnB = pt.NormalWorldOnB;
            //            var obAHandle = obA.UserObject;
            //            var obBHandle = obB.UserObject;
            //            if (obAHandle == null) continue;
            //            Console.WriteLine("--------------------");
            //            Console.WriteLine(ptA);
            //            Console.WriteLine(ptB);
            //            Console.WriteLine(normalOnB);
            //            Console.WriteLine("--------------------");
            //        }
            //    }
            //}
        }

        public void ExitPhysics()
        {
            //    // remove/dispose constraints
            //    int i;
            //    for (i = DynamicsWorld.NumConstraints - 1; i >= 0; i--)
            //    {
            //        TypedConstraint constraint = DynamicsWorld.GetConstraint(i);
            //        DynamicsWorld.RemoveConstraint(constraint);
            //        constraint.Dispose(); ;
            //    }

            //    // remove the rigidbodies from the dynamics world and delete them
            //    for (i = DynamicsWorld.NumCollisionObjects - 1; i >= 0; i--)
            //    {
            //        CollisionObject obj = DynamicsWorld.CollisionObjectArray[i];
            //        RigidBody body = obj as RigidBody;
            //        if (body != null && body.MotionState != null)
            //        {
            //            body.MotionState.Dispose();
            //        }
            //        DynamicsWorld.RemoveCollisionObject(obj);
            //        obj.Dispose();
            //    }

            //    DynamicsWorld.Dispose();
            //    Broadphase.Dispose();
            //    if (Dispatcher != null)
            //    {
            //        Dispatcher.Dispose();
            //    }
            //    Configuration.Dispose();
            //    Solver.Dispose();
        }

        public void AddRigidBody(RigidBody body)
        {
            DynamicsWorld.AddRigidBody(body);
        }

        public class PhysicsDebugDraw : DebugDraw
        {
            GraphicsDevice device;

            DebugDrawModes _debugMode;
            public override DebugDrawModes DebugMode
            {
                get { return _debugMode; }
                set { _debugMode = value; }
            }

            public PhysicsDebugDraw(GraphicsDevice device)
            {
                this.device = device;
            }

            public override void Draw3dText(ref Vector3 location, string textString)
            {
                throw new NotImplementedException();
            }

            public override void DrawContactPoint(ref Vector3 pointOnB, ref Vector3 normalOnB, float distance, int lifeTime, Color color)
            {
                VertexPositionColor[] vertices = new VertexPositionColor[2];
                vertices[0] = new VertexPositionColor(pointOnB, color);
                vertices[1] = new VertexPositionColor(pointOnB + normalOnB, color);
                device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
            }

            public override void DrawLine(ref Vector3 from, ref Vector3 to, Color color)
            {
                VertexPositionColor[] vertices = new VertexPositionColor[2];
                vertices[0] = new VertexPositionColor(from, color);
                vertices[1] = new VertexPositionColor(to, color);
                device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
            }

            public void DrawDebugWorld(DynamicsWorld world)
            {
                world.DebugDrawWorld();
            }

            public override void ReportErrorWarning(string warningString)
            {
                throw new NotImplementedException();
            }
        }
    }


}
