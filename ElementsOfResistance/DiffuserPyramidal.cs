using System;
using EngineeringUnits;
using EngineeringUnits.Units;
using SharpFluids;
using HydraulicResistance.Helpers;

namespace HydraulicResistance.ElementsOfResistance
{
    public class DiffuserPyramidal
    {
        /*
        В пирамидальных диффузорах сопротивление почти всегда выше, чем в конических, хотя структура потока и характер кривых сопротивления 
        в основном такие же, как и для конических диффузоров.
        Сопротивление плоских диффузоров (диффузоры с расширением только в одной плоскости) при одинаковых углах и степенях расширения 
        заметно меньше, чем в диффузорах с расширением сечения в двух плоскостях, и во многих случаях даже несколько меньше, чем в конических
        */
public static double Resistance( FluidList fluid,
                                                            double tempCels,
                                                            double equivalentRoughness, 
                                                            double flowRateCubicMeterPerHour, 
                                                            double widthSmallMillimeter,
                                                            double heightSmallMillimeter,
                                                            double widthBigMillimeter, 
                                                            double heightBigMillimeter,
                                                            double lengthDiffuserMillimeter,
                                                            double angleAlphaDegree,
                                                            double angleBetaDegree){
            double ksi = default;
            /* 
            для всех расчётов длина прямой проставки принимается в 10 раз больше бОльшего диаметра,
            приблизительно оценивалось, и с такой длиной проставки сопротивление получается наибольшем, 
            так что чтобы немного упростить функцию длина проставки будет постоянной для всех диффузоров.
            Длина проставки входит в сложную формулу ζнер из формулы пункта 38 на странице 193
            При углах расширения начиная от 90 до 180 потери в диффузоре становятся близкими к потерям при внезапном расширении,
            поэтому если требуется очень короткий переходный участок, проще применить внезапное расширение
            */
            var equivalentDiamSmallMillimeter=Mathematics.GetEquivalentDiameter(widthSmallMillimeter,heightSmallMillimeter).As(LengthUnit.Millimeter);
            double lengthStraightPartBeforeDiffuserMillimeter = 10 * equivalentDiamSmallMillimeter;
            var xCherta = lengthDiffuserMillimeter / equivalentDiamSmallMillimeter;
            var lCherta = lengthStraightPartBeforeDiffuserMillimeter / equivalentDiamSmallMillimeter;
            var angleAlphaRadian=angleAlphaDegree*(Math.PI/180);
            var angleBetaRadian=angleBetaDegree*(Math.PI/180);
            var tanAlphaDividedBy2=Math.Tan(angleAlphaRadian/2);
            var tanBetaDividedBy2=Math.Tan(angleBetaRadian/2);
            var ta = tanAlphaDividedBy2;
            var tb = tanBetaDividedBy2;
            var a_x =widthSmallMillimeter+2*lengthDiffuserMillimeter*ta;
            var b_x =heightSmallMillimeter+2*lengthDiffuserMillimeter*tb;
            var D_g0=equivalentDiamSmallMillimeter;
            var a_x_ = a_x / D_g0;
            var b_x_ = b_x / D_g0;
            var a0_ =widthSmallMillimeter/D_g0;
            var b0_ =heightSmallMillimeter/D_g0;
            var D_gx_=_difsPDHydraulicX(a0_,b0_,xCherta,angleAlphaDegree,angleBetaDegree) ;
            var xTilda=_difsPXTilda(a0_,b0_,angleAlphaDegree,angleBetaDegree,xCherta);
            var areaSmall=Mathematics.GetAreaRectangle(widthSmallMillimeter,heightSmallMillimeter).As(AreaUnit.SquareMeter);
            var areaBig=Mathematics.GetAreaRectangle(widthBigMillimeter,heightBigMillimeter).As(AreaUnit.SquareMeter);
            var flow=new Flow(fluid,flowRateCubicMeterPerHour,widthSmallMillimeter,heightSmallMillimeter);
            var lambda=flow.GetLambda(tempCels,equivalentRoughness);
            var re=flow.GetReinoldsNumber(tempCels);
            var n = areaBig / areaSmall;
            var dzetaTr=_difsPDzetaTR(lambda,angleAlphaDegree,angleBetaDegree,n);
            var dzetaTrHatch=_difsPDzetaTRHatch(dzetaTr,xTilda);
            var phi=_difsPPhi(angleAlphaDegree,re);
            var dzetaR=_difsPDzetaR(phi,n);
            var s=_difsPs(angleAlphaDegree);
            var t=_difsPt(lCherta);
            var u=_difsPu(re);
            var dzetaN=_difsPDzetaN(angleAlphaDegree:angleAlphaDegree,
            n:n,l0_:lengthStraightPartBeforeDiffuserMillimeter,re:re,s:s,t:t,u:u);
            ksi=_difsPyr(difsPDzetaHatch:dzetaTrHatch,
                                                difsPDzetaR:dzetaR,
                                                difsPDzetaN:dzetaN);
            return ksi;
        }
        private static double _difsPyr(double difsPDzetaHatch, /* коэффициент сопротивления трения с поправкой
                                                        на относительную длину диффузора,
                                                        вычисляется по той же формуле, что и для конического
                                                        диффузора, но ζтр берётся для пирамидального диффузора. Пункт 38, страница 192 */
                    double difsPDzetaR /* коэффициент ζравн для пирамидального диффузора, характеризующий потери на расширение, которые имели бы место в диффузоре при 
                                                равномерном профиле скорости в его начальном сечении */,
                     double difsPDzetaN /* коэффициент ζнер, характеризующий дополнительные потери на расширение,
                                                обусловленные неравномерностью профиля скорости в начальном сечении диффузора,
                                                то есть при наличии перед ним прямой проставки  */ )
        {
            /*
            Коэффициент сопротивления пирамидального диффузора
            Это та же формула, которая используется для коэффициента сопротивления конического диффузора
            Пункт 38, страница 192
            */
            double difsPyrRet = default;
            difsPyrRet = difsPDzetaHatch + difsPDzetaR + difsPDzetaN;
            return difsPyrRet;
        }

