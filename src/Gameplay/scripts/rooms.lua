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
        cutscene.beach_before_enter()
    end,
    after_enter = function(room)
        cutscene.beach_after_enter()
    end
}