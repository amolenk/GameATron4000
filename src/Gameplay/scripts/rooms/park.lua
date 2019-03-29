park = {
    enter = function(room)
        if not state.park_entered then
            put_actor(guy, 600, 430)
            put_object(newspaper, 410, 420)
            put_object(park_bench, 323, 408)
            state.park_entered = true
        end
    end
}