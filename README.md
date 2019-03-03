
# MasterMind

  

**Execute app / run tests:**

  

 - Automatically:

  

To run unit tests and end to end tests, please, execute build.sh script. This will compile, execute the unit test,

run the end to end test container that will also execute the end to end tests.

Once the script is successfuly executed, you can reach the api through the url: http://localhost:8080

  

- Manually:

  

To run the tests manually, you should have dotnet core 2.2 installed, and then navigate to the test folder and run the following command: dotnet test

To run the app manually, please, use the command: dotnet run

**Swagger**

To run swagger, please, run the app and then navigate to http://localhost:8080/swagger