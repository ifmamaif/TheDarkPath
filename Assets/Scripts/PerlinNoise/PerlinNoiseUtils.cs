namespace PerlinNoise
{
    public static class PerlinNoiseUtils
    {

        // Hash lookup table as defined by Ken Perlin.  This is a randomly
        // arranged array of all numbers from 0-255 inclusive.
        // Doubled permutation to avoid overflow
        /*
         * Permutation table. This is just a random jumble of all numbers 0-255,
         * repeated twice to avoid wrapping the index at 255 for each lookup.
         * This needs to be exactly the same for all instances on all platforms,
         * so it's easiest to just keep it as static explicit data.
         * This also removes the need for any initialization of this class.
         *
         * Note that making this an int[] instead of a char[] might make the
         * code run faster on platforms with a high penalty for unaligned single
         * byte addressing. Intel x86 is generally single-byte-friendly, but
         * some other CPUs are faster with 4-aligned reads.
         * However, a char[] is smaller, which avoids cache trashing, and that
         * is probably the most important aspect on most architectures.
         * This array is accessed a *lot* by the noise functions.
         * A vector-valued noise over 3D accesses it 96 times, and a
         * float-valued 4D noise 64 times. We want this to fit in the cache!
         */
        public static readonly int[] HashTable = new int[]
        {   // First 256 values between [0,255]
	        151,    160,    137,    91,     90,     15,     131,    13,     201,    95,     96,     53,     194,    233,    7,      225,
            140,    36,     103,    30,     69,     142,    8,      99,     37,     240,    21,     10,     23,     190,    6,      148,
            247,    120,    234,    75,     0,      26,     197,    62,     94,     252,    219,    203,    117,    35,     11,     32,
            57,     177,    33,     88,     237,    149,    56,     87,     174,    20,     125,    136,    171,    168,    68,     175,
            74,     165,    71,     134,    139,    48,     27,     166,    77,     146,    158,    231,    83,     111,    229,    122,
            60,     211,    133,    230,    220,    105,    92,     41,     55,     46,     245,    40,     244,    102,    143,    54,
            65,     25,     63,     161,    1,      216,    80,     73,     209,    76,     132,    187,    208,    89,     18,     169,
            200,    196,    135,    130,    116,    188,    159,    86,     164,    100,    109,    198,    173,    186,    3,      64,
            52,     217,    226,    250,    124,    123,    5,      202,    38,     147,    118,    126,    255,    82,     85,     212,
            207,    206,    59,     227,    47,     16,     58,     17,     182,    189,    28,     42,     223,    183,    170,    213,
            119,    248,    152,    2,      44,     154,    163,    70,     221,    153,    101,    155,    167,    43,     172,    9,
            129,    22,     39,     253,    19,     98,     108,    110,    79,     113,    224,    232,    178,    185,    112,    104,
            218,    246,    97,     228,    251,    34,     242,    193,    238,    210,    144,    12,     191,    179,    162,    241,
            81,     51,     145,    235,    249,    14,     239,    107,    49,     192,    214,    31,     181,    199,    106,    157,
            184,    84,     204,    176,    115,    121,    50,     45,     127,    4,      150,    254,    138,    236,    205,    93,
            222,    114,    67,     29,     24,     72,     243,    141,    128,    195,    78,     66,     215,    61,     156,    180,
	        // To eliminate verifying indices, we double the buffer of the array.
	        // The second 256 values between (duplicated)
	        151,   160,    137,    91,     90,     15,     131,    13,     201,    95,     96,     53,     194,    233,    7,      225,
            140,   36,     103,    30,     69,     142,    8,      99,     37,     240,    21,     10,     23,     190,    6,      148,
            247,   120,    234,    75,     0,      26,     197,    62,     94,     252,    219,    203,    117,    35,     11,     32,
            57,    177,    33,     88,     237,    149,    56,     87,     174,    20,     125,    136,    171,    168,    68,     175,
            74,    165,    71,     134,    139,    48,     27,     166,    77,     146,    158,    231,    83,     111,    229,    122,
            60,    211,    133,    230,    220,    105,    92,     41,     55,     46,     245,    40,     244,    102,    143,    54,
            65,    25,     63,     161,    1,      216,    80,     73,     209,    76,     132,    187,    208,    89,     18,     169,
            200,   196,    135,    130,    116,    188,    159,    86,     164,    100,    109,    198,    173,    186,    3,      64,
            52,    217,    226,    250,    124,    123,    5,      202,    38,     147,    118,    126,    255,    82,     85,     212,
            207,   206,    59,     227,    47,     16,     58,     17,     182,    189,    28,     42,     223,    183,    170,    213,
            119,   248,    152,    2,      44,     154,    163,    70,     221,    153,    101,    155,    167,    43,     172,    9,
            129,   22,     39,     253,    19,     98,     108,    110,    79,     113,    224,    232,    178,    185,    112,    104,
            218,   246,    97,     228,    251,    34,     242,    193,    238,    210,    144,    12,     191,    179,    162,    241,
            81,    51,     145,    235,    249,    14,     239,    107,    49,     192,    214,    31,     181,    199,    106,    157,
            184,   84,     204,    176,    115,    121,    50,     45,     127,    4,      150,    254,    138,    236,    205,    93,
            222,   114,    67,     29,     24,     72,     243,    141,    128,    195,    78,     66,     215,    61,     156,    180,
        };

        public static readonly int[][] g_GRAD3_Simplex = new int[][]
        {
            new int[] {  1, 1, 0},
            new int[] { -1, 1, 0},
            new int[] {  1,-1, 0},
            new int[] { -1,-1, 0},

            new int[] {  1, 0, 1},
            new int[] { -1, 0, 1},
            new int[] {  1, 0,-1},
            new int[] { -1, 0,-1},

            new int[] {  0, 1, 1},
            new int[] {  0,-1, 1},
            new int[] {  0, 1,-1},
            new int[] {  0,-1,-1},
        };

        public static readonly int[][] g_VECTORI_DIRECTIE = new int[][]
        {
            new int[] { 0, 1, 1},
            new int[] { 0, 1,-1},
            new int[] { 0,-1, 1},
            new int[] { 0,-1,-1},

            new int[] { 1, 0, 1},
            new int[] { 1, 0,-1},
            new int[] {-1, 0, 1},
            new int[] {-1, 0,-1},

            new int[] { 1, 1, 0},
            new int[] { 1,-1, 0},
            new int[] {-1, 1, 0},
            new int[] {-1,-1, 0},
        };

        public static float BlurCurve(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public static float HermitMixture(float t)
        {
            return (t * t) * (3 - 2 * t);
        }

        public static float Gradient1D(int hash, float x)
        {
            ///This is a faster implementation of the original gradient hash bit value:
            //int h = hash & 15;
            //float grad = 1.0f + (h & 7);
            //if ((h & 8) != 0) grad = -grad;
            //return (grad * x);

            return (hash & 0xF) switch
            {
                0x0 => x,
                0x1 => 2 * x,
                0x2 => 3 * x,
                0x3 => 4 * x,
                0x4 => 5 * x,
                0x5 => 6 * x,
                0x6 => 7 * x,
                0x7 => 8 * x,
                0x8 => -1 * x,
                0x9 => -2 * x,
                0xA => -3 * x,
                0xB => -4 * x,
                0xC => -5 * x,
                0xD => -6 * x,
                0xE => -7 * x,
                0xF => -8 * x,
                _ => 0,// never happens
            };
        }

        public static float Gradient2D(int hash, float x, float y)
        {
            ///	Calcularea gradientului pe un colț al cubului

            int CODUL_HASH = hash & 63;                                   // Constrângere la 6 biți pentru codul hash.
            float COORDONATA1 = CODUL_HASH < 4 ? x : y;                   // Se ia coordonată x sau y în funcție de codul hash
            float COORDONATA2 = CODUL_HASH < 4 ? y : x;

            return ((CODUL_HASH & 1) != 0 ? -COORDONATA1 : COORDONATA1) +
                ((CODUL_HASH & 2) != 0 ? -2.0f * COORDONATA2 : 2.0f * COORDONATA2);  // Calcularea produsului scalar cu x și y
        }

        public static float Gradient2D_Imbunatit(int codulHash, float x, float y)
        {
            codulHash &= 0xF;

            switch (codulHash)
            {
                case 0x0: return x + 2 * y;
                case 0x1: return -x + 2 * y;
                case 0x2: return x - 2 * y;
                case 0x3: return -x - 2 * y;
                default:
                    break;
            }

            codulHash &= 3;

            return codulHash switch
            {
                0x0 => 2 * x + y,
                0x1 => 2 * x - y,
                0x2 => -2 * x + y,
                0x3 => -2 * x - y,
                _ => 0,
            };
        }

        public static float Gradient3D(int hash, float x, float y, float z)
        {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v;
            if (h < 4)
                v = y;
            else if (h == 12 || h == 14)
                v = x;
            else
                v = z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        public static float Gradient3D_Improved(int hash, float x, float y, float z)
        {
            return (hash & 0xF) switch
            {
                0x0 =>  x + y,
                0x1 => -x + y,
                0x2 =>  x - y,
                0x3 => -x - y,
                0x4 =>  x + z,
                0x5 => -x + z,
                0x6 =>  x - z,
                0x7 => -x - z,
                0x8 =>  y + z,
                0x9 => -y + z,
                0xA =>  y - z,
                0xB => -y - z,
                0xC =>  y + x,
                0xD => -y + z,
                0xE =>  y - x,
                0xF => -y - z,
                _ => 0,// never happens
            };
        }

        static readonly float EPSILON = 1e-7F;

        public static float Abs(float x)
        {
            return x > 0 ? x : -x;
        }

        public static double Abs(double x)
        {
            return x > 0 ? x : -x;
        }

        public static int AsInteger(float f)
        {
            int integer_f = (int)f;
            if (Abs(f - integer_f) > EPSILON)
            {
                return f < 0 ? integer_f + 1 : integer_f;
            }
            return integer_f;
        }

        public static int AsInteger(double f)
        {
            int integer_d = (int)f;
            if (Abs(f - integer_d) > EPSILON)
            {
                return f < 0 ? integer_d + 1 : integer_d;
            }
            return integer_d;
        }

        public static float LinearInterpolation(float a, float b, float greutate)
        {
            return a + greutate * (b - a);
        }

        public static int AsIntegerFast(float d)
        {
            return d > 0 ? (int)d : (int)d - 1;
        }

        public static float ProdusScalarf(float x, float y)
        {
            return x * y;
        }

        public static float ProdusScalarVector(int nDim, int[] g, float[] x)
        {
            float result = 0;
            for (int i = 0; i < nDim; i++)
            {
                result += (g[i] * x[i]);
            }
            return result;
        }

        public static int VerificareBit(int number, int bitPosition)
        {
            int bitChecker = 1 << bitPosition;
            int si = number & bitChecker;
            int result = si > 0 ? 1 : 0;
            return result;
        }
    }
}