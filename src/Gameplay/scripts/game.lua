
-- this script is execute once on game startup
function initialize_game()

    select_actor(guy)

    -- inventory
    set_owner(groceries, selected_actor)
    set_owner(grocerylist, selected_actor)

    -- park
    put_object(newspaper, 410, 420, park)
    put_object(park_bench, 323, 408, park)

    -- transportation
    put_object(beam_button, 73, 281, terminal)
    put_object(beam_glow, 442, 450, terminal)
    put_object(beam_terminal, 442, 403, terminal)
    put_object(cooker_in_crate, 280, 450, terminal)
    put_object(crate_left, 148, 450, terminal)
    put_object(crate_right, 256, 450, terminal)
    put_object(crate_right_front, 256, 450, terminal)
    put_object(crate_top, 204, 390, terminal)
    put_object(door_terminal, 774, 332, terminal)

    -- bridge
    put_actor(al, 990, 375, bridge)
    put_actor(ian, 810, 300, bridge)
    put_actor(carl, 305, 361, bridge)
    put_actor(richard, 185, 361, bridge)
    face_dir(face_back, ian)

    put_object(bottle, 1360, 288, bridge)
    put_object(cheese_grater, 1390, 328, bridge)
    put_object(countertop, 1460, 331, bridge)
    put_object(door_bridge, 22, 444, bridge)
    put_object(claw_hammer, 245, 350, bridge)
    put_object(fridge, 1291, 330, bridge)
    put_object(onair, 246, 262, bridge)
    put_object(outlet_booth, 429, 356, bridge)
    put_object(outlet_kitchen, 1562, 399, bridge)
    put_object(podcast_booth, 246, 410, bridge)
    put_object(power_cord, 390, 392, bridge)
    put_object(saucages, 1225, 296, bridge)
    put_object(todolist, 660, 250, bridge)

    change_room(park)
end

canned_responses = {
    "I can't do that.",
    "Why?",
    "Hmm, better not.",
    "That will probably crash the game!"
}

function no_result()
    local canned_response = canned_responses[rand(1, 4)]
    say_line(canned_response)
end

function save_actor_pos(actor)
    return {
        actor = actor,
        x = actor.x,
        y = actor.y,
        face_dir = actor.face_dir
    }
end

function restore_actor_pos(actor_pos)
    if actor_pos.actor.x ~= actor_pos.x
        or actor_pos.actor.y ~= actor_pos.y then
        walk_to(actor_pos.x, actor_pos.y, actor_pos.face_dir, actor_pos.actor)
    else
        face_dir(actor_pos.face_dir, actor_pos.actor)
    end
end

function save_object_pos(obj)
    return {
        object = obj,
        x = obj.x,
        y = obj.y
    }
end

function restore_object_pos(object_pos)
    put_object(object_pos.object, object_pos.x, object_pos.y)
end

function cutscene_beam_up()
    say_line("It's yesterday's newspaper!")
    put_object(newspaper_headline, 400, 600)
    wait(2000)
    say_line("Hmm, there seem to be a LOT of UFO sightings lately!", narrator)
    wait(1000)
    remove_object(newspaper_headline)
    say_line("What a bunch of nonsense!")
    wait(1000)
    put_object(beam_park, selected_actor.x, selected_actor.y + 10)
    wait(1000)
    say_line("Uh oh...")
    wait(1500)
    change_room(terminal)
end

function cutscene_check_list()

    local ian_pos = save_actor_pos(ian)
    camera_follow(ian)

    face_dir(face_front)

    if owned_by(grocerylist, selected_actor) then
        walk_to(todolist.x, todolist.y, face_back, ian)
        say_line("hmm, we still need to find this Hans Scottleman guy...", ian)
        restore_actor_pos(ian_pos)
        face_dir(face_back)
        camera_follow(selected_actor)
    else
        walk_to(grocerylist.x, grocerylist.y, face_back, ian)
        if not world.mission_changed then
            say_line("Hmm, looks like the mission orders have changed.", ian)
            face_dir(face_front, ian)
            say_line("Al, did you know we need to collect earthly food products?", ian)
            say_line("Too bad the hot dogs are burned!", al)
            world.mission_changed = true;
        else
            say_line("Still looking for those earthly food products.", ian)
        end

        walk_to(fridge.x, fridge.y, face_back, ian)
        
        if fridge.state == "open" then
            say_line("Damnit Al, you left the fridge open again!", ian)
            say_line("It wasn't me!", al)
        else
            change_state(fridge, "open")
        end

        if owned_by(groceries, selected_actor) then
            say_line("Hmm, we're not quite there yet!", ian)
            change_state(fridge, "closed")
        else
            say_line("Excellent, looks like we're all set!", ian)
            change_state(fridge, "closed")
            face_dir(face_front, ian)
            say_line("Er, why is that red shirt guy still in my spaceship?", ian)
            wait(1000)
            change_room(beach)
            return
        end

        restore_actor_pos(ian_pos)
        face_dir(face_back)
        camera_follow(selected_actor)
    end

end