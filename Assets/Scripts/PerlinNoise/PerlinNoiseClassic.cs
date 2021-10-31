namespace PerlinNoise
{
    public static partial class PerlinNoise
    {
        public static float PerlinNoiseClassic(float x)
        {
            int xInt = PerlinNoiseUtils.AsIntegerFast(x);
            x -= xInt;
            xInt &= 255;

            float vectorDirectieA = PerlinNoiseUtils.g_VECTORI_DIRECTIE[PerlinNoiseUtils.HashTable[xInt] % 12][0];
            float vectorDirectieB = PerlinNoiseUtils.g_VECTORI_DIRECTIE[PerlinNoiseUtils.HashTable[xInt + 1] % 12][0];

            float produsScalarA = PerlinNoiseUtils.ProdusScalarf(vectorDirectieA, x);
            float produsScalarB = PerlinNoiseUtils.ProdusScalarf(vectorDirectieB, x - 1);

            float amestecare = PerlinNoiseUtils.HermitMixture(x);

            float li = PerlinNoiseUtils.LinearInterpolation(produsScalarA, produsScalarB, amestecare);

            return li;
        }

        public static float PerlinNoiseClassic(float x, float y)
        {
            int xInt = PerlinNoiseUtils.AsIntegerFast(x);
            int yInt = PerlinNoiseUtils.AsIntegerFast(y);

            x -= xInt;
            y -= yInt;

            xInt &= 255;
            yInt &= 255;

            int gradient1 = PerlinNoiseUtils.HashTable[xInt + PerlinNoiseUtils.HashTable[yInt]] % 12;
            int gradient2 = PerlinNoiseUtils.HashTable[xInt + PerlinNoiseUtils.HashTable[yInt + 1]] % 12;
            int gradient3 = PerlinNoiseUtils.HashTable[xInt + 1 + PerlinNoiseUtils.HashTable[yInt]] % 12;
            int gradient4 = PerlinNoiseUtils.HashTable[xInt + 1 + PerlinNoiseUtils.HashTable[yInt + 1]] % 12;

            float xMinim = x - 1;
            float yMinim = y - 1;

            float contributieZgomot1 = PerlinNoiseUtils.ProdusScalarVector(2, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradient1], new float[] { x, y });
            float contributieZgomot2 = PerlinNoiseUtils.ProdusScalarVector(2, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradient2], new float[] { x, yMinim });
            float contributieZgomot3 = PerlinNoiseUtils.ProdusScalarVector(2, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradient3], new float[] { xMinim, y });
            float contributieZgomot4 = PerlinNoiseUtils.ProdusScalarVector(2, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradient4], new float[] { xMinim, yMinim });

            float valoareEstompata1 = PerlinNoiseUtils.HermitMixture(x);
            float valoareEstompata2 = PerlinNoiseUtils.HermitMixture(y);

            float noise1 = PerlinNoiseUtils.LinearInterpolation(contributieZgomot1, contributieZgomot3, valoareEstompata1);
            float noise2 = PerlinNoiseUtils.LinearInterpolation(contributieZgomot2, contributieZgomot4, valoareEstompata1);
            float liFinal = PerlinNoiseUtils.LinearInterpolation(noise1, noise2, valoareEstompata2);

            return liFinal;
        }

        public static float FadeOriginal(float x)
        {
            return (3 - 2 * x) * x * x;
        }

        public static float PerlinNoiseClassic(float x, float y, float z)
        {
            int xInt = PerlinNoiseUtils.AsIntegerFast(x),
                yInt = PerlinNoiseUtils.AsIntegerFast(y),
                zInt = PerlinNoiseUtils.AsIntegerFast(z);

            x -= xInt;
            y -= yInt;
            z -= zInt;

            xInt &= 255;
            yInt &= 255;
            zInt &= 255;

            int gradientIndicesA  = PerlinNoiseUtils.HashTable[zInt    ],
                gradientIndicesB  = PerlinNoiseUtils.HashTable[zInt + 1],

                gradientIndicesAA = PerlinNoiseUtils.HashTable[yInt + gradientIndicesA],
                gradientIndicesAB = PerlinNoiseUtils.HashTable[yInt + gradientIndicesB],
                gradientIndicesBA = PerlinNoiseUtils.HashTable[yInt + 1 + gradientIndicesA],
                gradientIndicesBB = PerlinNoiseUtils.HashTable[yInt + 1 + gradientIndicesB],

                gradientIndicesAAA = PerlinNoiseUtils.HashTable[xInt + gradientIndicesAA] % 12,
                gradientIndicesAAB = PerlinNoiseUtils.HashTable[xInt + gradientIndicesAB] % 12,
                gradientIndicesABA = PerlinNoiseUtils.HashTable[xInt + gradientIndicesBA] % 12,
                gradientIndicesABB = PerlinNoiseUtils.HashTable[xInt + gradientIndicesBB] % 12,
                gradientIndicesBAA = PerlinNoiseUtils.HashTable[xInt + 1 + gradientIndicesAA] % 12,
                gradientIndicesBAB = PerlinNoiseUtils.HashTable[xInt + 1 + gradientIndicesAB] % 12,
                gradientIndicesBBA = PerlinNoiseUtils.HashTable[xInt + 1 + gradientIndicesBA] % 12,
                gradientIndicesBBB = PerlinNoiseUtils.HashTable[xInt + 1 + gradientIndicesBB] % 12;

            float xMin = x - 1,
                  yMin = y - 1,
                  zMin = z - 1;

            float noiseAAA = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesAAA], new float[] { x   , y   , z    }),
                  noiseAAB = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesAAB], new float[] { x   , y   , zMin }),
                  noiseABA = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesABA], new float[] { x   , yMin, z    }),
                  noiseABB = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesABB], new float[] { x   , yMin, zMin }),
                  noiseBAA = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesBAA], new float[] { xMin, y   , z    }),
                  noiseBAB = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesBAB], new float[] { xMin, y   , zMin }),
                  noiseBBA = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesBBA], new float[] { xMin, yMin, z    }),
                  noiseBBB = PerlinNoiseUtils.ProdusScalarVector(3, PerlinNoiseUtils.g_VECTORI_DIRECTIE[gradientIndicesBBB], new float[] { xMin, yMin, zMin }),

                  hmX = PerlinNoiseUtils.HermitMixture(x),
                  hmY = PerlinNoiseUtils.HermitMixture(y),
                  hmZ = PerlinNoiseUtils.HermitMixture(z),

                  liAA = PerlinNoiseUtils.LinearInterpolation(noiseAAA, noiseBAA, hmX),
                  liAB = PerlinNoiseUtils.LinearInterpolation(noiseAAB, noiseBAB, hmX),
                  liBA = PerlinNoiseUtils.LinearInterpolation(noiseABA, noiseBBA, hmX),
                  liBB = PerlinNoiseUtils.LinearInterpolation(noiseABB, noiseBBB, hmX),

                  liA = PerlinNoiseUtils.LinearInterpolation(liAA, liBA, hmY),
                  liB = PerlinNoiseUtils.LinearInterpolation(liAB, liBB, hmY),
                         
                  liFinal = PerlinNoiseUtils.LinearInterpolation(liA, liB, hmZ);

            return liFinal;
        }
    }
}