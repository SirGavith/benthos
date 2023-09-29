﻿
using Raylib_cs;
namespace raylibTest;
//https://www.alibabacloud.com/blog/construct-a-simple-3d-rendering-engine-with-java_599599

class Program
{
    public static void Main()
    {
        var ballFalling = true;
        double elapsedTime = 0;

        var speed = 30;
        var theta = Math.PI / 4;
        var rho = 1f;
        var g = new Vector(0, -9.8f, 0);
        var windV = new Vector(50, 0, 0);

        var ball = new Ball(new Vector(0, 0, 0), 2, Color.YELLOW);
        var ballV = new Vector((float)Math.Cos(theta), (float)Math.Sin(theta), 0) * speed;
        var ballM = 200;
        var ballDrag = 0.47f;
        var ballCrossArea = (float)Math.PI * ball.Radius * ball.Radius;


        var scene = new Scene("Windy Day")
        {
            Log = true,
            OnTick = (deltaT) =>
            {
                if (ball.Pos.Y <= 0 && elapsedTime > 0.1)
                {
                    ballFalling = false;
                    Console.WriteLine(ball.Pos.X);
                }

                if (ballFalling)
                {
                    var F_net = new Vector();
                    var relativeV = ballV - windV;
                    F_net += g * ballM;
                    F_net += relativeV.Norm() * -0.5f * rho * ballCrossArea * ballDrag * (float)Math.Pow(relativeV.Magnitude(), 2);



                    ballV += F_net * (float)deltaT / ballM;
                    ball.Pos += ballV * (float)deltaT;
                    elapsedTime += deltaT;
                    // Console.WriteLine($"{elapsedTime:0.000} {deltaT} {ball.Pos.y - 1.5}");

                }
            },

            Drawables = new() {
            ball,
            // new(new Vertex(0, 0, 0), 0.1f, Color.WHITE),
            new Box(new Vector(-1,-0.1f, -1), new Vector(2, 0.1f, 2), Color.BLUE)
        }
        };

        scene.Play();
    }
}