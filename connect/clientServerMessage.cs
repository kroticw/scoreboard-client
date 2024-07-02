using System.Collections.Generic;
using Newtonsoft.Json;

namespace connect;


public class HistoryOfMatch {
    [JsonProperty("history")]
    public List<ClientServerMessage>? History { get; set; }

    public void addInHistory(ClientServerMessage evnt)
    {
        if (History == null) { 
            History = new List<ClientServerMessage>();
        }
        History.Add(evnt);
    }
}

public class ClientServerMessage {
    [JsonProperty("time")]
    public long Time { get; set; }

    [JsonProperty("command_one")]
    public Command CommandOne { get; set; }
    [JsonProperty("command_two")]
    public Command CommandTwo { get; set; }

    [JsonProperty("period")]
    public Period period { get; set; }
}

public class Command {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("score")]
    public int Score { get; set; }
}

public class Period {
    [JsonProperty("time_start")]
    public long TimeStart { get; set; }

    [JsonProperty("count")]
    public int? Count { get; set; }

    [JsonProperty("time_in_period")]
    public long TimeInPeriod { get; set; }
    [JsonProperty("pause")]
    public bool Pause { get; set; }
}