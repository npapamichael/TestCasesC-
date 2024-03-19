using NSubstitute.ReturnsExtensions;
using System.Text;

namespace SmartBuilding
{
    public class BuildingController
    {
        //Write BuildingController code here...


        //Private strings assign
        private string buildingID;
        private string currentState;
        private string temp;
        private string previousState;


        //private dependencies assign
        private ILightManager light;
        private IDoorManager door;
        private IFireAlarmManager alarm;
        private IWebService web;
        private IEmailService email;

        public BuildingController(string id)
        {

            buildingID = id.ToLower();
            currentState = "out of hours";

        }
        public BuildingController(string id, string startState)
        {
            string[] States = { "closed", "out of hours", "open" };
            if (!States.Contains(startState))
            {
                //throw exception
                throw new ArgumentException("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'");

            }
            else
            {
                buildingID = id;
                currentState = startState;
            }
        }
        //New Building controller needed for the dependencies
        public BuildingController(string id, ILightManager iLightManager, IFireAlarmManager iFireAlarmManager,
            IDoorManager iDoorManager, IWebService iWebService, IEmailService iEmailService)
        {
            buildingID = id.ToLower();
            light = iLightManager;
            door = iDoorManager;
            alarm = iFireAlarmManager;
            web = iWebService;
            email = iEmailService;
            currentState = "out of hours";

        }
        public string GetStatusReport()
        {
            //Get status reports for lights, doors and fire alarm
            string lightStatus = light.GetStatus();
            string doorStatus = door.GetStatus();
            string alarmStatus = alarm.GetStatus();

            string invalid = "invalid";

            string[] doorSplitApart = doorStatus.Split(','); //This will split the status of any one of three containing "FAULT".
            string[] alarmSplitApart = alarmStatus.Split(',');
            string[] lightSplitApart = lightStatus.Split(','); 

            if (lightSplitApart[0] != "Lights" && doorSplitApart[0] != "Doors" && alarmSplitApart[0] != "FireAlarm")
            {
                return invalid;
            }
            else
            {
                string finalStatus = lightStatus + doorStatus + alarmStatus;
                if (lightSplitApart.Contains("FAULT"))
                    web.LogEngineerRequired("Lights,");
                if (doorSplitApart.Contains("FAULT"))
                    web.LogEngineerRequired("Doors,");
                if (alarmSplitApart.Contains("FAULT"))
                    web.LogEngineerRequired("FireAlarm");


                // Return the combined status reports
                return finalStatus;
            }


        }
        public void SetBuildingID(string id)
        {
            buildingID = id.ToLower(); //L1R4 Setting the building id into lowercase 
        }
        public string GetBuildingID()
        {
            return buildingID;
        }
        public string GetCurrentState()
        {
            return currentState;
        }
        public bool SetCurrentState(string newState)
        {
            // Valid states
            string[] validStates = { "closed", "out of hours", "open", "fire drill", "fire alarm" };

            // Check if the supplied state is valid
            if (Array.Exists(validStates, state => state.Equals(newState, StringComparison.OrdinalIgnoreCase)))
            {
                previousState = currentState.ToLower();
                currentState = newState.ToLower();  // Set the currentState if it's a valid state
            }

            try // try used as an additional requirement on L4R2 needed for L4R4
            {
                if (newState == "fire alarm" && alarm != null && door != null && light != null && web != null)
                {
                    alarm.SetAlarm(true);
                    door.OpenAllDoors();
                    light.SetAllLights(true);
                    web.LogFireAlarm("fire alarm");

                }
            }
            catch (Exception ex) {
                email.SendMail("smartbuilding@uclan.ac.uk", "failed to log alarm", ex.Message); //exception message with the mock email
            }

            if (newState == "open" && door != null && door.OpenAllDoors()) // checking if OpenAllDoors is not null as to return true
            {
                previousState = currentState;
                return true;

            }
            else if (newState == "open" && door != null && !door.OpenAllDoors()) //same logic but if door is not null and OpenAllDoors does not work then return false
            {
                return false;

            }
            if (newState == "closed" && door != null)
            {
                door.LockAllDoors();
                light.SetAllLights(false);
            }

            //The switch used to ensure all possible transitions to states are valid and correct and also changing the current(previous) state is changed to the newState(current)           switch (previousState)
            switch(previousState)
            {
                case "out of hours":
                    if (newState == "open" || newState == "closed" || newState == "fire drill" || newState == "fire alarm" || newState == "out of hours")
                    {
                        temp = previousState;
                        previousState = currentState;
                        currentState = newState.ToLower();
                        return true;
                    }
                    break;
                case "open":
                    if (newState == "out of hours" || newState == "fire drill" || newState == "fire alarm" || newState == "open")
                    {
                        temp = previousState;
                        previousState = currentState;
                        currentState = newState.ToLower();
                        return true;
                    }
                    break;
                case "closed":
                    if (newState == "out of hours" || newState == "fire drill" || newState == "fire alarm" || newState == "closed")
                    {

                        temp = previousState;
                        previousState = currentState;
                        currentState = newState.ToLower();
                        return true;
                    }
                    break;
                case "fire drill":
                case "fire alarm":
                    if (temp == currentState)
                    {

                        temp = previousState;
                        previousState = currentState;
                        currentState = newState.ToLower();
                        return true;
                    }
                    break;

            }
                // Invalid state transition
                return false;


        }


    }
}

       
            
           
           

