using System;
using HydraulicResistance.Helpers;
using SharpFluids;
using EngineeringUnits;
using EngineeringUnits.Units;

public class TeeExhaust
{
    public static double GetTeeExhaustOnPassAllRoundResistance(FluidList fluid,
                                                double flowRateCollectorPipeCubicMetersPerHour,
                                                double flowRateTurnPipeCubicMetersPerHour,
                                                double diamCollectorPipeMillimeters,
                                                double diamTurnPipeMillimeters,
                                                double diamPassPipeMillimeters)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaCircle(diamCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaCircle(diamPassPipeMillimeters).As(AreaUnit.SquareMeter);
        return ksi;
    }
    public static double GetTeeExhaustOnPassAllRectangularResistance(FluidList fluid,
                                                                    double flowRateCollectorPipeCubicMetersPerHour,
                                                                    double flowRateTurnPipeCubicMetersPerHour,
                                                                    double widthCollectorPipeMillimeters,
                                                                    double heightCollectorPipeMillimeters,
                                                                    double widthTurnPipeMillimeters,
                                                                    double heightTurnPipeMillimeters,
                                                                    double widthPassPipeMillimeters,
                                                                    double heightPassPipeMillimeters)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters,heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaRectangle(widthTurnPipeMillimeters,heightTurnPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeters,heightPassPipeMillimeters);

