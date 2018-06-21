using Model.DB.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTO.CodeDTO
{
    public class CodeHistoryDTO
    {
        public int Id { get; set; }

        public string CodeText { get; set; }
        public string Error { get; set; }
        public string Result { get; set; }
        public bool IsFavouriteCode { get; set; }
        public DateTime time { get; set; }

        public int UserCodeId { get; set; }
        public UserCodeDTO UserCode { get; set; }

    }
}
