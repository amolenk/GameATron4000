ufo = {
    enter = function(room)
        if not state.ufo_entered then
            put_actor(guy, 600, 430)
            put_object(newspaper, 410, 420)
            state.ufo_entered = true
        end
    end
}