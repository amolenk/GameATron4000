al = {
    id = "al",
    type = "actor",
    name = "Al",
    text_col = "Yellow",
    use_pos = pos_infront,
    use_dir = face_back,
    verbs = {
        look_at = function(actor)
            say_line("I guess it's an alien. Hard to see at this resolution.")
        end,
        talk_to = function(actor)
            start_talking("meet_al")
        end
    }
}

guy = {
    id = "guy",
    type = "actor",
    name = "Guy Scotthrie",
    classes = { class_untouchable },
    text_col = "White"
}

ian = {
    id = "ian",
    type = "actor",
    name = "Ian",
    text_col = "Magenta",
    use_pos = pos_infront,
    use_dir = face_back,
    verbs = {
        look_at = function(actor)
            say_line("It's an alien looking fellow!")
            say_line("He looks a bit more important than the other one!")
            say_line("Hey, I'm right here!", al)
        end,
        talk_to = function(actor)
            if world.knows_about_list then
                cutscene_check_list()
            else
                say_line("Go away, can't you see I'm extremely busy!", actor)
            end
        end
    }
}