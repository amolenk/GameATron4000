-- fridge = {
--     id = "fridge",
--     name = "fridge",
--     classes = {},
--     state = "closed",
--     verbs = {
--         open = function(obj)
--              change_state(obj, "open")
--         end
--         close = function(obj)
--             change_state(obj, "closed")
--         end
--         lookat = function(obj)
--            if obj.state == "open" then
--            say_line(guy, "It's an open fridge!")
--              else
--              say_line(guy, "It's a closed fridge!")
--          end
--     }
-- }

groceries = {
    id = "groceries",
    type = "object",
    name = "bag of groceries",
    verbs = {
        lookat = function()
            say_line(guy, "It's my shopping bag with groceries.")
            say_line(guy, "There's even a bottle of Info Support Awesome Sauce™ in there!")
        end,
        pickup = function()
            pickup_obj(groceries)
        end
    }
}

newspaper = {
    id = "newspaper",
    type = "object",
    name = "newspaper",
    verbs = {
        lookat = function(obj)
            if obj.state == "lookedAt" then
                say_line(guy, "It's still just an old newspaper!")
            else
                say_line(guy, "It looks like an old newspaper!")
                change_state(obj, "lookedAt")
            end
        end,
        pickup = function()
            return "ufo"
        end
    }
}

park_bench = {
    id = "park_bench",
    type = "object",
    classes = { class_untouchable }
}