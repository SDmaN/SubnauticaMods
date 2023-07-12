# Power Order

This mod allows you to prioritize power sources. By default game uses power sources in order they were loaded. For example if you have solar panel loaded firstly and thermal plant loaded secondly, your base will drain your solar panel until its power becomes zero and only then it will start using thermal plant.
With this mod you can change that order.

This mod is update of https://www.nexusmods.com/subnautica/mods/58 for the latest Subnautica version.

## Installation

Put archive contents into BepInEx/plugins folder or just use the Vortex.

## Configuration

After running for the first time this mod will create config.json in BepInEx/config/PowerOrder folder. You can edit this file to change order of power sources.

Default order is:

```
{
    "Order": [
        "RegenPowerCell",
        "SolarPanel",
        "ThermalPlant",
        "PowerTransmitter",
        "BioReactor",
        "NuclearReactor"
    ]
}
```
