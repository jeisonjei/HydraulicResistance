using System;
using EngineeringUnits;
using EngineeringUnits.Units;

namespace HydraulicResistance.Helpers
{
    public class Mathematics{
    public static Area GetAreaCircle(double diameterMillimeters){
        var radius = new Length(diameterMillimeters / 2,LengthUnit.Millimeter);
        var area=Math.PI*radius.Pow(2);
        return area.ToUnit(AreaUnit.SquareMeter);
    }
    public static Area GetAreaRectangle(double widthMillimeters,double heightMillimeters){
        var width=new Length(widthMillimeters,LengthUnit.Millimeter);
        var height=new Length(heightMillimeters,LengthUnit.Millimeter);
        var area = width * height;
        return area.ToUnit(AreaUnit.SquareMeter);
    }
    public static Length GetEquivalentDiameter(double widthMillimeters,double heightMillimeters){
        var eqD=(2*widthMillimeters*heightMillimeters)/(widthMillimeters+heightMillimeters);
        var equivalentDiameterMillimeter=new Length(eqD,LengthUnit.Millimeter);
            return equivalentDiameterMillimeter;
        }
}
}