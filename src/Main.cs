using System.Numerics;
using System.Xml;
using Raylib_cs;
namespace raylibTest;
//https://www.alibabacloud.com/blog/construct-a-simple-3d-rendering-engine-with-java_599599

partial class Program
{
    public static void Main()
    {
        var scene = new Scene("BallDrop")
        {
            // ScaleFactor = 100,
            Log = true,
        };


        Ball ball = new(new Vertex(0.8f, 1.5f, 0.8f), 0.0325f, Color.YELLOW);
        Ball origin = new(new Vertex(0, 0, 0), 0.1f, Color.WHITE)
        {
            Order = 1
        };

        origin.RegenTriangles();

        scene.Drawables = new() {
            // ball,
            origin,
            //Court
            // new Box(new Vertex(-1,-0.1f, -1), new Vertex(2, 0.1f, 2), Color.WHITE)

            // new Box(new Vertex(-50, -50, -50), new Vertex (100, 100, 100), Color.GOLD),
            // new Ball(new Vertex(0, 0, 0), 50, Color.YELLOW),
            // new Arrow(new Vertex(0, 0, 0), new Vertex(100, 0, 0), Color.BLUE),
        };

        var ballFalling = true;
        var ballVelo = new Vertex(0, 0, 0);
        var gravity = new Vertex(0, -9.8f, 0);
        double elapsedTime = 0;

        scene.OnTick = (deltaT) => {
            if (ball.Pos.y < 0)
            {
                ballVelo.y = -ballVelo.y;
                ball.Pos.y = 0;
            }

            // Thread.Sleep(10);

            if (ballFalling)
            {
                ballVelo += gravity * (float)deltaT;
                ball.Pos += ballVelo * (float)deltaT;
                elapsedTime += deltaT;
                // Console.WriteLine($"{elapsedTime:0.000} {deltaT} {ball.Pos.y - 1.5}");
            }
        };

        scene.Play();
    }
}