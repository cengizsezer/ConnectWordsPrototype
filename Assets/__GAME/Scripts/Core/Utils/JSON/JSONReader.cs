using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System;
using MyProject.Constant;




public static class JSONReader
{
  
    public static  Task<T> DeserializeJson<T>(this string data, JsonSerializerSettings settings = null)
    {
       
        //Debug.Log("******************************************** DESERIALIZE Enter");
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
       // Debug.Log("******************************************** DESERIALIZE memory");
        var streamReader = new StreamReader(stream);
      //  Debug.Log("******************************************** DESERIALIZE stream");
        var jsonReader = new JsonTextReader(streamReader);
      //  Debug.Log("******************************************** DESERIALIZE jsontextrider");
        var serializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.Create(settings);
       // Debug.Log("******************************************** DESERIALIZE jsontextserializer");
        T _t = serializer.Deserialize<T>(jsonReader);
      //  Debug.Log("******************************************** DESERIALIZE Exit");
        return Task.FromResult(_t);
      
    }
}
