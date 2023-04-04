# Hydraulic Resistance
GitHub repository <https://github.com/jeisonjei/HydraulicResistance>  
Library for calculations of fittings's hydraulic resistance based on "Handbook of Hydraulic Resistance" I.E.Idel'chik. This is alpha version of library so use with caution
## Arguments
`equivalentRoughnessMillimeter` - 0.1 mm for ducts and 0.2 mm for water heating pipes can be used. Detailed table for different types of pipes can be found at <https://ccjx.space/handbook/Эквивалентная-шероховатость-труб-и-каналов> - page on russian but can be translated using browser tools.  
`tempCels` - air or water temperature in degrees Celsius.
  
Rest of arguments should be self-explained
## Elbows
Depending on shape use ElbowRound or ElbowRectangular:
```
double roundElbowResistance=ElbowRound.Resistance(...arguments...);
```
## Tees
Depending on flow direction and shape use appropriate classes. Methods `.OnPassResistance()` means main passage resistance and `.ToTurnResistance()` means branch passage resistance
```
double supplyTeeRectangularBranchResistance=TeeSupplyRectangular.ToTurnResistance(...arguments...);
```
## Diffusers
To calculate conical diffuser's resistance:
```
double conicalDiffuserResistance=DiffuserConical.Resistance(...arguments...)
```
To calculate pyramidal diffuser's resistance:
```
double pyramidalDiffuserResistance=DiffuserPyramidal.Resistance(...arguments...)
```
## Confusers
To calculate conical confuser's resistance:
```
double conicalConfuserResistance=ConfuserConical.Resistance(...arguments...)
```
To calculate pyramidal confuser's resistance:
```
double pyramidalConfuserResistance=ConfuserPyramidal.Resistance(...arguments...)
```
To calculate flat confuser's resistance:
```
double flatConfuserResistance=ConfuserFlat.Resistance(...arguments...);
```
## Velocity, Reinolds number and Lambda
To get velocity:
```
var flow=new Flow(...arguments...);
var velocity=flow.GetFlowVelocity(...arguments...);
```
To get Lambda:
```
var flow=new Flow(...arguments...);
var lambda=flow.GetLambda(...arguments...);
```
## Attention
Flat Diffusers still in development
