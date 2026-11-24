CharacterBReeder

Interactive Genetic Algorithms for Procedural 2D Creature Generation
<img width="800" height="422" alt="image" src="https://github.com/user-attachments/assets/867c094f-b401-4749-b681-4fa73b9297a0" />

About the Project

CharacterBReeder is an experimental project focused on the procedural generation of 2D creature assets using Interactive Genetic Algorithms (IGA).

Developed in Unity, the project explores the integration of evolutionary computation and human evaluation as a method for generating and evolving visual characteristics of virtual creatures.

Unlike traditional Genetic Algorithms, which commonly rely on predefined mathematical fitness functions, this approach incorporates the user's visual preferences into the evolutionary process.

Users evaluate the generated creatures, and their feedback influences the selection and evolution of individuals in subsequent generations.

The project investigates how human preferences can guide procedural content generation, allowing users to participate directly in the evolutionary process of virtual character creation.

CharacterBReeder is being developed as the practical product of my Master's research in Creative Industries.

Project Overview

Project: CharacterBReeder

Main Concept: Procedural 2D Creature Generation

Core Technique: Interactive Genetic Algorithms

Research Areas: Procedural Content Generation, Evolutionary Computation, Human-Computer Interaction

Engine: Unity

Programming Language: C#

Dimension: 2D

Development Status: Work in Progress

Interactive Genetic Algorithm

The project uses an Interactive Genetic Algorithm to evolve populations of procedurally generated creatures based on human evaluation.

Instead of relying exclusively on an automated fitness function, the system uses the user's preferences as part of the evolutionary process.

The general workflow consists of the following stages:

Initial Population: Generate an initial population of procedurally created 2D creatures.

Visual Evaluation: Present the generated creatures to the user for individual evaluation.

Preference Collection: Collect user feedback about the generated individuals.

Selection: Use the evaluation results to influence which individuals contribute to the next generation.

Evolution: Apply genetic operations to generate new individuals from the selected population.

Next Generation: Present the newly generated creatures for another evaluation cycle.

This process creates an iterative interaction between the user and the evolutionary algorithm, allowing visual preferences to guide the generation of new creatures.

Procedural Creature Generation

Each individual represents a procedurally generated 2D creature.

The generation system defines the visual characteristics of these individuals through genetic information, allowing different combinations of traits to produce visual variations across the population.

As the algorithm evolves, the user's evaluations influence which visual characteristics are more likely to be represented in subsequent generations.

The goal is to explore a system capable of generating diverse creature designs while incorporating human preferences into the evolutionary process.
