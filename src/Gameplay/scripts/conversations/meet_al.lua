meet_al = {

    id = "meet_al",
    type = "conversation",

    start = function(cnv)
        say_line("Hi there!", al)
        cnv.main(cnv)
    end,

    main = function(cnv)
        option("My name is Guy Scotthrie and I want to be a space pirate!", "pirate", not cnv.talked_about_pirate)
        option("What's going on here?", "whatsgoingon", not cnv.talked_about_going_ons);
        option("Can you beam me down back to the park?", "beamdown");
        option("Is this an actual spaceship?", "spaceship", not cnv.talked_about_spaceship);
        option("I'm selling these fine red polo shirts.", "shirts", not cnv.talked_about_shirts)
        option("Bye!", "bye");
    end,

    pirate = function(cnv)
        say_line("Excellent, we can always use a new recruit!", al)
        cnv.talked_about_pirate = true
        cnv.main(cnv)
    end,

    whatsgoingon = function(cnv)
        say_line("Oh, we're just removing all the tech nerds from this planet.", al)
        say_line("Yeah, we don't need any super intelligence popping up here!", ian)
        option("But we're decades way from realizing full Artificial General Intelligence!", "noaiyet")
        cnv.talked_about_going_ons = true
    end,

    noaiyet = function(cnv)
        say_line("I know, but Ian isn't convinced.", al)
        say_line("Those Azure data centers are doing something!", ian)
        cnv.datacenteroptions(cnv)
    end,

    datacenteroptions = function(cnv)
        if cnv.talked_about_bitcoins and cnv.talked_about_botcode then
            say_line("I guess I can't convince you...")
            cnv.main(cnv)
        else
            option("But they're just mining BitCoins!", "bitcoins", not cnv.talked_about_bitcoins)
            option("But they're just running Bot code for 8-bit adventure games!", "botcode", not cnv.talked_about_botcode)
            option("I guess I can't convince you...", "main")
        end
    end,

    bitcoins = function(cnv)
        say_line("You can probably make more money by just trading!", al)
        say_line("I'm not buying it!", ian)
        cnv.talked_about_bitcoins = true
        cnv.datacenteroptions(cnv)
    end,

    botcode = function(cnv)
        say_line("Nice try!", al)
        say_line("Nobody plays adventure games anymore!", ian)
        cnv.talked_about_botcode = true
        cnv.datacenteroptions(cnv)
    end,

    beamdown = function(cnv)
        if not cnv.pleading_count then
            say_line("Sure!", al)
            say_line("NO WAY!", ian)
            say_line("We need to get everything on the list!", ian)
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
            cnv.beamdown_giveup(cnv)
        end
    end,

    beamdown_reject = function(cnv)
        if cnv.pleading_count < 2 then
            say_line("I shouldn't...", al)
            cnv.pleading_count = cnv.pleading_count + 1
            cnv.beamdown_pleading(cnv)
        else
            say_line("Ok, because you asked nicely!", al)
            say_line("NO! The mission isn't finished yet!", ian)
            say_line("I give up...", guy)
            cnv.pleading_count = cnv.pleading_count + 1
            cnv.beamdown_giveup(cnv)
        end
    end,

    beamdown_giveup = function(cnv)
        say_line("Great, now I can get back to work!", ian)
        cnv.main(cnv)
    end,

    spaceship = function(cnv)
        say_line("Sure is!", al)
        say_line("It's an 8-bit stellar class beam-er-upper™ convertible!", al)
        say_line("Stop divulging top secret information!", ian)
        cnv.talked_about_spaceship = true
        cnv.main(cnv)
    end,

    shirts = function(cnv)
        say_line("Not interested, we all wear these same uniforms.", al)
        cnv.talked_about_shirts = true
        cnv.main(cnv)
    end,

    bye = function(cnv)
    end
}