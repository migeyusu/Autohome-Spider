using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Car
{
    public class SubModel : Entity
    {
        private IList<PropertyGroup> _propertyGroups;
        private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// 是否在售
        /// </summary>
        public bool IsAvailable { get; set; }

        public int Year { get; set; }

        /// <summary>
        /// 存放序列化后的字符串
        /// </summary>
        public byte[] PropertyData { get; set; }

        [NotMapped]
        public bool PropertyUpdate { get; set; }

        [NotMapped]
        public IList<PropertyGroup> PropertyGroups {
            get {
                if (PropertyUpdate || _propertyGroups == null) {
                    using (var memoryStream = new MemoryStream(PropertyData)) {
                        _propertyGroups = _binaryFormatter.Deserialize(memoryStream) as IList<PropertyGroup>;
                    }
                }
                return _propertyGroups;
            }
            set {
                if (value == null) {
                    return;
                }
                _propertyGroups = value;
                using (var memoryStream = new MemoryStream()) {
                    _binaryFormatter.Serialize(memoryStream, value);
                    PropertyData = memoryStream.ToArray();
                }
            }
        }
    }

    /// <summary>
    /// 属性组
    /// </summary>
    [Serializable]
    public class PropertyGroup
    {
        public string Name { get; set; }

        public IList<PropertyField> PropertyFields { get; set; }

        public PropertyGroup()
        {
            PropertyFields = new List<PropertyField>();
        }
    }

    /// <summary>
    /// 键值对
    /// </summary>
    [Serializable]
    public class PropertyField
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}