namespace PerlinNoise
{
    public static partial class PerlinNoise
    {
        public static readonly int[][] SIMPLEX_TABLE = new int[][]   //64, 4
        {
            new int[]{0,1,2,3}, new int[]{0,1,3,2}, new int[]{0,0,0,0}, new int[]{0,2,3,1}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{1,2,3,0},
            new int[]{0,2,1,3}, new int[]{0,0,0,0}, new int[]{0,3,1,2}, new int[]{0,3,2,1}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{1,3,2,0},
            new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0},
            new int[]{1,2,0,3}, new int[]{0,0,0,0}, new int[]{1,3,0,2}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{2,3,0,1}, new int[]{2,3,1,0},
            new int[]{1,0,2,3}, new int[]{1,0,3,2}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{2,0,3,1}, new int[]{0,0,0,0}, new int[]{2,1,3,0},
            new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0},
            new int[]{2,0,1,3}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{3,0,1,2}, new int[]{3,0,2,1}, new int[]{0,0,0,0}, new int[]{3,1,2,0},
            new int[]{2,1,0,3}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{0,0,0,0}, new int[]{3,1,0,2}, new int[]{0,0,0,0}, new int[]{3,2,0,1}, new int[]{3,2,1,0}
        };

        public static readonly int[][] grad4 = new int[][]          //32, 4
        {
            new int[]{ 0, 1,1,1} , new int[]{ 0, 1, 1,-1}, new int[]{ 0, 1,-1,1}, new int[]{ 0, 1,-1,-1},
            new int[]{ 0,-1,1,1} , new int[]{ 0,-1, 1,-1}, new int[]{ 0,-1,-1,1}, new int[]{ 0,-1,-1,-1},
            new int[]{ 1, 0,1,1} , new int[]{ 1, 0, 1,-1}, new int[]{ 1, 0,-1,1}, new int[]{ 1, 0,-1,-1},
            new int[]{-1, 0,1,1} , new int[]{-1, 0, 1,-1}, new int[]{-1, 0,-1,1}, new int[]{-1, 0,-1,-1},
            new int[]{ 1, 1,0,1} , new int[]{ 1, 1, 0,-1}, new int[]{ 1,-1, 0,1}, new int[]{ 1,-1, 0,-1},
            new int[]{-1, 1,0,1} , new int[]{-1, 1, 0,-1}, new int[]{-1,-1, 0,1}, new int[]{-1,-1, 0,-1},
            new int[]{ 1, 1,1,0} , new int[]{ 1, 1,-1, 0}, new int[]{ 1,-1, 1,0}, new int[]{ 1,-1,-1, 0},
            new int[]{-1, 1,1,0} , new int[]{-1, 1,-1, 0}, new int[]{-1,-1, 1,0}, new int[]{-1,-1,-1, 0}
        };

        public static float Dot(int dim, int[] g, float[] values)
        {
            float sum = 0;
            for(int i=0;i< dim;i++)
            {
                sum += g[i] * values[i];
            }
            return sum;
        }

        public static float Sum(this float[] array, int dim)
        {
            float result = 0;
            for (int i = 0; i < dim; i++)
            {
                result += array[i];
            }
            return result;
        }

        public static int Sum(this int[] array, int dim)
        {
            int result = 0;
            for (int i = 0; i < dim; i++)
            {
                result += array[i];
            }
            return result;
        }

        public static float ContributionCorner(int dim, int gi, float[] values )
        {
            if(dim < 2 || dim > 4)
            {
                return 0.0f;
            }

            float t0 = 0.5f;
            for(int i=0; i<dim;i++)
            {
                t0 -= values[i] * values[i];
            }

            if(t0 < 0)
            {
                return 0.0f;
            }

            t0 *= t0;
            t0 *= t0;
            int[] gradientVect;
            
            switch(dim)
            {
                case 2:
                case 3:
                    gradientVect = PerlinNoiseUtils.g_GRAD3_Simplex[gi];
                    break;
                case 4:
                    gradientVect = grad4[gi];
                    break;
                //never
                default:
                    gradientVect = new int[1];
                    break;
            };
            return t0 * Dot(dim, gradientVect, values);

        }

        public static float PerlinNoiseSimplex(float x)
        {
            const int DIM = 2;

            int[] i01 = new int[DIM];
            i01[0] = PerlinNoiseUtils.AsIntegerFast(x);
            i01[1] = i01[0] + 1;

            float[] x01 = new float[DIM];
            x01[0] = x - i01[0];
            x01[1] = x01[0] - 1.0f;

            float[] nt01 = new float[DIM];
            for(int i=0;i<DIM;i++)
            {
                nt01[i] = 1.0f - x01[i] * x01[i];
                nt01[i] *= nt01[i];
                nt01[i] *= nt01[i];
                nt01[i] = nt01[i] * PerlinNoiseUtils.Gradient1D(PerlinNoiseUtils.HashTable[i01[i]], x01[i]);
            }

            return 0.395f * nt01.Sum(2);
        }

        public static float PerlinNoiseSimplex(float x, float y)
        {
            const int DIM = 2;
            const int CORNERS = 3;
            float[] inputs = new float[] { x, y };

            const float FACTOR_SKEW = 0.36602540378f;  // 0.5 * (sqrt(3.0) - 1.0);
            float valueSkewed = inputs.Sum(DIM) * FACTOR_SKEW;
            int[] inputsInt = new int[]
            {
                PerlinNoiseUtils.AsIntegerFast(x + valueSkewed),
                PerlinNoiseUtils.AsIntegerFast(y + valueSkewed),
            };

            float FACTOR_UNSKEW = 0.2113248654f;    // (3.0 - sqrt(3.0)) / 6.0;
            float valueUnskewed = inputsInt.Sum(DIM) * FACTOR_UNSKEW;

            float[][] xy = new float[CORNERS][]
            {
                new float[DIM],
                new float[DIM],
                new float[DIM],
            };

            xy[0][0] = inputs[0] - inputsInt[0] - valueUnskewed;
            xy[0][1] = inputs[1] - inputsInt[1] - valueUnskewed;

            bool decalajBool = xy[0][0] > xy[0][1];
            int decalajX = decalajBool? 1 : 0,
                decalajY = decalajBool? 0 : 1;

            xy[1][0] = xy[0][0] - decalajX + FACTOR_UNSKEW;
            xy[1][1] = xy[0][1] - decalajY + FACTOR_UNSKEW;
            xy[2][0] = xy[0][0] - 1.0f + 2.0f * FACTOR_UNSKEW;
            xy[2][0] = xy[0][1] - 1.0f + 2.0f * FACTOR_UNSKEW;

            inputsInt[0] &= 255;
            inputsInt[1] &= 255;

            int[][] offset = new int[CORNERS][]
            {
                new int[]{ 0, 0 },
                new int[]{ decalajX, decalajY },
                new int[]{ 1, 1 },
            };

            float[] noiseContributions = new float[CORNERS];
            for(int index=0; index < CORNERS;index++)
            {
                int gradient = PerlinNoiseUtils.HashTable[inputsInt[0] + offset[index][0] +
                               PerlinNoiseUtils.HashTable[inputsInt[1] + offset[index][1]]] % 12;

                noiseContributions[index] = ContributionCorner(DIM, gradient, xy[index]);
            };

            return 70.0f * noiseContributions.Sum(CORNERS);
        }

        // 3D simplex noise
        public static float PerlinNoiseSimplex(float xin, float yin, float zin)
        {
            const int CORNERS = 4;
            const int dim = 3;
            float[] input = new float[] { xin, yin, zin };


            float F3 = 0.33333333333f; // 1.0f / 3.0f;
                                       // Very nice and simple skew factor for 3D
            float s = input.Sum(3) * F3;
            int[] ijk = new int[dim];
            for (int index = 0; index < dim; index++)
            {
                ijk[index] = PerlinNoiseUtils.AsIntegerFast(input[index] + s);
            }

            float G3 = 0.16666666666f;
            float t = ijk.Sum(3) * G3;

            float[][] xyz = new float[4][]
            {
                new float[3],
                new float[3],
                new float[3],
                new float[3],
            };

            xyz[0][0] = input[0] - ijk[0] + t;
            xyz[0][1] = input[1] - ijk[1] + t;
            xyz[0][2] = input[2] - ijk[2] + t;

            bool x0Marey0 = xyz[0][0] >= xyz[0][1];
            bool y0Marez0 = xyz[0][1] >= xyz[0][2];
            bool x0Marez0 = xyz[0][0] >= xyz[0][2];

            int i1 = (x0Marey0 && x0Marez0) ? 1 : 0;
            int j1 = (!x0Marey0 && y0Marez0) ? 1 : 0;
            int k1 = (!(x0Marey0 || x0Marez0)) ? 1 : 0;
            int i2 = (x0Marey0 || x0Marez0) ? 1 : 0;
            int j2 = (!x0Marey0 || y0Marez0) ? 1 : 0;
            int k2 = (!(y0Marez0 && x0Marez0)) ? 1 : 0;

            xyz[1][0] = xyz[0][0] - i1 + G3;
            xyz[1][1] = xyz[0][1] - j1 + G3;
            xyz[1][2] = xyz[0][2] - k1 + G3;

            xyz[2][0] = xyz[0][0] - i2 + 2.0f * G3;
            xyz[2][1] = xyz[0][1] - j2 + 2.0f * G3;
            xyz[2][2] = xyz[0][2] - k2 + 2.0f * G3;

            xyz[3][0] = xyz[0][0] - 1.0f + 3.0f * G3;
            xyz[3][1] = xyz[0][1] - 1.0f + 3.0f * G3;
            xyz[3][2] = xyz[0][2] - 1.0f + 3.0f * G3;

            ijk[0] &= 255;
            ijk[1] &= 255;
            ijk[2] &= 255;

            int[] gradients = new int[]
            {
                PerlinNoiseUtils.HashTable[ijk[0] + PerlinNoiseUtils.HashTable[ijk[1] + PerlinNoiseUtils.HashTable[ijk[2]]]] % 12,
                PerlinNoiseUtils.HashTable[ijk[0] + i1 + PerlinNoiseUtils.HashTable[ijk[1] + j1 + PerlinNoiseUtils.HashTable[ijk[2] + k1]]] % 12,
                PerlinNoiseUtils.HashTable[ijk[0] + i2 + PerlinNoiseUtils.HashTable[ijk[1] + j2 + PerlinNoiseUtils.HashTable[ijk[2] + k2]]] % 12,
                PerlinNoiseUtils.HashTable[ijk[0] + 1 + PerlinNoiseUtils.HashTable[ijk[1] + 1 + PerlinNoiseUtils.HashTable[ijk[2] + 1]]] % 12,
            };

            float[] noiseContributions = new float[]
            {
                ContributionCorner(3, gradients[0], xyz[0]),
                ContributionCorner(3, gradients[1], xyz[1]),
                ContributionCorner(3, gradients[2], xyz[2]),
                ContributionCorner(3, gradients[3], xyz[3]),
            };

            return 32.0f * noiseContributions.Sum(CORNERS);
        }

        public static float SimplexNoiseSimplex(float x, float y, float z, float w)
        {
            int dim = 4;
            float[] input = new float[] { x, y, z, w };
            float F4 = 0.30901699437f;
            float G4 = 0.13819660112f;

            float s = input.Sum(4) * F4;
            int[] tableIndices = new int[4];
            for (int index = 0; index < dim; index++)
            {
                tableIndices[index] = PerlinNoiseUtils.AsIntegerFast(input[index] + s);
            }

            float t = tableIndices.Sum(4) * G4;
            float[][] xyzw = new float[5][]
            {
                new float[dim],
                new float[dim],
                new float[dim],
                new float[dim],
                new float[dim],
            };

            for (int index = 0; index < dim; index++)
            {
                xyzw[0][index] = input[index] - tableIndices[index] - t;
            }

            int c1 = (xyzw[0][0] > xyzw[0][1]) ? 32 : 0;
            int c2 = (xyzw[0][0] > xyzw[0][2]) ? 16 : 0;
            int c3 = (xyzw[0][1] > xyzw[0][2]) ? 8 : 0;
            int c4 = (xyzw[0][0] > xyzw[0][3]) ? 4 : 0;
            int c5 = (xyzw[0][1] > xyzw[0][3]) ? 2 : 0;
            int c6 = (xyzw[0][2] > xyzw[0][3]) ? 1 : 0;
            int c = c1 + c2 + c3 + c4 + c5 + c6;

            int[][] ijkl = new int[5][]
            {
                new int[4] { 0, 0, 0, 0 },
                new int[4],
                new int[4],
                new int[4],
                new int[4] { 1, 1, 1, 1 },
            };

            for (int index = 1; index < 4; index++)
            {
                for (int iter = 0; iter < dim; iter++)
                {
                    ijkl[index][iter] = SIMPLEX_TABLE[c][iter] >= (4 - index) ? 1 : 0;
                    xyzw[index][iter] = xyzw[0][iter] - ijkl[index][iter] + index * G4;
                }
            }

            for (int index = 0; index < dim; index++)
            {
                xyzw[4][index] = xyzw[0][index] - 1.0f + 4.0f * G4;
                tableIndices[index] &= 255;
            }

            int cornerNumber = 5;
            int[] gi = new int[cornerNumber];
            float[] noiseContributions = new float[cornerNumber];
            for (int index = 0; index < cornerNumber; index++)
            {
                gi[index] = PerlinNoiseUtils.HashTable[tableIndices[0] + ijkl[index][0] +
                            PerlinNoiseUtils.HashTable[tableIndices[1] + ijkl[index][1] +
                            PerlinNoiseUtils.HashTable[tableIndices[2] + ijkl[index][2] +
                            PerlinNoiseUtils.HashTable[tableIndices[3] + ijkl[index][3]]]]] % 32;

                noiseContributions[index] = ContributionCorner(4, gi[index], xyzw[index]);
            }

            return 27.0f * noiseContributions.Sum(cornerNumber);
        }
    }
}