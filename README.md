# UnityChatGPT-Virtual-Classroom

![Screenshot (714)](https://github.com/Wilson-Daniel/Web3Nest-Project/assets/77532147/021fabc8-5244-4ffe-b508-9021b3fcaf8c)
![Screenshot (715)](https://github.com/Wilson-Daniel/Web3Nest-Project/assets/77532147/43dc615e-5620-4201-9d35-71f54b62188b)
![Screenshot (716)](https://github.com/Wilson-Daniel/Web3Nest-Project/assets/77532147/6833eda0-2c68-4934-91c4-56438e6f05d3)

Tried to create an innovative educational platform that combines real-time code generation and interactive 3D model creation. You can learn coding with ChatGPT's assistance, visualize concepts with 3D models, and get expert answers to your questions. Collaborative learning, instant feedback, and continuous updates make this experience impactful and personalized!!


## How to run
- Unity 2021.3.8f or greater
- Create an account in OpenAI.com
- Get a new API Key and your API Organization
- Update ChatGPTSettings file located under Assets/Settings/ChatGPT/
  ![Screenshot (717)](https://github.com/Wilson-Daniel/Web3Nest-Project/assets/77532147/928a9752-8226-49ab-ba87-bf3e13c26442)

  Get the API Key from `https://platform.openai.com/account/api-keys`
  ![Screenshot (718)](https://github.com/Wilson-Daniel/Web3Nest-Project/assets/77532147/320aac21-f8b1-423a-bc2e-d307b732ea5e)
  
  You will be directed to API keys page
  - Click on `Create new secret key`
    ![Screenshot (720)](https://github.com/Wilson-Daniel/Web3Nest-Project/assets/77532147/48dedaca-56bf-4c28-b245-c70d4c67a605)
    
  - Give the desired key name and click on `Create secret key`
    
  Get the Organization ID from Settings available on the left side
  ![Screenshot (720)](https://github.com/Wilson-Daniel/Web3Nest-Project/assets/77532147/68813eeb-79ed-4c6e-bad0-5b3c612a56b5)
  - Copy Organization ID and paste it in the API Organization section.
  
  Put gpt-3.5-turbo in API Model  
  
## Some changes in C# file to obtain Response JSON object
1. Change IEnumerator Ask function in ChatGPTClient.cs file.
   ```javascript
   public IEnumerator Ask(string prompt, Action<ChatGPTResponse> callBack)
    {
        var url = chatGTPSettings.debug ? $"{chatGTPSettings.apiURL}?debug=true" : chatGTPSettings.apiURL;

        using(UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            var requestParams = JsonConvert.SerializeObject(new ChatGPTRequest
            {
                Model = chatGTPSettings.apiModel,
                Messages = new ChatGPTChatMessage[]
                   {
                       new ChatGPTChatMessage
                       {
                            Role = "user",
                            Content = prompt
                       }
                   }
            });

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestParams);
            
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.disposeCertificateHandlerOnDispose = true;

            request.SetRequestHeader("Content-Type", "application/json");

            // required to authenticate against OpenAI
            request.SetRequestHeader("Authorization", $"Bearer {chatGTPSettings.apiKey}");
            request.SetRequestHeader("OpenAI-Organization", chatGTPSettings.apiOrganization);

            var requestStartDateTime = DateTime.Now;

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string responseInfo = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseInfo);

                response.ResponseTotalTime = (DateTime.Now - requestStartDateTime).TotalMilliseconds;

                callBack(response);
            }
        }
    }
    ```
2. Change the class parameters in ChatGPTResponse.cs file
   This file is to modified according to the obect received by the API. Be aware to include all the parameters else the response object
     ```javascript
       {
      "id": "chatcmpl-123",
      "object": "chat.completion",
      "created": 1677652288,
      "choices": [{
        "index": 0,
        "message": {
          "role": "assistant",
          "content": "\n\nHello there, how may I assist you today?",
        },
        "finish_reason": "stop"
      }],
      "usage": {
        "prompt_tokens": 9,
        "completion_tokens": 12,
        "total_tokens": 21
      }
    }
     ```
   will not be received
   For json propert import ``using Newtonsoft.Json;``
   
   ```javascript
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
    ```
4. Change the class parameters in ChatGPTRequest.cs file
   ```javascript
   public class ChatGPTRequest
    {
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }
    
        [JsonProperty(PropertyName = "messages")]
        public ChatGPTChatMessage[] Messages { get; set; }
    }
    ```
5. Save changes and now you're good to go.


## Challenges faced
1. OpenAI Unity Package ``https://github.com/srcnalt/OpenAI-Unity`` sometime also give a "HTTP/1.1 401 Unauthorized
0x00007ff7c48af07d (Unity) StackWalker::GetCurrentCallstack....." error.
2. 6/10 time the response from ChatGPT API will be null.
3. If free trial period of your OpenAI account is over then you have to buy token or you'll get the same above error.
5. AI Assistant api is only available in gpt-3.5-turbo-0301 model.


