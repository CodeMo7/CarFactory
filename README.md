# Car Factory System

## Overview
This project simulates a car factory that supports multiple engine types:
- Gasoline Engine
- Electronic Engine
- Hybrid Engine

## Design Concepts Used
- Interface-based design (IEngine)
- Dependency Injection
- Factory Pattern
- Polymorphism

## Features
- Create cars with different engine types
- Replace engine at runtime
- Speed control (Start, Stop, Accelerate, Brake)
- Hybrid engine optimization (switches engines based on speed)

## How to Run
1. Build the project
2. Run Program.cs

## Notes
- Hybrid engine uses electric below 50 km/h and gasoline above
- Engines are decoupled from car logic
