bridge = {
    id = "bridge",
    type = "room",
    walkbox = {
        { 0, 420 },
        { 613, 420 },
        { 790, 290 },
        { 960, 290 },
        { 1115, 420 },
        { 1740, 420 },
        { 1740, 449 },
        { 0, 449}
    },
    before_enter = function(room)
        put_actor(guy, 150, 430)
    end,
    after_enter = function(room)
        -- say_line("Hey, Richard!", carl)
        -- say_line("Hey, buddy!", richard)
    end
}

park = {
    id = "park",
    type = "room",
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
    end,
    after_enter = function(room)
    end
}

-- TODO Rename to Terminal
transport = {
    id = "transport",
    type = "room",
    walkbox = {
        { 0, 412 },
        { 220, 306 },
        { 634, 306 },
        { 884, 417 },
        { 884, 449 },
        { 0, 449}
    },
    before_enter = function(room)
        if prev_room == park then
            put_actor(guy, 420, 380)
        elseif prev_room == bridge then
            put_actor(guy, 700, 353)
        end
    end
}

ufo = {
    id = "ufo",
    type = "room",
    walkbox = {
        { 0, 370 },
        { 290, 370 },
        { 345, 295 },
        { 495, 295 },
        { 580, 395 },
        { 799, 395 },
        { 799, 449 },
        { 0, 449}
    },
    before_enter = function(room)
        put_actor(guy, 150, 430)
        face_dir(face_back, ian)
    end,
    after_enter = function(room)
        --say_line("I guess you CAN believe everything you read in the newspapers!")
        walk_to(530, 390, face_front, ian)
    end
}

beach = {
    id = "beach",
    type = "room",
    walkbox = {
        { 0, 370 },
        { 290, 370 },
        { 345, 295 },
        { 495, 295 },
        { 580, 395 },
        { 799, 395 },
        { 799, 449 },
        { 0, 449}
    },
    before_enter = function(room)
        -- TODO
    end,
    after_enter = function(room)
        -- TODO
    end
}