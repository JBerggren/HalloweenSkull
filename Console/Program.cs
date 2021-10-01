using Iot.Device.ServoMotor;
using NetCoreAudio;
using System;
using System.Device.Gpio;
using System.Device.Pwm.Drivers;
using System.Threading;
using System.Threading.Tasks;

namespace HalloweenSkull
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GpioController controller = new GpioController();
            controller.OpenPin(5, PinMode.Output);
            controller.Write(5, PinValue.Low);
            while (true)
            {
                Console.WriteLine("What would you like to do?\n1 {angle}- Control servo\n2 {on/off} - Toggle light\n3 {file} - Play sound\n4 - Laugh\nq - Quit");
                var choice = Console.ReadLine().Split(new char[] { ' ' });
                switch (choice[0])
                {
                    case "1":
                        var angle = int.Parse(choice[1]);
                        TurnServo(angle);
                        break;
                    case "2":
                        var on = choice[1] == "on";
                        ToggleLights(controller, on);
                        break;
                    case "3":
                        var file = choice[1];
                        PlaySound(file);
                        break;
                    case "4":
                        PlaySound("laugh.mp3");
                        var stopBlink = new ManualResetEventSlim();
                        BlinkLights(controller, stopBlink);
                        TurnServo(180);
                        TurnServo(0);
                        TurnServo(180);
                        TurnServo(0);
                        TurnServo(180);
                        TurnServo(0);
                        stopBlink.Set();
                        break;
                    case "q":
                        return;
                }
            }
        }

        private static void BlinkLights(GpioController controller, ManualResetEventSlim resetEvent)
        {
            Task.Run(() =>
            {
                while (!resetEvent.IsSet)
                {
                    controller.Write(5, PinValue.High);
                    Thread.Sleep(100);
                    controller.Write(5, PinValue.Low);
                    Thread.Sleep(100);
                }
                controller.Write(5, PinValue.Low);
            });
        }

        private static void PlaySound(string file)
        {
            var player = new Player();
            player.Play(file);
        }

        private static void TurnServo(int angle)
        {
            var pwmChannel = new SoftwarePwmChannel(18, 50, 0, true);
            var servo = new ServoMotor(pwmChannel, 180, 1000, 2000);
            servo.Start();
            servo.WriteAngle(angle);
            Thread.Sleep(300);
            servo.Stop();
            pwmChannel.Stop();
            servo.Dispose();
            pwmChannel.Dispose();
        }        

        private static void ToggleLights(GpioController controller, bool on)
        {
            controller.Write(5, on ? PinValue.High : PinValue.Low);
        }
    }
}
