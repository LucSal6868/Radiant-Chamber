# Radiant Chamber

A Unity simulation made for SimJam that visualizes radiation particle emissions in 3D space.

## Overview

Radiant Chamber renders radiation particle trails that travel outward from an emitter in random directions. Each trail simulates realistic scatter behavior — particles travel mostly straight but deflect at random intervals, mimicking how radiation particles interact with matter.

## Features

- Particle trails with configurable length, speed, and drag
- Randomized direction scatter with smoothed transitions
- Per-trail noise for visual variation
- Tunable via ScriptableObject (`RadiationData`) — no code changes needed

## Controls

| Key | Action |
|-----|--------|
| `Space` | Emit a radiation particle trail |

## Configuration

Create a `RadiationData` asset via **Assets > Create > Radiation > Radiation Data** and assign it to the `RadiationEmitter` component.

| Property | Description |
|----------|-------------|
| `length_range` | Min/max trail length |
| `scatter_frequency` | 1-in-N chance of direction change per step |
| `scatter_angle` | Max deflection angle in degrees |
| `scatter_smoothness` | How gradually direction changes (0 = instant, 1 = very smooth) |
| `speed` | Particles emitted per frame |
| `drag` | Exponential speed decay per frame |
| `particle_interval` | Distance between particles in the trail |
| `particle_noise` | Positional randomness per particle |

## Requirements

- Unity 6 or later
- XR Interaction Toolkit 3.4.1
- Universal Render Pipeline (URP)
