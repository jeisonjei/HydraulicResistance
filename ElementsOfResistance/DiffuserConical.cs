using System;
using SharpFluids;
using EngineeringUnits;
using EngineeringUnits.Units;
using HydraulicResistance.Helpers;

namespace HydraulicResistance.ElementsOfResistance
{
    public class DiffuserConical{
        /*
        Основными геометрическими характеристиками диффузоров с прямыми стенками являются
        1) Угол расширения angleDegree
        2) Степень расширения n=areaBig/areaSmall
        3) Относительная длина length/areaSmall - обратите внимание - диаметр меньшего сечения
        - Для расчёта потерь давления в диффузоре с ksi используется скорость в меньшем (начальном) сечении
        - Начало отрыва струи в диффузоре зависит как от его геометрических параметров, так и от режима течения (чисел Рейнольдса и Маха)
        - l0 - прямая проставка (то есть прямой участок) перед диффузором. В сетях обычно перед диффузором есть значительный прямой участок,
        поэтому можно предварительно принимать l0=10*D0
        - lд - длина диффузора
        */
        public static double Resistance( FluidList fluid,
                                                            double tempCels,
                                                            double equivalentRoughness, 
                                                            double flowRateCubicMeterPerHour, 
                                                            double diamSmallMillimeter,
                                                            double diamBigMillimeter, 
                                                            double angleDegree){
            double ksi = default;
            double lengthDiffuserMillimeter=_getDiffuserLength(diamSmallMillimeter,diamBigMillimeter,angleDegree);
            /* 
            для всех расчётов длина прямой проставки принимается в 10 раз больше бОльшего диаметра,
            приблизительно оценивалось, и с такой длиной проставки сопротивление получается наибольшем, 
            так что чтобы немного упростить функцию длина проставки будет постоянной для всех диффузоров.
            Длина проставки входит в сложную формулу ζнер из формулы пункта 38 на странице 193
            */
            double lengthStraightPartBeforeDiffuserMillimeter = 10 * diamSmallMillimeter;
            var length=new Length(lengthDiffuserMillimeter,LengthUnit.Millimeter);
            var diamSmall=new Length(diamSmallMillimeter,LengthUnit.Millimeter);
            var diamBig=new Length(diamBigMillimeter,LengthUnit.Millimeter);
            var xCherta=_difsXCherta(lengthDiffuserMillimeter,diamSmallMillimeter);
            var lCherta = lengthStraightPartBeforeDiffuserMillimeter / diamSmallMillimeter;
            var xTilda=_difsXTilda(xCherta,angleDegree);
            var areaSmall=Mathematics.GetAreaCircle(diamSmallMillimeter).As(AreaUnit.SquareMeter);
            var areaBig=Mathematics.GetAreaCircle(diamBigMillimeter).As(AreaUnit.SquareMeter);
            var flow=new Flow(fluid,flowRateCubicMeterPerHour,diamBigMillimeter);
            var lambda=flow.GetLambda(tempCels,equivalentRoughness);
            var re=flow.GetReinoldsNumber(tempCels);
            var n=_difsN(areaSmall,areaBig);
            var dzetaTr=_difsDzetaTR(lambda,angleDegree,n);
            var dzetaTrHatch=_difsDzetaTRHatch(dzetaTr,xTilda);
            var phi=_difsdPhi(angleDegree,re);
            var dzetaR=_difsDzetaR(phi,n);
            var a=_difsA(angleDegree);
            var b=_difsB(angleDegree,lCherta);
            var c=_difsC(re);
            var dzetaN=_difsDzetaN(angleDegree,a,b,c,n,lCherta,re);
            ksi=_getDiffuserConicalResistance(dzetaTRHatch:dzetaTrHatch,
                                                dzetaR:dzetaR,
                                                dzetaN:dzetaN);
            return ksi;
        }
        private static double _getDiffuserLength(double diamSmallMillimeter,double diamBigMillimeter,double angleDegree){
            double lengthMillimeter = default;
            var diamSmall=new Length(diamSmallMillimeter,LengthUnit.Millimeter);
            var diamBig=new Length(diamBigMillimeter,LengthUnit.Millimeter);
            double areaSmall=Mathematics.GetAreaCircle(diamSmallMillimeter).As(AreaUnit.SquareMeter);
            double areaBig=Mathematics.GetAreaCircle(diamBigMillimeter).As(AreaUnit.SquareMeter);
            double n=_difsN(areaSmall,areaBig);
            double angleRadian=angleDegree*(Math.PI/180);
            lengthMillimeter = (diamSmallMillimeter * n-diamSmallMillimeter) / (2 * Math.Tan(angleRadian/2));
            return lengthMillimeter;
        }
        private static double _getDiffuserConicalResistance(double dzetaTRHatch, /*  */
                             double dzetaR,
                              double dzetaN)
        {
            /*
            Общий коэффициент сопротивления диффузора ζ
            Пункт 38 на странице 192
            */
            double difsConRet = default;
            difsConRet = dzetaTRHatch + dzetaR + dzetaN;
            return difsConRet;
        }
        private static double _difsDzetaTR(double lambda, double alpha, double n)
        {
            /*
            Коэффициент сопротивления трения для конических диффузоров ζтр. 
            Пункт 39, 
            Формула 5-6 на странице 193
            */
            double difsDzetaTRRet = default;
            double alph;

            alph = alpha * (Math.PI / 180d);
            difsDzetaTRRet = lambda / (8d * Math.Sin(alph / 2d)) * (1d - 1d / Math.Pow(n, 2d));
            return difsDzetaTRRet;
        }
        private static double _difsDzetaTRHatch(double dzetaTR, /* коэффициент сопротивления трения конического или пирамидального диффузора 
                                                        (формулы разные). Для конических диффузоров функция difsDzetaTR*/
                                         double xTilda)
        {
            /*
            Коэффициент сопротивления трения для конических и пирамидальных диффузоров ζ'тр
            Пункт 38 на странице 192
            */
            double difsDzetaTRHatchRet = default;

            difsDzetaTRHatchRet = (1d + 0.5d / Math.Pow(1.5d, xTilda)) * dzetaTR;
            return difsDzetaTRHatchRet;
        }
        private static double _difsXCherta(double x, double d)
        {
            /*
            Конические диффузоры. Отношение длины диффузора к диаметру.
            Пункт 39 на странице 193
            Формула между 5-6 и 5-7
            */
            double difsXChertaRet = default;

            difsXChertaRet = x / d;
            return difsXChertaRet;
        }
        private static double _difsXTilda(double xCherta, double alpha)
        {
            /*
            Относительная длина конического диффузора
            Пункт 39, страница 193
            Формула между 5-6 и 5-7
            */
            double difsXTildaRet = default;
            double alph;

            alph = alpha * (Math.PI / 180d);
            difsXTildaRet = Math.Log(1d + 2d * xCherta * Math.Tan(alph / 2d)) / (2d * Math.Tan(alph / 2d));
            return difsXTildaRet;
        }
        private static double _difsDzetaR(double dPhi /* аналог коэффициента полноты удара, функция difsPhi */,
                                     double n /* коэффициент расширения (предположительно) */)
        {
            /*
            Коэффициент ζравн, характеризующий потери на расширение, которые имели бы место в диффузоре при равномерном
            профиле скорости в его начальном сечении
            Формула 5-7 на странице 193
            1.92 - это коэффициент m
            */
            double difsDzetaRRet = default;
            difsDzetaRRet = dPhi * Math.Pow(1d - 1d / n, 1.92d);
            return difsDzetaRRet;
        }
        private static double _difsdPhi(double alpha, double re)
        {
            /*
            Аналог коэффициента полноты удара
            Страница 217, таблица "Значения Ф при различных числах Re
            */
            double difsdPhiRet = default;
            switch (re)
            {
                case var @case when @case <= 0.5d * Math.Pow(10d, 5d):
                    {
                        switch (alpha)
                        {
                            case var case1 when case1 == 0d:
                                {
                                    difsdPhiRet = 0d;
                                    break;
                                }
                            case var case2 when case2 > 0d:
                                {
                                    switch (alpha)
                                    {
                                        case var case3 when case3 <= 5d:
                                            {
                                                difsdPhiRet = 0.12d;
                                                break;
                                            }
                                        case var case4 when case4 > 5d:
                                            {
                                                switch (alpha)
                                                {
                                                    case var case5 when case5 <= 10d:
                                                        {
                                                            difsdPhiRet = 0.26d;
                                                            break;
                                                        }
                                                    case var case6 when case6 > 10d:
                                                        {
                                                            switch (alpha)
                                                            {
                                                                case var case7 when case7 <= 15d:
                                                                    {
                                                                        difsdPhiRet = 0.35d;
                                                                        break;
                                                                    }
                                                                case var case8 when case8 > 15d:
                                                                    {
                                                                        switch (alpha)
                                                                        {
                                                                            case var case9 when case9 <= 20d:
                                                                                {
                                                                                    difsdPhiRet = 0.45d;
                                                                                    break;
                                                                                }
                                                                            case var case10 when case10 > 20d:
                                                                                {
                                                                                    switch (alpha)
                                                                                    {
                                                                                        case var case11 when case11 <= 25d:
                                                                                            {
                                                                                                difsdPhiRet = 0.58d;
                                                                                                break;
                                                                                            }
                                                                                        case var case12 when case12 > 25d:
                                                                                            {
                                                                                                switch (alpha)
                                                                                                {
                                                                                                    case var case13 when case13 <= 30d:
                                                                                                        {
                                                                                                            difsdPhiRet = 0.75d;
                                                                                                            break;
                                                                                                        }
                                                                                                    case var case14 when case14 > 30d:
                                                                                                        {
                                                                                                            switch (alpha)
                                                                                                            {
                                                                                                                case var case15 when case15 <= 40d:
                                                                                                                    {
                                                                                                                        difsdPhiRet = 0.9d;
                                                                                                                        break;
                                                                                                                    }
                                                                                                                case var case16 when case16 > 40d:
                                                                                                                    {
                                                                                                                        switch (alpha)
                                                                                                                        {
                                                                                                                            case var case17 when case17 <= 45d:
                                                                                                                                {
                                                                                                                                    difsdPhiRet = 0.95d;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            case var case18 when case18 > 45d:
                                                                                                                                {
                                                                                                                                    switch (alpha)
                                                                                                                                    {
                                                                                                                                        case var case19 when case19 <= 50d:
                                                                                                                                            {
                                                                                                                                                difsdPhiRet = 0.98d;
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                        case var case20 when case20 > 50d:
                                                                                                                                            {
                                                                                                                                                switch (alpha)
                                                                                                                                                {
                                                                                                                                                    case var case21 when case21 <= 60d:
                                                                                                                                                        {
                                                                                                                                                            difsdPhiRet = 1d;
                                                                                                                                                            break;
                                                                                                                                                        }
                                                                                                                                                    case var case22 when case22 > 60d:
                                                                                                                                                        {
                                                                                                                                                            switch (alpha)
                                                                                                                                                            {
                                                                                                                                                                case var case23 when case23 <= 80d:
                                                                                                                                                                    {
                                                                                                                                                                        difsdPhiRet = 1.02d;
                                                                                                                                                                        break;
                                                                                                                                                                    }
                                                                                                                                                                case var case24 when case24 > 80d:
                                                                                                                                                                    {
                                                                                                                                                                        switch (alpha)
                                                                                                                                                                        {
                                                                                                                                                                            case var case25 when case25 <= 140d:
                                                                                                                                                                                {
                                                                                                                                                                                    difsdPhiRet = 1d;
                                                                                                                                                                                    break;
                                                                                                                                                                                }
                                                                                                                                                                            case var case26 when case26 > 140d:
                                                                                                                                                                                {
                                                                                                                                                                                    switch (alpha)
                                                                                                                                                                                    {
                                                                                                                                                                                        case var case27 when case27 <= 180d:
                                                                                                                                                                                            {
                                                                                                                                                                                                difsdPhiRet = 1d;
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
                case var case28 when case28 > 0.5d * Math.Pow(10d, 5d):
                    {
                        switch (re)
                        {
                            case var case29 when case29 <= 2d * Math.Pow(10d, 5d):
                                {
                                    switch (alpha)
                                    {
                                        case var case30 when case30 == 0d:
                                            {
                                                difsdPhiRet = 0d;
                                                break;
                                            }
                                        case var case31 when case31 > 0d:
                                            {
                                                switch (alpha)
                                                {
                                                    case var case32 when case32 <= 5d:
                                                        {
                                                            difsdPhiRet = 0.08d;
                                                            break;
                                                        }
                                                    case var case33 when case33 > 5d:
                                                        {
                                                            switch (alpha)
                                                            {
                                                                case var case34 when case34 <= 10d:
                                                                    {
                                                                        difsdPhiRet = 0.15d;
                                                                        break;
                                                                    }
                                                                case var case35 when case35 > 10d:
                                                                    {
                                                                        switch (alpha)
                                                                        {
                                                                            case var case36 when case36 <= 15d:
                                                                                {
                                                                                    difsdPhiRet = 0.24d;
                                                                                    break;
                                                                                }
                                                                            case var case37 when case37 > 15d:
                                                                                {
                                                                                    switch (alpha)
                                                                                    {
                                                                                        case var case38 when case38 <= 20d:
                                                                                            {
                                                                                                difsdPhiRet = 0.32d;
                                                                                                break;
                                                                                            }
                                                                                        case var case39 when case39 > 20d:
                                                                                            {
                                                                                                switch (alpha)
                                                                                                {
                                                                                                    case var case40 when case40 <= 25d:
                                                                                                        {
                                                                                                            difsdPhiRet = 0.43d;
                                                                                                            break;
                                                                                                        }
                                                                                                    case var case41 when case41 > 25d:
                                                                                                        {
                                                                                                            switch (alpha)
                                                                                                            {
                                                                                                                case var case42 when case42 <= 30d:
                                                                                                                    {
                                                                                                                        difsdPhiRet = 0.6d;
                                                                                                                        break;
                                                                                                                    }
                                                                                                                case var case43 when case43 > 30d:
                                                                                                                    {
                                                                                                                        switch (alpha)
                                                                                                                        {
                                                                                                                            case var case44 when case44 <= 40d:
                                                                                                                                {
                                                                                                                                    difsdPhiRet = 0.82d;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            case var case45 when case45 > 40d:
                                                                                                                                {
                                                                                                                                    switch (alpha)
                                                                                                                                    {
                                                                                                                                        case var case46 when case46 <= 45d:
                                                                                                                                            {
                                                                                                                                                difsdPhiRet = 0.88d;
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                        case var case47 when case47 > 45d:
                                                                                                                                            {
                                                                                                                                                switch (alpha)
                                                                                                                                                {
                                                                                                                                                    case var case48 when case48 <= 50d:
                                                                                                                                                        {
                                                                                                                                                            difsdPhiRet = 0.93d;
                                                                                                                                                            break;
                                                                                                                                                        }
                                                                                                                                                    case var case49 when case49 > 50d:
                                                                                                                                                        {
                                                                                                                                                            switch (alpha)
                                                                                                                                                            {
                                                                                                                                                                case var case50 when case50 <= 60d:
                                                                                                                                                                    {
                                                                                                                                                                        difsdPhiRet = 0.95d;
                                                                                                                                                                        break;
                                                                                                                                                                    }
                                                                                                                                                                case var case51 when case51 > 60d:
                                                                                                                                                                    {
                                                                                                                                                                        switch (alpha)
                                                                                                                                                                        {
                                                                                                                                                                            case var case52 when case52 <= 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    difsdPhiRet = 0.95d;
                                                                                                                                                                                    break;
                                                                                                                                                                                }
                                                                                                                                                                            case var case53 when case53 > 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    switch (alpha)
                                                                                                                                                                                    {
                                                                                                                                                                                        case var case54 when case54 <= 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                difsdPhiRet = 0.97d;
                                                                                                                                                                                                break;
                                                                                                                                                                                            }
                                                                                                                                                                                        case var case55 when case55 > 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                switch (alpha)
                                                                                                                                                                                                {
                                                                                                                                                                                                    case var case56 when case56 <= 180d:
                                                                                                                                                                                                        {
                                                                                                                                                                                                            difsdPhiRet = 0.99d;
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
                            case var case57 when case57 > 2d * Math.Pow(10d, 5d):
                                {
                                    switch (alpha)
                                    {
                                        case var case58 when case58 == 0d:
                                            {
                                                difsdPhiRet = 0d;
                                                break;
                                            }
                                        case var case59 when case59 > 0d:
                                            {
                                                switch (alpha)
                                                {
                                                    case var case60 when case60 <= 5d:
                                                        {
                                                            difsdPhiRet = 0.04d;
                                                            break;
                                                        }
                                                    case var case61 when case61 > 5d:
                                                        {
                                                            switch (alpha)
                                                            {
                                                                case var case62 when case62 <= 10d:
                                                                    {
                                                                        difsdPhiRet = 0.09d;
                                                                        break;
                                                                    }
                                                                case var case63 when case63 > 10d:
                                                                    {
                                                                        switch (alpha)
                                                                        {
                                                                            case var case64 when case64 <= 15d:
                                                                                {
                                                                                    difsdPhiRet = 0.18d;
                                                                                    break;
                                                                                }
                                                                            case var case65 when case65 > 15d:
                                                                                {
                                                                                    switch (alpha)
                                                                                    {
                                                                                        case var case66 when case66 <= 20d:
                                                                                            {
                                                                                                difsdPhiRet = 0.25d;
                                                                                                break;
                                                                                            }
                                                                                        case var case67 when case67 > 20d:
                                                                                            {
                                                                                                switch (alpha)
                                                                                                {
                                                                                                    case var case68 when case68 <= 25d:
                                                                                                        {
                                                                                                            difsdPhiRet = 0.37d;
                                                                                                            break;
                                                                                                        }
                                                                                                    case var case69 when case69 > 25d:
                                                                                                        {
                                                                                                            switch (alpha)
                                                                                                            {
                                                                                                                case var case70 when case70 <= 30d:
                                                                                                                    {
                                                                                                                        difsdPhiRet = 0.52d;
                                                                                                                        break;
                                                                                                                    }
                                                                                                                case var case71 when case71 > 30d:
                                                                                                                    {
                                                                                                                        switch (alpha)
                                                                                                                        {
                                                                                                                            case var case72 when case72 <= 40d:
                                                                                                                                {
                                                                                                                                    difsdPhiRet = 0.77d;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            case var case73 when case73 > 40d:
                                                                                                                                {
                                                                                                                                    switch (alpha)
                                                                                                                                    {
                                                                                                                                        case var case74 when case74 <= 45d:
                                                                                                                                            {
                                                                                                                                                difsdPhiRet = 0.82d;
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                        case var case75 when case75 > 45d:
                                                                                                                                            {
                                                                                                                                                switch (alpha)
                                                                                                                                                {
                                                                                                                                                    case var case76 when case76 <= 50d:
                                                                                                                                                        {
                                                                                                                                                            difsdPhiRet = 0.88d;
                                                                                                                                                            break;
                                                                                                                                                        }
                                                                                                                                                    case var case77 when case77 > 50d:
                                                                                                                                                        {
                                                                                                                                                            switch (alpha)
                                                                                                                                                            {
                                                                                                                                                                case var case78 when case78 <= 60d:
                                                                                                                                                                    {
                                                                                                                                                                        difsdPhiRet = 0.91d;
                                                                                                                                                                        break;
                                                                                                                                                                    }
                                                                                                                                                                case var case79 when case79 > 60d:
                                                                                                                                                                    {
                                                                                                                                                                        switch (alpha)
                                                                                                                                                                        {
                                                                                                                                                                            case var case80 when case80 <= 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    difsdPhiRet = 0.95d;
                                                                                                                                                                                    break;
                                                                                                                                                                                }
                                                                                                                                                                            case var case81 when case81 > 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    switch (alpha)
                                                                                                                                                                                    {
                                                                                                                                                                                        case var case82 when case82 <= 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                difsdPhiRet = 0.97d;
                                                                                                                                                                                                break;
                                                                                                                                                                                            }
                                                                                                                                                                                        case var case83 when case83 > 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                switch (alpha)
                                                                                                                                                                                                {
                                                                                                                                                                                                    case var case84 when case84 <= 180d:
                                                                                                                                                                                                        {
                                                                                                                                                                                                            difsdPhiRet = 0.98d;
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

                        break;
                    }
            }

            return difsdPhiRet;
        }

        private static double _difsN(double f1, double f2)
        {
            /*
            Отношение площадей. В названии формулы говорится о диффузорах, но вообще-то функция просто вычисляет отношение двух чисел
            */
            double difsNRet = default;

            difsNRet = f2 / f1;
            return difsNRet;
        }

        private static double _difsDzetaN(double alpha, /* угол раскрытия диффузора */
                                 double A /* коэффициент, функция difsA */,
                                double b /* коэффициент, функция difsB */,
                                 double c /* коэффициент, функция difsC */,
                                  double n, /* соотношение площадей диффузора */
                                  double l /* длина диффузора */,
                                  double re /* число Рейнольдса */ )
        {
            /*
            Коэффициент ζнер из формулы пункта 38 на странице 193, учитывающий дополнительные потери на расширение, обусловленные неравномерностью профиля скорости
            в начальном сечении диффузора, то есть при наличии перед ним прямой проставки длиной T0.
            Пункт 39 на странице 193, между формулами 5-7 и 5-8
            */
            double difsDzetaNRet = default;
            double alph;
            double f;

            alph = alpha * (Math.PI / 180d);

            difsDzetaNRet = 0.044d * Math.Pow(0.345d * alph, A) * (1d - Math.Pow(0.2d * n + 0.8d, -3.82d)) * Math.Pow(0.154d * l, b) * Math.Pow(2.31d * Math.Pow(10d, -6) * re + 0.2d + 2.54d * Math.Pow(1d + 0.081d * alph, -1.51d), c);
            return difsDzetaNRet;
        }

        private static double _difsA(double alpha)
        {
            /*
            Коэффициент a, входящий в формулу для ζнер
            Пункт 39 на странице 192, формула между 5-7 и 5-8
            */
            double difsARet = default;
            double alph;
            alph = alpha * (Math.PI / 180d);
            difsARet = 0.924d / (1d + 1.3d * Math.Pow(10d, -5) * Math.Pow(alph, 3.14d));
            return difsARet;
        }

        private static double _difsB(double alpha, double l)
        {
            /*
            Коэффициент b, входящий в формулу для ζнер
            Пункт 39 на странице 192, формула между 5-7 и 5-8
            */

            double difsBRet = default;
            double alph;
            alph = alpha * (Math.PI / 180d);
            difsBRet = (0.3d + 1.55d * Math.Pow(1.1d, -alph)) / (1d + 1.03d * Math.Pow(10d, -8) * Math.Pow(l, 7.5d));
            return difsBRet;
        }

        private static double _difsC(double re)
        {
            /*
            Коэффициент c, входящий в формулу для ζнер
            Пункт 39 на странице 192, формула между 5-7 и 5-8
            */

            double difsCRet = default;
            difsCRet = 1.05d / (1d + 2.3d * Math.Pow(10d, -62) * Math.Pow(re, 11d));
            return difsCRet;
        }
    }
}