using UnityEngine;

namespace Assets.Scripts.NNoise
{
    public interface INoiseMaker
    {
        float Make_2D(Vector2 point, bool GetPositiveRes = true);
        float Make_2D(Vector2 point, float frequency, float amplitude = 1.0f, bool GetPositiveRes = true);
        float MakeOctave_2D(Vector2 point, float frequency, float amplitude, int octave = 8, bool GetPositiveRes = true);

        float Make_3D(Vector3 point, bool GetPositiveRes = true);
    }
}
