using System;
using HydraulicResistance.Helpers;
using SharpFluids;
using EngineeringUnits;
using EngineeringUnits.Units;

public class TeeExhaust
{
    public static double OnPassRound(
                                                double flowRateCollectorPipeCubicMeterPerHour,
                                                double flowRateTurnPipeCubicMeterPerHour,
                                                double diamCollectorPipeMillimeter,
                                                double diamTurnPipeMillimeter,
                                                double diamPassPipeMillimeter,
                                                double angleDegree)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaCircle(diamCollectorPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaCircle(diamPassPipeMillimeter).As(AreaUnit.SquareMeter);
        ksi = _getTeeExhaustOnPassResistance(areaCollectorPipeSquareMeter:areaCollectorPipeSquareMeter,
                                            areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                            flowRateCollectorPipeCubicMeterPerHour: flowRateCollectorPipeCubicMeterPerHour,
                                            flowRateTurnPipeCubicMeterPerHour: flowRateTurnPipeCubicMeterPerHour,
                                            angleDegree: angleDegree);
        return ksi;
    }
    public static double OnPassRectangular(
                                                                    double flowRateCollectorPipeCubicMeterPerHour,
                                                                    double flowRateTurnPipeCubicMeterPerHour,
                                                                    double widthCollectorPipeMillimeter,
                                                                    double heightCollectorPipeMillimeter,
                                                                    double widthTurnPipeMillimeter,
                                                                    double heightTurnPipeMillimeter,
                                                                    double widthPassPipeMillimeter,
                                                                    double heightPassPipeMillimeter,
                                                                    double angleDegree)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeter,heightCollectorPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaRectangle(widthTurnPipeMillimeter,heightTurnPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeter,heightPassPipeMillimeter);
                ksi = _getTeeExhaustOnPassResistance(areaCollectorPipeSquareMeter:areaCollectorPipeSquareMeter,
                                            areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                            flowRateCollectorPipeCubicMeterPerHour: flowRateCollectorPipeCubicMeterPerHour,
                                            flowRateTurnPipeCubicMeterPerHour: flowRateTurnPipeCubicMeterPerHour,
                                            angleDegree: angleDegree);
        
