# Hydraulic Resistance
Library for calculations of fittings's hydraulic resistance made by "Handbook of Hydraulic Resistance" I.E.Idel'chik. This is alpha version of library so use with caution
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
## Attention
Pyramidal diffusers, confusers still in development
