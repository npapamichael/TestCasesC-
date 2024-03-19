using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartBuilding;
using System.Runtime.CompilerServices;
using NSubstitute;



namespace SmartBuildingTests
{
    [TestFixture]
    public class BuildingControllerTests
    {

        //assigning the private strings needed
        private string buildingID;
        private string currentState;


        //Gives ids and checks if they are equal and converts them to lowercase
        [TestCase("BuilDING1")]
        [TestCase("bUiLdInG1")]
        [TestCase("BUILDING1")]
        [TestCase("Building1")]
        public void L1R1_L1R2_IfCalled_AssignBuildingID(string buildingID)
        {
            //Arrange
            BuildingController controller;
            //Act
            controller = new BuildingController(buildingID);
            //Assert
            ClassicAssert.AreEqual(buildingID.ToLower(), controller.GetBuildingID());



        }

        //Converts to Lowercase
        [TestCase("BuilDING1")]
        [TestCase("bUiLdInG1")]
        [TestCase("BUILDING1")]
        [TestCase("Building1")]

        public void L1R3_ConvertToLowerCase(string buildingID)
        {
            //Arrange
            BuildingController controller;

            //Act
            controller = new BuildingController(buildingID);

            //Assert
            ClassicAssert.AreEqual(buildingID.ToLower(), controller.GetBuildingID());

        }

        //Converts Uppercase to Lowercase
        [TestCase("BuilDING1")]
        [TestCase("bUiLdInG1")]
        [TestCase("BUILDING1")]
        [TestCase("Building1")]
        public void L1R4_SetBuildingID_UpperCase_ConvertsToLowerCase(string buildingID)
        {
            // Arrange
            BuildingController controller = new BuildingController("building1");

            // Act
            controller.SetBuildingID(buildingID);

            // Assert
            ClassicAssert.AreEqual(buildingID.ToLower(), controller.GetBuildingID());
        }


        //Sets initial currentstate to out of hours
        [TestCase("open")]
        [TestCase("random randomish")]
        [TestCase("closed")]
        public void L1R5_InitialValueIsOutOfHours(string buildingID)
        {
            //Arrange
            BuildingController controller;
            //Act
            controller = new BuildingController(buildingID);
            //Assert
            ClassicAssert.AreEqual("out of hours", controller.GetCurrentState());

        }


        //Returns the value of our of hours 
        [TestCase("out of hours")]
        public void L1R6_ReturnGetCurrentStateValue(string buildingID)
        {
            //Arrange
            BuildingController controller;

            //Act
            controller = new BuildingController(buildingID);
            controller.SetBuildingID("out of hours");
            string currentState = controller.GetCurrentState();
            //Assert
            ClassicAssert.AreEqual("out of hours", currentState);

        }

        [TestCase("closed")]
        [TestCase("out of hours")]
        [TestCase("open")]
        [TestCase("fire drill")]
        [TestCase("fire alarm")]
        public void L1R7_ValidState_ReturnsTrue_SetsCurrentState(string validState)
        {
            // Arrange
            BuildingController controller = new BuildingController("buildingID");

            // Act
            controller.SetCurrentState(validState);

            // Assert
            ClassicAssert.AreEqual(validState.ToLower(), controller.GetCurrentState());
        }

        //If an invalid state is given the current state wont change 
        [TestCase("invalid state")]
        public void L1R7_InvalidState_ReturnsFalse_DontChangeCurrentState(string invalidState)
        {
            // Arrange
            BuildingController controller = new BuildingController("buildingID");

            // Act
            bool result = controller.SetCurrentState(invalidState);

            // Assert
            ClassicAssert.IsFalse(result);
            // Checking if the currentState remains the same
            ClassicAssert.AreEqual("out of hours", controller.GetCurrentState());


        }

        //Makes sure the appropriate changes that can be made from closed are valid 
        [TestCase("out of hours")]
        [TestCase("fire drill")]
        [TestCase("fire alarm")]
        public void L2R1_SetCurrentState_ValidFromClosed_ReturnsTrue(string validchange)
        {
            //Arrange
            BuildingController controller = new BuildingController("BuildingID");

            //Act
            controller.SetCurrentState("closed");
            bool newstate = controller.SetCurrentState(validchange);

            //Assert
            ClassicAssert.IsTrue(newstate);

        }

        //Makes sure the appropriate changes that can be made from open are valid 
        [TestCase("out of hours")]
        [TestCase("fire alarm")]
        [TestCase("fire drill")]
        public void L2R1_SetCurrentState_ValidFromOpen_ReturnsTrue(string validchange)
        {
            //Arrange
            BuildingController controller = new BuildingController("BuildingID");

            //Act
            controller.SetCurrentState("open");
            bool newstate = controller.SetCurrentState(validchange);


            //Assert
            ClassicAssert.IsTrue(newstate);

        }

