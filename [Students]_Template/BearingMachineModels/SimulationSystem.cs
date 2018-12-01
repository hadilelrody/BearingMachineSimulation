﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearingMachineModels
{
    public class SimulationSystem : ICloneable
    {
        public SimulationSystem()
        {
            DelayTimeDistribution = new List<TimeDistribution>();
            BearingLifeDistribution = new List<TimeDistribution>();

            CurrentSimulationTable = new List<CurrentSimulationCase>();
            CurrentPerformanceMeasures = new PerformanceMeasures();

            ProposedSimulationTable = new List<ProposedSimulationCase>();
            ProposedPerformanceMeasures = new PerformanceMeasures();
        }
        
        ///////////// INPUTS /////////////
        public int DowntimeCost { get; set; }
        public int RepairPersonCost { get; set; }
        public int BearingCost { get; set; }
        public int NumberOfHours { get; set; }
        public int NumberOfBearings { get; set; }
        public int RepairTimeForOneBearing { get; set; }
        public int RepairTimeForAllBearings { get; set; }
        public List<TimeDistribution> DelayTimeDistribution { get; set; }
        public List<TimeDistribution> BearingLifeDistribution { get; set; }

        ///////////// OUTPUTS /////////////
        public List<CurrentSimulationCase> CurrentSimulationTable { get; set; }
        public PerformanceMeasures CurrentPerformanceMeasures { get; set; }
        public List<ProposedSimulationCase> ProposedSimulationTable { get; set; }
        public PerformanceMeasures ProposedPerformanceMeasures { get; set; }

        public object Clone()
        {
            SimulationSystem system = (SimulationSystem)MemberwiseClone();
            system.DelayTimeDistribution = new List<TimeDistribution>(DelayTimeDistribution);
            system.BearingLifeDistribution = new List<TimeDistribution>(BearingLifeDistribution);
            system.CurrentSimulationTable = new List<CurrentSimulationCase>(CurrentSimulationTable);
            system.ProposedSimulationTable = new List<ProposedSimulationCase>(ProposedSimulationTable);
            system.ProposedPerformanceMeasures = (PerformanceMeasures)ProposedPerformanceMeasures.Clone();
            system.CurrentPerformanceMeasures = (PerformanceMeasures)CurrentPerformanceMeasures.Clone();
            return system;
        }
    }
}
