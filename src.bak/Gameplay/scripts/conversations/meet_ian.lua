meet_ian = {

    id = "meet_ian",
    type = "conversation",

    start = function(cnv)
        face_dir(face_front, ian)
        say_line("What do you want?", ian)
        cnv.main(cnv)
    end,

    main = function(cnv)
        option("My name is Guy Scotthrie and I want to be a space pirate!", "pirate", not world.talked_about_pirate)
        option("What's going on here?", "whats_going_on", not cnv.talked_about_goingons)
        option("Why abduct all the tech nerds?", "nerds", cnv.talked_about_goingons and not cnv.talked_about_nerds)
        option("How do you know if you've got everybody?", "how", cnv.talked_about_goingons and not cnv.talked_about_how)
        option("Are you done with the mission yet?", "check", cnv.talked_about_goingons and cnv.talked_about_how)
        option("I'm selling these fine red polo shirts.", "shirts", not world.talked_about_shirts)
        option("Bye!", "bye");
    end,

    pirate = function(cnv)
        say_line("Excellent, we can always use a new recruit!", ian)
        world.talked_about_pirate = true
        cnv.main(cnv)
    end,

    whats_going_on = function(cnv)
        say_line("We're on a top-secret mission to abduct all the tech nerds from this planet.", ian)
        cnv.talked_about_goingons = true
        cnv.main(cnv)
    end,

    nerds = function(cnv)
        say_line("We don't need any super intelligence popping up here!", ian)
        say_line("Those Azure data centers are doing something!", ian)
        say_line("But they're just running Bot code for 8-bit adventure games!")
        say_line("Nice try!", ian)
        say_line("Nobody plays adventure games anymore!", ian)
        cnv.talked_about_nerds = true
        cnv.main(cnv)
    end,

    how = function(cnv)
        say_line("That's classified information!", ian)
        say_line("There's no harm in telling me, I'm already abducted!")
        say_line("Makes sense.", ian)
        say_line("We've got top-secret mission orders that contain all operational details.", ian)
        cnv.talked_about_how = true
        cnv.main(cnv)
    end,

    check = function(cnv)
        say_line("Let's check the top-secret mission orders.", ian)
        cutscene_check_list()
        cnv.main(cnv)
    end,

    shirts = function(cnv)
        say_line("Not interested, we all wear these same uniforms.", ian)
        say_line("Less sprites to draw.", ian)
        cnv.world = true
        cnv.main(cnv)
    end,

    bye = function(cnv)
        face_dir(face_back, ian)
    end
}