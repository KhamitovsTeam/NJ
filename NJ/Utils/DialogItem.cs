using System.Collections.Generic;

namespace Chip
{
    public class DialogItem
    {
        private readonly List<string> _messages = new List<string>();
        private int _index = 0;

        public DialogItem(params string[] messages)
        {
            foreach (var message in messages)
            {
                _messages.Add(message);
            }
        }

        public bool HasNext()
        {
            return _index < _messages.Count;
        }

        public string Next()
        {
            var message = _index == 0 || _index < _messages.Count ? _messages[_index] : "";
            _index++;
            return message;
        }
    }
}