talk_to_richard = {

    id = "talk_to_richard",
    type = "conversation",

    start = function(cnv)
        if not cnv.talked then
            say_line("Hey guys!")
            say_line("Hey buddy!", richard)
            say_line("What are you doing here?")
            say_line("Well, we we're recording our podcast at CCCCCC™...", richard)
            say_line("...when suddenly this bright red light appeared!", carl)
            say_line("And now we're here!", richard)
            say_line("Recording the ultimate UFO Geek Out episode!", carl)
            say_line("Yeah, with all this alien stuff lying around, we've got content for days!", richard)
        end
        cnv.talked = true
        cnv.main(cnv)
    end,

    main = function(cnv)
        option("How is the show going?", "show", not cnv.talked_about_show)
        option("Can I see that hammer?", "see_hammer", cnv.talked_about_show and not cnv.asked_for_hammer and not world.traded_hammer)
        option("Aren't there more interesting things on this ship to talk about?", "interest", cnv.talked_about_show and not world.traded_hammer)
        option("CCCCCC™?", "cccccc", not cnv.talked_about_cccccc)
        option("I'm off trying to escape!", "bye");
    end,

    show = function(cnv)
        say_line("It's going great!", richard)
        if world.booth_disconnected then
            say_line("Well, until we lost power!", richard)
        end
        say_line("We're examining this alien artifact!", carl)
        if not world.traded_hammer then
            say_line("It kinda looks like a regular claw hammer.")
            say_line("It most definitely is not!", richard)
            say_line("It's an *alien* claw hammer!", carl)
        end
        cnv.talked_about_show = true
        cnv.main(cnv)
    end,

    see_hammer = function(cnv)
        say_line("Sorry, we need it for the show.", carl)
        cnv.asked_for_hammer = true
        cnv.main(cnv)
    end,

    interest = function(cnv)
        say_line("There are.", carl)
        say_line("But El Capitan over there won't let us touch any of their stuff!", richard)
        cnv.main(cnv)
    end,

    cccccc = function(cnv)
        say_line("Conference Covering Crafting Critical Code, and, er, Crystallography.", carl)
        cnv.talked_about_cccccc = true
        cnv.main(cnv)
    end,

    bye = function(cnv)
        cnv.goodbye = not cnv.goodbye
        if cnv.goodbye then
            say_line("Tune in next time!", carl)
        else
            say_line("Good luck with that!", richard)
        end
    end
}