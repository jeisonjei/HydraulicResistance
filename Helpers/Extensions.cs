using System;
namespace HydraulicResistance.Helpers
{
    public static class Extensions{
        public static double Grade(this double v,double grade){
            return Math.Pow(v,grade);
        }
    }
}