using System;

namespace KinectWPF.Helpers
{
    public static class RandomHelper
    {
        public static int GenerateRandomFruitType()
        {
            Random random = new Random();

            return random.Next(0, 3);
        }

        public static int GenerateRandomLeftPosition()
        {
            Random random = new Random();

            return random.Next(100, 900);
        }
    }
}
