
-- this script is execute once on game startup
function initialize_game()

    select_actor(guy)
--    camera_follow(guy)

    -- inventory
    set_owner(groceries, selected_actor)
    set_owner(grocerylist, selected_actor)

    -- park
    put_object(newspaper, 410, 420, park)
    put_object(park_bench, 323, 408, park)

    -- transportation
    put_object(beamcontrol, 73, 281, transport)
    put_object(cooker, 280, 440, transport)
    put_object(crate, 256, 435, transport)
    put_object(transport_door, 775, 329, transport)
    put_object(transport_glow, 442, 450, transport)

    -- bridge
    put_actor(al, 968, 375, bridge)
    put_actor(ian, 650, 420, bridge)
    put_actor(carl, 300, 319, bridge)
    put_actor(richard, 217, 318, bridge)
    put_object(bottle, 1371, 312, bridge)
    put_object(bridge_transport_door, 30, 430, bridge)
    put_object(claw_hammer, 500, 430, bridge)
    put_object(fridge, 1436, 402, bridge)    
    put_object(podcast_booth, 322, 402, bridge)
    put_object(power_cord, 193, 392, bridge)
    put_object(todolist, 650, 300, bridge)

    change_room(bridge)

    -- debug
    change_state(beamcontrol, "off")
    change_state(transport_door, "open")
--    set_owner(claw_hammer, selected_actor)
    put_actor(selected_actor, 400, 400)
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

function cutscene_beam_up()
    say_line("It's yesterday's newspaper!")
    put_object(newspaper_headline, 400, 600)
    wait(2000)
    say_line("Hmm, there seem to be a LOT of UFO sightings lately!", narrator)
    wait(1000)
    remove_object(newspaper_headline)
    say_line("What a bunch of nonsense!")
    wait(1000)
    put_object(tractorbeam, selected_actor.x, selected_actor.y + 10)
    wait(1000)
    say_line("Uh oh...")
    wait(1500)
    change_room(transport)
end

function cutscene_check_list()

    local ianX = ian.x;
    local ianY = ian.y;

    say_line("Shouldn't you check the list?")
    face_dir(face_front)

    if owned_by(grocerylist, guy) then
        walk_to(todolist.x, todolist.y, face_back, ian)
        say_line("hmm, we still need to find this Hans Scottleman guy...", ian)
    else
        walk_to(grocerylist.x, grocerylist.y, face_back, ian)
        if not world.mission_changed then
            say_line("Hmm, looks like the mission parameters have changed!", ian)
            say_line("We need to collect earthly food products now!", ian)
            world.mission_changed = true;
        else
            say_line("Still looking for those earthly food products!", ian)
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
            say_line("Ehm...", ian)
            say_line("Why is that red shirt guy still in my spaceship?", ian)
            wait(1000)
            change_room(beach)
            return
        end

        walk_to(ianX, ianY, face_front, ian)
    end

end