        //Makes sure the appropriate changes that can be made from out of hours are valid 
        [TestCase("open")]
        [TestCase("fire alarm")]
        [TestCase("fire drill")]
        [TestCase("closed")]
        public void L2R1_SetCurrentState_ValidFromOutOfHours_ReturnsTrue(string validchange)
        {
            //Arrange
            BuildingController controller = new BuildingController("BuildingID");

            //Act
            controller.SetCurrentState("out of hours");
            bool newstate = controller.SetCurrentState(validchange);

            //Assert
            ClassicAssert.IsTrue(newstate);

        }

        //Makes sure the appropriate changes that can be made from fire drill are valid 
        [TestCase("out of hours", "out of hours")]
        [TestCase("closed", "closed")]
        [TestCase("open", "open")]
        public void L2R1_SetCurrentState_ValidFromFireDrill_ReturnsTrue(string validchange, string invalid)
        {
            //Arrange
            BuildingController controller = new BuildingController("BuildingID");

            //Act
            controller.SetCurrentState(validchange);
            controller.SetCurrentState("fire drill");
            bool newstate = controller.SetCurrentState(invalid);

            //Assert
            ClassicAssert.IsTrue(newstate);

        }

        //Makes sure the appropriate changes that can be made from fire alarm are valid 
        [TestCase("out of hours", "out of hours")]
        [TestCase("closed", "closed")]
        [TestCase("open", "open")]
        public void L2R1_SetCurrentState_ValidFromFireAlarm_ReturnsTrue(string validchange, string invalid)
        {
            //Arrange
            BuildingController controller = new BuildingController("BuildingID");

            //Act
            controller.SetCurrentState(validchange);
            controller.SetCurrentState("fire alarm");
            bool newstate = controller.SetCurrentState(invalid);

            //Assert
            ClassicAssert.IsTrue(newstate);

        }
        //If current state attemps to change to itself it will return true and remain the same 
        [TestCase("out of hours", "out of hours")]
        [TestCase("closed", "closed")]
        [TestCase("open", "open")]
        public void L2R2_SetCurrentState_PresentState(string current, string valid)
        {
            //Arrange
            BuildingController controller = new BuildingController("BuildingID");
            //Act
            controller.SetCurrentState(current);
            bool newstate = controller.SetCurrentState(valid);
            //Assert
            ClassicAssert.IsTrue(newstate);

        }

        //For open, closed, out of hours it can accept all letters but will change them to lowercase
        [TestCase("")]
        [TestCase("Oper")]
        [TestCase("this is a long striiiiing")]
        public void L2R3_SecondConstructor_InvalidState_ThrowException(string state)
        {
            //Arrange 
            BuildingController bc;

            //Act + Arrange
            var ex = Assert.Catch<ArgumentException>(() => bc = new BuildingController("id", state)); //Throwing the arguement exception
            StringAssert.Contains("Argument Exception: BuildingController can only be initialised to the following states 'open', 'closed', 'out of hours'", ex.Message);

        }

        //Check if its a valid state
        [TestCase("open")]
        [TestCase("closed")]
        [TestCase("out of hours")]
        public void L2R3_Check_ValidState(string validchange)
        {
            //Arrange
            BuildingController controller = new BuildingController("BuildingID", validchange);

            //Act + Assert
            ClassicAssert.AreEqual(validchange, controller.GetCurrentState());

        }

        //New dependencies
        [Test]
        public void L3R1_BuildingControllerClass_UnitTestFriendly()
        {
            //Arrange
            BuildingController controller;
            ILightManager light = Substitute.For<ILightManager>();
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();
            IDoorManager door = Substitute.For<IDoorManager>();
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();
            string buildingID = "BuildingID";
            //Act
            controller = new BuildingController(buildingID, light, alarm, door, web, email);
            //Assert
            ClassicAssert.AreEqual(buildingID.ToLower(), controller.GetBuildingID());

        }
       //Returns a status string with the correct order
        [TestCase("Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK", "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK", "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK")]
        [TestCase("Lights,OK,FAULT,FAULT,OK,OK,FAULT,OK,FAULT,OK,FAULT", "Doors,FAULT,OK,FAULT,OK,FAULT,OK,FAULT,OK,OK,FAULT", "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK")]
        [TestCase("Lights,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "Doors,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "FireAlarm,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,OK")]

        public void L3R3_GetStatusReport_ReturnStringIfValid(string lightStatus, string doorStatus, string alarmStatus)
        {
            //Arrange
            BuildingController controller;
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();

            ILightManager light = Substitute.For<ILightManager>();
            light.GetStatus().Returns(lightStatus);
            IDoorManager door = Substitute.For<IDoorManager>();
            door.GetStatus().Returns(doorStatus);
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();
            alarm.GetStatus().Returns(alarmStatus);
            //Act
            controller = new BuildingController("BuildingID", light, alarm, door, web, email);
            string finalStatus = controller.GetStatusReport();
            //Assert
            ClassicAssert.AreEqual(finalStatus, lightStatus + doorStatus + alarmStatus);

        }


