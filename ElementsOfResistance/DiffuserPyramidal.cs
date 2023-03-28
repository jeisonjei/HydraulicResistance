using System;
using EngineeringUnits;
using EngineeringUnits.Units;
using SharpFluids;
using HydraulicResistance.Helpers;

namespace HydraulicResistance.ElementsOfResistance
{
    public class DiffuserPyramidal
    {
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
            alph = alpha / (180d * Math.PI);
            bet = beta / (180d * Math.PI);

            difsPDzetaTRRet = lambda / 16d * (1d - 1d / Math.Pow(n, 2d)) * (1d / Math.Sin(alph / 2d) + 1d / Math.Sin(bet / 2d));
            return difsPDzetaTRRet;
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
        private static double _difsPDzetaN(double alpha, double n, double l, double re, double s, double t, double u)
        {
            /*
            Коэффициент ζнер для пирамидального диффузора
            Пункт 40 на странице 193, формула после формулы 5-8
            */
            double difsPDzetaNRet = default;
            double alph;
            alph = alpha * (Math.PI / 180d);

            difsPDzetaNRet = 0.024d * Math.Pow(0.625d * alph, s) * (1d - Math.Pow(2.81d * n - 1.81d, -1.04d)) * Math.Pow(0.303d * l, t) * Math.Pow(4.8d * Math.Pow(10d, -7) * re + 1.8d, u);
            return difsPDzetaNRet;
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
            для расчёта коэффициентов ax с чертой и bx с чертой
            Пункт 40 на странице 193
            */
            double difsPSizeChertaRet = default;
            difsPSizeChertaRet = size / difsPDGidraulic;
            return difsPSizeChertaRet;
        }

        private static double _difsPDGidraulicX(double asize1cherta, double bsize1cherta, double xCherta, double alpha, double beta)
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