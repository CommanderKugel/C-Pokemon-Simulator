using static allPokemon;


Pokemon[] TeamA = [MyGarchomp, MyCloister, MySnorlax];
Pokemon[] TeamB = [MyZapdos, MyHeatran, MyGengar];
Trainer trainerA = new RandomSamlesTrainer();
Trainer trainerB = new MostDmgTrainer();

Simulation.Simulate(TeamA, TeamB, trainerA, trainerB);
