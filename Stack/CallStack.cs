using System.Collections.Generic;

namespace Stack
{
    public class CallStack
    {
        public List<ActivationRecord> records;

        public CallStack()
        {
            records = new List<ActivationRecord>();
        }

        public void Push (ActivationRecord ar)
        {
            this.records.Add(ar);
        }

        public ActivationRecord Pop ()
        {
            var length = records.Count;
            var result = records[length - 1];
            records.RemoveAt(length - 1);
            return result;
        }

        public ActivationRecord Peek()
        {
            return records[records.Count - 1];
        }
    }
}
