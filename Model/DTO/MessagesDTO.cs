using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTO
{
    public class MessagesDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string InboxText { get; set; }
        public string OutboxText { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public bool IsNew { get; set; }
        public bool IsInBox { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }
    }
}
