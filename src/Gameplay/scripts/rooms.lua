beach = {
    id = "beach",
    type = "room",
    scale = {
        min = 75,
        max = 75,
        start_pos = 370,
        end_pos = 450
    },
    walkbox = {
        { 0, 370 },
        { 900, 370 },
        { 900, 449 },
        { 0, 449}
    },
    before_enter = function(room)
        put_actor(guy, 490, 420)
        put_object(beam_park, 490, 425)
    end,
    after_enter = function(room)
        wait(1500)
        remove_object(beam_park)
        wait(1000)
        say_line("That's just great!")
        say_line("I hope there's a supermarket around here...")
        wait(1000)
        walk_to(850, 430)
        say_line("Thanks for playing!", narrator)
        say_line("GAME OVER", narrator)
    end
}

bridge = {
    id = "bridge",
    type = "room",
    scale = {
        min = 40,
        max = 100,
        start_pos = 275,
        end_pos = 400
    },
    walkbox = {
        { 0, 420 },
        { 390, 420 },
        { 390, 375 },
        { 485, 375 },
        { 520, 430 },
        { 613, 430 },
        { 600, 380 },
        { 685, 332 },
        { 715, 332 },
        { 730, 300 },
        { 760, 290 },
        { 910, 290 },
        { 940, 300 },
        { 955, 332 },
        { 985, 332 },
        { 1070, 380 },
        { 1057, 430 },
        { 1150, 430 },
        { 1150, 415 },
        { 1740, 415 },
        { 1740, 449 },
        { 0, 449}
    },
    before_enter = function(room)
        put_actor(guy, 65, 440)
    end,
    after_enter = function(room)
        if not world.visited_bridge then
            wait(1000)
            say_line("No, you can't have access to the reactor core!", ian)
            say_line("Can we at least have a look at the warp drive?", carl)
            say_line("NO!", ian)
            walk_to(455, 430, face_front, ian)
            say_line("Jeez, these earthlings are nosy!", ian)
            walk_to(810, 300, face_back, ian)
            world.visited_bridge = true
        end
    end
}

park = {
    id = "park",
    type = "room",
    scale = {
        min = 70,
        max = 100,
        start_pos = 270,
        end_pos = 400
    },
    walkbox = {
        { 935, 295 },
        { 935, 318 },
        { 710, 375 },
        { 710, 385 },
        { 935, 442 },
        { 935, 449 },
        { 591, 449 },
        { 393, 375 },
        { 108, 336 },
        { 0, 330 },
        { 0, 312 },
        { 130, 315 },
        { 366, 338 },
        { 470, 355 },
        { 804, 316 }
    },
    before_enter = function(room)
        put_actor(guy, 600, 430)
    end
}

terminal = {
    id = "terminal",
    type = "room",
    scale = {
        min = 80,
        max = 100,
        start_pos = 325,
        end_pos = 400
    },
    walkbox = {
        { 0, 430 },
        { 180, 335 },
        { 600, 335 },
        { 884, 430 },
        { 884, 449 },
        { 360, 449 },
        { 360, 400 },
        { 80, 400 },
        { 80, 449 },
        { 0, 449}
    },
    before_enter = function(room)
        if prev_room == park then
            put_actor(guy, 440, 363)
        elseif prev_room == bridge then
            put_actor(guy, 665, 365)
        end
    end,
    after_enter = function(room)
        if prev_room == park then
            wait(1000)
            say_line("GET ME OUT OF HERE!")
        end
    end
}