using System;
namespace CrossLang.Models
{
	public class FilterObject
	{
		public string FieldName { get; set; }
		public int Operator { get; set; }
        public List<dynamic> ContainedValues { get; set; }
    }
}
