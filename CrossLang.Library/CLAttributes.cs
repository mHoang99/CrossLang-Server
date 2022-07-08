using System;
namespace CrossLang.Library
{
    /// <summary>
    /// Class Attribute cho trường bắt buộc
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class Required : Attribute
    {

    }

    /// <summary>
    /// Class Attribute cho trường không được phép trùng
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class Unique : Attribute
    {

    }

    /// <summary>
    /// Class Attribute cho trường khóa chính
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {

    }

    /// <summary>
    /// Class Attribute cho trường là email
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class Email : Attribute
    {
    }

    /// <summary>
    /// Class Attribute cho trường số điện thoại
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumber : Attribute
    {

    }

    /// <summary>
    /// Class đánh dấu các trường là cột của bảng
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class DBColumn : Attribute
    {
    }

    /// <summary>
    /// Class đánh dấu các trường là cột hien thi len tren danh sach
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayColumn : Attribute
    {
    }

    /// <summary>
    /// Class đánh dấu các trường là loại json
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class DBJSON : Attribute
    {
    }

    /// <summary>
    /// Class Attribute cho trường có độ dài tối đa
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength : Attribute
    {
        #region Properties
        public int Value { get; set; }
        #endregion

        #region Constructor
        public MaxLength(int maxLength = 255)
        {
            this.Value = maxLength;
        }
        #endregion
    }

    /// <summary>
    /// Class Attribute cho tên bảng
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName : Attribute
    {
        #region Properties
        public string Value { get; set; }
        #endregion

        #region Constructor
        public TableName(string tableName = "")
        {
            this.Value = tableName;
        }
        #endregion
    }

    /// <summary>
    /// Class Attribute cho tên bảng
    /// </summary>
    /// CREATEDBY: VMHOANG
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionName : Attribute
    {
        #region Properties
        public string Value { get; set; }
        #endregion

        #region Constructor
        public CollectionName(string collection = "")
        {
            this.Value = collection;
        }
        #endregion
    }
}

