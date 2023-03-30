using System;
using EngineeringUnits.Units;
using SharpFluids;
using HydraulicResistance.Helpers;

namespace HydraulicResistance.ElementsOfResistance
{
    public class ElbowRectangular
    {

        public static double Resistance( FluidList fluid,
                                                    double temperatureDegreesCelsius,
                                                    double equivalentRoughnessMillimeter,
                                                    double angleDegree,
                                                    double rd,
                                                    double flowRateCubicMeterPerHour,
                                                    double widthMillimeter,
                                                    double heightMillimeter){
            var flow=new Flow(fluid,flowRateCubicMeterPerHour,widthMillimeter,heightMillimeter);
            var re=flow.GetReinoldsNumber(temperatureDegreesCelsius);
            var lambda=flow.GetLambda(temperatureDegreesCelsius,equivalentRoughnessMillimeter);
            double ksi=_getElbowResistance(equivalentRoughnessMillimeter,angleDegree,rd,lambda,re,widthMillimeter,heightMillimeter,0);
            return ksi;
        }
        private static double _getElbowResistance(double equivalentRoughnessMillimeters, /* эквивалентная шероховатость, для стальных труб 0.2 мм, для воздуховодов 0.1 мм */
                            double angleDegrees /* угол поворота */,
                            double rd /* кривизна - отношение радиуса поворота к диаметру (или гидравлическому диаметру в случае прямоугольных труб)*/,
                            double lambda /* коэффициент сопротивления трения */, 
                            double re /* число Рейнольдса */, 
                            double widthMillimeters = 0d /* сторона A для прямоугольных труб или воздуховодов */, 
                            double heightMillimeters = 0d /* сторона B для прямоугольных труб или воздуховодов */, 
                            double diameterMillimeters = 0d /* диаметр для круглых труб или воздуховодов */)
        {
            /* Сопротивление колен и отводов, диаграмма 6-1 со страницы 277 
            */
            double dzetam;
            var A = default(double);
            var b = default(double);
            var c = default(double);
            var kre = default(double);
            var kd = default(double);
            double ln;
            double deltar;
            double elbowp;


            switch (angleDegrees)
            {
                case var @case when @case <= 70d:
                    {
                        A = 0.9d * Math.Sin(angleDegrees * Math.PI / 180d);
                        break;
                    }
                case var case1 when case1 > 70d:
                    {
                        switch (angleDegrees)
                        {
                            case var case2 when case2 <= 100d:
                                {
                                    A = 1d;
                                    break;
                                }
                            case var case3 when case3 > 100d:
                                {
                                    A = 0.7d + 0.35d * (angleDegrees / 90d);
                                    break;
                                }
                        }

                        break;
                    }
            }
            switch (rd)
            {
                case var case4 when case4 >= 0.5d:
                    {
                        switch (rd)
                        {
                            case var case5 when case5 <= 1d:
                                {
                                    b = 0.21d * Math.Pow(rd, -2.5d);
                                    break;
                                }
                            case var case6 when case6 > 1d:
                                {
                                    b = 0.21d * Math.Pow(rd, -0.5d);
                                    break;
                                }
                        }

                        break;
                    }
                case var case7 when case7 < 0.5d:
                    {
                        b = 0.21d * Math.Pow(rd, -2.5d);
                        break;
                    }
            }
            switch (diameterMillimeters)
            {
                case default(double):
                    {
                        switch (heightMillimeters / widthMillimeters)
                        {
                            case 1d:
                                {
                                    c = 1d;
                                    break;
                                }
                            case var case8 when case8 <= 4d:
                                {
                                    c = 0.85d + 0.125d / (heightMillimeters / widthMillimeters);
                                    break;
                                }
                            case var case9 when case9 > 4d:
                                {
                                    c = 1.115d - 0.84d / (heightMillimeters / widthMillimeters);
                                    break;
                                }
                        }

                        break;
                    }
                case var case10 when case10 == 0d:
                    {
                        switch (heightMillimeters / widthMillimeters)
                        {
                            case 1d:
                                {
                                    c = 1d;
                                    break;
                                }
                            case var case11 when case11 <= 4d:
                                {
                                    c = 0.85d + 0.125d / (heightMillimeters / widthMillimeters);
                                    break;
                                }
                            case var case12 when case12 > 4d:
                                {
                                    c = 1.115d - 0.84d / (heightMillimeters / widthMillimeters);
                                    break;
                                }
                        }

                        break;
                    }
                case var case13 when case13 > 0d:
                    {
                        c = 1d;
                        break;
                    }
            }
            double hydraulicDiameterMillimeters=default;
            if (heightMillimeters!=0&&widthMillimeters!=0)
            {
                hydraulicDiameterMillimeters=2*(widthMillimeters*heightMillimeters)/(widthMillimeters+heightMillimeters);
            }
            else
            {
                hydraulicDiameterMillimeters = diameterMillimeters;
            }
            deltar = equivalentRoughnessMillimeters / hydraulicDiameterMillimeters;
            switch (rd)
            {
                case var case14 when case14 <= 0.55d:
                    {
                        kre = 1d + 4400d / re;
                        break;
                    }
                case var case15 when case15 > 0.55d:
                    {
                        switch (rd)
                        {
                            case var case16 when case16 <= 0.7d:
                                {
                                    kre = 5.45d / Math.Pow(re, 0.131d);
                                    break;
                                }
                            case var case17 when case17 > 0.7d:
                                {
                                    ln = Math.Log(Math.Pow(re, Math.Pow(10d, -5)));
                                    kre = 1.3d - 0.29d * ln;
                                    break;
                                }
                        }

                        break;
                    }
            }
            switch (rd)
            {
                case var case18 when case18 <= 0.55d:
                    {
                        switch (re)
                        {
                            case var case19 when case19 <= 4d * Math.Pow(10d, 4d):
                                {
                                    switch (deltar)
                                    {
                                        case 0d:
                                            {
                                                kd = 1d;
                                                break;
                                            }
                                        case var case20 when case20 > 0d:
                                            {
                                                switch (deltar)
                                                {
                                                    case var case21 when case21 <= 0.001d:
                                                        {
                                                            kd = 1d;
                                                            break;
                                                        }
                                                    case var case22 when case22 > 0.001d:
                                                        {
                                                            kd = 1d;
                                                            break;
                                                        }
                                                }

                                                break;
                                            }
                                    }

                                    break;
                                }
                            case var case23 when case23 > 4d * Math.Pow(10d, 4d):
                                {
                                    switch (deltar)
                                    {
                                        case 0d:
                                            {
                                                kd = 1d;
                                                break;
                                            }
                                        case var case24 when case24 > 0d:
                                            {
                                                switch (deltar)
                                                {

                                                    case var case25 when case25 <= 0.001d:
                                                        {
                                                            kd = 1d + 0.5d * Math.Pow(10d, 3d) * deltar;
                                                            break;
                                                        }
                                                    case var case26 when case26 > 0.001d:
                                                        {
                                                            kd = 1.5d;
                                                            break;
                                                        }
                                                }

                                                break;
                                            }
                                    }

                                    break;
                                }
                        }

                        break;
                    }
                case var case27 when case27 > 0.55d:
                    {
                        switch (re)
                        {
                            case var case28 when case28 <= 4d * Math.Pow(10d, 4d):
                                {
                                    switch (deltar)
                                    {
                                        case 0d:
                                            {
                                                kd = 1d;
                                                break;
                                            }
                                        case var case29 when case29 > 0d:
                                            {
                                                switch (deltar)
                                                {
                                                    case var case30 when case30 <= 0.001d:
                                                        {
                                                            kd = 1d;
                                                            break;
                                                        }
                                                    case var case31 when case31 > 0.001d:
                                                        {
                                                            kd = 1d;
                                                            break;
                                                        }
                                                }

                                                break;
                                            }
                                    }

                                    break;
                                }
                            case var case32 when case32 > 4d * Math.Pow(10d, 4d):
                                {
                                    switch (re)
                                    {
                                        case var case33 when case33 <= 2d * Math.Pow(10d, 5d):
                                            {
                                                switch (deltar)
                                                {
                                                    case 0d:
                                                        {
                                                            kd = 1d;
                                                            break;
                                                        }
                                                    case var case34 when case34 > 0d:
                                                        {
                                                            switch (deltar)
                                                            {
                                                                case var case35 when case35 <= 0.001d:
                                                                    {
                                                                        kd = 1.5d;
                                                                        break;
                                                                    }
                                                                case var case36 when case36 > 0.001d:
                                                                    {
                                                                        kd = 2d;
                                                                        break;
                                                                    }
                                                            }

                                                            break;
                                                        }
                                                }

                                                break;
                                            }
                                        case var case37 when case37 > 2d * Math.Pow(10d, 5d):
                                            {
                                                switch (deltar)
                                                {
                                                    case 0d:
                                                        {
                                                            kd = 1d;
                                                            break;
                                                        }
                                                    case var case38 when case38 > 0d:
                                                        {
                                                            switch (deltar)
                                                            {
                                                                case var case39 when case39 <= 0.001d:
                                                                    {
                                                                        kd = 1d + deltar * Math.Pow(10d, 3d);
                                                                        break;
                                                                    }
                                                                case var case40 when case40 > 0.001d:
                                                                    {
                                                                        kd = 2d;
                                                                        break;
                                                                    }
                                                            }

                                                            break;
                                                        }
                                                }

                                                break;
                                            }
                                    }

                                    break;
                                }
                        }

                        break;
                    }
            }
            dzetam = A * b * c;
            elbowp = kd * kre * dzetam + 0.0175d * angleDegrees * lambda * rd;
            return elbowp;
        }

    }
}