        private static double _difsPN(double f1, double f2)
        {
            /*
            Отношение площадей диффузора. Это формула дублируется
            */
            double difsPNRet = default;

            difsPNRet = f2 / f1;
            return difsPNRet;
        }

        private static double _difsPDzetaTR(double lambda, double alpha, double beta, double n)
        {
            /*
            Коэффициент сопротивления трения  пирамидального диффузора ζтр
            Формула 5-8 на странице 193
            */
            double difsPDzetaTRRet = default;
            double alph;
            double bet;
            alph = alpha * (Math.PI/180);
            bet = beta * (Math.PI/180);

            difsPDzetaTRRet = lambda / 16d * (1d - 1d / Math.Pow(n, 2d)) * (1d / Math.Sin(alph / 2d) + 1d / Math.Sin(bet / 2d));
            return difsPDzetaTRRet;
        }
                private static double _difsPDzetaTRHatch(double dzetaTR, /* коэффициент сопротивления трения конического или пирамидального диффузора 
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


        private static double _difsPDzetaR(double difsPPhi, double n)
        {
            /*
            Коэффициент ζравн для пирамидального диффузора,
            Пункт 40 на странице 193 (там не формула, а просто указание что ζравн принимается по формуле 5-7)
            1.76 - коэффициент m
            */
            double difsPDzetaRRet = default;

            difsPDzetaRRet = difsPPhi * Math.Pow(1d - 1d / n, 1.76d);
            return difsPDzetaRRet;
        }