        return ksi;
    }
    public static double OnPassRectangularButTurnRound(
                                                                                double flowRateCollectorPipeCubicMeterPerHour,
                                                                                double flowRateTurnPipeCubicMeterPerHour,
                                                                                double widthCollectorPipeMillimeter,
                                                                                double heightCollectorPipeMillimeter,
                                                                                double diamTurnPipeMillimeter,
                                                                                double widthPassPipeMillimeter,
                                                                                double heightPassPipeMillimeter,
                                                                                double angleDegree)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeter,heightCollectorPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeter,heightPassPipeMillimeter).As(AreaUnit.SquareMeter);
        ksi = _getTeeExhaustOnPassResistance(areaCollectorPipeSquareMeter:areaCollectorPipeSquareMeter,
                                            areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                            flowRateCollectorPipeCubicMeterPerHour: flowRateCollectorPipeCubicMeterPerHour,
                                            flowRateTurnPipeCubicMeterPerHour: flowRateTurnPipeCubicMeterPerHour,
                                            angleDegree: angleDegree);

        return ksi;
    }
    public static double OnTurnRound(
                                                    double flowRateCollectorPipeCubicMeterPerHour,
                                                    double flowRateTurnPipeCubicMeterPerHour,
                                                    double diamCollectorPipeMillimeter,
                                                    double diamTurnPipeMillimeter,
                                                    double diamPassPipeMillimeter,
                                                    double angleDegree)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaCircle(diamCollectorPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaCircle(diamPassPipeMillimeter).As(AreaUnit.SquareMeter);
        var velocityCollectorPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateCollectorPipeCubicMeterPerHour,areaCollectorPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        var velocityTurnPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateTurnPipeCubicMeterPerHour,areaTurnPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        double flowRatePassPipeCubicMeterPerHour = flowRateCollectorPipeCubicMeterPerHour - flowRateTurnPipeCubicMeterPerHour;
        var velocityPassPipeMeterPerSecond =Flow.GetFlowVelocity(flowRatePassPipeCubicMeterPerHour,areaPassPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        ksi = _getTeeExhaustOnTurnResistance(velocityCollectorPipeMeterPerSecond: velocityCollectorPipeMeterPerSecond,
                                            velocityTurnPipeMeterPerSecond: velocityTurnPipeMeterPerSecond,
                                            velocityPassPipeMeterPerSecond: velocityPassPipeMeterPerSecond,
                                            areaCollectorPipeSquareMeter: areaCollectorPipeSquareMeter,
                                            areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                            areaPassPipeSquareMeter: areaPassPipeSquareMeter,
                                            angleDegree: angleDegree);
        return ksi;
    }
    public static double OnTurnRectangular(
                                                                    double flowRateCollectorPipeCubicMeterPerHour,
                                                                    double flowRateTurnPipeCubicMeterPerHour,
                                                                    double widthCollectorPipeMillimeter,
                                                                    double heightCollectorPipeMillimeter,
                                                                    double widthTurnPipeMillimeter,
                                                                    double heightTurnPipeMillimeter,
                                                                    double widthPassPipeMillimeter,
                                                                    double heightPassPipeMillimeter,
                                                                    double angleDegree)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeter,heightCollectorPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaRectangle(widthTurnPipeMillimeter,heightTurnPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeter,heightPassPipeMillimeter).As(AreaUnit.SquareMeter);
        var velocityCollectorPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateCollectorPipeCubicMeterPerHour,areaCollectorPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        var velocityTurnPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateTurnPipeCubicMeterPerHour,areaTurnPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        double flowRatePassPipeCubicMeterPerHour = flowRateCollectorPipeCubicMeterPerHour - flowRateTurnPipeCubicMeterPerHour;
        var velocityPassPipeMeterPerSecond =Flow.GetFlowVelocity(flowRatePassPipeCubicMeterPerHour,areaPassPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        ksi = _getTeeExhaustOnTurnResistance(velocityCollectorPipeMeterPerSecond: velocityCollectorPipeMeterPerSecond,
                                            velocityTurnPipeMeterPerSecond: velocityTurnPipeMeterPerSecond,
                                            velocityPassPipeMeterPerSecond: velocityPassPipeMeterPerSecond,
                                            areaCollectorPipeSquareMeter: areaCollectorPipeSquareMeter,
                                            areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                            areaPassPipeSquareMeter: areaPassPipeSquareMeter,
                                            angleDegree: angleDegree);

        return ksi;
    }
    public static double OnTurnRectangularButTurnRound(
                                                                                double flowRateCollectorPipeCubicMeterPerHour,
                                                                                double flowRateTurnPipeCubicMeterPerHour,
                                                                                double widthCollectorPipeMillimeter,
                                                                                double heightCollectorPipeMillimeter,
                                                                                double diamTurnPipeMillimeter,
                                                                                double widthPassPipeMillimeter,
                                                                                double heightPassPipeMillimeter,
                                                                                double angleDegree)
    {
        double ksi = default;
        var areaCollectorPipeSquareMeter =Mathematics.GetAreaRectangle(widthCollectorPipeMillimeter,heightCollectorPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaTurnPipeSquareMeter =Mathematics.GetAreaCircle(diamTurnPipeMillimeter).As(AreaUnit.SquareMeter);
        var areaPassPipeSquareMeter =Mathematics.GetAreaRectangle(widthPassPipeMillimeter,heightPassPipeMillimeter).As(AreaUnit.SquareMeter);
        var velocityCollectorPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateCollectorPipeCubicMeterPerHour,areaCollectorPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        var velocityTurnPipeMeterPerSecond =Flow.GetFlowVelocity(flowRateTurnPipeCubicMeterPerHour,areaTurnPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        double flowRatePassPipeCubicMeterPerHour = flowRateCollectorPipeCubicMeterPerHour - flowRateTurnPipeCubicMeterPerHour;
        var velocityPassPipeMeterPerSecond =Flow.GetFlowVelocity(flowRatePassPipeCubicMeterPerHour,areaPassPipeSquareMeter).As(SpeedUnit.MeterPerSecond);
        ksi = _getTeeExhaustOnTurnResistance(velocityCollectorPipeMeterPerSecond: velocityCollectorPipeMeterPerSecond,
                                            velocityTurnPipeMeterPerSecond: velocityTurnPipeMeterPerSecond,
                                            velocityPassPipeMeterPerSecond: velocityPassPipeMeterPerSecond,
                                            areaCollectorPipeSquareMeter: areaCollectorPipeSquareMeter,
                                            areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                            areaPassPipeSquareMeter: areaPassPipeSquareMeter,
                                            angleDegree: angleDegree);

        return ksi;
    }
    private static double _getTeeExhaustOnPassResistance(
                                                        double areaCollectorPipeSquareMeter /* площадь сечения сборного воздуховода */,
                                                        double areaTurnPipeSquareMeter /* площадь сечения бокового ответвления */,
                                                        double flowRateCollectorPipeCubicMeterPerHour, /* расход в сборном участке */
                                                        double flowRateTurnPipeCubicMeterPerHour, /* расход в боковом участке */
                                                        double angleDegree /* угол */
                                                        )
    {
        /*
		 * Сопротивление тройника вытяжного на проход, формула 7-2
		 */
        double K = _getTeeExhaustCoefficientK(areaTurnPipeSquareMeter: areaTurnPipeSquareMeter,
                                        areaCollectorPipeSquareMeter: areaCollectorPipeSquareMeter,
                                        flowRateTurnPipeCubicMeterPerHour: flowRateTurnPipeCubicMeterPerHour,
                                        flowRateCollectorPipeCubicMeterPerHour: flowRateCollectorPipeCubicMeterPerHour);
        double ksi = default;
        double angleRadian;
        angleRadian = angleDegree * (Math.PI / 180d);
        ksi = 1d - Math.Pow(1d - flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour, 2d) - (1.4d - flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour) * Math.Pow(flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour, 2d) * Math.Sin(angleRadian) - 2d * K * (areaCollectorPipeSquareMeter / areaTurnPipeSquareMeter) * (flowRateTurnPipeCubicMeterPerHour / flowRateCollectorPipeCubicMeterPerHour) * Math.Cos(angleRadian);
        return ksi;

    }
    private static double _getTeeExhaustOnTurnResistance(
                                                        double velocityCollectorPipeMeterPerSecond, /* скорость в сборном участке */
                                                        double velocityTurnPipeMeterPerSecond, /* скорость в боковом участке */
                                                        double velocityPassPipeMeterPerSecond, /* скорость в прямом участке (до соединения с боковым) */
                                                        double areaCollectorPipeSquareMeter, /* площадь сечения сборного участка */
                                                        double areaTurnPipeSquareMeter, /* площадь сечения бокового участка */
                                                        double areaPassPipeSquareMeter, /* площадь сечения прямого участка (до соединения с боковым) */
                                                        double angleDegree /* угол */)

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
        double angleRadian;
        angleRadian = angleDegree * (Math.PI / 180);
        ksi = A * (1 + Math.Pow(velocityTurnPipeMeterPerSecond / velocityCollectorPipeMeterPerSecond, 2) - 2 * (areaPassPipeSquareMeter / areaCollectorPipeSquareMeter) * Math.Pow(velocityPassPipeMeterPerSecond / velocityCollectorPipeMeterPerSecond, 2) - 2 * (areaTurnPipeSquareMeter / areaCollectorPipeSquareMeter) * Math.Pow(velocityTurnPipeMeterPerSecond / velocityCollectorPipeMeterPerSecond, 2) * Math.Cos(angleRadian));
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