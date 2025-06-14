# Cell Tower Mapper

A C# Windows Forms application that visualizes cell tower locations on a map using NZMG coordinate data from CSV files.

## Features
- Load cell tower data from CSV files
- Display towers on a map with power range indicators
- Count towers based on power thresholds
- Convert NZMG coordinates to screen positions for Hamilton region
- Handle data validation and error reporting

## Technologies Used
- C# .NET Framework
- Windows Forms
- Graphics and Drawing operations
- File I/O operations
- Data validation

## How to Use
1. Run the application in Visual Studio
2. Use File → Open to load a CSV file with cell tower data
3. Towers will be displayed on the map with range circles
4. Enter a power value and use Tools → Count Towers to analyze data

## CSV Format Expected
Licencee,Location,Easting,Northing,Power
Vodafone,Hamilton Central,2710000,6385000,75.5
Spark,Hamilton East,2715000,6380000,50.0

## Code Structure
- **Form1.cs**: Main application logic
- **DrawTower()**: Renders cell towers and range circles
- **CalculateX/Y()**: Coordinate conversion methods
- **CountTowers()**: Data analysis functionality

## Learning Outcomes
This project demonstrates:
- Object-oriented programming principles
- File handling and data parsing
- Graphics programming
- Input validation
- Error handling

---
*Created as part of C# programming studies Compx101 Waikato University*