using SharpFluids;
using EngineeringUnits;
using EngineeringUnits.Units;
using HydraulicResistance.Helpers;
using Microsoft.VisualBasic.CompilerServices;

namespace HydraulicResistance.ElementsOfResistance
{
    public class TeeSupply
    {
        /*
        Со страницы 333:
        В общем случае основные потери в вытяжном тройнике складываются из 
        1) потерь на турбулентное смешение двух потоков, обладающих различными скоростями ("удар"),
        2) потерь на поворот потока при выходе его из бокового ответвления в сборный рукав,
        3) потерь на расширение потока в диффузорой части
        4) потерь в плавном отводе
        */
        public static double GetTeeSupplyOnPassAllRoundResistance(
                                                        double flowRateCollectorPipeCubicMetersPerHour,
                                                        double flowRateTurnPipeCubicMetersPerHour,
                                                        double diamCollectorPipeMillimeters,
                                                        double diamTurnPipeMillimeters,
                                                        double diamPassPipeMillimeters)
        {
            /*
            Сопротивление приточного тройника на проход, все ответвления круглого сечения
            */
            double ksi = default;
            var areaCollectorPipeSquareMeters = Mathematics.GetAreaCircle(diamCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaTurnPipeSquareMeters = Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaPassPipeSquareMeters = Mathematics.GetAreaCircle(diamPassPipeMillimeters).As(AreaUnit.SquareMeter);
            ksi = _getTeeSupplyOnPassResistance(flowRateCollectorPipeCubicMetersPerHour: flowRateCollectorPipeCubicMetersPerHour,
                                                flowRateTurnPipeCubicMetersPerHour: flowRateTurnPipeCubicMetersPerHour,
                                                areaCollectorPipeSquareMeters: areaCollectorPipeSquareMeters,
                                                areaTurnPipeSquareMeters: areaTurnPipeSquareMeters,
                                                areaPassPipeSquareMeters: areaPassPipeSquareMeters);
            return ksi;
        }
        public static double GetTeeSupplyOnPassAllRectangularResistance(
                                                                        double flowRateCollectorPipeCubicMetersPerHour,
                                                                        double flowRateTurnPipeCubicMetersPerHour,
                                                                        double widthCollectorPipeMillimeters,
                                                                        double heightCollectorPipeMillimeters,
                                                                        double widthTurnPipeMillimeters,
                                                                        double heightTurnPipeMillimeters,
                                                                        double widthPassPipeMillimeters,
                                                                        double heightPassPipeMillimeters)
        {
            /*
            Сопротивление приточного тройника на проход, все ответвления прямоугольного сечения
            */
            double ksi = default;
            var areaCollectorPipeSquareMeters = Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters, heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaTurnPipeSquareMeters = Mathematics.GetAreaRectangle(widthTurnPipeMillimeters, heightTurnPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaPassPipeSquareMeters = Mathematics.GetAreaRectangle(widthPassPipeMillimeters, heightPassPipeMillimeters).As(AreaUnit.SquareMeter);
            ksi = _getTeeSupplyOnPassResistance(flowRateCollectorPipeCubicMetersPerHour: flowRateCollectorPipeCubicMetersPerHour,
                                              flowRateTurnPipeCubicMetersPerHour: flowRateTurnPipeCubicMetersPerHour,
                                              areaCollectorPipeSquareMeters: areaCollectorPipeSquareMeters,
                                              areaTurnPipeSquareMeters: areaTurnPipeSquareMeters,
                                              areaPassPipeSquareMeters: areaPassPipeSquareMeters);
            return ksi;
        }
        public static double GetTeeSupplyOnPassAllRectangularButTurnRoundResistance(
                                                                                    double flowRateCollectorPipeCubicMetersPerHour,
                                                                                    double flowRateTurnPipeCubicMetersPerHour,
                                                                                    double widthCollectorPipeMillimeters,
                                                                                    double heightCollectorPipeMillimeters,
                                                                                    double diamTurnPipeMillimeters,
                                                                                    double widthPassPipeMillimeters,
                                                                                    double heightPassPipeMillimeters)
        {
            /*
            Сопротивление приточного тройника на проход, прямой и сборный участки прямоугольные, а ответвление - круглого сечения (к примеру случай с врезкой с воздуховод)
            */
            double ksi = default;
            var areaCollectorPipeSquareMeters = Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters, heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaTurnPipeSquareMeters = Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaPassPipeSquareMeters = Mathematics.GetAreaRectangle(widthPassPipeMillimeters, heightPassPipeMillimeters).As(AreaUnit.SquareMeter);
            ksi = _getTeeSupplyOnPassResistance(flowRateCollectorPipeCubicMetersPerHour: flowRateCollectorPipeCubicMetersPerHour,
                                                flowRateTurnPipeCubicMetersPerHour: flowRateTurnPipeCubicMetersPerHour,
                                                areaCollectorPipeSquareMeters: areaCollectorPipeSquareMeters,
                                                areaTurnPipeSquareMeters: areaTurnPipeSquareMeters,
                                                areaPassPipeSquareMeters: areaPassPipeSquareMeters);
            return ksi;
        }
        public static double GetTeeSupplyOnTurnAllRoundResistance(
                                                        double flowRateCollectorPipeCubicMetersPerHour,
                                                        double flowRateTurnPipeCubicMetersPerHour,
                                                        double diamCollectorPipeMillimeters,
                                                        double diamTurnPipeMillimeters,
                                                        double diamPassPipeMillimeters,
                                                        double angleDegrees)
        {
            double ksi = default;
            var areaCollectorPipeSquareMeters = Mathematics.GetAreaCircle(diamCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaTurnPipeSquareMeters = Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaPassPipeSquareMeters = Mathematics.GetAreaCircle(diamPassPipeMillimeters).As(AreaUnit.SquareMeter);
            ksi = _getTeeSupplyOnTurnResistance(flowRateCollectorPipeCubicMetersPerHour: flowRateCollectorPipeCubicMetersPerHour,
                                            flowRateTurnPipeCubicMetersPerHour: flowRateTurnPipeCubicMetersPerHour,
                                            areaCollectorPipeSquareMeters: areaCollectorPipeSquareMeters,
                                            areaTurnPipeSquareMeters: areaTurnPipeSquareMeters,
                                            areaPassPipeSquareMeters: areaPassPipeSquareMeters,
                                            angleDegrees);
            return ksi;
        }
        public static double GetTeeSupplyOnTurnAllRectangularResistance(
                                                                        double flowRateCollectorPipeCubicMetersPerHour,
                                                                        double flowRateTurnPipeCubicMetersPerHour,
                                                                        double widthCollectorPipeMillimeters,
                                                                        double heightCollectorPipeMillimeters,
                                                                        double widthTurnPipeMillimeters,
                                                                        double heightTurnPipeMillimeters,
                                                                        double widthPassPipeMillimeters,
                                                                        double heightPassPipeMillimeters,
                                                                        double angleDegrees)
        {
            double ksi = default;
            var areaCollectorPipeSquareMeters = Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters, heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaTurnPipeSquareMeters = Mathematics.GetAreaRectangle(widthTurnPipeMillimeters, heightTurnPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaPassPipeSquareMeters = Mathematics.GetAreaRectangle(widthPassPipeMillimeters, heightPassPipeMillimeters).As(AreaUnit.SquareMeter);
            ksi = _getTeeSupplyOnTurnResistance(flowRateCollectorPipeCubicMetersPerHour: flowRateCollectorPipeCubicMetersPerHour,
                                flowRateTurnPipeCubicMetersPerHour: flowRateTurnPipeCubicMetersPerHour,
                                areaCollectorPipeSquareMeters: areaCollectorPipeSquareMeters,
                                areaTurnPipeSquareMeters: areaTurnPipeSquareMeters,
                                areaPassPipeSquareMeters: areaPassPipeSquareMeters,
                                angleDegrees);
            return ksi;
        }
        public static double GetTeeSupplyOnTurnAllRectangularButTurnRoundResistance(
                                                                                    double flowRateCollectorPipeCubicMetersPerHour,
                                                                                    double flowRateTurnPipeCubicMetersPerHour,
                                                                                    double widthCollectorPipeMillimeters,
                                                                                    double heightCollectorPipeMillimeters,
                                                                                    double diamTurnPipeMillimeters,
                                                                                    double widthPassPipeMillimeters,
                                                                                    double heightPassPipeMillimeters,
                                                                                    double angleDegrees)
        {
            double ksi = default;
            var areaCollectorPipeSquareMeters = Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters, heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaTurnPipeSquareMeters = Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
            var areaPassPipeSquareMeters = Mathematics.GetAreaRectangle(widthPassPipeMillimeters, heightPassPipeMillimeters).As(AreaUnit.SquareMeter);
            ksi = _getTeeSupplyOnTurnResistance(flowRateCollectorPipeCubicMetersPerHour: flowRateCollectorPipeCubicMetersPerHour,
                                flowRateTurnPipeCubicMetersPerHour: flowRateTurnPipeCubicMetersPerHour,
                                areaCollectorPipeSquareMeters: areaCollectorPipeSquareMeters,
                                areaTurnPipeSquareMeters: areaTurnPipeSquareMeters,
                                areaPassPipeSquareMeters: areaPassPipeSquareMeters,
                                angleDegrees);
            return ksi;
        }

        private static double _getTeeSupplyOnPassResistance(double flowRateCollectorPipeCubicMetersPerHour, /* расход в сборном участке */
                                                         double flowRateTurnPipeCubicMetersPerHour /* расход в боковом ответвлении */,
                                                          double areaCollectorPipeSquareMeters /* площадь сечения сборного участка */,
                                                           double areaTurnPipeSquareMeters, /* площадь сечения бокового ответвления */
                                                           double areaPassPipeSquareMeters /* площадь сечения прямого участка до объединения */)
        {
            /*
            Коэффициент сопротивления тройника приточного на проход
            Пункт 16 на странице 336
            Коэффициент τ (tau) - таблица на странице 366 из диаграммы 7-20
            */
            double tau = default;
            if (areaTurnPipeSquareMeters + areaPassPipeSquareMeters > areaCollectorPipeSquareMeters && areaPassPipeSquareMeters == areaCollectorPipeSquareMeters)
            {
                switch (areaTurnPipeSquareMeters / areaCollectorPipeSquareMeters)
                {
                    case <= 0.4:
                        {
                            tau = 0.4;
                            break;
                        }
                    case > 0.4:
                        {
                            switch (flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour)
                            {
                                case <= 0.5:
                                    {
                                        tau = 2 * (2 * flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour - 1);
                                        break;
                                    }
                                case > 0.5:
                                    {
                                        tau = 0.3 * (2 * flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour - 1);
                                        break;
                                    }
                            }

                            break;
                        }
                }
            }
            double ksi = tau * Math.Pow(flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour, 2);
            return ksi;
        }
        private static double _getTeeSupplyOnTurnResistance(double flowRateCollectorPipeCubicMetersPerHour, /* расход в сборном участке */
                                                             double flowRateTurnPipeCubicMetersPerHour, /* расход в боковом ответвлении */
                                                              double areaCollectorPipeSquareMeters, /* площадь сечения сборного участка */
                                                               double areaTurnPipeSquareMeters, /* площадь сечения бокового ответвления */
                                                               double areaPassPipeSquareMeters, /* площадь сечения прямого ответвления до объединения */
                                                                double angleDegrees)
        {
            /*
            Коэффициент сопротивления тройника приточного на поворот
            Пункт 16 на странице 336,
            Значения A и Kb - таблицы 7-4 и 7-5 соответственно
            */
            double A = default;
            double Kb = default;
            if (areaTurnPipeSquareMeters + areaPassPipeSquareMeters > areaCollectorPipeSquareMeters && areaPassPipeSquareMeters == areaCollectorPipeSquareMeters)
            {
                Kb = 0;
                switch (areaTurnPipeSquareMeters / areaCollectorPipeSquareMeters)
                {
                    case <= 0.35:
                        {
                            switch (flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour)
                            {
                                case <= 0.4:
                                    {
                                        A = 1.1 - 0.7 * flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour;
                                        break;
                                    }
                                case > 0.4:
                                    {
                                        A = 0.85;
                                        break;
                                    }
                            }

                            break;
                        }
                    case > 0.35:
                        {
                            switch (flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour)
                            {
                                case <= 0.6:
                                    {
                                        A = 1 - 0.6 * flowRateTurnPipeCubicMetersPerHour / flowRateCollectorPipeCubicMetersPerHour;
                                        break;
                                    }
                                case > 0.6:
                                    {
                                        A = 0.6;
                                        break;
                                    }
                            }

                            break;
                        }
                }
            }
            else if (areaTurnPipeSquareMeters + areaPassPipeSquareMeters == areaCollectorPipeSquareMeters)
            {
                A = 1;
                switch (angleDegrees)
                {
                    case 15:
                        Kb = 0.04;
                        break;
                    case 30:
                        Kb = 0.16;
                        break;
                    case 45:
                        Kb = 0.36;
                        break;
                    case 60:
                        Kb = 0.64;
                        break;
                    case 90:
                        Kb = 1;
                        break;
                    default:
                        Kb = 1;
                        break;
                }
            }
            double velocityCollectorPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateCollectorPipeCubicMetersPerHour,areaCollectorPipeSquareMeters).As(SpeedUnit.MeterPerSecond);
            double velocityTurnPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateTurnPipeCubicMetersPerHour,areaTurnPipeSquareMeters).As(SpeedUnit.MeterPerSecond);
            double block1 = velocityTurnPipeMeterPerSecond/velocityCollectorPipeMeterPerSecond;
            double angle = Math.PI * angleDegrees / 180.0;
            double ksi = A * (1 + Math.Pow(block1, 2) - 2 * block1 * Math.Cos(angle)) - Kb * Math.Pow(block1, 2);
            return ksi;
        }
    }
}