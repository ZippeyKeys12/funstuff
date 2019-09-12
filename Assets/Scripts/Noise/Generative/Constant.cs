namespace ZNoise
{
    public class Constant : Generator
    {
        private readonly float a;

        public Constant(float a)
        {
            this.a = a;
        }

        public override Sample1D Get(float x, float frequency)
        {
            return a;
        }

        public override Sample2D Get(float x, float y, float frequency)
        {
            return a;
        }

        public override Sample3D Get(float x, float y, float z, float frequency)
        {
            return a;
        }
    }
}