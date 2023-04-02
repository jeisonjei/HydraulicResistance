using System;
using EngineeringUnits;
using EngineeringUnits.Units;
using SharpFluids;
using HydraulicResistance.Helpers;
namespace HydraulicResistance.ElementsOfResistance
{
    public class ConfuserPyramidal
    {
        /*
        Конические диффузоры. Сопротивление конфузора можно представить как сумму двух частей - 
        местного сопротивления и сопротивления трения.
        Формула для расчёта местного сопротивления конфузора едина для конических, пирамидальных и плоских конфузоров,
        а формулы для расчёта сопротивления трения различаются в зависимости от формы конфузора.
        Для расчёта сопротивления трения используются те же формулы, что и для расчёта сопротивления трения диффузоров
        */
        public static double Resistance(FluidList fluid,
                                        double tempCels,
                                        double equivalentRoughness, 
                                        double flowRateCubicMeterPerHour, 
                                        double widthBigMillimeter,
                                        double heightBigMillimeter, 
                                        double widthSmallMillimeter,
                                        double heightSmallMillimeter,
                                        double angleAlphaDegree,
                                        double angleBetaDegree){
            double ksi = default;
            double areaBigSquareMeter=Mathematics.GetAreaRectangle(widthBigMillimeter,heightBigMillimeter).As(AreaUnit.SquareMeter);
            double areaSmallSquareMeter = Mathematics.GetAreaRectangle(widthSmallMillimeter,heightSmallMillimeter).As(AreaUnit.SquareMeter);
            double dzetaM = _getDzetaM(areaBigSquareMeter,areaSmallSquareMeter,angleAlphaDegree);
            var flow=new Flow(fluid,flowRateCubicMeterPerHour,widthBigMillimeter,heightBigMillimeter); /* при расчёте lambda берётся число Re на входе, то есть используется диаметр бОльшего участка,
            страница 206 */
            double lambda=flow.GetLambda(tempCels,equivalentRoughness);
            double dzetaTr=_getDzetaTr(lambda:lambda,
            angleAlphaDegree:angleAlphaDegree,angleBetaDegree:angleBetaDegree,
            areaBigSquareMeter:areaBigSquareMeter,areaSmallSquareMeter:areaSmallSquareMeter);
            ksi = dzetaM + dzetaTr;
            return ksi;
        }
        private static double _getDzetaTr(double lambda, double angleAlphaDegree, double angleBetaDegree, double areaBigSquareMeter,double areaSmallSquareMeter)
        {
            /*
            Коэффициент сопротивления трения  пирамидального диффузора или конфузора ζтр
            Формула 5-8 на странице 193
            */
            double angleAlphaRadian = angleAlphaDegree * (Math.PI/180);
            double angleBetaRadian = angleBetaDegree * (Math.PI/180);
            double n = areaBigSquareMeter/areaSmallSquareMeter; /* здесь в отличие от функции _getDzetaM здесь большая площадь делится на меньшую */
            double dzetaTr = lambda / 16 * (1 - 1 / n.Grade(2)) * (1 / Math.Sin(angleAlphaRadian / 2) + 1 / Math.Sin(angleBetaRadian / 2));
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
            n = areaSmallSquareMeter/areaBigSquareMeter;
            double ksi = (-0.0125d * n.Grade(4) + 0.0224d * n.Grade(3) - 0.00723d * n.Grade(2) + 0.00444d * n - 0.00745d) * (a.Grade(3) - 2d * Math.PI * a.Grade(2) - 10d * a);
            return ksi;
        }

    }
}