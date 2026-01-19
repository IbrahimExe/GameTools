# GameTools

**Repository Purpose**  
**GameTools** is my personal repository for the *Game Tools and Pipelines* course. It serves as a running technical log of the tools I build throughout the semester. Each assignment represents a small but realistic tools-programming task, focused on automating repetitive work, building simple pipelines, and improving data reliability through refactoring and testing.

---

## Course Context

**Course:** Game Tools and Pipelines  

The course explores how tools programmers:
- Replace repetitive manual tasks with software tools
- Build pipelines that improve team efficiency
- Support artists, designers, and programmers with reliable utilities
- Rapidly develop and iterate on internal tools

---

## Assignment 1 — CSV Weapon Tool

### Objective
Create a basic C# console application that reads weapon data from a CSV file and outputs sorted or filtered results via command-line arguments.

### What was Implemented:
- A **C# .NET console application**
- Manual **command-line argument parsing**
- Weapon model with CSV parsing
- Console and file output
- Basic error handling

![Assignment1_Output](https://github.com/user-attachments/assets/0a28b8cc-7afc-49db-b714-c88c3ae18030)

### What I Learned
- File I/O in C#
- CLI tooling fundamentals
- Defensive parsing

---

## Assignment 2a — Refactoring & Tool Quality Improvements

### Objective
Refactor Assignment 1 into a cleaner, more maintainable, and testable tool while extending functionality.

### What was Implemented:
- Weapon parsing moved into the model
- Introduced `WeaponType` enum
- Added `WeaponCollection` with persistence
- Implemented sorting and querying
- Added NUnit unit tests
- Improved error handling

![Assignment2a_Output](https://github.com/user-attachments/assets/4ee94c24-c4d2-4f7c-a508-2fdc140febd2)

---

## File Overview

- **Weapon.cs** – Weapon model, CSV parsing, comparisons
- **WeaponCollection.cs** – Data storage, queries, persistence
- **IPersistence.cs** – Load/save interface
- **Program.cs** – CLI orchestration
- **UnitTests.cs** – NUnit test coverage
- **data2.csv** – Assignment dataset

---

## Personal Notes

This repository is meant as:
- A technical snapshot per assignment
- A reference for tools-programming patterns
- A foundation to build future tools on
