using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerlinNoise
{
    public class Random
    {
        const uint ms_Scale = 1103515245;
        const uint ms_Offset = 12345;
        const uint ms_Module = 2147483647; // 2 << 31; INT_MAX

        static uint ms_Seed_s = InitSeed();
        static object gs_Mutex = new object();

        private uint seed;
        private readonly uint scale;
        private readonly uint offset;
        private readonly uint module;

        public Random(uint seedValue = 0, uint scaleValue = ms_Scale, uint offsetValue = ms_Offset, uint moduleValue = ms_Module)
        {
            seed = seedValue == 0 ? InitSeed() : seedValue;
            scale = scaleValue;
            offset = offsetValue;
            module = moduleValue;
        }

        public uint Rand()
        {
            return seed = ((scale * seed) + offset) % module;
        }

        public uint Rand(uint minInclusive, uint maxExclusive)
        {
            return minInclusive + (Rand() % (maxExclusive - minInclusive + 1));
        }

        // Random value between 0 and 1
        public float Rand01()
        {
            return ((float)Rand()) / (float)(ms_Module);
        }

        public uint NewSeed(uint newseed)
        {
            return seed = ((ms_Scale * newseed) + ms_Offset) % ms_Module;
        }

        private static uint InitSeed()
        {
            uint newSeed = (uint)
                (DateTime.Now.Second +
                DateTime.Now.Minute * 100 +
                 DateTime.Now.Hour * 10000 +
                 DateTime.Now.Day * 1000000 +
                 DateTime.Now.Month * 100000000);


            return newSeed;
        }

        public static uint Rand_s()
        {
            uint newSeed;
            lock (gs_Mutex)
            {
                newSeed = ms_Seed_s = ((ms_Scale * ms_Seed_s) + ms_Offset) % ms_Module;
            }
            return newSeed;
        }

        public static uint Rand_s(uint minInclusive, uint maxExclusive)
        {
            return minInclusive + (Rand_s() % (maxExclusive - minInclusive + 1));
        }

        public static float Rand01_s()
        {
            return ((float)Rand_s()) / (float)(ms_Module);
        }
    }
}