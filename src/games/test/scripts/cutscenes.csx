public void CutScene_BeamUp()
{
    SayLine("It's yesterday's newspaper!");
    park.Place(newspaperHeadline, 400, 450);
    Delay(2000);

    narrator.SayLine("Hmm, there seem to be a LOT of UFO sightings lately!");
    Delay(1000);

    park.Remove(newspaperHeadline);
    SayLine("What a bunch of nonsense!");
    Delay(1000);

    park.Place(parkBeam, Protagonist.Position.X, Protagonist.Position.Y + 10);
    Delay(1000);

    SayLine("Uh oh...");
    Delay(1500);

//    terminal.Enter();
}