        //OpenAllDoors returned false and current state remained the same
        [Test]
        public void L3R4_OpenAllDoors_ReturnsFalse()
        {
            //Arrange
            BuildingController controller;
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();
            ILightManager light = Substitute.For<ILightManager>();
            IDoorManager door = Substitute.For<IDoorManager>();
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();

            //Act
            door.OpenAllDoors().Returns(false);
            controller = new BuildingController("BuildingID", light, alarm, door, web, email);
            bool setCurrentStateResult = controller.SetCurrentState("open");

            //Assert
            ClassicAssert.IsFalse(setCurrentStateResult); // Ensure SetCurrentState returns false
            door.OpenAllDoors();
        }

        //OpenAllDoors returned true and state changed to true 
        [Test]
        public void L3R5_OpenAllDoors_ReturnsTrue()
        {
            //Arrange
            BuildingController controller;
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();
            ILightManager light = Substitute.For<ILightManager>();
            IDoorManager door = Substitute.For<IDoorManager>();
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();

            //Act
            door.OpenAllDoors().Returns(true);
            controller = new BuildingController("BuildingID", light, alarm, door, web, email);
            bool setCurrentStateResult = controller.SetCurrentState("open");

            //Assert
            ClassicAssert.IsTrue(setCurrentStateResult); // Ensure SetCurrentState returns false
            door.OpenAllDoors();
        }

        //When state becomes closed all doors lock  and lights close
        [Test]
        public void L4R1_WhenClosed_LockAllDoors_TurnOffLights()
        {
            //Arrange
            BuildingController controller;
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();
            ILightManager light = Substitute.For<ILightManager>();
            IDoorManager door = Substitute.For<IDoorManager>();
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();

            //Act
            door.LockAllDoors().Returns(true);
            controller = new BuildingController("BuildingID", light, alarm, door, web, email);
            bool setCurrentStateResult = controller.SetCurrentState("closed");

            //Assert
            door.Received().LockAllDoors();
            light.Received().SetAllLights(false);
        }

        //When state is fire alarm all doors open all lights turn on and online log call is made
        [Test]
        public void L4R2_WhenFireAlarm_OpenAllDoors_TurnOnLights_WebLog()
        {
            //Arrange
            BuildingController controller;
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();
            ILightManager light = Substitute.For<ILightManager>();
            IDoorManager door = Substitute.For<IDoorManager>();
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();

            //Act
            controller = new BuildingController("BuildingID", light, alarm, door, web, email);
            bool setCurrentStateResult = controller.SetCurrentState("fire alarm");

            //Assert
            alarm.Received().SetAlarm(true);
            door.Received().OpenAllDoors();
            light.Received().SetAllLights(true);
            web.Received().LogFireAlarm("fire alarm");
        }


        //If any of the status reports contain a fault a web engineer is called saying which are faulty
        [TestCase("Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK", "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK", "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK")]
        [TestCase("Lights,OK,FAULT,FAULT,OK,OK,FAULT,OK,FAULT,OK,FAULT", "Doors,FAULT,OK,FAULT,OK,FAULT,OK,FAULT,OK,OK,FAULT", "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK")]
        [TestCase("Lights,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "Doors,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT", "FireAlarm,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,FAULT,OK")]

        public void L4R3_ParseThreeReports_WebEngineerRequired(string lightStatus, string doorStatus, string alarmStatus)
        {
            //Arrange
            BuildingController controller;
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();
            ILightManager light = Substitute.For<ILightManager>();
            IDoorManager door = Substitute.For<IDoorManager>();
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();
            door.GetStatus().Returns(doorStatus);
            light.GetStatus().Returns(lightStatus);
            alarm.GetStatus().Returns(alarmStatus);

            //Act
            controller = new BuildingController("BuildingID", light, alarm, door, web, email);
            string weblogreport = controller.GetStatusReport();

            //Assert
            web.Received().LogEngineerRequired(Arg.Any<string>());
        }

        //Additional L4R2 requirement
        //Throws exception and emails the appropriate address
        [Test]
        public void L4R4_SendEmail_ThrowException()
        {
            //Arrange
            BuildingController controller;
            IWebService web = Substitute.For<IWebService>();
            IEmailService email = Substitute.For<IEmailService>();
            ILightManager light = Substitute.For<ILightManager>();
            IDoorManager door = Substitute.For<IDoorManager>();
            IFireAlarmManager alarm = Substitute.For<IFireAlarmManager>();
            web.LogFireAlarm(Arg.Any<string>()).Returns(x => { throw new Exception("exception"); });


            //Act
            controller = new BuildingController("BuildingID", light, alarm, door, web, email);
            controller.SetCurrentState("fire alarm");

            //Assert
            email.Received().SendMail("smartbuilding@uclan.ac.uk", "failed to log alarm", Arg.Is<string>(x => (x.Contains("exception"))));
        }

    }
}

        //This is a test to check if the project template is correctly configured on your machine
        //you should uncomment the test method below and check that the test appears in the text explorer. (Test -> Test Explorer)
        //When you run the unit test it should pass with the message "Example Test Passed"
        //If the test is not visible or the unit test does not run or pass: Ask a tutor for help
        //When you have confirmed that the template can run unit tests, you can delete the test. (and this comment)



     


    