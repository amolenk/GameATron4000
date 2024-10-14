
Room checkIn = AddRoom("check_in", room => room
    .WithWalkboxArea(
        new Point(0, 365),
        new Point(500, 365),
        new Point(485, 270),
        new Point(800, 270),
        new Point(800, 320),
        new Point(640, 320),
        new Point(640, 350),
        new Point(600, 350),
        new Point(600, 450),
        new Point(0, 450))
    .When.BeforeEnter(() =>
    {
        checkIn.Place(guy, 160, 440);
        guy.FaceAwayFromCamera();
    }));
    
Room security = AddRoom("security", room => room
    .WithWalkboxArea(
        new Point(0, 270),
        new Point(560, 270),
        new Point(660, 390),
        new Point(1101, 390),
        new Point(1101, 450),
        new Point(190, 450),
        new Point(190, 330),
        new Point(0, 330))
    .WithScaleSettings(90, 100, 200, 350)
    .When.BeforeEnter(() =>
    {
        security.Place(guy, 20, 300);
        guy.FaceCamera();
        
        if (IsFlagSet("speedrun"))
        {
            security.Remove(accessDeniedSign);
        }
    })
    .When.AfterEnter(() =>
    {
        guy.MoveTo(150, 300);
    }));

Room lobby = AddRoom("lobby", room => room
    .WithWalkboxArea(
        new Point(0, 240),
        new Point(1000, 240),
        new Point(1000, 300),
        new Point(620, 300),
        new Point(620, 410),
        new Point(900, 410),
        new Point(900, 450),
        new Point(0, 450),
        new Point(0, 410),
        new Point(340, 410),
        new Point(340, 300),
        new Point(0, 300))
    .When.BeforeEnter(() =>
    {
        lobby.Place(guy, 50, 280);
        guy.FaceCamera();
    }));
