using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class ChatGPTResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("object")]
    public string Object { get; set; }

    [JsonProperty("created")]
    public long Created { get; set; }

    [JsonProperty("choices")]
    public List<ChatGPTChatChoice> Choices { get; set; }

    [JsonProperty("usage")]
    public ChatGPTChatUsage Usage { get; set; }

    public double ResponseTotalTime {get; set; }

}
public class ChatGPTMessage
{
    [JsonProperty("role")]
    public string Role {get; set; }

    [JsonProperty("content")]
    public string Content {get; set; }
}
public class ChatGPTChoice
{
    [JsonProperty("index")]
    public int Index {get; set; }

    [JsonProperty("message")]
    public ChatGPTChatMessage Message {get; set; }

    [JsonProperty("finish_reason")]
    public string FinishReason {get; set; }

}
public class ChatGPTUsage
{
    [JsonProperty("prompt_token")]
    public int PromptToken {get; set; }

    [JsonProperty("completion_token")]
    public int CompletionToken {get; set; }

    [JsonProperty("total_token")]
    public string TotalToken {get; set; }


}