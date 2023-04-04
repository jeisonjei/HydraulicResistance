using System;
using EngineeringUnits;
using EngineeringUnits.Units;
using HydraulicResistance.Helpers;
using SharpFluids;

namespace HydraulicResistance.ElementsOfResistance
{
    public class ConfuserFlat
    {
        /*
        Плоский конфузор - это конфузор с сужением только в одной плоскости
        */
        public static double Resistance(FluidList fluid,
                                        double tempCels,
                                        double equivalentRoughness,
                                        double flowRateCubicMeterPerHour,
                                        double widthBigMillimeter,
                                        double heightBigMillimeter,
                                        double widthSmallMillimeter,
                                        double heightSmallMillimeter,
                                        double angleDegree)
        {
            double ksi = default;
            double areaBigSquareMeter = Mathematics.GetAreaRectangle(widthBigMillimeter, heightBigMillimeter).As(AreaUnit.SquareMeter);
            double areaSmallSquareMeter = Mathematics.GetAreaRectangle(widthSmallMillimeter, heightSmallMillimeter).As(AreaUnit.SquareMeter);
            double dzetaM = _getDzetaM(areaBigSquareMeter, areaSmallSquareMeter, angleDegree);
            var flow = new Flow(fluid, flowRateCubicMeterPerHour, widthBigMillimeter, heightBigMillimeter); /* при расчёте lambda берётся число Re на входе, то есть используется диаметр бОльшего участка,
            страница 206 */
            double lambda = flow.GetLambda(tempCels, equivalentRoughness);
            double dzetaTr = _getDzetaTr(lambda: lambda,
            widthBigMillimeter: widthBigMillimeter, heightBigMillimeter: heightBigMillimeter,
            widthSmallMillimeter: widthSmallMillimeter, heightSmallMillimeter: heightSmallMillimeter,
            angleDegree: angleDegree);
            ksi = dzetaM + dzetaTr;
            return ksi;
        }
        private static double _getDzetaTr(double lambda, double widthBigMillimeter, double heightBigMillimeter, double widthSmallMillimeter, double heightSmallMillimeter, double angleDegree)
        {
            double dzetaTr = default;
            double angleRadian = angleDegree * (Math.PI / 180);
            double sinAlphaDividedBy2 = Math.Sin(angleRadian / 2);
            double a = sinAlphaDividedBy2;
            double a0 = widthBigMillimeter;
            double b0 = heightBigMillimeter;
            var areaBig = Mathematics.GetAreaRectangle(widthBigMillimeter, heightBigMillimeter).As(AreaUnit.SquareMeter);
            var areaSmall = Mathematics.GetAreaRectangle(widthBigMillimeter, heightBigMillimeter).As(AreaUnit.SquareMeter);
            double n = areaBig / areaSmall; /* в отличие от функции _getDzetaM здесь большая площадь делится на меньшую */
            dzetaTr = (lambda / (4 * a)) * ((a0 / b0) * (1 - 1 / n) + 0.5 * (1 - 1 / n.Grade(2)));
            return dzetaTr;
        }
        private static double _getDzetaM(double areaBigSquareMeter /* площадь сечения участка большего размера */
            , double areaSmallSquareMeter, /* площадь сечения участка меньшего размера */
             double angleDegree /* угол сужения */ )
        {
            /*
            Местное сопротивление конфузора - ζм из формулы пункта 95 на странице 206
            TODO: в расчёте сопротивления конфузоров почему-то пренебрегли частью ζтр - сопротивлением трения из формулы пункта 95 на странице 206.
            В пункте 95 сказано, что коэффициент сопротивления трения сужающегося участка определяется по формулам 5-6, 5-8, 5-9, 5-10
            */
            double n;
            double angleRadian;
            angleRadian = angleDegree * Math.PI / 180d;
            double a = angleRadian;
            n = areaSmallSquareMeter / areaBigSquareMeter; /* в отличие от случая с диффузорами, здесь площадь меньшего сечения делится на площадь большего, 
            то есть смысл величины n в том, на сколько нужно умножить начальное сечение чтобы получить конечное, для диффузоров это число > 0, а для конфузоров < 0, 
            так как сечение уменьшается*/
            double ksi = (-0.0125d * n.Grade(4) + 0.0224d * n.Grade(3) - 0.00723d * n.Grade(2) + 0.00444d * n - 0.00745d) * (a.Grade(3) - 2d * Math.PI * a.Grade(2) - 10d * a);
            return ksi;
        }

    }
}