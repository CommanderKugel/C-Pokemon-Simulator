using static allPokemon;

Battle b = new([MyGarchomp, MyCloister, MySnorlax], [MyZapdos, MyHeatran, MyGengar], t1: new MostDmgTrainer(), t2: new MostDmgTrainer());
Perft.debugRollout(b);