        private static double _difsPPhi(double alpha, double re)
        {
            /*
            Аналог коэффициента полноты удара для пирамидального диффузора,
            Диаграмма 5-4, таблица "Значения Ф при различных числах Re" на странице 227
            */
            double difsPPhiRet = default;
            switch (re)
            {
                case var @case when @case <= 0.5d * Math.Pow(10d, 5d):
                    {
                        switch (alpha)
                        {
                            case var case1 when case1 == 0d:
                                {
                                    difsPPhiRet = 0d;
                                    break;
                                }
                            case var case2 when case2 > 0d:
                                {
                                    switch (alpha)
                                    {
                                        case var case3 when case3 <= 5d:
                                            {
                                                difsPPhiRet = 0.1d;
                                                break;
                                            }
                                        case var case4 when case4 > 5d:
                                            {
                                                switch (alpha)
                                                {
                                                    case var case5 when case5 <= 10d:
                                                        {
                                                            difsPPhiRet = 0.2d;
                                                            break;
                                                        }
                                                    case var case6 when case6 > 10d:
                                                        {
                                                            switch (alpha)
                                                            {
                                                                case var case7 when case7 <= 15d:
                                                                    {
                                                                        difsPPhiRet = 0.28d;
                                                                        break;
                                                                    }
                                                                case var case8 when case8 > 15d:
                                                                    {
                                                                        switch (alpha)
                                                                        {
                                                                            case var case9 when case9 <= 20d:
                                                                                {
                                                                                    difsPPhiRet = 0.36d;
                                                                                    break;
                                                                                }
                                                                            case var case10 when case10 > 20d:
                                                                                {
                                                                                    switch (alpha)
                                                                                    {
                                                                                        case var case11 when case11 <= 25d:
                                                                                            {
                                                                                                difsPPhiRet = 0.48d;
                                                                                                break;
                                                                                            }
                                                                                        case var case12 when case12 > 25d:
                                                                                            {
                                                                                                switch (alpha)
                                                                                                {
                                                                                                    case var case13 when case13 <= 30d:
                                                                                                        {
                                                                                                            difsPPhiRet = 0.6d;
                                                                                                            break;
                                                                                                        }
                                                                                                    case var case14 when case14 > 30d:
                                                                                                        {
                                                                                                            switch (alpha)
                                                                                                            {
                                                                                                                case var case15 when case15 <= 40d:
                                                                                                                    {
                                                                                                                        difsPPhiRet = 0.84d;
                                                                                                                        break;
                                                                                                                    }
                                                                                                                case var case16 when case16 > 40d:
                                                                                                                    {
                                                                                                                        switch (alpha)
                                                                                                                        {
                                                                                                                            case var case17 when case17 <= 45d:
                                                                                                                                {
                                                                                                                                    difsPPhiRet = 0.89d;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            case var case18 when case18 > 45d:
                                                                                                                                {
                                                                                                                                    switch (alpha)
                                                                                                                                    {
                                                                                                                                        case var case19 when case19 <= 50d:
                                                                                                                                            {
                                                                                                                                                difsPPhiRet = 0.97d;
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                        case var case20 when case20 > 50d:
                                                                                                                                            {
                                                                                                                                                switch (alpha)
                                                                                                                                                {
                                                                                                                                                    case var case21 when case21 <= 60d:
                                                                                                                                                        {
                                                                                                                                                            difsPPhiRet = 1.04d;
                                                                                                                                                            break;
                                                                                                                                                        }
                                                                                                                                                    case var case22 when case22 > 60d:
                                                                                                                                                        {
                                                                                                                                                            switch (alpha)
                                                                                                                                                            {
                                                                                                                                                                case var case23 when case23 <= 80d:
                                                                                                                                                                    {
                                                                                                                                                                        difsPPhiRet = 1.1d;
                                                                                                                                                                        break;
                                                                                                                                                                    }
                                                                                                                                                                case var case24 when case24 > 80d:
                                                                                                                                                                    {
                                                                                                                                                                        switch (alpha)
                                                                                                                                                                        {
                                                                                                                                                                            case var case25 when case25 <= 140d:
                                                                                                                                                                                {
                                                                                                                                                                                    difsPPhiRet = 1.09d;
                                                                                                                                                                                    break;
                                                                                                                                                                                }
                                                                                                                                                                            case var case26 when case26 > 140d:
                                                                                                                                                                                {
                                                                                                                                                                                    switch (alpha)
                                                                                                                                                                                    {
                                                                                                                                                                                        case var case27 when case27 <= 180d:
                                                                                                                                                                                            {
                                                                                                                                                                                                difsPPhiRet = 1.06d;
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
                            case var case29 when case29 < 2d * Math.Pow(10d, 5d):
                                {
                                    switch (alpha)
                                    {
                                        case var case30 when case30 == 0d:
                                            {
                                                difsPPhiRet = 0d;
                                                break;
                                            }
                                        case var case31 when case31 > 0d:
                                            {
                                                switch (alpha)
                                                {
                                                    case var case32 when case32 <= 5d:
                                                        {
                                                            difsPPhiRet = 0.1d;
                                                            break;
                                                        }
                                                    case var case33 when case33 > 5d:
                                                        {
                                                            switch (alpha)
                                                            {
                                                                case var case34 when case34 <= 10d:
                                                                    {
                                                                        difsPPhiRet = 0.2d;
                                                                        break;
                                                                    }
                                                                case var case35 when case35 > 10d:
                                                                    {
                                                                        switch (alpha)
                                                                        {
                                                                            case var case36 when case36 <= 15d:
                                                                                {
                                                                                    difsPPhiRet = 0.28d;
                                                                                    break;
                                                                                }
                                                                            case var case37 when case37 > 15d:
                                                                                {
                                                                                    switch (alpha)
                                                                                    {
                                                                                        case var case38 when case38 <= 20d:
                                                                                            {
                                                                                                difsPPhiRet = 0.36d;
                                                                                                break;
                                                                                            }
                                                                                        case var case39 when case39 > 20d:
                                                                                            {
                                                                                                switch (alpha)
                                                                                                {
                                                                                                    case var case40 when case40 <= 25d:
                                                                                                        {
                                                                                                            difsPPhiRet = 0.48d;
                                                                                                            break;
                                                                                                        }
                                                                                                    case var case41 when case41 > 25d:
                                                                                                        {
                                                                                                            switch (alpha)
                                                                                                            {
                                                                                                                case var case42 when case42 <= 30d:
                                                                                                                    {
                                                                                                                        difsPPhiRet = 0.6d;
                                                                                                                        break;
                                                                                                                    }
                                                                                                                case var case43 when case43 > 30d:
                                                                                                                    {
                                                                                                                        switch (alpha)
                                                                                                                        {
                                                                                                                            case var case44 when case44 <= 40d:
                                                                                                                                {
                                                                                                                                    difsPPhiRet = 0.84d;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            case var case45 when case45 > 40d:
                                                                                                                                {
                                                                                                                                    switch (alpha)
                                                                                                                                    {
                                                                                                                                        case var case46 when case46 <= 45d:
                                                                                                                                            {
                                                                                                                                                difsPPhiRet = 0.89d;
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                        case var case47 when case47 > 45d:
                                                                                                                                            {
                                                                                                                                                switch (alpha)
                                                                                                                                                {
                                                                                                                                                    case var case48 when case48 <= 50d:
                                                                                                                                                        {
                                                                                                                                                            difsPPhiRet = 0.97d;
                                                                                                                                                            break;
                                                                                                                                                        }
                                                                                                                                                    case var case49 when case49 > 50d:
                                                                                                                                                        {
                                                                                                                                                            switch (alpha)
                                                                                                                                                            {
                                                                                                                                                                case var case50 when case50 <= 60d:
                                                                                                                                                                    {
                                                                                                                                                                        difsPPhiRet = 1.04d;
                                                                                                                                                                        break;
                                                                                                                                                                    }
                                                                                                                                                                case var case51 when case51 > 60d:
                                                                                                                                                                    {
                                                                                                                                                                        switch (alpha)
                                                                                                                                                                        {
                                                                                                                                                                            case var case52 when case52 <= 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    difsPPhiRet = 1.1d;
                                                                                                                                                                                    break;
                                                                                                                                                                                }
                                                                                                                                                                            case var case53 when case53 > 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    switch (alpha)
                                                                                                                                                                                    {
                                                                                                                                                                                        case var case54 when case54 <= 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                difsPPhiRet = 1.09d;
                                                                                                                                                                                                break;
                                                                                                                                                                                            }
                                                                                                                                                                                        case var case55 when case55 > 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                switch (alpha)
                                                                                                                                                                                                {
                                                                                                                                                                                                    case var case56 when case56 <= 180d:
                                                                                                                                                                                                        {
                                                                                                                                                                                                            difsPPhiRet = 1.06d;
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
                            case var case57 when case57 >= 2d * Math.Pow(10d, 5d):
                                {
                                    switch (alpha)
                                    {
                                        case var case58 when case58 == 0d:
                                            {
                                                difsPPhiRet = 0d;
                                                break;
                                            }
                                        case var case59 when case59 > 0d:
                                            {
                                                switch (alpha)
                                                {
                                                    case var case60 when case60 <= 5d:
                                                        {
                                                            difsPPhiRet = 0.05d;
                                                            break;
                                                        }
                                                    case var case61 when case61 > 5d:
                                                        {
                                                            switch (alpha)
                                                            {
                                                                case var case62 when case62 <= 10d:
                                                                    {
                                                                        difsPPhiRet = 0.12d;
                                                                        break;
                                                                    }
                                                                case var case63 when case63 > 10d:
                                                                    {
                                                                        switch (alpha)
                                                                        {
                                                                            case var case64 when case64 <= 15d:
                                                                                {
                                                                                    difsPPhiRet = 0.23d;
                                                                                    break;
                                                                                }
                                                                            case var case65 when case65 > 15d:
                                                                                {
                                                                                    switch (alpha)
                                                                                    {
                                                                                        case var case66 when case66 <= 20d:
                                                                                            {
                                                                                                difsPPhiRet = 0.3d;
                                                                                                break;
                                                                                            }
                                                                                        case var case67 when case67 > 20d:
                                                                                            {
                                                                                                switch (alpha)
                                                                                                {
                                                                                                    case var case68 when case68 <= 25d:
                                                                                                        {
                                                                                                            difsPPhiRet = 0.45d;
                                                                                                            break;
                                                                                                        }
                                                                                                    case var case69 when case69 > 25d:
                                                                                                        {
                                                                                                            switch (alpha)
                                                                                                            {
                                                                                                                case var case70 when case70 <= 30d:
                                                                                                                    {
                                                                                                                        difsPPhiRet = 0.6d;
                                                                                                                        break;
                                                                                                                    }
                                                                                                                case var case71 when case71 > 30d:
                                                                                                                    {
                                                                                                                        switch (alpha)
                                                                                                                        {
                                                                                                                            case var case72 when case72 <= 40d:
                                                                                                                                {
                                                                                                                                    difsPPhiRet = 0.84d;
                                                                                                                                    break;
                                                                                                                                }
                                                                                                                            case var case73 when case73 > 40d:
                                                                                                                                {
                                                                                                                                    switch (alpha)
                                                                                                                                    {
                                                                                                                                        case var case74 when case74 <= 45d:
                                                                                                                                            {
                                                                                                                                                difsPPhiRet = 0.89d;
                                                                                                                                                break;
                                                                                                                                            }
                                                                                                                                        case var case75 when case75 > 45d:
                                                                                                                                            {
                                                                                                                                                switch (alpha)
                                                                                                                                                {
                                                                                                                                                    case var case76 when case76 <= 50d:
                                                                                                                                                        {
                                                                                                                                                            difsPPhiRet = 0.97d;
                                                                                                                                                            break;
                                                                                                                                                        }
                                                                                                                                                    case var case77 when case77 > 50d:
                                                                                                                                                        {
                                                                                                                                                            switch (alpha)
                                                                                                                                                            {
                                                                                                                                                                case var case78 when case78 <= 60d:
                                                                                                                                                                    {
                                                                                                                                                                        difsPPhiRet = 1.04d;
                                                                                                                                                                        break;
                                                                                                                                                                    }
                                                                                                                                                                case var case79 when case79 > 60d:
                                                                                                                                                                    {
                                                                                                                                                                        switch (alpha)
                                                                                                                                                                        {
                                                                                                                                                                            case var case80 when case80 <= 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    difsPPhiRet = 1.1d;
                                                                                                                                                                                    break;
                                                                                                                                                                                }
                                                                                                                                                                            case var case81 when case81 > 80d:
                                                                                                                                                                                {
                                                                                                                                                                                    switch (alpha)
                                                                                                                                                                                    {
                                                                                                                                                                                        case var case82 when case82 <= 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                difsPPhiRet = 1.09d;
                                                                                                                                                                                                break;
                                                                                                                                                                                            }
                                                                                                                                                                                        case var case83 when case83 > 140d:
                                                                                                                                                                                            {
                                                                                                                                                                                                switch (alpha)
                                                                                                                                                                                                {
                                                                                                                                                                                                    case var case84 when case84 <= 180d:
                                                                                                                                                                                                        {
                                                                                                                                                                                                            difsPPhiRet = 1.06d;
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

            return difsPPhiRet;
        }
        private static double _difsPDzetaN(double angleAlphaDegree, double n, double l0_, double re, double s, double t, double u)
        {
            /*
            Коэффициент ζнер для пирамидального диффузора
            Пункт 40 на странице 193, формула после формулы 5-8
            */
            double difsPDzetaNRet = default;
            double angleAlphaRadian = angleAlphaDegree * (Math.PI / 180);
            double block1 = 0.024*(0.625*angleAlphaRadian).Grade(s);
            double block2 =1-(2.81*n-1.81).Grade(-1.04);
            double block3 =(0.303*l0_).Grade(t);
            // a для работы функции Grade()
            double a = 1;
            double block4 =(4.8*((a*10).Grade(-7))*re+1.8).Grade(u);
            double dzeta = block1 * block2 * block3 * block4;
            // difsPDzetaNRet = 0.024 * Math.Pow(0.625 * alph, s) * (1 - Math.Pow(2.81 * n - 1.81, -1.04)) * Math.Pow(0.303 * l, t) * Math.Pow(4.8 * Math.Pow(10, -7) * re + 1.8, u);
            return dzeta;
        }
        private static double _difsPs(double alpha)
        {
            /*
            Коэффициент s 
            Пункт 40 на странице 193
            */
            double difsPsRet = default;
            double alph;
            alph = alpha * (Math.PI / 180d);

            difsPsRet = 1.06d / (1d + 2.82d * Math.Pow(10d, -3) * Math.Pow(alph, 2.24d));
            return difsPsRet;
        }
        private static double _difsPt(double l)
        {
            /*
            Коэффициент t
            Пункт 40 на странице 193
            */
            double difsPtRet = default;

            difsPtRet = 0.73d / (1d + 4.31d * Math.Pow(10d, -6) * Math.Pow(l, 7.31d));
            return difsPtRet;
        }
        private static double _difsPu(double re)
        {
            /*
            Коэффициент u
            Пункт 40 на странице 193
            */
            double difsPuRet = default;

            difsPuRet = 1d / (1d + 1.1d * Math.Pow(10d, -30.1d) * Math.Pow(re, 5.62d));
            return difsPuRet;
        }
        private static double _difsPDGidraulic(double asize, double bsize)
        {
            /*
            Гидравлический диаметр прямоугольного воздуховода
            Из названия функции может показаться, что она имеет отношение к пирамидальным диффузорам,
            но на самом деле это просто формула расчёта гидравлического диаметра воздуховода
            */
            double difsPDGidraulicRet = default;
            difsPDGidraulicRet = 2d * (asize * bsize) / (asize + bsize);
            return difsPDGidraulicRet;
        }
        private static double _difsPSizeCherta(double size, double difsPDGidraulic)
        {
            /*
            Отношение размера стороны воздуховода к гидравлическому диаметру,
            для расчёта коэффициентов a0 с чертой и b0 с чертой
            Пункт 40 на странице 193
            */
            double difsPSizeChertaRet = default;
            difsPSizeChertaRet = size / difsPDGidraulic;
            return difsPSizeChertaRet;
        }
        private static double _difsPXTilda(double a0_,double b0_,double angleAlphaDegree,double angleBetaDegree,double x_){
            double angleAlphaRadian = angleAlphaDegree * (Math.PI / 180);
            double angleBetaRadian=angleBetaDegree*(Math.PI/180);
            // развёрнутое название для пояснения
            double tanAlphaDividedBy2=Math.Tan(angleAlphaRadian/2);
            double tanBetaDividedBy2=Math.Tan(angleBetaRadian/2);
            // краткое название для использования
            double ta = tanAlphaDividedBy2;
            double tb = tanBetaDividedBy2;
            double block1 =(a0_+b0_)/
            (
                (4*a0_*Math.Tan(angleBetaRadian/2))-
                (b0_*Math.Tan(angleAlphaRadian/2))
            );
            double block21 =(a0_*tb+b0_*ta);
            double block22 =ta+tb;
            double block23 =2*ta*tb;
            double block2 =(block21*block22)/block23;
            double block31 =2*a0_*ta*tb.Grade(2);
            double block32 =a0_*b0_*ta*tb;
            double block33 =2*b0_*ta.Grade(2)*tb;
            double block34 =a0_*b0_*ta*tb;
            double block3 =Math.Log((block31+block32)/(block33+block34));
            double block4 =(ta+tb)/(8*ta*tb);
            double block51 =4*x_*ta*tb;
            double block52 =2*x_*(a0_*tb+b0_*ta);
            double block53 =a0_*b0_;
            double block54 =a0_*b0_;
            double block5=Math.Log((block51+block52+block53)/(block54));
            double xTilda = (block1 - block2) * block3 + block4 * block5;
            return xTilda;
        }

        private static double _difsPDHydraulicX(double asize1cherta, double bsize1cherta, double xCherta, double alpha, double beta)
        {
            /*
            Безразмерный гидравлический диаметр диффузора
            Пункт 40 на странице 193
            */
            double difsPDGidraulicXRet = default;
            double A;
            double b;
            double x;
            double alph;
            double bet;
            double ta;
            double tb;
            alph = alpha * (Math.PI / 180d);
            bet = beta * (Math.PI / 180d);
            A = asize1cherta;
            b = bsize1cherta;
            x = xCherta;
            ta = Math.Tan(alph / 2d);
            tb = Math.Tan(bet / 2d);
            difsPDGidraulicXRet = (2d * A * b + 4d * x * (A * tb + b * ta) + 8d * Math.Pow(x, 2d) * ta * tb) / (A + b + 2d * x * (ta + tb));
            return difsPDGidraulicXRet;

        }


    }
}