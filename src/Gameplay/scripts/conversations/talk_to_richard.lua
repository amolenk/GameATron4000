talk_to_richard = {

    id = "talk_to_richard",
    type = "conversation",

    start = function(cnv)
        say_line("Hey Richard!")
        say_line("Hey buddy!", richard)
        cnv.main(cnv)
    end,

    main = function(cnv)
        option("What's up?", "geekout")
        option("I'm off trying to escape!", "bye");
    end,

    geekout = function(cnv)
        say_line("We're recording this epic UFO geek out episode!", richard)
        cnv.main(cnv)
    end,

    bye = function(cnv)
        -- TODO This is not being shown if it's the last line?!
        say_line("Tune in next time!", richard)
    end
}