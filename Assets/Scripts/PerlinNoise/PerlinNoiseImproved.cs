using UnityEngine;

namespace PerlinNoise
{
    public static partial class PerlinNoise
    {
        public static float MutationValue(float x)
        {
            //return (x + 1) / 2;               // For convenience we bound it to 0 - 1 (theoretical min/max before is -1 - 1)
            return (x + 0.7f) / 1.5f;           // Magic value to obtain [-0.2,1.13] 
        }

        public static float PerlinNoiseImproved(float x, float y, float z)
        {
            int xInt = PerlinNoiseUtils.AsInteger((double)x),
                yInt = PerlinNoiseUtils.AsInteger((double)y),
                zInt = PerlinNoiseUtils.AsInteger((double)z);

            x -= xInt;
            y -= yInt;
            z -= zInt;

            xInt &= 255;
            yInt &= 255;
            zInt &= 255;

            int a  = PerlinNoiseUtils.HashTable[xInt],
                b  = PerlinNoiseUtils.HashTable[xInt + 1],

                aa = PerlinNoiseUtils.HashTable[a + yInt],
                ab = PerlinNoiseUtils.HashTable[a + yInt + 1],
                ba = PerlinNoiseUtils.HashTable[b + yInt],
                bb = PerlinNoiseUtils.HashTable[b + yInt + 1],

                aaa = PerlinNoiseUtils.HashTable[aa + zInt],
                baa = PerlinNoiseUtils.HashTable[ba + zInt],
                aba = PerlinNoiseUtils.HashTable[ab + zInt],
                bba = PerlinNoiseUtils.HashTable[bb + zInt],
                aab = PerlinNoiseUtils.HashTable[aa + zInt + 1],
                abb = PerlinNoiseUtils.HashTable[ab + zInt + 1],
                bab = PerlinNoiseUtils.HashTable[ba + zInt + 1],
                bbb = PerlinNoiseUtils.HashTable[bb + zInt + 1];

            float gradientAAA = PerlinNoiseUtils.Gradient3D(aaa, x    , y    , z    ),
                  gradientBAA = PerlinNoiseUtils.Gradient3D(baa, x - 1, y    , z    ),
                  gradientABA = PerlinNoiseUtils.Gradient3D(aba, x    , y - 1, z    ),
                  gradientBBA = PerlinNoiseUtils.Gradient3D(bba, x - 1, y - 1, z    ),
                  gradientAAB = PerlinNoiseUtils.Gradient3D(aab, x    , y    , z - 1),
                  gradientBAB = PerlinNoiseUtils.Gradient3D(bab, x - 1, y    , z - 1),
                  gradientABB = PerlinNoiseUtils.Gradient3D(abb, x    , y - 1, z - 1),
                  gradientBBB = PerlinNoiseUtils.Gradient3D(bbb, x - 1, y - 1, z - 1);

            float bcX = PerlinNoiseUtils.BlurCurve(x),
                  bcY = PerlinNoiseUtils.BlurCurve(y),
                  bcZ = PerlinNoiseUtils.BlurCurve(z),

                  liAA = PerlinNoiseUtils.LinearInterpolation(gradientAAA, gradientBAA, bcX),
                  liBA = PerlinNoiseUtils.LinearInterpolation(gradientABA, gradientBBA, bcX),
                  liAB = PerlinNoiseUtils.LinearInterpolation(gradientAAB, gradientBAB, bcX),
                  liBB = PerlinNoiseUtils.LinearInterpolation(gradientABB, gradientBBB, bcX),

                  liA = PerlinNoiseUtils.LinearInterpolation(liAA, liBA, bcY),
                  liB = PerlinNoiseUtils.LinearInterpolation(liAB, liBB, bcY),

                  liFinal = PerlinNoiseUtils.LinearInterpolation(liA, liB, bcZ);

            return liFinal;
        }

        public static float PerlinNoiseImproved(float x, float y)
        {
            int xInt = PerlinNoiseUtils.AsInteger((double)x),
                yInt = PerlinNoiseUtils.AsInteger((double)y);

            x -= xInt;
            y -= yInt;

            xInt &= 255;
            yInt &= 255;

            int a   = PerlinNoiseUtils.HashTable[xInt] + yInt,
                b   = PerlinNoiseUtils.HashTable[xInt + 1] + yInt,
                aa  = PerlinNoiseUtils.HashTable[a],
                ba  = PerlinNoiseUtils.HashTable[a + 1],
                ab  = PerlinNoiseUtils.HashTable[b],
                bb  = PerlinNoiseUtils.HashTable[b + 1];

            float gradientAA = PerlinNoiseUtils.Gradient2D(PerlinNoiseUtils.HashTable[aa], x    , y    ),
                  gradientAB = PerlinNoiseUtils.Gradient2D(PerlinNoiseUtils.HashTable[ab], x - 1, y    ),
                  gradientBA = PerlinNoiseUtils.Gradient2D(PerlinNoiseUtils.HashTable[ba], x    , y - 1),
                  gradientBB = PerlinNoiseUtils.Gradient2D(PerlinNoiseUtils.HashTable[bb], x - 1, y - 1),

                  bcX = PerlinNoiseUtils.BlurCurve(x),
                  bcY = PerlinNoiseUtils.BlurCurve(y),

                  noiseA     = PerlinNoiseUtils.LinearInterpolation(gradientAA, gradientAB, bcX),
                  noiseB     = PerlinNoiseUtils.LinearInterpolation(gradientBA, gradientBB, bcX),
                  noiseFinal = PerlinNoiseUtils.LinearInterpolation(noiseA, noiseB, bcY);

            return noiseFinal;
        }

        public static float PerlinNoiseImproved(float x)
        {
            int xInt = PerlinNoiseUtils.AsInteger((double)x);
            x -= xInt;
            xInt &= 255;

            int a = PerlinNoiseUtils.HashTable[PerlinNoiseUtils.HashTable[xInt]],
                b = PerlinNoiseUtils.HashTable[PerlinNoiseUtils.HashTable[xInt + 1]];

            float gradientA = PerlinNoiseUtils.Gradient1D(PerlinNoiseUtils.HashTable[a], x),
                  gradientB = PerlinNoiseUtils.Gradient1D(PerlinNoiseUtils.HashTable[b], x - 1),

                  bc = PerlinNoiseUtils.BlurCurve(x),

                  li = PerlinNoiseUtils.LinearInterpolation(gradientA, gradientB, bc);

            return li;
        }

        public static float[,] PerlinNoiseImproved(int width, int height)
        {
            const int perlinScale = 1;
            float perlinOffsetX = 0f;
            float perlinOffsetY = 0f;

            float[,] noiseMap = new float[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xCoord = (float)x / width * perlinScale + perlinOffsetX;
                    float yCoord = (float)y / height * perlinScale + perlinOffsetY;
                    noiseMap[x, y] = PerlinNoiseImproved(xCoord, yCoord);
                }
            }

            return noiseMap;
        }
    }
}