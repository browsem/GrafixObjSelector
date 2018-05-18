/*=============================================================================|
|  PROJECT RSConnectGIOToSnap7                                           1.0.0 |
|==============================================================================|
|  Copyright (C) 2018 Bjarke Fjeldsted                                         |
|  All rights reserved.                                                        |
|==============================================================================|
|  GrafixObjSelector is free software: you can redistribute it and/or modify   |
|  it under the terms of the Lesser GNU General Public License as published by |
|  the Free Software Foundation, either version 3 of the License, or           |
|  (at your option) any later version.                                         |
|                                                                              |
|  It means that you can distribute your commercial software which includes    |
|  GrafixObjSelector without the requirement to distribute the source code     |
|  of your application and without the requirement that your application be    |
|  itself distributed under LGPL.                                              |
|                                                                              |
|  RSConnectGIOToSnap7 is distributed in the hope that it will be useful,      |
|  but WITHOUT ANY WARRANTY; without even the implied warranty of              |
|  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the               |
|  Lesser GNU General Public License for more details.                         |
|                                                                              |
|  The licence can be found at  http://www.gnu.org/licenses/                   |
|                                                                              |
|=============================================================================*/

using System;
using System.Collections.Generic;
using System.Text;

using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;

namespace GrafixObjSelector
{
    /// <summary>
    /// Code-behind class for the GrafixObjSelector Smart Component.
    /// </summary>
    /// <remarks>
    /// The code-behind class should be seen as a service provider used by the 
    /// Smart Component runtime. Only one instance of the code-behind class
    /// is created, regardless of how many instances there are of the associated
    /// Smart Component.
    /// Therefore, the code-behind class should not store any state information.
    /// Instead, use the SmartComponent.StateCache collection.
    /// </remarks>
    public class CodeBehind : SmartComponentCodeBehind
    {
        /// <summary>
        /// Called when the value of a dynamic property value has changed.
        /// </summary>
        /// <param name="component"> Component that owns the changed property. </param>
        /// <param name="changedProperty"> Changed property. </param>
        /// <param name="oldValue"> Previous value of the changed property. </param>
        public override void OnPropertyValueChanged(SmartComponent component, DynamicProperty changedProperty, Object oldValue)
        {
        }

        /// <summary>
        /// Called when the value of an I/O signal value has changed.
        /// </summary>
        /// <param name="component"> Component that owns the changed signal. </param>
        /// <param name="changedSignal"> Changed signal. </param>
        public override void OnIOSignalValueChanged(SmartComponent component, IOSignal changedSignal)
        {
            for (int i = 0; i <= 6; i++)
            {

                if (changedSignal.Name == "SourceSelector" + i.ToString())
                {
                    ChangeComp(component, i);
                }
            }
        }

        /// <summary>
        /// Called during simulation.
        /// </summary>
        /// <param name="component"> Simulated component. </param>
        /// <param name="simulationTime"> Time (in ms) for the current simulation step. </param>
        /// <param name="previousTime"> Time (in ms) for the previous simulation step. </param>
        /// <remarks>
        /// For this method to be called, the component must be marked with
        /// simulate="true" in the xml file.
        /// </remarks>
        public override void OnSimulationStep(SmartComponent component, double simulationTime, double previousTime)
        {
        }
        private void ChangeComp(SmartComponent component, int Choise)
        {
            int NextAct = 0;
            string Selected = "SourceSelector" + Choise.ToString();
            if (component.IOSignals[Selected].Value.Equals(1))
            {
                //Record if we want to turn this one ON
                NextAct = Choise;
            }
            else
            { //We want to turn it OFF, and select the next highest
                for (int i = 6; i >= 0; i--)
                {
                    Selected = "SourceSelector" + i.ToString();
                    if (NextAct == 0)
                    {
                        if (component.IOSignals[Selected].Value.Equals(1))
                        {
                            NextAct = i;
                        }
                    }
                }
            }
            component.Properties["SourceSelectNumber"].Value = NextAct;
            Selected = "Component" + NextAct.ToString();
            component.Properties["ResultingComponent"].Value = component.Properties[Selected].Value;

        }

    }
}
