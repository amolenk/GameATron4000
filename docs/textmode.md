# Run the game in text adventure mode

The game can be played in text adventure by using the Bot Framework emulator.

1. Open the `GameATron4000.code-workspace` workspace in Visual Studio Code.

2. In Visual Studio Code, select **Debug | Start Debugging**.

3. Start the Bot Framework Emulator.

4. Select **File | New Bot Configuration...**.

6. On the **New bot configuration** dialog, enter *GameATron4000* as the bot name and *http://localhost:5000/api/messages* as the endpoint URL. Leave all other fields blank.

    <img src="./images/new-bot-configuration.png" alt="New bot configuration" width="300"/>

7. Click **Save and connect**, name your bot file *GameATron4000.Development.bot* and save it in the project's `/src` folder.

8. The emulator will connect to the bot. You can now enter messages like *look at newspaper* to play the game.

    <img src="./images/text-gameplay.png" alt="Text adventure gameplay" width="400"/>

9. When you're done playing, select **Debug | Stop Debugging** in Visual Studio Code.