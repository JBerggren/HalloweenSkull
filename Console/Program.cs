using Iot.Device.ServoMotor;
using System;
using System.Device.Pwm.Drivers;

namespace HalloweenSkull
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pwmChannel = new SoftwarePwmChannel(18, 50, 0, true);
            var servo = new ServoMotor(pwmChannel, 180, 1000, 2000);
            servo.Start();
            Console.WriteLine("Ready!");
            try
            {
                while (true)
                {
                    var angle = Int32.Parse(Console.ReadLine());
                    servo.WriteAngle(angle);
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Quitting!");
            }
           
            servo.Stop();
            pwmChannel.Stop();
            servo.Dispose();
            pwmChannel.Dispose();
        }
    }
}
