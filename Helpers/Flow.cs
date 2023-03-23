using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringUnits;
using EngineeringUnits.Units;
using SharpFluids;

namespace HydraulicResistance.Helpers
{
	enum Shape
	{
		Rectangular,
		Round
	}

	public class Flow
	{
		private readonly Fluid _fluid;
		private readonly double _flowRate;
		private readonly double _width;
		private readonly double _height;
		private readonly double _diam;
		private readonly Shape _shape;
		public double Δ { get; set; }

		public Flow(FluidList fluid,double flowRate, double diam)
		{
			_fluid = new Fluid(fluid);
			_flowRate = flowRate;
			_diam = diam;
			_shape = Shape.Round;
		}
		public Flow(FluidList fluid, double flowRate, double width,double height)
		{
			_fluid = new Fluid(fluid);
			_flowRate = flowRate;
			_width = width;
			_height = height;
			_shape = Shape.Rectangular;
		}

		public Density GetFluidDensity(double tempCels)
		{
			_fluid.UpdatePT(Pressure.FromAtmosphere(1), Temperature.FromDegreesCelsius(tempCels));
			var density = _fluid.Density;
			return density;
		}
		public DynamicViscosity GetFluidDinamycViscosity(double tempCels)
		{
			_fluid.UpdatePT(Pressure.FromAtmosphere(1), Temperature.FromDegreesCelsius(tempCels));
			var dynamicViscosity=_fluid.DynamicViscosity;
			return dynamicViscosity;
		}
		public KinematicViscosity GetFluidKinematicViscosity(double tempCels)
		{
			_fluid.UpdatePT(Pressure.FromAtmosphere(1), Temperature.FromDegreesCelsius(tempCels));
			var kinematicViscosity=_fluid.DynamicViscosity/_fluid.Density; 
			return kinematicViscosity;
		}
		public Area GetTubeArea()
		{
			var area = new Area(default,AreaUnit.SquareMillimeter);
			if (_shape==Shape.Round)
			{
				var diam=new Length(_diam,LengthUnit.Millimeter);
				area = Math.PI * (diam/2).Pow(2);
			}
			else if (_shape==Shape.Rectangular)
			{
				var width=new Length(_width,LengthUnit.Millimeter);
				var height=new Length(_height,LengthUnit.Millimeter);
				area = width * height;
			}
			else
			{
				return default; 
			}
			return area.ToUnit(AreaUnit.SquareMeter);
		}
		public Length GetHidraulicDiameter(){
			Length hidraulicDiam=new Length(default,LengthUnit.Millimeter);
			if (_shape==Shape.Round)
			{
				var diam = new Length(_diam, LengthUnit.Millimeter);
				hidraulicDiam = diam;
			}
			else if (_shape==Shape.Rectangular)
			{
				var width=new Length(_width,LengthUnit.Millimeter);
				var height=new Length(_height,LengthUnit.Millimeter);
				hidraulicDiam=(2*width*height)/(width+height);
			}
			return hidraulicDiam;
		}
		public Speed GetFlowVelocity()
		{
			var area = GetTubeArea();
			var flow=new VolumeFlow(_flowRate,VolumeFlowUnit.CubicMeterPerHour);
			var velocity = flow / area;
			return velocity.ToUnit(SpeedUnit.MeterPerSecond);
		}
		public static Speed GetFlowVelocity(double flowRateCubicMeterPerHour,double areaSquareMeter)
		{
			var area = new Area(areaSquareMeter,AreaUnit.SquareMeter);
			var flow=new VolumeFlow(flowRateCubicMeterPerHour,VolumeFlowUnit.CubicMeterPerHour);
			var velocity = flow / area;
			return velocity.ToUnit(SpeedUnit.MeterPerSecond);
		}
		public double GetReinoldsNumber(double tempCels){
			var velocity = GetFlowVelocity();
			var hidraulicDiam = GetHidraulicDiameter();
			var kinematicViscosity=GetFluidKinematicViscosity(tempCels);
			var re=(velocity*hidraulicDiam.ToUnit(LengthUnit.Meter)/kinematicViscosity);
			return re;
		}
		public double GetLambda(double tempCels,double equivalentRoughness){
			var re=GetReinoldsNumber(tempCels);
			var hidraulicDiam = GetHidraulicDiameter();
			var er=new Length(equivalentRoughness,LengthUnit.Millimeter);
			var lambda=0.11*Math.Pow(((er/hidraulicDiam)+(0.68/re)),0.25);
			return lambda;
		}
		public Pressure GetPressureLossPerMeter(double tempCels,double equivalentRoughness){
			var lambda=GetLambda(tempCels,equivalentRoughness);
			var density=GetFluidDensity(tempCels);
			var velocity = GetFlowVelocity();
			var diam=GetHidraulicDiameter();
			var length=new Length(1,LengthUnit.Meter);
			var pressureLossPerMeter=length*lambda*(density*velocity.Pow(2))/(2*diam.ToUnit(LengthUnit.Meter));
			return pressureLossPerMeter;
		}
		public static VolumeFlow GetVolumeFlow(double velocityMetersPerSecond,double areaSquareMeters)
		{
			var volumeFlow=new VolumeFlow((velocityMetersPerSecond*areaSquareMeters),VolumeFlowUnit.CubicMeterPerSecond);
            return volumeFlow.ToUnit(VolumeFlowUnit.CubicMeterPerHour);
        }

	}
}
