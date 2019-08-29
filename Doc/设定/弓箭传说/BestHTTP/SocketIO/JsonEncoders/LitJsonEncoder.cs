﻿namespace BestHTTP.SocketIO.JsonEncoders
{
    using LitJson;
    using System;
    using System.Collections.Generic;

    public sealed class LitJsonEncoder : IJsonEncoder
    {
        public List<object> Decode(string json)
        {
            JsonReader reader = new JsonReader(json);
            return JsonMapper.ToObject<List<object>>(reader);
        }

        public string Encode(List<object> obj)
        {
            JsonWriter writer = new JsonWriter();
            JsonMapper.ToJson(obj, writer);
            return writer.ToString();
        }
    }
}

