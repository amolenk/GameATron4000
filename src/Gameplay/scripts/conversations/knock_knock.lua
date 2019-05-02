knock_knock = {

    id = "knock_knock",
    type = "conversation",

    start = function(cnv)
        say_line("Knock, knock!")
        say_line("Who's there?", richard)
        cnv.main(cnv)
    end,

    main = function(cnv)
        option("Spell.", "spell")
        option("Boo.", "boo")
        option("Alec.", "alec")
    end,

    spell = function(cnv)
        say_line("Spell who?", richard)
        say_line("Okay...");
        say_line("'W'")
        say_line("'H'")
        say_line("'O'")
        say_line("*drumroll*", narrator)
    end,

    boo = function(cnv)
        say_line("Boo who?", richard)
        say_line("No need to cry, it's only a joke!")
        say_line("He got you there, Richard!", carl)
    end,

    alec = function(cnv)
        say_line("Alec who?", richard)
        say_line("Alec-tricity.")
        say_line("Ain't that a shocker?")
        say_line("That's a high voltage joke!", carl)
    end
}