        return ksi;
    }
    public static double GetTeeExhaustOnPassAllRectangularButTurnRoundResistance(FluidList fluid,
                                                                                double flowRateCollectorPipeCubicMetersPerHour,
                                                                                double flowRateTurnPipeCubicMetersPerHour,
                                                                                double widthCollectorPipeMillimeters,
                                                                                double heightCollectorPipeMillimeters,
                                                                                double diamTurnPipeMillimeters,
                                                                                double widthPassPipeMillimeters,
                                                                                double heightPassPipeMillimeters)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters,heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeters,heightPassPipeMillimeters).As(AreaUnit.SquareMeter);

        return ksi;
    }
    public static double GetTeeExhaustOnTurnAllRoundResistance(FluidList fluid,
                                                    double flowRateCollectorPipeCubicMetersPerHour,
                                                    double flowRateTurnPipeCubicMetersPerHour,
                                                    double diamCollectorPipeMillimeters,
                                                    double diamTurnPipeMillimeters,
                                                    double diamPassPipeMillimeters,
                                                    double angleDegrees)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaCircle(diamCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaCircle(diamPassPipeMillimeters).As(AreaUnit.SquareMeter);

        return ksi;
    }
    public static double GetTeeExhaustOnTurnAllRectangularResistance(FluidList fluid,
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
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters,heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaRectangle(widthTurnPipeMillimeters,heightTurnPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeters,heightPassPipeMillimeters).As(AreaUnit.SquareMeter);

        return ksi;
    }
    public static double GetTeeExhaustOnTurnAllRectangularButTurnRoundResistance(FluidList fluid,
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
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeters,heightCollectorPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeters).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeters,heightPassPipeMillimeters).As(AreaUnit.SquareMeter);

        return ksi;
    }
    private static double _getTeeExhaustOnPassResistance(
                                                        double areaCollectorPipeSquareMeters /* площадь сечения сборного воздуховода */,
                                                        double areaTurnPipeSquareMeters /* площадь сечения бокового ответвления */,
                                                        double flowRateCollectorPipeCubicMeterPerHour, /* расход в сборном участке */
                                                        double flowRateTurnPipeCubicMeterPerHour, /* расход в боковом участке */
                                                        double angleDegrees /* угол */
                                                        )
    {
        /*
		 * Сопротивление тройника вытяжного на проход, формула 7-2
		 */
        double K = _getTeeExhaustCoefficientK(areaTurnPipeSquareMeter: areaTurnPipeSquareMeters,
                                        areaCollectorPipeSquareMeter: areaCollectorPipeSquareMeters,
                                        flowRateTurnPipeCubicMeterPerHour: flowRateTurnPipeCubicMeterPerHour,
                                        flowRateCollectorPipeCubicMeterPerHour: flowRateCollectorPipeCubicMeterPerHour);
        double ksi = default;
        double alph;
        alph = angleDegrees * (Math.PI / 180d);
        ksi = 1d - Math.Pow(1d - flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour, 2d) - (1.4d - flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour) * Math.Pow(flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour, 2d) * Math.Sin(alph) - 2d * K * (areaCollectorPipeSquareMeters / areaTurnPipeSquareMeters) * (flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour) * Math.Cos(alph);
        return ksi;

    }
    private static double _getTeeExhaustOnTurnResistance(
                                                        double velocityCollectorPipeMeterPerSecond, /* скорость в сборном участке */
                                                        double velocityTurnPipeMeterPerSecond, /* скорость в боковом участке */
                                                        double velocityPassPipeMeterPerSecond, /* скорость в прямом участке (до соединения с боковым) */
                                                        double areaCollectorPipeSquareMeter, /* площадь сечения сборного участка */
                                                        double areaTurnPipeSquareMeter, /* площадь сечения бокового участка */
                                                        double areaPassPipeSquareMeter, /* площадь сечения прямого участка (до соединения с боковым) */
                                                        double angleDegrees /* угол */)

    {
        /*
		 Сопротивление тройника вытяжного на поворот, формула 7-1
		 */
        double flowRateTurnPipeCubicMeterPerHour = Flow.GetVolumeFlow(velocityTurnPipeMeterPerSecond, areaTurnPipeSquareMeter).As(VolumeFlowUnit.CubicMeterPerHour);
        double flowRateCollectorPipeCubicMeterPerHour = Flow.GetVolumeFlow(velocityCollectorPipeMeterPerSecond, areaCollectorPipeSquareMeter).As(VolumeFlowUnit.CubicMeterPerHour);
        double A = _getTeeExhaustCoefficientA(areaCollectorPipeSquareMeter: areaCollectorPipeSquareMeter,
                                            areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                            flowRateCollectorPipeCubicMeterPerHour: flowRateCollectorPipeCubicMeterPerHour,
                                            flowRateTurnPipeCubicMeterPerHour: flowRateTurnPipeCubicMeterPerHour);
        double ksi = default;
        double alph;
        alph = angleDegrees * (Math.PI / 180d);
        ksi = A * (1d + Math.Pow(velocityTurnPipeMeterPerSecond / velocityCollectorPipeMeterPerSecond, 2d) - 2d * (areaPassPipeSquareMeter / areaCollectorPipeSquareMeter) * Math.Pow(velocityPassPipeMeterPerSecond / velocityCollectorPipeMeterPerSecond, 2d) - 2d * (areaTurnPipeSquareMeter / areaCollectorPipeSquareMeter) * Math.Pow(velocityTurnPipeMeterPerSecond / velocityCollectorPipeMeterPerSecond, 2d) * Math.Cos(alph));
        return ksi;

    }

    private static double _getTeeExhaustCoefficientA(
                                                 double areaCollectorPipeSquareMeter,   /* площадь сечения сборного участка */
                                                 double areaTurnPipeSquareMeter, /* площадь сечения бокового участка */
                                                 double flowRateCollectorPipeCubicMeterPerHour, /* расход в сборном участке */
                                                 double flowRateTurnPipeCubicMeterPerHour /* расход в боковом участке */)
    {
        /* 
		Коэффициент A по таблице 7-1
		 */
        double A = default;
        switch (areaTurnPipeSquareMeter / areaCollectorPipeSquareMeter)
        {
            case <= 0.35:
                {
                    A = 1;
                    break;
                }
            case > 0.35:
                {
                    switch (flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour)
                    {
                        case <= 0.4:
                            {
                                A = 0.9 * (1 - flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour);
                                break;
                            }
                        case > 0.4:
                            {
                                A = 0.55;
                                break;
                            }
                    }

                    break;
                }
        }
        return A;
    }
    private static double _getTeeExhaustCoefficientK(
                                                    double areaCollectorPipeSquareMeter, /* площадь сечения сборного участка */
                                                    double areaTurnPipeSquareMeter, /* площадь сечения бокового участка */
                                                    double flowRateCollectorPipeCubicMeterPerHour, /* расход в сборном участке */
                                                    double flowRateTurnPipeCubicMeterPerHour /* расход в боковом участке */)
    {
        /* 
		Коэффициент Kп(') из таблицы 7-3
		 */
        double K = default;
        switch (areaTurnPipeSquareMeter / areaCollectorPipeSquareMeter)
        {
            case <= 0.35:
                {
                    K = 0.8 * (flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour);
                    break;
                }
            case > 0.35:
                {
                    switch (flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour)
                    {
                        case <= 0.6:
                            {
                                K = 0.5;
                                break;
                            }
                        case > 0.6:
                            {
                                K = 0.8 * (flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour);
                                break;
                            }
                    }

                    break;
                }
        }
        return K;
    }
}