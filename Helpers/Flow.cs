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
    internal class Flow
    {
        private readonly Fluid _fluid;
        private readonly double _flowRate;
        private readonly double _width;
        private readonly double _height;
        private readonly double _diam;
        private readonly Shape _shape;

        public Flow(Fluid fluid,double flowRate, double diam)
        {
            _fluid = fluid;
            _flowRate = flowRate;
            _diam = diam;
            _shape = Shape.Round;
        }
        public Flow(Fluid fluid, double flowRate, double width,double height)
        {
            _fluid = fluid;
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
        public double GetTubeArea()
        {
            double area = default;
            if (_shape==Shape.Round)
            {
                area = Math.PI * Math.Pow(_diam / 2000d, 2d);
            }
            else if (_shape==Shape.Rectangular)
            {
                area = _width / 1000d * (_height / 1000d);
            }
            else
            {
                return default; 
            }
            return area;
        }
        public double GetFlowVelocity()
        {
            double area=GetTubeArea();
            double velocity = _flowRate / (3600*area);
            return velocity;
        }

    }
}
