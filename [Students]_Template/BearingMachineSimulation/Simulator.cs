﻿using System;
using System.Collections.Generic;
using BearingMachineModels;

namespace BearingMachineSimulation
{
    /// <summary>
    /// 
    /// </summary>
    static class Simulator
    {
        static Random random = new Random();
        static int totalDelayC = 0;
        static int totalDelayP= 0;
        /// <summary>
        /// Extract the random value out of a TimeDistribution object using a given random variable
        /// </summary>
        /// <param name="Distribution">Used to evaluate final random value</param>
        /// <param name="RandomVariable">The random number required by TimeDistribution parameter</param>
        /// <returns>The requested random value</returns>
        static private int CalculateRandomValue(List<TimeDistribution> Distribution, int RandomVariable)
        {
            for (int i = 0; i < Distribution.Count; i++)
            {
                if (!Distribution[i].IsCalculated)
                {
                    if (i == 0)
                    {
                        Distribution[i].CummProbability = Distribution[i].Probability;
                        Distribution[i].MinRange = 1;
                    }
                    else
                    {
                        Distribution[i].CummProbability = Distribution[i].Probability + Distribution[i - 1].CummProbability;
                        Distribution[i].MinRange = Distribution[i - 1].MaxRange + 1;
                    }
                    Distribution[i].MaxRange = (int)(Distribution[i].CummProbability * 100);
                    Distribution[i].IsCalculated = true;
                }
                if (RandomVariable <= Distribution[i].MaxRange && RandomVariable >= Distribution[i].MinRange)
                {
                    return Distribution[i].Time;
                }
            }
            if (RandomVariable < 1 || RandomVariable > 100)
                throw new ArgumentOutOfRangeException("RandomValue should be between 1 and 100");
            else
                throw new Exception("Debug meeeeeeeee");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Case"></param>
        /// <param name="system"></param>
        static void CalculateCase(CurrentSimulationCase Case, SimulationSystem system , CurrentSimulationCase previousCase)
        {
            Case.Bearing.RandomHours = random.Next(1, 100);
            Case.Bearing.Hours = CalculateRandomValue(system.BearingLifeDistribution, Case.Bearing.RandomHours);
            Case.RandomDelay = random.Next(1, 10);
            Case.Delay = CalculateRandomValue(system.BearingLifeDistribution, Case.RandomDelay);
            Case.AccumulatedHours = previousCase.AccumulatedHours + Case.Bearing.Hours;
            totalDelayC += Case.Delay;
            system.CurrentPerformanceMeasures.BearingCost += system.BearingCost;
            system.CurrentPerformanceMeasures.DelayCost += Case.Delay * system.DowntimeCost;
            system.CurrentPerformanceMeasures.DowntimeCost = Case.Bearing.Index * system.RepairTimeForOneBearing * system.DowntimeCost;
            system.CurrentPerformanceMeasures.RepairPersonCost = Case.Bearing.Index * system.RepairTimeForOneBearing * (system.RepairPersonCost / 60);
            system.CurrentPerformanceMeasures.TotalCost += 
            system.CurrentPerformanceMeasures.BearingCost + system.CurrentPerformanceMeasures.DelayCost + system.CurrentPerformanceMeasures.DowntimeCost + system.CurrentPerformanceMeasures.RepairPersonCost;
            system.CurrentSimulationTable.Add(Case);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Case"></param>
        /// <param name="system"></param>
        /// <param name="CurrentCases"></param>
        static void CalculateCase(ProposedSimulationCase Case, SimulationSystem system, ProposedSimulationCase PreviousCase , params CurrentSimulationCase[] CurrentCases )
        {
            int min = CurrentCases[0].Bearing.Hours;
            for (int i= 0; i<CurrentCases.Length; i++ )
            {
                if (CurrentCases[i] == null)
                {
                    CalculateCase(CurrentCases[i], system);
                }
                Case.Bearings[i] = CurrentCases[i].Bearing;
                if(CurrentCases[i].Bearing.Hours < min)
                {
                    min = CurrentCases[i].Bearing.Hours;
                }
            }
            Case.FirstFailure = min;
            Case.AccumulatedHours = PreviousCase.AccumulatedHours + min;
            Case.RandomDelay = random.Next(1, 10);
            Case.Delay = CalculateRandomValue(system.DelayTimeDistribution, Case.RandomDelay);
            system.ProposedSimulationTable.Add(Case);
            totalDelayP += Case.Delay;
            system.ProposedPerformanceMeasures.BearingCost += CurrentCases.Length * system.BearingCost;
            system.ProposedPerformanceMeasures.DelayCost += Case.Delay * system.DowntimeCost;
            system.ProposedPerformanceMeasures.DowntimeCost += system.DowntimeCost * system.RepairTimeForAllBearings;
            system.ProposedPerformanceMeasures.RepairPersonCost += system.RepairTimeForAllBearings * (system.RepairPersonCost / 60);
            system.ProposedPerformanceMeasures.TotalCost += system.ProposedPerformanceMeasures.BearingCost + system.ProposedPerformanceMeasures.DelayCost + system.ProposedPerformanceMeasures.DowntimeCost + system.ProposedPerformanceMeasures.RepairPersonCost;
        }
    }
}
