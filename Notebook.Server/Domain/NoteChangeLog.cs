using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notebook.Server.Domain
{
    public class NoteChangeLog
    {
        public long Id { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Log { get; set; }
        public string Email { get; set; }
        //public string OldValueJson { get; set; }
        //public string NewValueJson { get; set; }

        //[NotMapped]
        //public Dictionary<string, string> OldValue
        //{
        //    get => DeserializeDictionary(OldValueJson);
        //    set => OldValueJson = SerializeDictionary(value);
        //}

        //[NotMapped]
        //public Dictionary<string, string> NewValue
        //{
        //    get => DeserializeDictionary(NewValueJson);
        //    set => NewValueJson = SerializeDictionary(value);
        //}

        //private Dictionary<string, string> DeserializeDictionary(string json)
        //{
        //    return string.IsNullOrEmpty(json)
        //        ? new Dictionary<string, string>()
        //        : JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        //}

        //private string SerializeDictionary(Dictionary<string, string> dictionary)
        //{
        //    return JsonConvert.SerializeObject(dictionary);
        //}

        //public void AddOrUpdateOldValue(string key, string value)
        //{
        //    var dictionary = OldValue;
        //    dictionary[key] = value; 
        //    OldValue = dictionary;
        //}

        //public void AddOrUpdateNewValue(string key, string value)
        //{
        //    var dictionary = NewValue;
        //    dictionary[key] = value; 
        //    NewValue = dictionary;
        //}
    }
}
