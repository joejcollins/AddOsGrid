/**
 * Some utility functions used by classes in the uk.me.jstott.jcoord package.
 * 
 * (c) 2006 Jonathan Stott
 * 
 * Created on 11-Feb-2006
 * 
 * @author Jonathan Stott
 * @version 1.0
 * @since 1.0
 */
using System;

namespace Ncoord
{
    internal class Util
    {
        /**
         * Calculate sin^2(x).
         * 
         * @param x
         *          x
         * @return sin^2(x)
         * @since 1.0
         */
        public static double sinSquared(double x)
        {
            return Math.Sin(x) * Math.Sin(x);
        }

        /**
         * Calculate cos^2(x).
         * 
         * @param x
         *          x
         * @return cos^2(x)
         * @since 1.0
         */
        public static double cosSquared(double x)
        {
            return Math.Cos(x) * Math.Cos(x);
        }

        /**
         * Calculate tan^2(x).
         * 
         * @param x
         *          x
         * @return tan^2(x)
         * @since 1.0
         */
        public static double tanSquared(double x)
        {
            return Math.Tan(x) * Math.Tan(x);
        }

        /**
         * Calculate sec(x).
         * 
         * @param x
         *          x
         * @return sec(x)
         * @since 1.0
         */
        public static double sec(double x)
        {
            return 1.0 / Math.Cos(x);
        }

        /**
         * Convert from Degrees to Radians
         * 
         * 
         */
        public static double ToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }

        /**
         * Convert from Radians to Degrees
         * 
         * 
         */
        public static double ToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }
    }
}