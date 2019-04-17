park = {
    id = "park",
    type = "room",
    before_enter = function(room)
        put_actor(guy, 600, 430)
    end,
    after_enter = function(room)
        say_teleportation_line()
    end
}

ufo = {
    id = "ufo",
    type = "room",
    before_enter = function(room)
        put_actor(guy, 150, 430)
    end,
    after_enter = function(room)
        say_teleportation_line()
    end
}

function say_teleportation_line()
    if world.teleported then
        say_line("Wow, I just teleported from the " .. prev_room.id .. " to the " .. current_room.id .. " room!")
        world.teleported = null
    end
end