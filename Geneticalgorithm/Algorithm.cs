﻿using System;
using System.Threading;

namespace Geneticalgorithm
{
    public class Algorithm
    {

        public static string FinalResult { get; set; }
        public static string Characters { get; set; }
        public static double CrossoverRate { get; set; }
        public static double MutationRate { get; set; }

        public static Population NewGeneration(Population population, bool elitism)
        {
     
            Random r = new Random();
            //nova população do mesmo tamanho da antiga porém toda nulla
            Population newPopulation = new Population(population.PopulationLenth);

            Individual bestIndividual = null;

            //se tiver elitismo, mantém o melhor indivíduo da geração atual
            if (elitism)
            {
                bestIndividual = population.GetBestIndividual();
                newPopulation.SetIndividuo(bestIndividual);
            }

            //insere novos indivíduos na nova população, até atingir o tamanho máximo
            while (newPopulation.Length() < newPopulation.PopulationLenth)
            {
                Thread.Sleep(30);
                //seleciona os 2 pais por torneio
                Individual[] fathers = GetTwoRandomBestIndividual(population);

                Individual[] child = new Individual[2];

                //verifica a taxa de crossover, se sim realiza o crossover, se não, mantém os pais selecionados para a próxima geração
                if (r.NextDouble() <= CrossoverRate)
                {
                    if(bestIndividual != null)
                        child = Crossover(bestIndividual, fathers[0]);
                    else
                        child = Crossover(fathers[1], fathers[0]);
                }
                else
                {
                    child[0] = new Individual(fathers[0].Genes);
                    child[1] = new Individual(fathers[1].Genes);
                }

                //adiciona os filhos na nova geração
                newPopulation.SetIndividuo(child[0]);
                newPopulation.SetIndividuo(child[1]);
            }

            //ordena a nova população
            newPopulation.SortPopulation();
            return newPopulation;
        }

        public static Individual[] Crossover(Individual individual1, Individual individual2)
        {
            Random r = new Random();

            //sorteia o ponto de corte
            int Cutoff1 = r.Next((individual1.Genes.Length));//3
            int Cutoff2 = individual2.Genes.Length - Cutoff1;//7
       

            Individual[] sons = new Individual[2];

            //pega os genes dos pais
            string fatherGene1 = individual1.Genes;
            string fatherGene2 = individual2.Genes;

            string SonGene1;
            string sonGene2;

            //realiza o corte
            //father1 = abcdefgh56
            //father 2 = asdcvbnm,.
            SonGene1 = fatherGene1.Substring(0, Cutoff1);//abc
            SonGene1 += fatherGene2.Substring(Cutoff1, Cutoff2 );//abccvbn
            //SonGene1 += fatherGene1.Substring(Cutoff2, fatherGene1.Length);

            sonGene2 = fatherGene2.Substring(0, Cutoff1);//asd
            sonGene2 += fatherGene1.Substring(Cutoff1, Cutoff2);//asddefg
            //sonGene2 += fatherGene2.Substring(Cutoff2, fatherGene2.Length);//asddefgm,.

            //cria o novo indivíduo com os genes dos pais
            sons[0] = new Individual(SonGene1);
            sons[1] = new Individual(sonGene2);

            return sons;
        }

        //mudar o nome desse metodo para get two best intermediate individuals
        public static Individual[] GetTwoRandomBestIndividual(Population population)
        {
            Random r = new Random();
            Population randomPopulation = new Population(4);

            //seleciona 3 indivíduos aleatóriamente na população
            for (int i = 0; i < 4; i++)
            {
                randomPopulation.SetIndividuo(population.GetIndivdualAt(r.Next(population.PopulationLenth)));
            }

            //ordena a população
            randomPopulation.SortPopulation();

            Individual[] pais = new Individual[2];

            //seleciona os 2 melhores deste população
            pais[0] = randomPopulation.GetIndivdualAt(0);
            pais[1] = randomPopulation.GetIndivdualAt(1);

            return pais;
        }

    }
}
