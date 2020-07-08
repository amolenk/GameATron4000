meet_al = {

    id = "meet_al",
    type = "conversation",

    start = function(cnv)
        say_line("Hi there!", al)
        cnv.main(cnv)
    end,

    main = function(cnv)
        option("My name is Guy Scotthrie and I want to be a space pirate!", "pirate", not world.talked_about_pirate)
        option("Can you beam me down back to the park?", "beamdown");
        option("Is this an actual spaceship?", "spaceship", not cnv.talked_about_spaceship)
        option("I'm selling these fine red polo shirts.", "shirts", not world.talked_about_shirts)
        option("Sorry about your hot dogs!", "hotdogs", not world.talked_about_hotdogs and cooker.state == "burned")
        option("Bye!", "bye");
    end,

    pirate = function(cnv)
        say_line("Excellent, we can always use a new recruit!", al)
        world.talked_about_pirate = true
        cnv.main(cnv)
    end,

    beamdown = function(cnv)
        if not cnv.pleading_count then
            local orig_dir_ian = ian.face_dir
            say_line("Sure!", al)
            face_dir(face_front, ian)
            say_line("NO WAY!", ian)
            say_line("We need to get everything on the list!", ian)
            face_dir(orig_dir_ian, ian)
            world.knows_about_list = true
            cnv.pleading_count = 0
            cnv.beamdown_pleading(cnv)
        else
            say_line("I shouldn't...", al)
            cnv.beamdown_pleading(cnv)
        end
    end,

    beamdown_pleading = function(cnv)
        if cnv.pleading_count < 3 then
            option("Please?", "beamdown_reject", cnv.pleading_count == 0)
            option("Pretty please?", "beamdown_reject", cnv.pleading_count == 1)
            option("Pretty please with a cherry on top?", "beamdown_reject", cnv.pleading_count == 2)
            option("I give up...", "beamdown_giveup")
        else
            say_line("I give up...", guy)
            cnv.main(cnv)
        end
    end,

    beamdown_reject = function(cnv)
        if cnv.pleading_count < 2 then
            say_line("I shouldn't...", al)
            cnv.pleading_count = cnv.pleading_count + 1
            cnv.beamdown_pleading(cnv)
        else
            local orig_dir_ian = ian.face_dir
            say_line("Ok, because you asked nicely!", al)
            face_dir(face_front, ian)
            say_line("NO! The mission isn't finished yet!", ian)
            face_dir(orig_dir_ian, ian)
            say_line("I give up...", guy)
            cnv.pleading_count = cnv.pleading_count + 1
            cnv.main(cnv)
        end
    end,

    spaceship = function(cnv)
        local orig_dir_ian = ian.face_dir
        say_line("Sure is!", al)
        say_line("It's an 8-bit stellar class beam-er-upper™ convertible!", al)
        face_dir(face_front, ian)
        say_line("Stop divulging top secret information!", ian)
        face_dir(orig_dir_ian, ian)
        cnv.talked_about_spaceship = true
        cnv.main(cnv)
    end,

    shirts = function(cnv)
        say_line("Not interested, we all wear these same uniforms.", al)
        say_line("Less sprites to draw.", al)
        world.talked_about_shirts = true
        cnv.main(cnv)
    end,

    hotdogs = function(cnv)
        say_line("I was really looking forward to those!", al)
        world.talked_about_hotdogs = true
        cnv.main(cnv)
    end,

    bye = function(cnv)
    end
}