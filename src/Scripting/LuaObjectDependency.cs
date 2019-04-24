using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GameATron4000.Core;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using NLua;

namespace GameATron4000.Scripting
{
    public class LuaObjectDependency : IObjectDependency
    {
        private readonly string _objectId;
        private readonly string _state;
        private readonly LuaGameScript _script;

        private LuaObjectDependency(string objectId, string state, LuaGameScript script)
        {
            _objectId = objectId;
            _state = state;
            _script = script;
        }

        public static LuaObjectDependency FromString(string value, LuaGameScript script)
        {
            var parts = value.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return null;
//                throw new ArgumentException($"Invalid object dependency '{value}'.", nameof(value));
            }

            return new LuaObjectDependency(parts[0], parts[1], script);
        }

        public IObject Object
        {
            get
            {
                return _script.Objects.FirstOrDefault(o => o.Id == _objectId);
                // TODO Throw if null?
            }
        }

        public string State
        {
            get { return _state; }
        }
    }
}