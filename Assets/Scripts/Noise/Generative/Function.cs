using System;
using UnityEngine;

namespace Noise {
    public class Function : Generator {
        protected readonly Func<float, float, Sample<float>> func_a;
        protected readonly Func<float, float, float, Sample<Vector2>> func_b;
        protected readonly Func<float, float, float, float, Sample<Vector3>> func_c;

        public Function(Func<float, float, Sample<float>> func_a, Func<float, float, float, Sample<Vector2>> func_b, Func<float, float, float, float, Sample<Vector3>> func_c) {
            this.func_a = func_a;
            this.func_b = func_b;
            this.func_c = func_c;
        }

        public override Sample<float> Get(float x, float frequency) {
            return func_a(x, frequency);
        }

        public override Sample<Vector2> Get(float x, float y, float frequency) {
            return func_b(x, y, frequency);
        }

        public override Sample<Vector3> Get(float x, float y, float z, float frequency) {
            return func_c(x, y, z, frequency);
        }
    }
}