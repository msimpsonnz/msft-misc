using System;
using System.Text;
using System.Text.Json;

namespace NET3.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = "{\"name\": \"myname\"}";
            ReadOnlySpan<byte> utf8 = Encoding.UTF8.GetBytes(json);
            Utf8JsonReaderLoop(utf8);
            // ReadOnlySpan<byte> utf8 = ...
            // Person person = JsonSerializer.Parse<Person>(utf8);

        }


        public static void Utf8JsonReaderLoop(ReadOnlySpan<byte> dataUtf8)
        {
            var json = new Utf8JsonReader(dataUtf8, isFinalBlock: true, state: default);

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.ValueSpan;
                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                        break;
                    case JsonTokenType.StartArray:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.String:
                        string valueString = json.GetString();
                        break;
                    case JsonTokenType.Number:
                        if (!json.TryGetInt32(out int valueInteger))
                        {
                            throw new FormatException();
                        }
                        break;
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        bool valueBool = json.GetBoolean();
                        break;
                    case JsonTokenType.Null:
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            dataUtf8 = dataUtf8.Slice((int)json.BytesConsumed);
            JsonReaderState state = json.CurrentState;
        }
    }
}
